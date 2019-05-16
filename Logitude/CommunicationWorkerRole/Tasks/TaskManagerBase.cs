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
        private StringBuilder Infos { get; set; }
        private StringBuilder Warnings { get; set; }
        private StringBuilder Exceptions { get; set; }
        string TaskId;
        string TaskHistoryId;
        int Tenant;
        public TaskManagerBase(string Id, int tenant)
        {
            TaskId = Id;
            Tenant = tenant;
            this.Infos = new StringBuilder();
            this.Warnings = new StringBuilder();
            this.Exceptions = new StringBuilder();
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
                StartTask();
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    //TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);
                    
                    

                    Task.Status = null;
                    AddSchedulerQueue(Task);
                    SubmitLogsData();


                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                #region Exception handling
                try
                {
                    if (Task.Retries <= 1)
                    {
                        Task.Retries++;
                        ReScheduleFaildTask(Task,5);
                    }

                    if (Task.Retries > 1 && Task.Retries <= 2)
                    {
                        Task.Retries++;
                        ReScheduleFaildTask(Task, 10);
                    }
                    if (Task.Retries >= 3)
                    {
                        Task.Retries = 0;
                        AddSchedulerQueue(Task);
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "", null);
                    }
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        string errorMessage = ex.Message + Environment.NewLine; 
                        if (ex.InnerException != null) { 
                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                        } 
                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                        LogException(errorMessage);
                        SubmitLogsData();
                        //TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);
                        //TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
                        //TaskSchedulerHistoryPM TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                        //if (TaskSchedulerHistory != null)
                        //{
                        //    TaskSchedulerHistory.EndDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                        //    TaskSchedulerHistory.EndDateTimeUTC = DateTime.UtcNow;
                        //    TaskSchedulerHistory.IsError = true;
                        //    TaskSchedulerHistory.RunResult = "Error " + ex.Message;
                        //    IWebFreightContext objectContext = new WebFreightContext();
                        //    TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
                        //    TaskSchedulerHistoryService.Update(TaskSchedulerHistory);

                        //}
                        //ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "", null);
                        scope.Complete();
                    }
                    
                }
                catch (Exception exc)
                {
                    ExceptionHandler.HandleException(exc, DateTime.Now, 0, "", "WorkerRole", "", null);
                    //scope.Complete();
                }

                #endregion
            }
        }

        private void SubmitLogsData()
        {
            IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
            TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
            SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, Tenant);
            TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(Tenant);
            SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(Tenant);
            var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
            if (TaskSchedulerHistory != null)
            {
                if (!string.IsNullOrEmpty(Exceptions.ToString()))
                {
                    TaskSchedulerHistory.LogType = "Exception";
                    TaskSchedulerHistory.IsError = true;
                    TaskSchedulerHistory.RunResult = "Exception";
                    TaskSchedulerHistory.LogFirstLine = Exceptions.ToString();
                }
                else if (!string.IsNullOrEmpty(Warnings.ToString()))
                {
                    TaskSchedulerHistory.LogType = "Warning";
                    TaskSchedulerHistory.RunResult = "Warning";
                    TaskSchedulerHistory.LogFirstLine = Warnings.ToString();
                }
                else
                {
                    TaskSchedulerHistory.LogType = "Info";
                    TaskSchedulerHistory.RunResult = "Succeeded";
                    TaskSchedulerHistory.LogFirstLine = Infos.ToString();
                }
                StringBuilder MyFinalLog = new StringBuilder();
                MyFinalLog.AppendLine(Exceptions.ToString());
                MyFinalLog.AppendLine(Warnings.ToString());
                MyFinalLog.AppendLine(Infos.ToString());
                SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(TaskHistoryId);
                if (SchedulerLog == null)
                {
                    SchedulerLog = new SchedulerLogsPM() { Tenant = Tenant, HistoryId = TaskHistoryId };
                    SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
                    SchedulerLog.Log = MyFinalLog.ToString();
                    SchedulerLogsService.Create(SchedulerLog);
                }
                else
                {
                    SchedulerLog.Log += Environment.NewLine + MyFinalLog.ToString();
                    SchedulerLogsService.Update(SchedulerLog);
                }


                TaskSchedulerHistory.EndDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                TaskSchedulerHistory.EndDateTimeUTC = DateTime.UtcNow;
                TaskSchedulerHistoryService.Update(TaskSchedulerHistory);

            }
        }

        public virtual void StartTask()
        {

        }

        public void LogInfo(string Message)
        {
            this.Infos.AppendLine(Message);
        }

        public void Logwarning(string Message)
        {
            this.Warnings.AppendLine(Message);
        }

        public void LogException(string Message)
        {
            this.Exceptions.AppendLine(Message);
        }

        private void AddSchedulerQueue(TasksSchedulerPM task)
        {
            var queueservice = new DbQueueService();
            if (task.NextRunTime < DateTime.Now)
            {
                task.NextRunTime = DateTime.Now;
                task.NextRunTimeUTC = DateTime.UtcNow;
            }
            switch (task.TriggerType)
            {
                case "D":
                    {
                        if (task.RepeatInMinutes != null && task.RepeatInMinutes > 0)
                        {
                            task.NextRunTime = task.NextRunTime.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                            task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0); 
                        }
                        else
                        {
                            task.NextRunTime = task.NextRunTime.Value.AddDays(1);
                            task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddDays(1);
                        } 
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
                        //queueservice.InitializeQueue("SchedularQueue", 0);
                        //queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);
                        break;
                    }
                case "M":
                    {
                        DateTime NextRunTime;
                        task.NextRunTime = task.NextRunTime.Value.AddMonths(1);
                        //queueservice.InitializeQueue("SchedularQueue", 0);
                        //queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);
                        break;
                    }
                default: // Once
                    {
                        break;
                    }
                   
            }
            if (task.TriggerType.ToUpper() != "O")
            {
                queueservice.InitializeQueue("SchedularQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTimeUTC);

            }

            var objectContext = WebFreightContext.GetContext(task.Tenant);
            TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
            service.Update(task);

            queueservice.Complete();
        }

        private void ReScheduleFaildTask(TasksSchedulerPM task,int DelaySeconds)
        {
            var queueservice = new DbQueueService();
            var NextRunTime = DateTime.Now.AddSeconds(DelaySeconds + 0.0);
            queueservice.InitializeQueue("SchedularQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() }, { "Retries", task.Retries.ToString() } }, null, null, null, NextRunTime);
            //var objectContext = WebFreightContext.GetContext(task.Tenant);
            //TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
            //service.Update(task);
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
