using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyBroker.Domain.Interfaces;

namespace MyBroker.Domain
{
    public class BaseStrategyDecision:IStrategyDecision
    {

        #region IStrategyDecision Members

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

        public string AdditionalInfo
        {
            get;
            set;
        }

        public int Direction
        {
            
            get 
            {
                if (TakeProfit - StopLoss == 0)
                    return 0;
                return (TakeProfit - StopLoss) > 0 ? 1 : -1; 
            }
        }

        #endregion
    }
}
