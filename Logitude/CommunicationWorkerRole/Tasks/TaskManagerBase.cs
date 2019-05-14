using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.QueueService;
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
                    Task.Status = null;
                    AddSchedulerQueue(Task);
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

        private void AddSchedulerQueue(TasksSchedulerPM task)
        {
            var queueservice = new DbQueueService();
            switch (task.TriggerType)
            {
                case "D":
                    {
                        if (task.RepeatInMinutes != null && task.RepeatInMinutes > 0)
                        {
                            task.NextRunTime = task.NextRunTime.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                        }
                        else
                        {
                            task.NextRunTime = task.NextRunTime.Value.AddDays(1);
                        }
                        queueservice.InitializeQueue("SchedularQueue", 0);
                        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);
                        break;
                    }
                case "W":
                    {
                        DateTime NextRunTime;
                        var ToDay = DateTime.Now.DayOfWeek;
                        var ToDayString = DateTime.Now.DayOfWeek.ToString();
                        NextRunTime = Next(DateTime.Now, ToDay);
                        task.NextRunTime = NextRunTime;
                        if (task.Sunday)
                        {
                            NextRunTime = Next(DateTime.Now, DayOfWeek.Sunday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Monday)
                        {
                            NextRunTime = Next(DateTime.Now, DayOfWeek.Monday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Tuesday)
                        {
                            NextRunTime = Next(DateTime.Now, DayOfWeek.Tuesday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Wednesday)
                        {
                            NextRunTime = Next(DateTime.Now, DayOfWeek.Wednesday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Thursday)
                        {
                            NextRunTime = Next(DateTime.Now, DayOfWeek.Thursday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Friday)
                        {
                            NextRunTime = Next(DateTime.Now, DayOfWeek.Friday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Satarday)
                        {
                            NextRunTime = Next(DateTime.Now, DayOfWeek.Saturday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        queueservice.InitializeQueue("SchedularQueue", 0);
                        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);
                        break;
                    }
                case "M":
                    {
                        DateTime NextRunTime;
                        task.NextRunTime = task.NextRunTime.Value.AddMonths(1);
                        queueservice.InitializeQueue("SchedularQueue", 0);
                        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);
                        break;
                    }
                default: // Once
                    {
                        break;
                    }

            }
            var objectContext = WebFreightContext.GetContext(task.Tenant);
            TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
            service.Update(task);

            queueservice.Complete();
        }

        private DateTime Next(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}
