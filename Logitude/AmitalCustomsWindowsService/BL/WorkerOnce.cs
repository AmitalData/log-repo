using AmitalCustomsWindowsService.Utils;
using CustomsWorkerRole;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
///using CustomsWorkerRole.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.BL
{
    

    public class WorkerOnce<TWorker> : AmitalCustomsWindowsService.BL.IWorkerBaseWorkOnce
        where TWorker : WorkerEntryPointDoneLog, new()
    {


        private double _intervalInSec;
        private int _id;
        protected  TWorker _TWorker;
        private DateTime _LastReprtAt = DateTime.MinValue;
        public WorkerOnce(double interval, int id, bool debugMode = false, Object DebugObject=null ,int? Tenant=null)
        {

            if (LogitudeSettings.IsCostomsDeploy)
            {
                string testFormat = DateTime.Now.ToString();

                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;


                testFormat = DateTime.Now.ToString();
            }

            _TWorker =  new TWorker();
            _TWorker.DebugMode = debugMode;
            _TWorker.DebugObject = DebugObject;
            _TWorker.Tenant = Tenant;
            _TWorker.ThreadId = ///_TWorker.GetHashCode().ToString(); //
            Guid.NewGuid().ToString();
            _TWorker.BatchServiceCode = typeof(TWorker).Name;
            _intervalInSec =interval;
            this.MyType = _TWorker.NameOf();
            Simplog.Server.Infrastructure.WebFreightEntryPoint.UsingAzure = true; // For Log -ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role", log);
            
            _id = id;
        }
        ///public abstract void DoIt();
        public void ExecuteTask()
        {
            DateTime lastRunTime = DateTime.MinValue;
            
            while (ServiceStarted)
            {
                // check the current time against the last run plus interval
                var lastRun = ((TimeSpan)(DateTime.UtcNow.Subtract(lastRunTime))).TotalSeconds;
            
                if (lastRun >= _intervalInSec)
                {
                    // if time to do something, do so
                    // exception handling omitted here for simplicity
                    //Logger.LogMe("Multithreaded Service working; id = " + this._id.ToString(), false);
                    try
                    {
                        _TWorker.WorkOnce();
                    }
                    catch (Exception e)
                    {
                        //_TWorker.
                        Logger.LogMe(this.GetType().FullName + ":" + e.ToString(), true);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }


                    // set new run time
                    lastRunTime = DateTime.UtcNow;
                }
                if (_TWorker.DebugMode && (Environment.UserInteractive || this.MyType == "LoadTestWR"))
                {
                    Logger.LogMe("_TWorker.DebugMode && Environment.UserInteractive", false);
                    return;
                }
                if (DateTime.Now.Subtract(_LastReprtAt) > TimeSpan.FromHours(1))
                {
                    _LastReprtAt = DateTime.Now;
                    Logger.LogMe(typeof(TWorker).FullName + ":Still Alive", false);
                }
                Thread.Sleep(TimeSpan.FromSeconds(_intervalInSec));

            }

            Logger.LogMe(typeof(TWorker).FullName + ":ServiceStarted=" + ServiceStarted.ToString() , false);
            Logger.LogMe(typeof(TWorker).FullName + ":ExecuteTask:OUtOUtOUtOUtOUtOUtOUtOUt !!OUt !!", false);
            WhileServiceStarted_IsOut = true;
            if (_TWorker.DebugMode)
            {
                return;
            }
            ///Thread.CurrentThread.Abort();
        }

        public void InvokeStatistics()
        {
            _TWorker.LogStatisticInDB();
        }
        bool _ServiceStarted;
        

        


        public bool FromTesterForm { get; set; }
        public string MyType { get; private set; }

        public int ManagedThreadId
        {
            get
            {
                return _ManagedThreadId;
            }

            set
            {
                _ManagedThreadId = value;
            }
        }

        public bool ServiceStarted
        {
            get
            {
                return _ServiceStarted;
            }

            set
            {
                _ServiceStarted = value;
                //ExecuteTask();
            }
        }

        public bool WhileServiceStarted_IsOut { get; private set; }

        int _ManagedThreadId;
        
    }
}
