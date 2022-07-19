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
        string RestartTime = "";
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
              
                CommunicationWorkerRole.ThreadedRoleEntryPoint d = new CommunicationWorkerRole.ThreadedRoleEntryPoint();
                d.OnStart();
                EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+ "StartLogitudeBatchServices (OnStart) without arguments");
                d.Run();
               
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+ "LogitudeBatchServices StartLogitudeBatchServices without arguments Exception : " + ex.ToString(), EventLogEntryType.Error);
              
            }
        }
        public void StartLogitudeBatchServices(string[] args)
        {
            try
            {
               
                ProcessStartTime = DateTime.Now;
                StartBatchServiceInternalManager();
                AnalyseBatchServiceParameters(args);         
                CommunicationWorkerRole.ThreadedRoleEntryPoint d = new CommunicationWorkerRole.ThreadedRoleEntryPoint(IncludedServicesAsArgs, IgnoredServicessAsArgs);
                d.OnStart();
                BatchServiceTimer.Start();
                EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+"StartLogitudeBatchServices (OnStart) with arguments " + "IgnoredServices :" + IgnoredServicessAsArgs + " IncludedServices: " + IncludedServicesAsArgs + " MaxWorkingTimeInMinutes:" + MaxWorkingTimeInMinutes + " MaxMemoryMB:" + MaxMemoryMB);
                d.Run();
            
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+ "LogitudeBatchServices StartLogitudeBatchServices with arguments Exception : " + ex.ToString(), EventLogEntryType.Error);
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
            string restartTimeParam = args.Where(a => a.Contains("-RestartTime")).FirstOrDefault();      
            if (restartTimeParam != null)
            {
                RestartTime = restartTimeParam.Split(':')[1];
            }
            var temp = args.Where(a => a.Contains("-MPWTIM")).FirstOrDefault();
            if (temp != null)
            {
                MaxWorkingTimeInMinutes = int.Parse(temp.Split(':')[1]);  
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
                    try
                    {
                        EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+ " Max memeory exceeded for Process WIth PID : " + CurrentProcess.Id + " Stopped, the Process Argumants are : " + LogitudeBatchServiceHelper.GetCommandLineArgs(CurrentProcess));
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+ "Exception in trying to write on eventlog (StopProcess) | Exception: " + ex.ToString());
                    }
                    StopProcess(CurrentProcess);
                  
                }

                if (CheckIsMaxWorkingTimeInMinutesExceeded(CurrentProcess))
                {
                    try
                    {
                        EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+ " Max Working Time In Minutes Exceeded for Process WIth PID : " + CurrentProcess.Id + " Stopped, the Process Argumants are : " + LogitudeBatchServiceHelper.GetCommandLineArgs(CurrentProcess));
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName()+"|"+ "Exception in trying to write on eventlog (StopProcess) | Exception: " + ex.ToString());
                    }
                    StopProcess(CurrentProcess);
                   
                }

                if (CheckIsStopTimeReached(CurrentProcess))
                {
                    try
                    {
                        EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName() + "|" + " Stop Time reached for Process WIth PID : " + CurrentProcess.Id + " Stopped, the Process Argumants are : " + LogitudeBatchServiceHelper.GetCommandLineArgs(CurrentProcess));
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(LogitudeBatchServiceHelper.getWorkerRoleName() + "|" + "Exception in trying to write on eventlog (StopProcess) | Exception: " + ex.ToString());
                    }
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
        private bool CheckIsStopTimeReached(Process currentProcess)
        {
            string currnetTime = DateTime.Now.ToString("HH_mm");
            if (RestartTime != "" && RestartTime == currnetTime)
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

            BatchServiceTimer.Stop();
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
