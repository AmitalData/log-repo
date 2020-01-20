using CommunicationWorkerRole.Tasks;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.Helpers;


namespace CommunicationWorkerRole
{
    class SchedularWorkerRole : WorkerEntryPoint
    {
        DbQueueService queueservice;
        private List<Thread> TasksThreads = new List<Thread>();
        int Tenant;
        public SchedularWorkerRole()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
        }
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "SchedularWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            CheckandRescheduleMissingTasks();
            StartThreadAliveTesterThread();
            ConnectClient();
            return base.OnStart();
        }

        private void StartThreadAliveTesterThread()
        {
            //Thread thread = new Thread(CheckandRescheduleDeadThreads);
            System.Timers.Timer timer1 = new System.Timers.Timer()
            {
                Interval = 60000//5 Mins
            };
            timer1.Enabled = true;
            timer1.Elapsed += Timer1_Elapsed; //+= new System.EventHandler(OnTimerEvent);
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckandRescheduleDeadThreads();
        }

        private void CheckandRescheduleDeadThreads()
        {
            var objectContext = WebFreightContext.GetContext(0);
            TasksSchedulerRepository TasksSchedulerRepository = new TasksSchedulerRepository(objectContext);
            TasksSchedulerQuery TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
            List<TasksSchedulerPM> InprogressTasks = TasksSchedulerQuery.GetAllInprogressTasksSchedulerPMs();
            foreach (var Task in InprogressTasks)
            {
                //var x = Process.GetCurrentProcess().Threads;
                var TaskThread = TasksThreads.Where(a => a.Name == Task.Name).FirstOrDefault();
                if (TaskThread == null || !TaskThread.IsAlive)
                {
                    //TasksSchedulerService service = new TasksSchedulerService(objectContext, Tenant);
                    Task.Status = null;
                    Task.Version = Task.Version + 1;
                    SchedulerHelper SchedulerHelper = new SchedulerHelper();
                    SchedulerHelper.AddSchedulerQueue(Task);
                    var Msg = "The Task " + Task.Name + " Stopped abnormally and reschedualed to start again on " + Task.NextRunTime;
                    LogInfoToDB(Msg, Task);
                    //service.Update(Task);
                }

            }
            //Thread.Sleep(new TimeSpan(0, 1, 0));
        }

        public void LogInfoToDB(string Message, TasksSchedulerPM Task)
        {
            var Tenant = Task.Tenant;
            if (!string.IsNullOrEmpty(Message))
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(Tenant);
                TaskSchedulerHistoryService TaskSchedulerHistoryService = new TaskSchedulerHistoryService(objectContext, Tenant);
                //SchedulerLogsService SchedulerLogsService = new SchedulerLogsService(objectContext, Tenant);
                TaskSchedulerHistoryQuery TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(Tenant);
                //SchedulerLogsQuery SchedulerLogsQuery = new SchedulerLogsQuery(Tenant);
                TaskSchedulerHistoryPM TaskSchedulerHistory = new TaskSchedulerHistoryPM() { Tenant = Tenant, TaskId = Task.Id };
                TaskSchedulerHistory.EndDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                TaskSchedulerHistory.StartDateTime = TenantServerConfigration.GetCurrentDateTime(TaskSchedulerHistory.Tenant);
                TaskSchedulerHistory.EndDateTimeUTC = DateTime.UtcNow;
                TaskSchedulerHistory.StartDateTimeUTC = DateTime.UtcNow;

                //var TaskSchedulerHistory = TaskSchedulerHistoryQuery.GetLastTaskSchedulerHistoryPM(TaskId);
                //if (TaskSchedulerHistory != null)
                //{

                TaskSchedulerHistory.LogType = "Warning";
                TaskSchedulerHistory.RunResult = "Warning";
                TaskSchedulerHistory.LogFirstLine = Message;
                TaskSchedulerHistoryService.Create(TaskSchedulerHistory);

                StringBuilder MyFinalLog = new StringBuilder();
                MyFinalLog.AppendLine(Message.ToString());
                //SchedulerLogsPM SchedulerLog = SchedulerLogsQuery.GetSchedulerLogsByHistory(TaskSchedulerHistory.Id);
                //if (SchedulerLog == null)
                //{
                //    SchedulerLog = new SchedulerLogsPM() { Tenant = Tenant, HistoryId = TaskSchedulerHistory.Id };
                //    SchedulerLog.CreateDate = TenantServerConfigration.GetCurrentDateTime(SchedulerLog.Tenant);
                //    //SchedulerLog.Log = StringHelper.TruncateLongString(MyFinalLog.ToString(), 4000);
                //    SchedulerLogsService.Create(SchedulerLog);
                //}
                //else
                //{
                //    //SchedulerLog.Log += StringHelper.TruncateLongString(Environment.NewLine + MyFinalLog.ToString(), 4000);
                //    SchedulerLogsService.Update(SchedulerLog);
                //}


                TaskManagerBase.AppendLogMessageToFile(TaskSchedulerHistory,  Environment.NewLine + Message);

                //}
            }
        }

        private void CheckandRescheduleMissingTasks()
        {
            TasksSchedulerRepository TasksSchedulerRepository = new TasksSchedulerRepository(0);
            TasksSchedulerQuery TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
            List<TasksSchedulerPM> InprogressTasks = TasksSchedulerQuery.GetAllInprogressTasksSchedulerPMs();
            foreach (var Task in InprogressTasks)
            {
                Task.Status = null;
                Task.Version = Task.Version + 1;
                SchedulerHelper SchedulerHelper = new SchedulerHelper();
                SchedulerHelper.AddSchedulerQueue(Task);
                var Msg = "The Task " + Task.Name + " Stopped abnormally and reschedualed to start again on " + Task.NextRunTime;
                LogInfoToDB(Msg, Task);

            }
        }

        public override void Run()
        {
            while (IsRunning)
            {

                if (!General.IsUpdating())
                {
                    ReceiveOnce();

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public void ReceiveOnce()
        {
            try
            {
                Tenant = 0;
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("SchedularQueue", Tenant);
                var message = queueservice.Receive();
                LastActivity = DateTime.UtcNow;
                if (message != null && message.MessageValues != null)
                {

                    try
                    {
                        string Id = message.MessageValues["TaskId"].ToString();
                        Tenant = int.Parse(message.MessageValues["Tenant"]);
                        int Version = int.Parse(message.MessageValues.ContainsKey("Version") ? message.MessageValues["Version"].ToString() : "0");
                        int Retries = int.Parse(message.MessageValues.ContainsKey("Retries") ? message.MessageValues["Retries"].ToString() : "0");
                        if (!string.IsNullOrEmpty(Id))
                        {
                            var objectContext = WebFreightContext.GetContext(Tenant);
                            TasksSchedulerRepository TasksSchedulerRepository = new TasksSchedulerRepository(objectContext);
                            TasksSchedulerService service = new TasksSchedulerService(objectContext, Tenant);
                            TasksSchedulerQuery TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
                            TasksSchedulerPM Task = TasksSchedulerQuery.GetSingleTasksSchedulerPM(Id);
                            Task.Retries = Retries;
                            Task.LastRunStartTime = TenantServerConfigration.GetCurrentDateTime(Task.Tenant);
                            Task.LastRunStartTimeUTC = DateTime.UtcNow;
                            if (Task != null)
                            {
                                if (Task.InActive)
                                {
                                    queueservice.Complete();
                                }
                                else
                                {
                                    if (Version >= Task.Version)
                                    {
                                        List<object> args = new List<object>();
                                        if (!string.IsNullOrEmpty(Task.Id))
                                        {
                                            args.Add(Task.Id);
                                        }
                                        args.Add(Task.Tenant);


                                        object[] ArrArgs = args.ToArray();
                                        var WRItem = System.Activator.CreateInstance(Type.GetType("CommunicationWorkerRole.Tasks." + Task.ProcedureCode), ArrArgs) as TaskManagerBase;
                                        Task.Status = "In progress";
                                        WRItem.Task = Task;
                                        WRItem.queueservice = queueservice;
                                        WRItem.RetryNumber = message.RetryNumber;
                                        WRItem.MessageId = message.MessageId;
                                        Thread thread = new Thread(WRItem.Run) { Name = Task.Name };
                                        //Task.Status = "In progress";
                                        service.Update(Task);
                                        var CurThread = TasksThreads.Where(a => a.Name == Task.Name).FirstOrDefault();
                                        if (CurThread != null)
                                        {
                                            TasksThreads.Remove(CurThread);
                                        }
                                        thread.Start();
                                        TasksThreads.Add(thread);
                                        //queueservice.Complete();
                                        //AddSchedulerQueue(Task);// need to be Moved
                                    }

                                    queueservice.Complete();
                                }



                            }


                            //queueservice.Complete();

                            // Add New Queue for the executed WR
                        }

                        //queueservice.Complete();
                        LogDoneItemInMemory();
                    }
                    catch (Exception ex)
                    {

                        ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);
                        queueservice.CompleteAsFailed();
                    }

                }
            }
            catch (Exception ex)
            {

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Schedular worker role start", null, null);
                Thread.Sleep(10000);
            }
        }

              

        //private void AddSchedulerQueue(TasksSchedulerPM task)
        //{
        //    var queueservice = new DbQueueService();
        //    bool RunTaskImmediately = false;
        //    if (task.NextRunTime < DateTime.Now)
        //    {
        //        RunTaskImmediately = true;
        //        var NewNextRunTime = new DateTime(task.NextRunTime.Value.Year, task.NextRunTime.Value.Month, DateTime.Now.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
        //        var NewNextRunTimeUTC = new DateTime(task.NextRunTimeUTC.Value.Year, task.NextRunTimeUTC.Value.Month, DateTime.Now.Day, task.NextRunTimeUTC.Value.Hour, task.NextRunTimeUTC.Value.Minute, task.NextRunTimeUTC.Value.Second);
        //        task.NextRunTime = NewNextRunTime;
        //        task.NextRunTimeUTC = NewNextRunTimeUTC;
        //        //task.NextRunTime = DateTime.Now.Date;
        //        //task.NextRunTimeUTC = DateTime.UtcNow;
        //    }
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
        //                NextRunTime = Next(DateTime.Now, ToDay);
        //                task.NextRunTime = NextRunTime;
        //                if (task.Sunday)
        //                {
        //                    NextRunTime = Next(DateTime.Now, DayOfWeek.Sunday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Monday)
        //                {
        //                    NextRunTime = Next(DateTime.Now, DayOfWeek.Monday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Tuesday)
        //                {
        //                    NextRunTime = Next(DateTime.Now, DayOfWeek.Tuesday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Wednesday)
        //                {
        //                    NextRunTime = Next(DateTime.Now, DayOfWeek.Wednesday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Thursday)
        //                {
        //                    NextRunTime = Next(DateTime.Now, DayOfWeek.Thursday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Friday)
        //                {
        //                    NextRunTime = Next(DateTime.Now, DayOfWeek.Friday);
        //                    if (NextRunTime < task.NextRunTime)
        //                    {
        //                        task.NextRunTime = NextRunTime;
        //                    }
        //                }
        //                if (task.Satarday)
        //                {
        //                    NextRunTime = Next(DateTime.Now, DayOfWeek.Saturday);
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
        //        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, DateTime.Now);
        //        //queueservice.InitializeQueue("SchedularQueue", 0);
        //        //queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTimeUTC);

        //    }

        //    var objectContext = WebFreightContext.GetContext(task.Tenant);
        //    TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
        //    service.Update(task);

        //    //queueservice.Complete();
        //}
        //private DateTime Next(DateTime from, DayOfWeek dayOfWeek)
        //{
        //    int start = (int)from.DayOfWeek;
        //    int target = (int)dayOfWeek;
        //    if (target <= start)
        //        target += 7;
        //    return from.AddDays(target - start);
        //}
        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("SchedularQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Schedular Worker Role start", null, null);
            }
        }
    }



    /// <summary>
    /// Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('CustomsSchedularWR','CustomsSchedularWR');
    //  Insert into BATCHSERVICESDEFINITIONMODS(CODE, INACTIVE, NUMBEROFTHREADS) values('CustomsSchedularWR',0,1);
    /// </summary>
    public class CustomsSchedularWR
    : Logitude.Server.Tools.WorkerEntryPointDoneLog
    {
        SchedularWorkerRole _SchedularWorkerRole;
        public CustomsSchedularWR()
        {
            _SchedularWorkerRole = new SchedularWorkerRole();
        }
        public override void StartMe()
        {
            
        }

        bool _Start = false;
        public override void WorkOnce()
        {
            if (!_Start)
            {
                _SchedularWorkerRole.OnStart();
                _Start = true;
            }
            _SchedularWorkerRole.ReceiveOnce();


        }
    }
}
