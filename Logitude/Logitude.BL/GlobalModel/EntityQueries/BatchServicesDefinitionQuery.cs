using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.SystemLogs.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class BatchServicesDefinitionQuery
    {

        BatchServicesDefinitionRepository repository;

        public BatchServicesDefinitionQuery()
        {
            repository = new BatchServicesDefinitionRepository();
        }

        public BatchServicesDefinitionQuery(int tenant)
        {
            repository = new BatchServicesDefinitionRepository();
        }

        public BatchServicesDefinitionQuery(BatchServicesDefinitionRepository BatchServicesDefinitionRepository)
        {
            repository = BatchServicesDefinitionRepository;
        }

        public BatchServicesDefinitionPM GetSingleBatchServicesDefinitionPM(string code)
        {
            return (from a in repository.context.BatchServicesDefinitions
                    where a.Code == code
                    select new BatchServicesDefinitionPM() { Code = a.Code }).FirstOrDefault();
        }

        public BatchServicesDefinitionPM GetSinglePM(string code)
        {
            return (from a in repository.context.BatchServicesDefinitions
                    where a.Code == code
                    select new BatchServicesDefinitionPM() { Code = a.Code }).FirstOrDefault();
        }

        public IQueryable<BatchServicesDefinitionList> GetIQueryableEntityList(IQueryable<BatchServicesDefinition> iQueryable)
        {
            IQueryable<BatchServicesDefinitionList> result = from entity in iQueryable
                                                             select new BatchServicesDefinitionList()
                                                              {

                                                                  Code = entity.Code,
                                                                  //InActive = entity.InActive,
                                                                  //NumberOfThreads = entity.NumberOfThreads,
                                                                  ClassName = entity.ClassName,
                                                                  Parameter1 = entity.Parameter1,
                                                                  Parameter2 = entity.Parameter2,
                                                                  QueueDefinitionCode = entity.QueueDefinitionCode
                                                              };
            return result;
        }

        public IQueryable<BatchServicesDefinitionPM> GetAllActiveBatchServicesDefinitions()
        {

            var result = (from a in repository.context.BatchServicesDefinitions 
                          where a.BatchServicesDefinitionMods.InActive == false && a.Code != "WorkFlowsWR"
                          select new BatchServicesDefinitionPM()
                          {
                              ClassName = a.ClassName,
                              Code = a.Code,
                              InActive = a.BatchServicesDefinitionMods.InActive,
                              NumberOfThreads = a.BatchServicesDefinitionMods.NumberOfThreads,
                              Parameter1 = a.Parameter1,
                              Parameter2 = a.Parameter2,
                              QueueDefinitionCode = a.QueueDefinitionCode
                          }
                          );

            return result;
        }

        public IQueryable<BatchServicesDefinitionPM> GetAllBatchServicesDefinitionsPMs(DateTime? LastActivity, string status)
        {
            bool inactive = false;
            bool filterinactive;
            if (status == "All")
            {
                filterinactive = false;
            }
            if (status == "Active")
            {
                filterinactive = true;
                inactive = false;
            }
            else if (status == "InActive")
            {
                filterinactive = true;
                inactive = true;
            }
            else
            {
                filterinactive = false;
            }

            List<BatchServicesDefinitionPM> TempList = new List<BatchServicesDefinitionPM>();
            var result = (from a in repository.context.BatchServicesDefinitions
                          where filterinactive ? a.BatchServicesDefinitionMods.InActive == inactive : true
                          select new BatchServicesDefinitionPM()
                          {
                              ClassName = a.ClassName,
                              Code = a.Code,
                              InActive = a.BatchServicesDefinitionMods.InActive,
                              NumberOfThreads = a.BatchServicesDefinitionMods.NumberOfThreads,
                              Parameter1 = a.Parameter1,
                              Parameter2 = a.Parameter2,
                              QueueDefinitionCode = a.QueueDefinitionCode
                          }
                          );
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                BatchServicesLogRepository Repo = new BatchServicesLogRepository();

                foreach (var item in result)
                {
                    var Logs = Repo.GetBatchServicesLogsByCode(item.Code, LastActivity);
                    int DoneItemsInFiveMinutes = 0;
                    int DoneItemsInOneHour = 0;
                    int DoneItemsInOneMinute = 0;
                    int NumberOfDoneItems = 0;
                    DateTime? LActivity = null;// new DateTime(1990, 1, 1);
                    foreach (var Log in Logs)
                    {
                        item.CPU = Log.CPU;
                        DoneItemsInFiveMinutes += Log.DoneItemsInFiveMinutes;
                        DoneItemsInOneHour += Log.DoneItemsInOneHour;
                        DoneItemsInOneMinute += Log.DoneItemsInOneMinute;
                        NumberOfDoneItems += Log.NumberOfDoneItems != null ? (int)Log.NumberOfDoneItems : 0;
                        if (Log.LastActivity > LActivity && LActivity != null)
                        {
                            LActivity = Log.LastActivity;
                        }
                        else if (LActivity == null && Log.LastActivity !=null)
                        {
                            LActivity = Log.LastActivity;
                        }
                    }
                    item.DoneItemsInFiveMinutes = DoneItemsInFiveMinutes;
                    item.DoneItemsInOneHour = DoneItemsInOneHour;
                    item.DoneItemsInOneMinute = DoneItemsInOneMinute;
                    item.NumberOfDoneItems = NumberOfDoneItems;
                    item.LastActivity = LActivity;
                    TempList.Add(item);
                }
                scope.Complete();
            }
            return TempList.AsQueryable();
        }
        public IQueryable<BatchServicesDefinitionList> GetAllBatchServicesDefinitionsList(DateTime? LastActivity)
        {
            List<BatchServicesDefinitionList> TempList = new List<BatchServicesDefinitionList>();
            var result = (from a in repository.context.BatchServicesDefinitions
                          //join b in repository.context.BatchServicesDefinitionMods on a.Code equals b.Code
                          //where b.InActive == false
                          select new BatchServicesDefinitionList()
                          {
                              ClassName = a.ClassName,
                              Code = a.Code,
                              InActive = a.BatchServicesDefinitionMods.InActive,
                              NumberOfThreads = a.BatchServicesDefinitionMods.NumberOfThreads,
                              Parameter1 = a.Parameter1,
                              Parameter2 = a.Parameter2
                          }
                          );
            using (TransactionScope scope = TransactionFactory.GetNewTransactionWithDefaultIsolationLevel())//TransactionFactory.GetNewTransaction())
            {
                BatchServicesLogRepository Repo = new BatchServicesLogRepository();

                foreach (var item in result)
                {
                    var Logs = Repo.GetBatchServicesLogsByCode(item.Code, LastActivity);
                    int DoneItemsInFiveMinutes = 0;
                    int DoneItemsInOneHour = 0;
                    int DoneItemsInOneMinute = 0;
                    int NumberOfDoneItems = 0;
                    foreach (var Log in Logs)
                    {
                        item.CPU = Log.CPU;
                        DoneItemsInFiveMinutes += Log.DoneItemsInFiveMinutes;
                        DoneItemsInOneHour += Log.DoneItemsInOneHour;
                        DoneItemsInOneMinute += Log.DoneItemsInOneMinute;
                        NumberOfDoneItems += Log.NumberOfDoneItems != null ? (int)Log.NumberOfDoneItems : 0;
                    }
                    item.DoneItemsInFiveMinutes = DoneItemsInFiveMinutes;
                    item.DoneItemsInOneHour = DoneItemsInOneHour;
                    item.DoneItemsInOneMinute = DoneItemsInOneMinute;
                    item.NumberOfDoneItems = NumberOfDoneItems;
                    TempList.Add(item);
                }
                scope.Complete();
            }
            return TempList.AsQueryable();
        }
    }
}
