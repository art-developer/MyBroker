using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Domain;
using EfxRateProvider;
using MyBroker.Strategy;
using System.Threading;
using System.ComponentModel;

namespace MyBroker.Controller
{
    public class RateController
    {

        public delegate void MessageDelegate(string message);
        public event MessageDelegate MessageEvent;

        public delegate void StopDelegate();
        public event StopDelegate StopEvent;

        public delegate void HistoryDelegate(IDictionary<string, IDictionary<DateTime, decimal>> ratesHistory, IDictionary<string, IList<Candle>> candlesHistory);
        public event HistoryDelegate HistoryEvent;

        public delegate void NewRatesDelegate(RateRecord rec, Candle candle);
        public event NewRatesDelegate NewRatesEvent;

        private IDictionary<string,IList<Candle>> _candles;
        private IList<string> _rates = null;
        private bool _shouldStop = true;
        private Thread _mainThread;
        private DateTime _dtStart;
        private DateTime? _dtEnd;

        public const int CANDLES_INTERVAL_MINUTES = 5;

        public RateController(IList<string> rates, DateTime dtStart, DateTime? dtEnd)
        {
            _rates = rates;
            _candles = new Dictionary<string, IList<Candle>>();
            _dtEnd = dtEnd;
            _dtStart = dtStart;
        }

       
        public void Start()
        {
            SendMessageEvent("Запуск!");
            _mainThread = new Thread(new ThreadStart(MainThread));
            _shouldStop = false;
            _mainThread.Start();
        }

        public void Stop()
        {
            _shouldStop = true;
            SendMessageEvent("Останавливаем...");
        }

        private void MainThread()
        {

            if (_rates.Count == 0)
            {
                SendMessageEvent("Невыбрано ни одной валютной пары!");
                _shouldStop = true;
            }

            Dictionary<string, DateTime> lastProcessedDates = new Dictionary<string, DateTime>();

            if (!_shouldStop)
            {
                IDictionary<string, IDictionary<DateTime, decimal>> ratesHistory = GetRatesHistory(_rates, _dtStart.AddHours(-4), _dtStart);
                _candles = StrategyHelper.BuildCandles(ratesHistory, RateController.CANDLES_INTERVAL_MINUTES);
                if (HistoryEvent != null)
                    HistoryEvent(ratesHistory, _candles);
                foreach (string rateName in _rates)
                {
                    if (ratesHistory[rateName].Count == 0)
                        lastProcessedDates.Add(rateName, _dtStart);
                    else
                        lastProcessedDates.Add(rateName, ratesHistory[rateName].Keys.Max());
                }
            }

            while (!_shouldStop)
            {
                //читаем котировки из интернета
                IDictionary<string, IDictionary<DateTime, decimal>> ratesCache;
                IDictionary<string, IList<Candle>> candlesCache;
                if (_dtEnd == null)
                {
                    ratesCache = GetCurrentRates(_rates);
                    candlesCache = GetCurrentCandles(_rates);
                }
                else
                {
                    ratesCache = GetRatesHistory(_rates, lastProcessedDates.Values.Min().AddSeconds(1), lastProcessedDates.Values.Min().AddHours(2));
                    candlesCache = StrategyHelper.BuildCandles(ratesCache, CANDLES_INTERVAL_MINUTES);
                }

                //обрабатываем котировки по каждой валютной паре
                foreach (string rateName in _rates)
                {
                    if (_shouldStop)
                        break;
                    if (!ratesCache.ContainsKey(rateName))
                        continue;
                    IDictionary<DateTime, decimal> rateCache = ratesCache[rateName];
                    IList<Candle> candleCache = candlesCache[rateName];

                    while (rateCache.Count > 0 && rateCache.Keys.Min() > lastProcessedDates[rateName] && !_shouldStop)
                    {

                        RateRecord rec = new RateRecord();
                        rec.Name = rateName;
                        rec.UpdateTime = rateCache.Keys.Min();
                        rec.Value = rateCache[rec.UpdateTime];
                        
                        Candle candle = candlesCache[rateName].Where(item => item.OpenTime < rec.UpdateTime && rec.UpdateTime < item.OpenTime.AddMinutes(RateController.CANDLES_INTERVAL_MINUTES)).FirstOrDefault();
                        SendNewRatesEvent(rec,candle);

                        rateCache.Remove(rec.UpdateTime);
                        candlesCache[rateName].Remove(candle);

                        lastProcessedDates[rateName] = rec.UpdateTime;
                    }

                }
                if (_dtEnd != null && lastProcessedDates.Values.Min() > _dtEnd)
                    break;
                Thread.Sleep(5000);
            }
            if (StopEvent != null)
                StopEvent();
            SendMessageEvent("Работа остановлена");
        }


        public IDictionary<string,IList<Candle>> GetCurrentCandles(IList<string> rates)
        {
            IDictionary<string, IList<Candle>> historyData = new Dictionary<string, IList<Candle>>();
            
            foreach (string rate in rates)
            {
                var hd = new List<Candle>();
                if (!_candles.ContainsKey(rate))
                    continue;
                var record = _candles[rate].OrderBy(item => item.OpenTime).LastOrDefault();
                if (record == null)
                    continue;
                hd.Add(record);
                historyData.Add(rate, hd);
            }
            return historyData;
        }

      
        public IDictionary<string, IDictionary<DateTime, decimal>> GetCurrentRates(IList<string> rates)
        {
            IDictionary<string, IDictionary<DateTime, decimal>> historyData = new Dictionary<string, IDictionary<DateTime, decimal>>();
            try
            {
                IList<RateRecord> rts = RateProvider.Instance.GetRates();
                foreach (string rate in rates)
                {
                    var hd = new Dictionary<DateTime, decimal>();
                    var record = rts.Where(item => item.Name == rate).FirstOrDefault();
                    if (record == null)
                        continue;
                    StrategyHelper.CloseCandle(_candles,record,CANDLES_INTERVAL_MINUTES);
                    hd.Add(record.UpdateTime, record.Value);
                    historyData.Add(rate, hd);
                }
            }
            catch (Exception ex)
            {
                SendMessageEvent(ex.Message);
            }
            return historyData;
        }

       
        public IDictionary<string, IDictionary<DateTime, decimal>> GetRatesHistory(IList<string> rates,DateTime startDate, DateTime endDate)
        {
            
            IDictionary<string, IDictionary<DateTime, decimal>> historyData = new Dictionary<string, IDictionary<DateTime, decimal>>();

            foreach (string rateName in rates)
            {
                SendMessageEvent(string.Format("Читаем историю {2} c {0:dd/MM/yyyy HH:mm} по {1:dd/MM/yyyy HH:mm}", startDate, endDate,rateName));
                historyData.Add(rateName, RateProvider.Instance.GetRatesHistory(rateName, startDate, endDate));

            }

            return historyData;
        }

        //send events
        private void SendMessageEvent(string message)
        {
            if (MessageEvent != null)
                MessageEvent(message);
        }

        private void SendNewRatesEvent(RateRecord rec,Candle candle)
        {
            if (NewRatesEvent != null)
                NewRatesEvent(rec,candle);
        }
    }
}
