using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace CommunicationWorkerRole
{

    public class WorkerEntryPoint : Logitude.Server.Tools.WorkerEntryPointDoneLog
    {
        /// <summary>
        /// Name of thread used in debugging and logging
        /// </summary>


        //public virtual bool OnStart()
        //{
        //    ThreadName = this.GetType().Name;
        //    return (true);
        //}
        public bool IsRunning = true;
        public int MaxWorkingTimeInMinutes = 0;
        private DateTime StartTime;
        public Thread CurrentThread = null;
        public override void StartMe()
        {
            if (CacheManager.CacheWrapper != null) return;
            WorkerEntryPoint.StartStatic();
        }

        public static void StartStatic()
        {
            CacheManager.CacheWrapper = new CacheWrapper(
            Cache
            );
            ThreadedRoleEntryPoint.StartStatic();

        }


        /// <summary>
        /// This method prevents unhandled exceptions from being thrown
        /// from the worker thread.
        /// </summary>
        internal void ProtectedRun()
        {
            if (LogitudeSettings.DeploymentStage != "Dev")
            {
                try
                {
                    StartCurrentThreadInternalManager();
                    StartTime = DateTime.Now;
                    Run();
                    AsyncRun();
                }
                catch (SystemException e)
                {
                    //
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()1 Method", null);
                    Thread.Sleep(60000);
                    //throw e;

                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()2 Method", null);
                    Thread.Sleep(60000);
                    //throw e;
                }
            }

            else
            {
                Run();
                AsyncRun();
            }
        }

        public virtual void Run()
        {
        }

        public virtual async void AsyncRun()
        {
        }

        public virtual void OnStop()
        {
            this.IsRunning = false;
        }

        public override void WorkOnce()
        {

        }
        void KillThread(Thread currentThread)
        {
            try
            {
                this.IsRunning = false;
                currentThread.Abort();
            }
            catch (Exception e)
            {
                 
            } 
        }
        private System.Timers.Timer CurrentThreadTimer;
        void StartCurrentThreadInternalManager()
        {
            if (CurrentThreadTimer == null)
            {
                this.CurrentThreadTimer = new System.Timers.Timer();

                TimeSpan t = new TimeSpan(0, 0, 30);
                CurrentThreadTimer.Interval = (int)t.TotalMilliseconds;
                CurrentThreadTimer.Stop();
                CurrentThreadTimer.Elapsed += CurrentThreadTimer_Elapsed;
                CurrentThreadTimer.Start();
            }
        }

        private void CurrentThreadTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Thread CurrentThread = RunningThreads.Where(a => a.Name == this.ThreadName).FirstOrDefault();


            if (CurrentThread != null && CurrentThread.IsAlive)
            {
                if (CheckIsMaxWorkingTimeInMinutesExceeded())
                {
                    KillThread(CurrentThread);
                }
            }
        }
        private bool CheckIsMaxWorkingTimeInMinutesExceeded()
        {
            int RunningPeriod = (DateTime.Now - StartTime).Minutes;
            if (MaxWorkingTimeInMinutes > 0 && RunningPeriod > MaxWorkingTimeInMinutes)
            {
                return true;
            }
            return false;
        }
    }



#if false
    public class WorkerEntryPoint : Logitude.Server.Tools.WorkerEntryPoint
    {
        /// <summary>
        /// Name of thread used in debugging and logging
        /// </summary>
        public string ThreadName;

        public int NumberOfDoneItems { get; set; }
        public DateTime? LastActivity { get; set; }
        public string ThreadId { get; set; }
        public Dictionary<DateTime,int>  DoneItemsInRange { get; set; }
        public string BatchServiceCode { get; set; }
        public decimal CPU { get; set; }


        //public virtual bool OnStart()
        //{
        //    ThreadName = this.GetType().Name;
        //    return (true);
        //}
        public override void StartMe()
        {
            if (CacheManager.CacheWrapper != null) return;
            WorkerEntryPoint.StartStatic();
        }

        public static void StartStatic()
        {
            CacheManager.CacheWrapper = new CacheWrapper(
            Cache
            );
            ThreadedRoleEntryPoint.StartStatic();
            
        }

         
        /// <summary>
        /// This method prevents unhandled exceptions from being thrown
        /// from the worker thread.
        /// </summary>
        internal void ProtectedRun()
        {
            if (LogitudeSettings.DeploymentStage != "Dev")
            {
                try
                {
                   
                    Run();
                    AsyncRun();
                }
                catch (SystemException e)
                {
                    //
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()1 Method", null);
                    Thread.Sleep(60000);
                    //throw e;

                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General :  Run()2 Method", null);
                    Thread.Sleep(60000);
                    //throw e;
                }
            }

            else
            {
                Run();
                AsyncRun();
            }
        }

        public virtual void Run()
        {
        }

        public virtual async void AsyncRun()
        {
        }

        public virtual void OnStop()
        {
        }

        public override void WorkOnce()
        {
           
        }
        private int threadID; 
        public void LogDoneItem()
        {
            try
            {
                threadID = AppDomain.GetCurrentThreadId();
                NumberOfDoneItems++;
                DateTime doneDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                if (DoneItemsInRange.Keys.Contains(doneDate))
                {
                    DoneItemsInRange[doneDate]++;
                }
                else
                {
                    DoneItemsInRange.Add(doneDate, 1);
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
                    Thread.Sleep(500);
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
    }


#endif
}
