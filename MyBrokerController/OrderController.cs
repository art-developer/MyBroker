using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Win32;
using MyBroker.WinApi;
using MyBroker.Domain;
using System.ComponentModel;

namespace MyBroker.Controller
{
    public class OrderController
    {
        protected static readonly AsyncOperation _asyncOperation = AsyncOperationManager.CreateOperation(false);

        public delegate void MessageDelegate(string message);
        public event MessageDelegate MessageEvent;

        public delegate void OpenOrderDelegate(Order order);
        public event OpenOrderDelegate OpenOrderEvent;

        public delegate void CloseOrderDelegate(Order order);
        public event CloseOrderDelegate CloseOrderEvent;

        
        /// <summary>
        /// валютные пары по которым совершать реальные сделки
        /// </summary>
        private IList<string> _activeRates;
        /// <summary>
        /// стратегия по которой разрешено совершать реальные сделки
        /// </summary>
        private string _activeStrategyName;
        private decimal _orderSumm;
        private bool _enableRealOrders = true;
        private MailController _mailController;
        private IList<Order> _openedOrders;
        Dictionary<string, int> _strategyResult;

        public OrderController(string activeStrategyName,decimal orderSumm,IList<string> activeRates)
        {
            _activeStrategyName=activeStrategyName;    
            _orderSumm=orderSumm;
            _openedOrders = new List<Order>();
            _strategyResult = new Dictionary<string, int>();
            _activeRates = activeRates;

        }

        public Order OpenOrder(string strategyName,decimal currentPrice,decimal stopLoss,decimal takeProfit,string rateName,DateTime openTime, string strategyInfo)
        {
            bool isReal = !string.IsNullOrEmpty(_activeStrategyName) && strategyName == _activeStrategyName && _enableRealOrders && _activeRates.Contains(rateName);
            Order order=new Order(_orderSumm,currentPrice,isReal,stopLoss,takeProfit,rateName,strategyName,openTime,strategyInfo);
            if (order.IsReal)
                _enableRealOrders = order.IsReal = StartFx.OpenOrder(rateName, order.Direction, _orderSumm);
            _openedOrders.Add(order);

            if (OpenOrderEvent != null)
                OpenOrderEvent(order);
            return order;
        }

        

       
        private void UpdateStrategyResult(Order order)
        {
            if (!_strategyResult.Keys.Contains(order.StrategyName))
                _strategyResult.Add(order.StrategyName, 0);
            _strategyResult[order.StrategyName] += order.IntProfit * order.Direction;
        }

        public string GetStrategyResultMessage(string strategyName)
        {
            return string.Format("ИТОГО по стратегии ({0}):{1} пунктов{2}", strategyName, _strategyResult[strategyName],Environment.NewLine);
        }

        public void CloseOrder(Order order,RateRecord rec)
        {
            if (order.RateName != rec.Name)
                return;
            order.ClosePrice = rec.Value;
            order.CloseTime = rec.UpdateTime;
            if (order.IsReal)
                order.IsReal = _enableRealOrders = StartFx.CloseOrder(order.RateName);
            UpdateStrategyResult(order);

            if (CloseOrderEvent != null)
                CloseOrderEvent(order);
            

            _openedOrders.Remove(order);
        }

        public void CloseOrdersByLimits(RateRecord rec)
        {
            for (int i = _openedOrders.Count-1; i >= 0;i--)
            {
                
                Order order = _openedOrders[i];
                if (rec.Name != order.RateName)
                    continue;
                if (order.Direction < 0 && (order.StopLoss < rec.Value || order.TakeProfit > rec.Value))
                {
                    CloseOrder(order, rec);
                }
                if (order.Direction > 0 && (order.StopLoss > rec.Value || order.TakeProfit < rec.Value))
                {
                    CloseOrder(order, rec);
                }
            }
        }

        public Order GetOpenedOrder(string strategyName,string rateName)
        {
            return _openedOrders.Where(item => item.StrategyName == strategyName && item.RateName == rateName).FirstOrDefault();
        }

        public IList<Order> GetOpenedOrders()
        {
            return _openedOrders;
        }
        
        private void LogMessage(string message)
        {
            if (MessageEvent != null)
                _asyncOperation.Post(delegate { MessageEvent(message); }, null);
        }



       
    }
}
