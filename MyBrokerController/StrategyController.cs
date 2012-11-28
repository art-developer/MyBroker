using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Domain;
using MyBroker.Domain.Interfaces;

namespace MyBroker.Controller
{
    public class StrategyController
    {
        private IList<IStrategyProvider> _strategyProviders;
        private OrderController _orderController;

        public StrategyController(OrderController orderController,IList<IStrategyProvider> strategyProviders)
        {
            _strategyProviders = strategyProviders;
            _orderController = orderController;
        }

        public void Init(IDictionary<string, IDictionary<DateTime, decimal>> historyData)
        {
            foreach (IStrategyProvider provider in _strategyProviders)
            {
                provider.Init(historyData);
            }
        }

        public void AddStrategyProvider(IStrategyProvider strategyProvider)
        {
            _strategyProviders.Add(strategyProvider);
        }

        public void ProcessTick(RateRecord rateRecord)
        {
            foreach(IStrategyProvider provider in _strategyProviders)
            {
                //analize
                IStrategyDecision decision = provider.GetStrategyDecision(rateRecord);
                //make orders
                OpenOrCloseOrders(provider.GetName(), decision, rateRecord);
            }
        }

        private void OpenOrCloseOrders(string strategyName, IStrategyDecision decision,RateRecord rec)
        {
            decimal lastValue = rec.Value;
            if (decision.Direction == 0)
                return;
            Order openedOrder = _orderController.GetOpenedOrder(strategyName,rec.Name);
            if (openedOrder!=null)
            {
                if (openedOrder.Direction * decision.Direction > 0)
                    openedOrder.TakeProfit = decision.TakeProfit;
                else
                {
                    _orderController.CloseOrder(openedOrder,rec);
                }
            }
            else
            { 
                _orderController.OpenOrder(strategyName,lastValue,decision.StopLoss,decision.TakeProfit,rec.Name,rec.UpdateTime,decision.AdditionalInfo);
            }
        }

        
    }
}
