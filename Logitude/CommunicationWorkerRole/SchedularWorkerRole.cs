using CommunicationWorkerRole.Tasks;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
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

namespace CommunicationWorkerRole
{
    class SchedularWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
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

            ConnectClient();
            return base.OnStart();
        }
        public override void Run()
        {

            while (IsRunning)
            {

                if (!General.IsUpdating())
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
                                if (!string.IsNullOrEmpty(Id))
                                {
                                    TasksSchedulerRepository TasksSchedulerRepository = new TasksSchedulerRepository(Tenant);
                                    TasksSchedulerQuery TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
                                    TasksSchedulerPM Task = TasksSchedulerQuery.GetSingleTasksSchedulerPM(Id);

                                    if (Task != null)
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
                                            var WRItem = System.Activator.CreateInstance(Type.GetType("CommunicationWorkerRole.Tasks." + Task.ServiceClassName), ArrArgs) as TaskManagerBase;
                                            WRItem.Task = Task;
                                            Thread thread = new Thread(WRItem.Run);
                                            //queueservice.Complete();
                                            thread.Start();
                                            //AddSchedulerQueue(Task);// need to be Moved
                                        }

                                    }

                                   
                                    //queueservice.Complete();

                                    // Add New Queue for the executed WR
                                }

                                queueservice.Complete();
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
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }
        private void AddSchedulerQueue(TasksSchedulerPM task)
        {
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
                        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() } }, null, null, null, task.NextRunTime);
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
                        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() } }, null, null, null, task.NextRunTime);
                        break;
                    }
                case "M":
                    {
                        DateTime NextRunTime;
                        task.NextRunTime = task.NextRunTime.Value.AddMonths(1);
                        queueservice.InitializeQueue("SchedularQueue", 0);
                        queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() } }, null, null, null, task.NextRunTime);
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
}
