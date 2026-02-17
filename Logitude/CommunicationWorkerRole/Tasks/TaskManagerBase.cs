using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.SystemLogs;
using Simplog.Data.Helpers;
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
            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);
                    IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                    TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);

                    TaskSchedulerHistoryPM TaskSchedulerHistory = new TaskSchedulerHistoryPM() { Tenant = Tenant, TaskId = TaskId };
                    TaskSchedulerHistoryService.Create(TaskSchedulerHistory);
                    TaskHistoryId = TaskSchedulerHistory.Id;
                    scope.Complete();
                }
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);
                    IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                    TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
                    //TaskSchedulerHistoryPM TaskSchedulerHistory = new TaskSchedulerHistoryPM() { Tenant = Tenant, TaskId = TaskId };
                    //TaskSchedulerHistoryService.Create(TaskSchedulerHistory);
                    //TaskHistoryId = TaskSchedulerHistory.Id;

                    StartTask();
                    TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
                    var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                    if (TaskSchedulerHistory != null)
                    {
                        TaskSchedulerHistory.EndDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                        TaskSchedulerHistory.EndDateTimeUTC = DateTime.UtcNow;
                        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);
                        //LogInfo("Done Execution ..");

                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                #region Exception handling
                try
                {
                    //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //    TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);
                    //    TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
                    //    TaskSchedulerHistoryPM TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                    //    if (TaskSchedulerHistory != null)
                    //    {
                    //        TaskSchedulerHistory.EndDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                    //        TaskSchedulerHistory.EndDateTimeUTC = DateTime.UtcNow;
                    //        TaskSchedulerHistory.IsError = true;
                    //        TaskSchedulerHistory.RunResult = "Error " + ex.Message;
                    //        IWebFreightContext objectContext = new WebFreightContext();
                    //        TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
                    //        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);

                    //    }
                    //    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "", null);
                    //    scope.Complete();
                    //}
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "", null);
                }
                catch (Exception exc)
                {
                    ExceptionHandler.HandleException(exc, DateTime.Now, 0, "", "WorkerRole", "", null);
                    //scope.Complete();
                }

                #endregion
            }



        }

        public virtual void StartTask()
        {

        }

        public void LogInfo(string Message)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, Tenant);
                SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(Tenant);
                TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(Tenant);
                var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);

                if (TaskSchedulerHistory != null)
                {
                    SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(TaskHistoryId);
                    if (SchedulerLog == null)
                    {
                        SchedulerLog = new SchedulerLogsPM() { Tenant = Tenant, HistoryId = TaskHistoryId };
                        SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
                        SchedulerLog.Log = Message;
                        SchedulerLogsService.Create(SchedulerLog);
                        TaskSchedulerHistory.LogType = "Info";
                        TaskSchedulerHistory.LogFirstLine = Message;
                        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);
                    }
                    else
                    {
                        SchedulerLog.Log += Environment.NewLine + Message;
                        SchedulerLogsService.Update(SchedulerLog);
                    }
                }
                scope.Complete();
            }
        }

        public void Logwarning(string Message)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, Tenant);
                SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(Tenant);
                TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(Tenant);
                var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);

                if (TaskSchedulerHistory != null)
                {
                    SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(TaskHistoryId);
                    if (SchedulerLog == null)
                    {
                        SchedulerLog = new SchedulerLogsPM() { Tenant = Tenant, HistoryId = TaskHistoryId };
                        SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
                        SchedulerLog.Log = Message;
                        SchedulerLogsService.Create(SchedulerLog);
                        TaskSchedulerHistory.LogType = "Warning";
                        TaskSchedulerHistory.LogFirstLine = Message;
                        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);
                    }
                    else
                    {
                        TaskSchedulerHistory.LogType = "Warning";
                        TaskSchedulerHistory.LogFirstLine = Message;
                        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);
                        SchedulerLog.Log += Environment.NewLine + Message;
                        SchedulerLogsService.Update(SchedulerLog);
                    }
                }
                scope.Complete();
            }

        }

        public void LogException(string Message)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, Tenant);
                SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(Tenant);
                TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(Tenant);
                var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);

                if (TaskSchedulerHistory != null)
                {
                    SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(TaskHistoryId);
                    if (SchedulerLog == null)
                    {
                        SchedulerLog = new SchedulerLogsPM() { Tenant = Tenant, HistoryId = TaskHistoryId };
                        SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
                        SchedulerLog.Log = Message;
                        SchedulerLogsService.Create(SchedulerLog);
                        TaskSchedulerHistory.LogType = "Exception";
                        TaskSchedulerHistory.LogFirstLine = Message;
                        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);
                    }
                    else
                    {
                        TaskSchedulerHistory.LogType = "Exception";
                        TaskSchedulerHistory.LogFirstLine = Message;
                        TaskSchedulerHistoryService.Update(TaskSchedulerHistory);
                        SchedulerLog.Log += Environment.NewLine + Message;
                        SchedulerLogsService.Update(SchedulerLog);
                    }
                }
                scope.Complete();
            }
        }
    }
}
