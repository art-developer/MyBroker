using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Domain;
using System.Diagnostics;
using MyBroker.Domain.Interfaces;

namespace MyBroker.Strategy
{
    public class FirstStrategy:IStrategyProvider
    {

        private IDictionary<string,IDictionary<DateTime, decimal>> _historyData;

        public delegate void MessageDelegate(string message);
        public static event MessageDelegate MessageEvent;

        private const int INTERVAL_MINUTES = 2;
        private const int LARGE_MEDIUM_INTERVAL_MINUTES = 120;

        private const decimal MAX_DIRECTION = 0.0005M;
        private const decimal STOP_LOSS_POINTS = 0.0210M;
        private const decimal TAKE_PROFIT_POINTS = 0.0210M;







        public static IStrategyDecision TestAnalize(IDictionary<DateTime, decimal> historyData)
        {
            decimal lastValue = StrategyHelper.GetLastValue(historyData);
            BaseStrategyDecision decision = new BaseStrategyDecision();
            

            if ((decimal)DateTime.Now.Minute % 2 == 0)
            {
                decision.TakeProfit = lastValue + (TAKE_PROFIT_POINTS * -1);
                decision.StopLoss = lastValue - (STOP_LOSS_POINTS * -1);
                return decision;
            }
            else
            {
                decision.TakeProfit = lastValue + (TAKE_PROFIT_POINTS);
                decision.StopLoss = lastValue - (STOP_LOSS_POINTS);
                return decision;
            }
            
        }




        #region IStrategyProvider Members

        public string GetName()
        {
            return "Стратегия по тренду (2 часа)";
        }

        public IStrategyProvider Init(IDictionary<string,IDictionary<DateTime, decimal>> historyData)
        {
            _historyData = historyData;
            return this;
        }

        public IStrategyDecision GetStrategyDecision(RateRecord rec)
        {
            DateTime date = rec.UpdateTime;
            decimal price = rec.Value;
            IDictionary<DateTime, decimal> historyData = _historyData[rec.Name];

            if (!historyData.Keys.Contains(date))
                historyData.Add(date,price);
            //return TestAnalize(historyData);
            decimal lastValue = StrategyHelper.GetLastValue(historyData);
            decimal decrease = StrategyHelper.GetMaximum(INTERVAL_MINUTES,historyData) - lastValue;
            decimal increase = lastValue - StrategyHelper.GetMinimum(INTERVAL_MINUTES,historyData);
            decimal largeMedium = (StrategyHelper.GetMaximum(LARGE_MEDIUM_INTERVAL_MINUTES, historyData) + StrategyHelper.GetMinimum(LARGE_MEDIUM_INTERVAL_MINUTES,historyData)) / 2;

            int momentDirection = 0;
            if (decrease > MAX_DIRECTION)
                momentDirection = -1;
            if (increase > MAX_DIRECTION)
                momentDirection = 1;

            int longDirection = 0;
            if (lastValue > largeMedium)
                longDirection = 1;
            if (lastValue < largeMedium)
                longDirection = -1;

            int result = 0;
            if (momentDirection == -1 && longDirection == +1)
                result = 1;
            if (momentDirection == 1 && longDirection == -1)
                result = -1;

            BaseStrategyDecision decision = new BaseStrategyDecision();
            decision.TakeProfit = lastValue  + (TAKE_PROFIT_POINTS*result);
            decision.StopLoss = lastValue - (STOP_LOSS_POINTS * result);
            StringBuilder additionalInfo = new StringBuilder();
            additionalInfo.AppendFormat("Максимальное изменение за две минуты:{0}", (decrease > increase ? decrease : increase)*momentDirection);
            additionalInfo.AppendFormat("Текущее отклонение от среднего за 30 минут:{0}", lastValue-largeMedium);
            return decision;
        }

        #endregion
    }
}
