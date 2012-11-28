using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroker.Domain
{
    public class RateRecord
    {
        public string Name { get; set; }
        public DateTime UpdateTime { get; set; }
        public decimal Value { get; set; }

        public string GetCsvString()
        {
            return string.Format("{0:dd.MM.yyyy HH:mm:ss};{1}", UpdateTime, Value);
        }
    }
}
