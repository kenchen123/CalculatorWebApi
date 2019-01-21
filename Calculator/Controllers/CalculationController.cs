using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Calculator.Models;

namespace Calculator.Controllers
{
    public class CalculationController : ApiController
    {
        static List<CalculationRecord> CalList = new List<CalculationRecord>();

        private static CalculationRecord Calculate(List<decimal> numbers)
        {
            var sum = numbers.Sum();
            return new CalculationRecord()
            {

                NumberList = numbers,
                Result = sum,
                CalculationTime = DateTime.Now
            };
        }

        [HttpGet]
        public IEnumerable<CalculationRecord> GetRecords()
        {
            return CalList;
        }

        [HttpGet]
        public CalculationRecord GetRecordsByKey(string key)
        {
            return CalList.FirstOrDefault(x => x.Key == key);
        }

        [HttpPost]
        public void PostRecord(string input)
        {
            var numbers = input.Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => decimal.Parse(x))
                .ToList();

            var result = Calculate(numbers);
            result.Key = Guid.NewGuid().ToString().Substring(0, 5);
            CalList.Add(result);

        }


        [HttpPut]
        public void UpdateRecord(string key, string input)
        {
            var oldRecord = CalList.SingleOrDefault(x => x.Key == key);
            var itemToRemove = CalList.SingleOrDefault(x => x.Key == key);
            if (itemToRemove != null)
                CalList.Remove(itemToRemove);

            var history = new List<CalculationHistory>();
            history.Add(new CalculationHistory
            {
                NumberList = oldRecord.NumberList,
                Result = oldRecord.Result,
                CalculationTime = oldRecord.CalculationTime

            });

            var numbers = input.Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => decimal.Parse(x))
                .ToList();

            var data = Calculate(numbers);
            data.Key = oldRecord.Key;
            data.UpdateHistory = history;
            CalList.Add(data);
        }

        [HttpDelete]
        public void DeleteRecord(string key)
        {
            var itemToRemove = CalList.SingleOrDefault(x => x.Key == key);
            if (itemToRemove != null)
                CalList.Remove(itemToRemove);

        }


    }
}
