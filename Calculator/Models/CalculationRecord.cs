using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calculator.Models
{
    public class CalculationRecordList
    {
        public static List<CalculationRecord> CalculationRecords { get; set; } = new List<CalculationRecord>();
    }

    public class CalculationRecord
    {
        public string Key { get; set; }
        public List<decimal> NumberList { get; set; }
        public decimal Result { get; set; }
        public DateTime CalculationTime { get; set; }
        public List<CalculationHistory> UpdateHistory { get; set; }
    }

    public class CalculationHistory
    {
        public List<decimal> NumberList { get; set; }
        public decimal Result { get; set; }
        public DateTime CalculationTime { get; set; }
    }
}