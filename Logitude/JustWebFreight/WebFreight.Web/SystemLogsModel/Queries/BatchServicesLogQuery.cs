using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.SystemLogsModel.EntityList;
using WebFreight.Web.SystemLogsModel.EntityPMs;

namespace WebFreight.Web.SystemLogsModel.Queries
{
    public class BatchServicesLogQuery
    {

          BatchServicesLogRepository repository;

        public BatchServicesLogQuery()
        {
            repository = new BatchServicesLogRepository(); 
        }       

        public BatchServicesLogQuery(BatchServicesLogRepository BatchServicesLogRepository)
        {
            repository = BatchServicesLogRepository;
        }

        public BatchServicesLogPM GetSinglePM(string id)
        {
            BatchServicesLogPM securedPm = new BatchServicesLogPM();
            BatchServicesLog log = repository.GetSingleBatchServicesLog(id);

            BatchServicesLogPM batchServicesLog = new BatchServicesLogPM()
            {
                Id = log.Id,
                LastActivity = log.LastActivity,
                BatchServiceCode = log.BatchServiceCode,
                CPU = log.CPU,
                CreateDate = log.CreateDate,
                NumberOfDoneItems= log.NumberOfDoneItems
              
            };

            return batchServicesLog;
        }

        public IQueryable<BatchServicesLogPM> GetBatchServicesLogsPMsByTenant()
        {
            IQueryable<BatchServicesLogPM> BatchServicesLogs = from a in repository.GetBatchServicesLogs()
                                             
                                                select new BatchServicesLogPM()
                                                {
                                                    Id = a.Id,
                                                    LastActivity = a.LastActivity,
                                                    BatchServiceCode = a.BatchServiceCode,
                                                    CPU = a.CPU,
                                                    CreateDate = a.CreateDate,
                                                    NumberOfDoneItems = a.NumberOfDoneItems,
                                                    DoneItemsInOneHour = a.DoneItemsInOneHour,
                                                    DoneItemsInOneMinute = a.DoneItemsInOneMinute,
                                                    DoneItemsInFiveMinutes = a.DoneItemsInFiveMinutes,
                                                     
                                                };
            return BatchServicesLogs;
        }

        public IQueryable<BatchServicesLogList> GetIQueryableEntityList(IQueryable<BatchServicesLog> iQueryable)
        {
            IQueryable<BatchServicesLogList> result = from batchServicesLog in iQueryable
                                                        select new BatchServicesLogList()
                                                  {
                                                      Id = batchServicesLog.Id,
                                                      LastActivity = batchServicesLog.LastActivity,
                                                      BatchServiceCode = batchServicesLog.BatchServiceCode,
                                                      CPU = batchServicesLog.CPU,
                                                      CreateDate = batchServicesLog.CreateDate,
                                                      NumberOfDoneItems = batchServicesLog.NumberOfDoneItems,
                                                      DoneItemsInOneHour = batchServicesLog.DoneItemsInOneHour,
                                                      DoneItemsInOneMinute = batchServicesLog.DoneItemsInOneMinute,
                                                      DoneItemsInFiveMinutes = batchServicesLog.DoneItemsInFiveMinutes,

                                                  };
            return result;
        }
    }
}