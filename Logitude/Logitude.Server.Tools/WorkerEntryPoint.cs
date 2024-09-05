using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Globalization;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Server.Tools
{
    public abstract class WorkerEntryPoint
    {
        public string ThreadName;
        public bool DebugMode;
        public object DebugObject;
        public int? Tenant;
        private static void EnsureHttpRuntime()
        {
            try
            {
                if (null == _httpRuntime)
                {
                    try
                    {
                        //Monitor.Enter(typeof(State));
                        if (null == _httpRuntime)
                        {
                            // Create an Http Content to give us access to the cache.
                            _httpRuntime = new HttpRuntime();

                        }
                    }
                    finally
                    {
                        //Monitor.Exit(typeof(State));
                    }

                }
            }
            catch (Exception e)
            {
                //Logger.LogMe(e.ToString(), true);
            }
        }
        public static Cache Cache
        {
            get
            {
                try
                {
                    WorkerEntryPoint.EnsureHttpRuntime();
                    return HttpRuntime.Cache;
                }
                catch (Exception e)
                {
                    //Logger.LogMe(e.ToString(), true);
                }
                return null;

            }



        }

        public abstract void StartMe();

        public virtual bool OnStart()
        {
            //StartMe();
            if (LogitudeSettings.IsCostomsDeploy)
            {
                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;


            }
            ThreadName = this.GetType().Name;
            return (true);
        }

        /// <summary>
        /// This method prevents unhandled exceptions from being thrown
        /// from the worker thread.
        /// </summary>
        public void ProtectedRun()
        {
            if (!SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
            {
                try
                {

                    Run();
                }
                catch (SystemException e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()1 Method", null);
                    //throw e;

                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()2 Method", null);
                    //throw e;
                }
            }

            else
            {
                Run();
            }
        }
        public abstract void WorkOnce();

        public virtual void Run()
        {
        }

        public virtual void OnStop()
        {
        }

        public static HttpRuntime _httpRuntime { get; set; }
    }




    public abstract class WorkerEntryPointDoneLog : Logitude.Server.Tools.WorkerEntryPoint
    {


        public int NumberOfDoneItems { get; set; }
        public DateTime? LastActivity { get; set; }
        public string ThreadId { get; set; }
        public Dictionary<DateTime, int> DoneItemsInRange { get; set; }
        public string BatchServiceCode { get; set; }
        public decimal CPU { get; set; }
        public string QueueGroupCodeRabbit { get; set; }
        public WorkerQueueType WorkerQueueType { get; set; }

        public string OverrideRMQ { get; set; }
        
        public virtual bool OnStart()
        {
            //StartMe();
            if (LogitudeSettings.IsCostomsDeploy)
            {
                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;


            }
            ThreadName = this.GetType().Name;
            return (true);
        }



        /// <summary>
        /// This method prevents unhandled exceptions from being thrown
        /// from the worker thread.
        /// </summary>



        private int threadID;
        public void LogDoneItemInMemory()
        {
            LogDoneItemInMemory(500);
        }
        public void LogDoneItemInMemory(int sleepInMS)
        {
            try
            {
                threadID = AppDomain.GetCurrentThreadId();
                NumberOfDoneItems++;
                DateTime doneDate = //new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                                    // WHY NOT utc AS writing to db ????
                new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);
                DoneItemsInRange = DoneItemsInRange ?? new Dictionary<DateTime, int>();
                if (DoneItemsInRange.Keys.Contains(doneDate))
                {
                    DoneItemsInRange[doneDate]++;
                }
                else
                {
                    DoneItemsInRange.Add(doneDate, 1);
                }
                var donotwait500ms = LogitudeSettings.IsCostomsDeploy;//yaron !!
                if (donotwait500ms)///Thread.Sleep(500);
                {
                    return;
                }
                System.Diagnostics.ProcessThreadCollection tx = default(System.Diagnostics.ProcessThreadCollection);
                Int16 t = default(Int16);
                Int16 tId = default(Int16);
                double CPUtimeEnd = 0;
                double CPUtimeStart = 0;

                tx = System.Diagnostics.Process.GetCurrentProcess().Threads;

                tId = -1;

                for (t = 0; t <= tx.Count - 1; t++)
                {
                    if (tx[t].Id == threadID)
                    {
                        tId = t;
                    }
                }
                if (tId != -1)
                {
                    CPUtimeStart = tx[tId].TotalProcessorTime.Milliseconds;
                    //Thread.Sleep(500);
                    Thread.Sleep(sleepInMS);
                    CPUtimeEnd = tx[tId].TotalProcessorTime.Milliseconds;

                    if ((CPUtimeEnd > CPUtimeStart) | (CPUtimeEnd == CPUtimeStart))
                    {
                        CPU = (long)(CPUtimeEnd - CPUtimeStart);
                        CPU = CPU / 5;
                        if (CPU > 100)
                            CPU = 100;
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Batch services logging....Mohammad.", null, null);
            }
        }


        public void LogStatisticInDB()
        {
            WorkerEntryPointDoneLog worker = this;
            if (!string.IsNullOrEmpty(worker.ThreadId))
            {
                BatchServiceLogParams logParams = new BatchServiceLogParams()
                {
                    BatchServiceCode = worker.BatchServiceCode,
                    CreateDate = DateTime.UtcNow,
                    Id = worker.ThreadId,
                    LastActivity = worker.LastActivity,
                    NumberOfDoneItems = worker.NumberOfDoneItems,
                    CPU = worker.CPU
                };

                if (worker.DoneItemsInRange != null)
                {
                    DateTime currentDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);

                    int doneItemsInOneMinute = worker.DoneItemsInRange.Where(d => d.Key <= currentDate && d.Key > currentDate.AddMinutes(-1)).Sum(d => d.Value);
                    int doneItemsInFiveMinutes = worker.DoneItemsInRange.Where(d => d.Key <= currentDate && d.Key > currentDate.AddMinutes(-5)).Sum(d => d.Value);
                    int doneItemsInOneHour = worker.DoneItemsInRange.Where(d => d.Key <= currentDate && d.Key > currentDate.AddMinutes(-60)).Sum(d => d.Value);

                    logParams.DoneItemsInFiveMinutes = doneItemsInFiveMinutes;
                    logParams.DoneItemsInOneHour = doneItemsInOneHour;
                    logParams.DoneItemsInOneMinute = doneItemsInOneMinute;
                }

                SystemLogs.BatchServicesLogger.Log(logParams);
            }
        }
    }

    public static class WorkerRoleServiceLocator
    {
        public static bool PleaseShutDown { get; set; }
        public static bool HaveCourierTenant { get; set; }
    }
    public enum WorkerQueueType
    {
        DB = 0,
        RabbitMQ = 1
    }

}
