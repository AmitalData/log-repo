using Logitude.BL.Interfaces;
using Logitude.BL.Security;
using Logitude.Customs.BL.EntityQueryServiceExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LogitudeBatchServices
{
    public partial class LogitudeBatchServices : ServiceBase
    {
        int MaxMemoryMB = 0;
        int MaxWorkingTimeInMinutes = 0;
        private System.Timers.Timer BatchServiceTimer;
        private DateTime ProcessStartTime;
        public LogitudeBatchServices()
        {
            InitializeComponent();
        }
        public LogitudeBatchServices(string[] args = null)
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            //EventLog.WriteEntry("My simple service started.");
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var Arg = (string)e.Argument;
            if (string.IsNullOrEmpty(Arg))
            {
                StartLogitudeBatchServices();
            }
            else
            {
                StartLogitudeBatchServices(Arg);
            }

        }

        public void StartLogitudeBatchServices()
        {
            try
            {
                EventLog.WriteEntry("worker_DoWork start");
                CommunicationWorkerRole.ThreadedRoleEntryPoint d = new CommunicationWorkerRole.ThreadedRoleEntryPoint();
                d.OnStart();
                EventLog.WriteEntry("OnStart Passed ");
                d.Run();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogitudeBatchServices Error");
                EventLog.WriteEntry(ex.Message);
            }
        }
        public void StartLogitudeBatchServices(string arg)
        {
            try
            {
                ProcessStartTime = DateTime.Now;
                if (BatchServiceTimer == null)
                {
                    this.BatchServiceTimer = new System.Timers.Timer();

                    TimeSpan t = new TimeSpan(0, 0, 30);
                    BatchServiceTimer.Interval = (int)t.TotalMilliseconds;
                    BatchServiceTimer.Stop();
                    BatchServiceTimer.Elapsed += BatchServiceTimer_Elapsed;
                }
                var args = arg.Split(' ').ToList();

                var IgnoredServices = args.Where(a => a.Contains("-Ignore")).FirstOrDefault();
                var IncludedServices = args.Where(a => a.Contains("-Include")).FirstOrDefault();
                MaxMemoryMB = int.Parse(args.Where(a => a.Contains("-MMMB")).FirstOrDefault());
                MaxWorkingTimeInMinutes = int.Parse(args.Where(a => a.Contains("-MPWTIM")).FirstOrDefault());
                EventLog.WriteEntry("worker_DoWork start");
                CommunicationWorkerRole.ThreadedRoleEntryPoint d = new CommunicationWorkerRole.ThreadedRoleEntryPoint(IncludedServices, IgnoredServices);
                d.OnStart();
                EventLog.WriteEntry("OnStart Passed ");
                d.Run();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogitudeBatchServices Error");
                EventLog.WriteEntry(ex.Message);
            }
        }

        private void BatchServiceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Process CurrentProcess = Process.GetCurrentProcess();
            if (!CurrentProcess.HasExited)
            {
                if (CheckIsMaxMemoryExceeded(CurrentProcess))
                {
                    RestartProcess(CurrentProcess);
                }

                if (CheckIsMaxWorkingTimeInMinutesExceeded(CurrentProcess))
                {
                    RestartProcess(CurrentProcess);
                }
            }
        }

        private bool CheckIsMaxWorkingTimeInMinutesExceeded(Process currentProcess)
        {
            int RunningPeriod = (DateTime.Now - ProcessStartTime).Minutes;
            if (MaxWorkingTimeInMinutes > 0 && RunningPeriod > MaxWorkingTimeInMinutes)
            {
                return true;
            }
            return false;
        }

        private bool CheckIsMaxMemoryExceeded(Process currentProcess)
        {
            var ProcessMemoInMB = currentProcess.WorkingSet64 / 1000000;
            if (MaxMemoryMB > 0 && ProcessMemoInMB >= MaxMemoryMB)
            {
                return true;
            }
            return false;
        }

        private void RestartProcess(Process currentProcess)
        {
            EventLog.WriteEntry("Process WIth PID : " + currentProcess.Id + " Restarted");
            currentProcess.WaitForExit((int)new TimeSpan(0, 1, 0).TotalMilliseconds);
            currentProcess.Kill();
        }

        protected override void OnStop()
        {
            //Dispose();
            //EventLog.WriteEntry("My simple service Stoped.");
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        public void RegisterClasses()
        {
            ContainerAccessor.Container.RegisterType<ILoggedContactUtil, LoggedContactUtil>("LoggedContactUtil", new InjectionFactory(c => new LoggedContactUtil()));
        }

        public void StartExternally(string arg)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync(argument: arg);
            //Task.Factory.StartNew(() => {
            //    StartLogitudeBatchServices(arg);
            //});
        }
    }
}
