using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole.Tasks
{
    public class TaskManagerBase
    {
        public TasksSchedulerPM Task { get; set; }
        string TaskId;
        string TaskHistoryId;
        int Tenant;
        public TaskManagerBase(string Id, int tenant)
        {
            TaskId = Id;
            Tenant = tenant;
        }
        public void Run()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                try
                {
                    TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);
                    IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                    TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
                    TaskSchedulerHistoryPM TaskSchedulerHistory = new TaskSchedulerHistoryPM() { Tenant = Tenant, TaskId = TaskId };
                    TaskSchedulerHistoryService.Create(TaskSchedulerHistory);
                    TaskHistoryId = TaskSchedulerHistory.Id;
                    StartTask();
                    TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
                    TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                    if (TaskSchedulerHistory != null)
                    {
                        TaskSchedulerHistory.EndDateTime = DateTime.Now;
                        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);

                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    #region Exception handling
                    try
                    {
                        TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);
                        TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
                        TaskSchedulerHistoryPM TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                        if (TaskSchedulerHistory != null)
                        {
                            TaskSchedulerHistory.EndDateTime = DateTime.Now;
                            TaskSchedulerHistory.IsError = true;
                            TaskSchedulerHistory.RunResult = "Error " + ex.Message;
                            IWebFreightContext objectContext = new WebFreightContext();
                            TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
                            TaskSchedulerHistoryService.Update(TaskSchedulerHistory);

                        }
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "", null);
                        scope.Complete();
                    }
                    catch (Exception exc)
                    {
                        ExceptionHandler.HandleException(exc, DateTime.Now, 0, "", "WorkerRole", "", null);
                        scope.Complete();
                    }
                  
                    #endregion
                }

            }

        }

        public virtual void StartTask()
        {

        }
    }
}
