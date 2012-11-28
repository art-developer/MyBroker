using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroker.Domain
{
    public class Candle
    {
        public Candle()
        { 
        
        }

        public string RateName
        {
            get;
            set;
        }

        public DateTime OpenTime
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

        public decimal HighPrice
        {
            get;
            set;
        }

        public decimal LowPrice
        {
            get;
            set;
        }

        public void AddNewData(DateTime updateTime,decimal price)
        {
            if (CandleData==null)
                CandleData = new Dictionary<DateTime, decimal>();
            if (CandleData.ContainsKey(updateTime))
                return;
            CandleData.Add(updateTime, price);
            DateTime openTime = CandleData.Keys.Min();
            OpenPrice = CandleData[openTime];
            DateTime closeTime = CandleData.Keys.Max();
            ClosePrice = CandleData[closeTime];
            HighPrice = CandleData.Max(item => item.Value);
            LowPrice = CandleData.Min(item => item.Value);
        }

        public void CloseCandle()
        {
            CandleData=null;
        }

        public IDictionary<DateTime, decimal> CandleData
        {
            get;
            set;
        }
    }
}
