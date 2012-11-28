using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroker.Domain
{
    public class Order
    {
        public Order(decimal summ, decimal openPrice, bool isReal, decimal stopLoss, decimal takeProfit, string rateName,string strategyName,DateTime openTime,string strategyInfo)
        {
            Summ = summ;
            OpenPrice = openPrice;
            IsReal = isReal;
            StopLoss = stopLoss;
            TakeProfit = takeProfit;
            RateName = rateName;
            StrategyName=strategyName;
            OpenTime = openTime;
            StrategyInfo = strategyInfo;
        }

        public string StrategyInfo
        {
            get;
            set;
        }

        public DateTime OpenTime
        {
            get;
            set;
        }

        public string StrategyName
        {
            get;
            set;
        }

        public string RateName
        {
            get;
            set;
        }
        public decimal Summ
        {
            get;
            set;
        }

        public decimal StopLoss
        {
            get;
            set;
        }

        public decimal TakeProfit
        {
            get;
            set;
        }

        public bool IsReal
        {
            get;
            set;
        }

        public decimal OpenPrice
        {
            get;
            set;
        }

        public decimal ClosePrice
        {
            get;
            set;
        }

        public DateTime CloseTime
        {
            get;
            set;
        }

        public int Direction
        {
            get { return (TakeProfit - StopLoss) > 0 ? 1 : -1; }
        }

        public decimal Profit
        {
            get { return ClosePrice - OpenPrice; }
        }

        public int IntProfit
        {
            get { return (int)Math.Ceiling(Profit * 10000); }
        }

        public int AbsIntegerProfit
        {
            get { return (int)Math.Ceiling(Math.Abs(Profit) * 10000); }
        }

        public bool IsProfit
        {
            get { return (Direction * Profit > 0); }
        }

        public string GetShortOpenMessage()
        {
            return string.Format("{0} - Открыта сделка на {1}",RateName, Direction > 0 ? "ПОКУПКУ" : "ПРОДАЖУ");
        }



        public string GetFullOpenMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Валюта:{0}{1}", RateName, Environment.NewLine);
            sb.AppendFormat("Стратегия:{0}{1}",StrategyName , Environment.NewLine);
            sb.AppendFormat("Операция:{0}{1}", Direction > 0 ? "Покупка" : "Продажа", Environment.NewLine);
            sb.AppendFormat("Дата открытия сделки:{0:yyyy-MM-dd HH:mm}{1}", OpenTime, Environment.NewLine);
            sb.AppendFormat("Цена открытия сделки:{0}{1}", OpenPrice, Environment.NewLine);
            sb.AppendFormat("Реальная операция:{0}{1}", IsReal ? "Да" : "Нет",Environment.NewLine);
            sb.AppendFormat("Дополнительная информация:{0}",Environment.NewLine);
            sb.AppendFormat("{0}",StrategyInfo);
            return sb.ToString();
        }

        

        public string GetShortResultMessage()
        {
            return string.Format("{0} - {1} - {2} пунктов",RateName, (Direction * Profit)>0 ? "ПРИБЫЛЬ" : "УБЫТОК", AbsIntegerProfit);
        }

        public string GetFullResultMessage()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendFormat("Валюта:{0}{1}", RateName, Environment.NewLine);
            sb.AppendFormat("Стратегия:{0}{1}", StrategyName, Environment.NewLine);
            sb.AppendFormat("Результат:{0}{1}", IsProfit ? "ПРИБЫЛЬ" : "УБЫТОК",Environment.NewLine);
            sb.AppendFormat("Операция:{0}{1}", Direction > 0 ? "Покупка" : "Продажа", Environment.NewLine);
            sb.AppendFormat("Дата открытия сделки:{0:yyyy-MM-dd HH:mm}{1}", OpenTime, Environment.NewLine);
            sb.AppendFormat("Дата закрытия сделки:{0:yyyy-MM-dd HH:mm}{1}", CloseTime, Environment.NewLine);
            sb.AppendFormat("Цена открытия сделки:{0}{1}", OpenPrice, Environment.NewLine);
            sb.AppendFormat("Цена закрытия сделки:{0}{1}", ClosePrice, Environment.NewLine);
            sb.AppendFormat("Количество пунктов:{0}{1}", AbsIntegerProfit, Environment.NewLine);
            sb.AppendFormat("Реальная операция:{0}{1}", IsReal ? "Да" : "Нет", Environment.NewLine);
            sb.AppendFormat("Дополнительная информация:{0}", Environment.NewLine);
            sb.AppendFormat("{0}", StrategyInfo);
            return sb.ToString();
        }
    }
}
