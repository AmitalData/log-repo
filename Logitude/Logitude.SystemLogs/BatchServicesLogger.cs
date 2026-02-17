using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SystemLogs
{
    public static class BatchServicesLogger
    {
        public static void Log(BatchServiceLogParams parameters)
        {
            BatchServicesLogRepository repository = new BatchServicesLogRepository();
            BatchServicesLog log = repository.GetSingleBatchServicesLog(parameters.Id);
            if (log == null)
            {
                log = new BatchServicesLog()
                {
                    BatchServiceCode = parameters.BatchServiceCode,
                    CPU = parameters.CPU,
                    CreateDate = parameters.CreateDate,
                    Id = parameters.Id,
                    LastActivity = parameters.LastActivity,
                    NumberOfDoneItems = parameters.NumberOfDoneItems,
                    DoneItemsInFiveMinutes = parameters.DoneItemsInFiveMinutes,
                    DoneItemsInOneHour = parameters.DoneItemsInOneHour,
                    DoneItemsInOneMinute = parameters.DoneItemsInOneMinute,
                };


                //ErrorMessage    "The field BatchServiceCode must be a string or array type with a maximum length of '40'."  string
                log.BatchServiceCode = log.BatchServiceCode ?? "";
                if (log.BatchServiceCode.Length > 40)
                {
                    log.BatchServiceCode = log.BatchServiceCode.Substring(0, 40);
                }

                repository.Add(log);
                repository.SubmitChanges();
            }
            else
            {
                log.CPU = parameters.CPU;
                log.LastActivity = parameters.LastActivity;
                log.NumberOfDoneItems = parameters.NumberOfDoneItems;
                log.DoneItemsInFiveMinutes = parameters.DoneItemsInFiveMinutes;
                log.DoneItemsInOneHour = parameters.DoneItemsInOneHour;
                log.DoneItemsInOneMinute = parameters.DoneItemsInOneMinute;
                repository.Update(log);
                repository.SubmitChanges();
            }
            
        }
    }

    public class BatchServiceLogParams
    {
        public string Id { get; set; }
        public string BatchServiceCode { get; set; }

        public decimal? CPU { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LastActivity { get; set; }

        public int? NumberOfDoneItems { get; set; }

        public int DoneItemsInFiveMinutes { get; set; }

        public int DoneItemsInOneHour { get; set; }

        public int DoneItemsInOneMinute { get; set; }
    }
}
