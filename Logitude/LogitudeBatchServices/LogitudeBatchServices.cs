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
        string IncludedServicesAsArgs = "";
        string IgnoredServicessAsArgs = "";
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
            var Args = (string[])e.Argument;
            if (Args == null)
            {
                StartLogitudeBatchServices();
            }
            else
            {
                StartLogitudeBatchServices(Args);
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
                EventLog.WriteEntry("LogitudeBatchServices Exception : " + ex.ToString(), EventLogEntryType.Error);
                //EventLog.WriteEntry("LogitudeBatchServices Error");
                //EventLog.WriteEntry(ex.Message);
            }
        }
        public void StartLogitudeBatchServices(string[] args)
        {
            try
            {
               
                ProcessStartTime = DateTime.Now;
                StartBatchServiceInternalManager();
                AnalyseBatchServiceParameters(args);

                EventLog.WriteEntry("worker_DoWork start");
                CommunicationWorkerRole.ThreadedRoleEntryPoint d = new CommunicationWorkerRole.ThreadedRoleEntryPoint(IncludedServicesAsArgs, IgnoredServicessAsArgs);
                d.OnStart();
                BatchServiceTimer.Start();
                EventLog.WriteEntry("OnStart Passed ");
                d.Run();
            }
            catch (Exception ex)
            { 
                EventLog.WriteEntry("LogitudeBatchServices Exception : " + ex.ToString(), EventLogEntryType.Error);
            }
            
        }

        private void AnalyseBatchServiceParameters(string[] argsList)
        {
            string[] args = argsList;
            if (argsList.Length == 1)
            {
                args = argsList[0].Split(' ');
            }
            var IgnoredServices = args.Where(a => a.Contains("-Ignore")).FirstOrDefault();
            var IncludedServices = args.Where(a => a.Contains("-Include")).FirstOrDefault();
            MaxMemoryMB = int.Parse(args.Where(a => a.Contains("-MMMB")).FirstOrDefault().Split(':')[1]);
            var temp = args.Where(a => a.Contains("-MPWTIM")).FirstOrDefault();
            if (temp != null)
            {
                MaxWorkingTimeInMinutes = int.Parse(temp.Split(':')[1]);
                EventLog.WriteEntry("MaxWorkingTimeInMinutes  " + MaxWorkingTimeInMinutes);
            }
            IncludedServicesAsArgs = !string.IsNullOrEmpty(IncludedServices) ? IncludedServices.Split(':')[1] : "";
            IgnoredServicessAsArgs = !string.IsNullOrEmpty(IgnoredServices) ? IgnoredServices.Split(':')[1] : "";
        }

        private void StartBatchServiceInternalManager()
        {
            if (BatchServiceTimer == null)
            {
                this.BatchServiceTimer = new System.Timers.Timer();

                TimeSpan t = new TimeSpan(0, 0, 30);
                BatchServiceTimer.Interval = (int)t.TotalMilliseconds;
                BatchServiceTimer.Stop();
                BatchServiceTimer.Elapsed += BatchServiceTimer_Elapsed;
            }
        }

        private void BatchServiceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Process CurrentProcess = Process.GetCurrentProcess();
            if (!CurrentProcess.HasExited)
            {
                if (CheckIsMaxMemoryExceeded(CurrentProcess))
                {
                    StopProcess(CurrentProcess);
                }

                if (CheckIsMaxWorkingTimeInMinutesExceeded(CurrentProcess))
                {
                    StopProcess(CurrentProcess);
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

        private void StopProcess(Process currentProcess)
        {
            EventLog.WriteEntry("Process WIth PID : " + currentProcess.Id + " Stopped");
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
