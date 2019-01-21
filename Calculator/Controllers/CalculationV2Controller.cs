using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Calculator.Models;

namespace Calculator.Controllers
{
    public class CalculationV2Controller : ApiController
    {
        
       
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
        public IHttpActionResult GetAll()
        {
            return Ok(CalculationRecordList.CalculationRecords);
        }

        [HttpGet]
        public IHttpActionResult GetByKey(string key)
        {
            var record = CalculationRecordList.CalculationRecords.FirstOrDefault(x => x.Key == key);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        //public IHttpActionResult Post([FromBody] CalculationRecord calculation)
        //{
        //    CalculationRecordList.CalculationRecords.Add(calculation);
        //    return Ok(CalculationRecordList.CalculationRecords);
        //}

        [HttpPost]
        public IHttpActionResult Post(string input)
        {
            var numbers = input.Split(' ')
              .Where(x => !string.IsNullOrEmpty(x))
              .Select(x => decimal.Parse(x))
              .ToList();

            var result = Calculate(numbers);
            result.Key = Guid.NewGuid().ToString().Substring(0, 5);

            CalculationRecordList.CalculationRecords.Add(result);
            return Ok(CalculationRecordList.CalculationRecords);
        }

        [HttpPut]
        public IHttpActionResult Put(string key, string input)
        {
            var oldRecord = CalculationRecordList.CalculationRecords.SingleOrDefault(x => x.Key == key);
            var itemToRemove = CalculationRecordList.CalculationRecords.SingleOrDefault(x => x.Key == key);

            if (itemToRemove != null)
                CalculationRecordList.CalculationRecords.Remove(itemToRemove);

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
            CalculationRecordList.CalculationRecords.Add(data);

            return Ok(CalculationRecordList.CalculationRecords);
        }

        [HttpDelete]
        public IHttpActionResult Delete(string key)
        {
            CalculationRecordList.CalculationRecords.RemoveAll(x => x.Key == key);
            return Ok(CalculationRecordList.CalculationRecords);
        }
    }
}
