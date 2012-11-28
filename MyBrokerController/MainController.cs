using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyBroker.Domain;
using MyBroker.Strategy;
using System.ComponentModel;
using MyBroker.Domain.Interfaces;

namespace MyBroker.Controller
{
    public class MainController
    {
        protected static readonly AsyncOperation _asyncOperation = AsyncOperationManager.CreateOperation(false);


        #region Private Variables

        private IList<string> _rates=null;
        private decimal _summ = 1000;
        private string _ratesFolder;
        private DateTime _dtStart;
        private DateTime? _dtEnd;

        private StrategyController _strategyController;
        private OrderController _orderController;
        private RateController _rateController;

        Thread _mainThread;
        private volatile bool _shouldStop;

        #endregion

        #region Constants

        private const int MAX_OPEN_ERRORS_COUNT = 10;
        private const int TIMER_INTERVAL = 5000;
        
        #endregion

        #region Events

        public delegate void NewRatesDelegate(RateRecord rec,Candle candle);
        public event NewRatesDelegate NewRatesEvent;

        public delegate void MessageDelegate(string message);
        public event MessageDelegate MessageEvent;

        public delegate void OpenOrderDelegate(Order order);
        public event OpenOrderDelegate OpenOrderEvent;

        public delegate void CloseOrderDelegate(Order order);
        public event CloseOrderDelegate CloseOrderEvent;

        public delegate void StopDelegate();
        public event StopDelegate StopEvent;

        #endregion

        private static MainController _instance = new MainController();
        private MainController()
        { 
            
        }

        public static MainController Instance
        {
            get 
            {
                return _instance;
            }
        }



        public void Start(IList<string> rates, decimal summ, string ratesFolder, DateTime dtStart, DateTime? dtEnd,string activeStrategy)
        {
            _summ = summ;
            _rates = rates;
            _ratesFolder = ratesFolder;
            _dtEnd = dtEnd;
            _dtStart = dtStart;


            _orderController = new OrderController(activeStrategy, summ, new List<string>() { "EUR/USD" });
            _orderController.MessageEvent += SendMessageEvent;
            _orderController.OpenOrderEvent += SendOpenOrderEvent;
            _orderController.CloseOrderEvent += SendCloseOrderEvent;

            _strategyController = new StrategyController(_orderController, new List<IStrategyProvider>() { new Sperandeo() });
            _rateController = new RateController(rates, dtStart, dtEnd);
            _rateController.HistoryEvent += OnHistoryEvent;
            _rateController.MessageEvent += SendMessageEvent;
            _rateController.NewRatesEvent += OnNewRatesEvent;
            _rateController.StopEvent += SendStopEvent;

            _rateController.Start();
        }

        private void OnNewRatesEvent(RateRecord rec, Candle candle)
        {
            SendNewRatesEvent(rec,candle);
            
            _strategyController.ProcessTick(rec);
            _orderController.CloseOrdersByLimits(rec);
        }

        public void Stop()
        {
            _rateController.Stop();
        }


        private void OnHistoryEvent(IDictionary<string, IDictionary<DateTime, decimal>> ratesHistory, IDictionary<string, IList<Candle>> candlesHistory)
        {
            //отображаем на историю на графике
            foreach (string key in candlesHistory.Keys)
            {
                foreach (Candle cndl in candlesHistory[key])
                {
                    SendNewRatesEvent(null, cndl);
                    Thread.Sleep(100);
                }
            }
            //инициализируем стратегии
            _strategyController.Init(ratesHistory);
        }


       
        //send events
        private void SendNewRatesEvent(RateRecord rec, Candle candle)
        {
            if (NewRatesEvent != null)
                _asyncOperation.Post(delegate {NewRatesEvent(rec,candle); }, null);
        }

       
        private void SendMessageEvent(string message)
        {
            if (MessageEvent != null)
                _asyncOperation.Post(delegate { MessageEvent(message); }, null);
        }

        private void SendOpenOrderEvent(Order order)
        {
            if (OpenOrderEvent != null)
                _asyncOperation.Post(delegate { OpenOrderEvent(order); }, null);
        }

        private void SendCloseOrderEvent(Order order)
        {
            if (CloseOrderEvent != null)
                _asyncOperation.Post(delegate { CloseOrderEvent(order); }, null);
        }

        private void SendStopEvent()
        {
            if (StopEvent != null)
                _asyncOperation.Post(delegate { StopEvent(); }, null);
        }
    }
}
