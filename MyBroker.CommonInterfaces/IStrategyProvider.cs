using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroker.CommonInterfaces
{
    public interface IStrategyProvider
    {
        string GetName();
        IStrategyDecision GetStrategyDecision(RateRecord rec);
        IStrategyProvider Init(Dictionary<DateTime, decimal> historyData);
    }
}
