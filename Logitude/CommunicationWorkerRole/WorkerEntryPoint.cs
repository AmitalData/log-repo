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
            if (!SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
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



}
