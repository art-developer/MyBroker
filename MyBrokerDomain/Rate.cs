using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroker.Domain
{
    public class RateDelete
    {
        private Dictionary<DateTime, decimal> _historyData = null;
        private decimal _currentValue;
        private DateTime _currentDate;

        public RateDelete()
        {
            _historyData = new Dictionary<DateTime, decimal>();
        }

        public void Add(DateTime date, decimal value)
        {
            _currentDate = date;
            _currentValue = value;
            _historyData.Add(date,value);
        }

        

        public decimal GetCurrentValue()
        {
            return _currentValue;
        }

        public Order OpenedOrder
        {
            get;
            set;
        }

        public string GetCsvString()
        {
            int increaseOrder = 0;
            int decreaseOrder = 0;

            if (OpenedOrder != null)
            {
                increaseOrder = OpenedOrder.Direction > 0 ? 2 : 0;
                decreaseOrder = OpenedOrder.Direction < 0 ? 2 : 0;
            }
            return string.Format("{0:dd.MM.yyyy HH:mm:ss};{1};{2};{3}", _currentDate, _currentValue, increaseOrder,decreaseOrder);
        }
    }
}
