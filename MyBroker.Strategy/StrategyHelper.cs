using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Domain;

namespace MyBroker.Strategy
{
    public static class StrategyHelper
    {
        public static decimal GetMinimum(int minutes,IDictionary<DateTime,decimal> historyData)
        {
            DateTime lastDate = historyData.Keys.Max();
            return historyData.Where(item => (item.Key.AddMinutes(minutes) > lastDate)).Min(item2 => item2.Value);
        }

        public static decimal GetMaximum(int minutes, IDictionary<DateTime, decimal> historyData)
        {
            DateTime lastDate = historyData.Keys.Max();
            return historyData.Where(item => (item.Key.AddMinutes(minutes) > lastDate)).Max(item2 => item2.Value);
        }

        public static decimal GetLastValue(IDictionary<DateTime, decimal> historyData)
        {
            DateTime lastDate = historyData.Keys.Max();
            return historyData[lastDate];
        }

        public static IDictionary<string, IList<Candle>> BuildCandles(IDictionary<string, IDictionary<DateTime, decimal>> pHistoryData, int intervalMinutes)
        {
            Dictionary<string, IList<Candle>> _candles = new Dictionary<string, IList<Candle>>();

            foreach (string rateName in pHistoryData.Keys)
            {
                IDictionary<DateTime, decimal> historyData = pHistoryData[rateName];
                List<Candle> candles = new List<Candle>();
                _candles.Add(rateName, candles);
                if (historyData.Count() == 0)
                    continue;
                DateTime minDate = historyData.Keys.Min();
                DateTime lastDate = historyData.Keys.Max();
                int firstIntervalStartMinute = ((minDate.Minute / 5) + 1) * 5;
                int minutesDiff = firstIntervalStartMinute - minDate.Minute;
                minDate = minDate.AddMinutes(minutesDiff);
                DateTime firstIntervalStartDate = new DateTime(minDate.Year, minDate.Month, minDate.Day, minDate.Hour, minDate.Minute, 0);
                DateTime currentTime = firstIntervalStartDate;
                
                while (currentTime < lastDate)
                {
                    DateTime nextTime = currentTime.AddMinutes(intervalMinutes);
                    Candle candle = new Candle();
                    candle.OpenTime = currentTime;

                    DateTime? openTime = null;
                    if (historyData.Keys.Where(item => item >= currentTime && item < nextTime).Count() > 0)
                        openTime = historyData.Keys.Where(item => item >= currentTime && item < nextTime).Min();
                    if (!openTime.HasValue)
                    {
                        currentTime = currentTime.AddMinutes(intervalMinutes);
                        continue;
                    }

                    DateTime? closeTime = null;
                    if (historyData.Keys.Where(item => item >= currentTime && item < nextTime).Count() > 0)
                        closeTime = historyData.Keys.Where(item => item >= currentTime && item < nextTime).Max();
                    if (!closeTime.HasValue)
                    {
                        currentTime = currentTime.AddMinutes(intervalMinutes);
                        continue;
                    }
                    candle.RateName = rateName;
                    candle.OpenPrice = historyData[openTime.Value];
                    candle.ClosePrice = historyData[closeTime.Value];
                    candle.HighPrice = historyData.Where(item => item.Key >= currentTime && item.Key < nextTime).Max(item => item.Value);
                    candle.LowPrice = historyData.Where(item => item.Key >= currentTime && item.Key < nextTime).Min(item => item.Value);
                    candles.Add(candle);
                    currentTime = currentTime.AddMinutes(intervalMinutes);
                }
                Candle lastCandle = candles.OrderBy(item => item.OpenTime).LastOrDefault();
                if (lastCandle != null)
                {
                    
                    foreach (KeyValuePair<DateTime, decimal> pair in historyData.Where(item => item.Key >= lastCandle.OpenTime && item.Key < lastCandle.OpenTime.AddMinutes(intervalMinutes)))
                        lastCandle.AddNewData(pair.Key, pair.Value);
                }
            }
            return _candles;

        }

        public static void CloseCandle(IDictionary<string, IList<Candle>> candles, RateRecord rec, int candlesIintervalMinutes)
        {

            if (!candles.ContainsKey(rec.Name))
                candles.Add(rec.Name, new List<Candle>());

            DateTime openCandleDate = new DateTime(rec.UpdateTime.Year, rec.UpdateTime.Month, rec.UpdateTime.Day, rec.UpdateTime.Hour, (rec.UpdateTime.Minute / candlesIintervalMinutes) * candlesIintervalMinutes, 0);
            DateTime closeCandleDate = new DateTime(rec.UpdateTime.Year, rec.UpdateTime.Month, rec.UpdateTime.Day, rec.UpdateTime.Hour, (rec.UpdateTime.Minute / candlesIintervalMinutes) * candlesIintervalMinutes, 0).AddMinutes(candlesIintervalMinutes);
            
            Candle currentCandle=candles[rec.Name].Where(item=>item.OpenTime==openCandleDate).FirstOrDefault();

            if (currentCandle==null)
            {
                ClosePreviousCandles(candles[rec.Name].Where(item=>item.CandleData!=null).ToList());
                currentCandle = new Candle();
                currentCandle.RateName = rec.Name;
                currentCandle.OpenTime = openCandleDate;
                candles[rec.Name].Add(currentCandle);
            }
            
            currentCandle.AddNewData(rec.UpdateTime, rec.Value);


        }

        private static void ClosePreviousCandles(IList<Candle> candles) 
        {
            foreach (Candle candle in candles)
            {
                candle.CloseCandle();
            }
        }
    }
}
