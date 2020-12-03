using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
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
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole.Tasks
{
    public class TaskManagerBase
    {
        public TasksSchedulerPM Task { get; set; }
        public DbQueueService queueservice { get; set; }
        public int RetryNumber { get; set; }
        private StringBuilder Infos { get; set; }
        private StringBuilder Warnings { get; set; }
        private StringBuilder Exceptions { get; set; }
        public string MessageId { get; set; }
        string TaskId;
        string TaskHistoryId;
        int Tenant;
        TaskSchedulerHistoryPM TaskSchedulerHistory;
        ConcurrentQueueService<LogQueueMessage> queueService = new ConcurrentQueueService<LogQueueMessage>("LogMessagesQueue");
         public TaskManagerBase(string Id, int tenant)
        {
            TaskId = Id;
            Tenant = tenant;
            this.Infos = new StringBuilder();
            this.Warnings = new StringBuilder();
            this.Exceptions = new StringBuilder();
        }

        private void AddFileLogQueueMessage(string message)
        {
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(TaskSchedulerHistory.LogDocumentId))
            {
                queueService.Enqueue(new LogQueueMessage() { FileName = TaskSchedulerHistory.LogDocumentId, FileExtension = "txt", FolderName = "taskmanagerlogs", Message = message, Tenant = Task.Tenant });
            }
        }

        public static void AppendLogMessageToFile(TaskSchedulerHistoryPM taskSchedulerHistory, string message)
        {
            ConcurrentQueueService<LogQueueMessage> queueService = new ConcurrentQueueService<LogQueueMessage>("LogMessagesQueue");
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(taskSchedulerHistory.LogDocumentId))
            {
                queueService.Enqueue(new LogQueueMessage() { FileName = taskSchedulerHistory.LogDocumentId, FileExtension = "txt", FolderName = "taskmanagerlogs", Message = message, Tenant = taskSchedulerHistory.Tenant });
            }
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

                    TaskSchedulerHistory = new TaskSchedulerHistoryPM() { Tenant = Tenant, TaskId = TaskId };
                    TaskSchedulerHistoryService.Create(TaskSchedulerHistory);
                    TaskHistoryId = TaskSchedulerHistory.Id;
                    scope.Complete();
                }
                StartTask();
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    //TaskSchedulerHistoryRepository TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(Tenant);


                    
                    Task.Status = null;
                    //queueservice.Complete();
                    TaskSchedulerHistoryPM LastExecutionHistory = SubmitLogsData();
                    Task.LastRunEndTime = LastExecutionHistory.EndDateTime;
                    Task.LastRunEndTimeUTC = LastExecutionHistory.EndDateTimeUTC;
                    Task.LastRunResult = LastExecutionHistory.LogType;
                    SchedulerHelper SchedulerHelper = new SchedulerHelper();
                    SchedulerHelper.AddSchedulerQueue(Task);

                    scope.Complete();
                }
            }
            catch (ThreadAbortException e)
            {
                //LogInfoToDB("After Aborting the thread ..");
                //Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                #region Exception handling
                try
                {
                    if (RetryNumber <= 1)
                    {
                        queueservice.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 30), MessageId);
                        //Task.Retries++;
                        //ReScheduleFaildTask(Task,5);
                    }

                    if (RetryNumber > 1 && RetryNumber <= 2)
                    {
                        queueservice.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 1, 0), MessageId);
                        //Task.Retries++;
                        //ReScheduleFaildTask(Task, 10);
                    }
                    if (RetryNumber >= 3)
                    {
                        //Task.Retries = 0;
                        //Task.Status = null;
                        queueservice.CompleteAsFailed();
                        SchedulerHelper SchedulerHelper = new SchedulerHelper();
                        SchedulerHelper.AddSchedulerQueue(Task);
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "", null);
                    }
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        string errorMessage = ex.Message + Environment.NewLine;
                        if (ex.InnerException != null)
                        {
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

        private TaskSchedulerHistoryPM SubmitLogsData()
        {
            IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
            TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
            //SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, Tenant);
            TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(Tenant);
            //SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(Tenant);
            var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
            if (TaskSchedulerHistory != null)
            {
                if (!string.IsNullOrEmpty(Exceptions.ToString()))
                {
                    TaskSchedulerHistory.LogType = "Exception";
                    TaskSchedulerHistory.IsError = true;
                    TaskSchedulerHistory.RunResult = "Exception";
                    TaskSchedulerHistory.LogFirstLine = StringHelper.TruncateLongString(Exceptions.ToString(), 1000);
                }
                else if (!string.IsNullOrEmpty(Warnings.ToString()))
                {
                    TaskSchedulerHistory.LogType = "Warning";
                    TaskSchedulerHistory.RunResult = "Warning";
                    TaskSchedulerHistory.LogFirstLine = StringHelper.TruncateLongString(Warnings.ToString(), 1000);
                }
                else
                {
                    TaskSchedulerHistory.LogType = "Info";
                    TaskSchedulerHistory.RunResult = "Succeeded";
                    TaskSchedulerHistory.LogFirstLine = StringHelper.TruncateLongString(Infos.ToString(), 1000);
                }
                StringBuilder MyFinalLog = new StringBuilder();
                MyFinalLog.AppendLine(Exceptions.ToString());
                MyFinalLog.AppendLine(Warnings.ToString());
                MyFinalLog.AppendLine(Infos.ToString());
                //SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(TaskHistoryId);
                //if (SchedulerLog == null)
                //{
                //    SchedulerLog = new SchedulerLogsPM() { Tenant = Tenant, HistoryId = TaskHistoryId };
                //    SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
                //    //SchedulerLog.Log = StringHelper.TruncateLongString(MyFinalLog.ToString(), 4000);
                //    SchedulerLogsService.Create(SchedulerLog);
                //}
                //else
                //{
                //    //SchedulerLog.Log += Environment.NewLine + MyFinalLog.ToString();
                //    //SchedulerLog.Log = StringHelper.TruncateLongString(SchedulerLog.Log, 4000);
                //    //SchedulerLogsService.Update(SchedulerLog);
                //}


                TaskSchedulerHistory.EndDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                TaskSchedulerHistory.EndDateTimeUTC = DateTime.UtcNow;
                TaskSchedulerHistoryService.Update(TaskSchedulerHistory);

            }
            return TaskSchedulerHistory;
        }

        public virtual void StartTask()
        {

        }

        public void LogInfoToDB(string Message)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
                //SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, Tenant);
                TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(Tenant);
                //SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(Tenant);
                var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPM(TaskHistoryId);
                if (TaskSchedulerHistory != null)
                {

                    TaskSchedulerHistory.LogType = "Info";
                    TaskSchedulerHistory.RunResult = "Succeeded";
                    TaskSchedulerHistory.LogFirstLine = StringHelper.TruncateLongString(Message.ToString(), 1000);

                    StringBuilder MyFinalLog = new StringBuilder(); 
                    MyFinalLog.AppendLine(Message.ToString());
                    //SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(TaskHistoryId);
                    //if (SchedulerLog == null)
                    //{
                    //    SchedulerLog = new SchedulerLogsPM() { Tenant = Tenant, HistoryId = TaskHistoryId };
                    //    SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
                    //    //SchedulerLog.Log = StringHelper.TruncateLongString(MyFinalLog.ToString(), 4000);
                    //    SchedulerLogsService.Create(SchedulerLog);
                    //}
                    //else
                    //{
                    //    //SchedulerLog.Log += Environment.NewLine + MyFinalLog.ToString();
                    //    //SchedulerLog.Log = StringHelper.TruncateLongString(SchedulerLog.Log, 4000);
                    //    //SchedulerLogsService.Update(SchedulerLog);
                    //}


                    TaskSchedulerHistory.EndDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                    TaskSchedulerHistory.EndDateTimeUTC = DateTime.UtcNow;
                    TaskSchedulerHistoryService.Update(TaskSchedulerHistory);

                }
            }
        }


        public void LogInfo(string Message)
        {
            if (!string.IsNullOrEmpty(Message) && this.Infos.Length < 1000)
                this.Infos.AppendLine(Message);

            this.AddFileLogQueueMessage(Message);
        }


        public void LogWarning(string Message)
        {
            if (!string.IsNullOrEmpty(Message) && this.Warnings.Length < 1000)
                this.Warnings.AppendLine(Message);

            this.AddFileLogQueueMessage(Message);

        }

        public void LogException(string Message)
        {
            if (!string.IsNullOrEmpty(Message) && this.Warnings.Length < 1000)
                this.Exceptions.AppendLine(Message);
         
                this.AddFileLogQueueMessage(Environment.NewLine + Message);
        }

        //private void AddSchedulerQueue(TasksSchedulerPM task)
        //{
        //    var queueservice = new DbQueueService();
        //    if (task.NextRunTime < DateTime.Now)
        //    {
        //        var NewNextRunTime = new DateTime(task.NextRunTime.Value.Year, task.NextRunTime.Value.Month, DateTime.Now.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
        //        var NewNextRunTimeUTC = new DateTime(task.NextRunTimeUTC.Value.Year, task.NextRunTimeUTC.Value.Month, DateTime.Now.Day, task.NextRunTimeUTC.Value.Hour, task.NextRunTimeUTC.Value.Minute, task.NextRunTimeUTC.Value.Second);
        //        task.NextRunTime = NewNextRunTime;
        //        task.NextRunTimeUTC = NewNextRunTimeUTC;
        //        //task.NextRunTime = DateTime.Now;
        //        //task.NextRunTimeUTC = DateTime.UtcNow;
        //    }
        //    var TodayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
        //    switch (task.TriggerType)
        //    {
        //        case "D":
        //            {
        //                if (task.RepeatInMinutes != null && task.RepeatInMinutes > 0)
        //                {
        //                    task.NextRunTime = task.NextRunTime.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
        //                    task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
        //                }
        //                else
        //                {
        //                    task.NextRunTime = task.NextRunTime.Value.AddDays(1);
        //                    task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddDays(1);
        //                }
        //                break;
        //            }
        //        case "W":
        //            {
        //                DateTime NextRunTime;
        //                var ToDay = DateTime.Now.DayOfWeek;
        //                var ToDayString = DateTime.Now.DayOfWeek.ToString();
        //                NextRunTime = Next(TodayDate, ToDay);
        //                task.NextRunTime = NextRunTime;
        //                if (task.Sunday)
        //                {
        //                    NextRunTime = Next(TodayDate, DayOfWeek.Sunday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Monday)
        //                {
        //                    NextRunTime = Next(TodayDate, DayOfWeek.Monday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Tuesday)
        //                {
        //                    NextRunTime = Next(TodayDate, DayOfWeek.Tuesday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Wednesday)
        //                {
        //                    NextRunTime = Next(TodayDate, DayOfWeek.Wednesday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Thursday)
        //                {
        //                    NextRunTime = Next(TodayDate, DayOfWeek.Thursday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Friday)
        //                {
        //                    NextRunTime = Next(TodayDate, DayOfWeek.Friday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Satarday)
        //                {
        //                    NextRunTime = Next(TodayDate, DayOfWeek.Saturday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                //queueservice.InitializeQueue("SchedularQueue", 0);
        //                //queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);
        //                break;
        //            }
        //        case "M":
        //            {
        //                DateTime NextRunTime;
        //                task.NextRunTime = task.NextRunTime.Value.AddMonths(1);
        //                //queueservice.InitializeQueue("SchedularQueue", 0);
        //                //queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);
        //                break;
        //            }
        //        default: // Once
        //            {
        //                break;
        //            }

        //    }
        //    if (task.TriggerType.ToUpper() != "O")
        //    {
        //        queueservice.InitializeQueue("SchedularQueue", 0);
        //        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);

        //    }

        //    var objectContext = WebFreightContext.GetContext(task.Tenant);
        //    TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
        //    service.Update(task);

        //    //queueservice.Complete();
        //}

        private void ReScheduleFaildTask(TasksSchedulerPM task, int DelaySeconds)
        {
            var queueservice = new DbQueueService();
            var NextRunTime = DateTime.Now.AddSeconds(DelaySeconds + 0.0);
            queueservice.InitializeQueue("SchedularQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() }, { "Retries", task.Retries.ToString() } }, task.Tenant, null, null, null, NextRunTime);
            //var objectContext = WebFreightContext.GetContext(task.Tenant);
            //TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
            //service.Update(task);
        }
        //private DateTime Next(DateTime from, DayOfWeek dayOfWeek)
        //{
        //    int start = (int)from.DayOfWeek;
        //    int target = (int)dayOfWeek;
        //    if (target <= start)
        //        target += 7;
        //    return from.AddDays(target - start);
        //}
    }
}
