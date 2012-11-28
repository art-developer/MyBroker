using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroker.Domain.Interfaces
{
    public interface IStrategyProvider
    {
        string GetName();
        IStrategyDecision GetStrategyDecision(RateRecord rec);
        IStrategyProvider Init(IDictionary<string,IDictionary<DateTime, decimal>> historyData);
    }
}
