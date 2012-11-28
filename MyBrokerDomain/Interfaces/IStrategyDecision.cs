using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroker.Domain.Interfaces
{
    public interface IStrategyDecision
    {
        decimal StopLoss
        {
            get;
        }

        decimal TakeProfit
        {
            get;
        }

        string AdditionalInfo
        {
            get;
        }

        int Direction
        {
            get;
        }
    }
}
