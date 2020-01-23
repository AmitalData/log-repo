using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using LogitudeBatchServices;
using LogitudeBatchServicesManager.Helpers;
using LogitudeBatchServicesManager.Models;

namespace LogitudeBatchServicesManager
{
    public partial class BatchManagerService : ServiceBase
    {
        private static List<int> ManagedProcessesIds = new List<int>();
        private static List<ProcessStartInfo> ManagedProcessesInfos = new List<ProcessStartInfo>();
        
        private System.Timers.Timer BatchManagerServiceTimer;
        public BatchManagerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ManagedProcessesIds = new List<int>();
                ManagedProcessesInfos = new List<ProcessStartInfo>();
                if (BatchManagerServiceTimer == null)
                {
                    this.BatchManagerServiceTimer = new System.Timers.Timer();

                    TimeSpan t = new TimeSpan(0, 0, 30);
                    BatchManagerServiceTimer.Interval = (int)t.TotalMilliseconds;
                    BatchManagerServiceTimer.Stop();
                    BatchManagerServiceTimer.Elapsed += BatchManagerServiceTimer_Elapsed;
                }

                var BatchManager = new BackgroundWorker();
                BatchManager.DoWork += BatchManager_DoWork;
                BatchManager.RunWorkerAsync();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("ERROR On BatchManagerService : " + e.ToString());
            }
            finally
            {
                BatchManagerServiceTimer.Start();
            }


        }

        private void BatchManagerServiceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var processId in ManagedProcessesIds)
            {
                 
                if (!CheckIsProcessRunning(processId))
                {
                    RestartProcess(processId); 
                }

            }
        }

        private void RestartProcess(int processId)
        {
            string ProcessUniqueKey = "*" + processId + "*";
            var ProcessToRestart = ManagedProcessesInfos.Where(a => a.Arguments.Contains(ProcessUniqueKey)).FirstOrDefault();
            if (ProcessToRestart != null)
            {
                ProcessToRestart.Arguments = ProcessToRestart.Arguments.Replace(ProcessUniqueKey, "");
                ManagedProcessesIds = ManagedProcessesIds.Where(a => a != processId).ToList();
                ManagedProcessesInfos = ManagedProcessesInfos.Where(a => !a.Arguments.Contains(ProcessUniqueKey)).ToList();
                using (Process exeProcess = Process.Start(ProcessToRestart))
                {
                    EventLog.WriteEntry("BatchManagerService Starting Process With Id : " + processId);
                    ManagedProcessesIds.Add(exeProcess.Id);
                    ProcessToRestart.Arguments = ProcessToRestart.Arguments + "*" + exeProcess.Id + "*";
                    ManagedProcessesInfos.Add(ProcessToRestart);
                }
            }
        }

        private bool CheckIsProcessRunning(int processId)
        {
            return Process.GetProcesses().Any(x => x.Id == processId);
        }

        private void BatchManager_DoWork(object sender, DoWorkEventArgs e)
        {
            EventLog.WriteEntry("BatchManager_DoWork");
            StartBatchManagerService();
        }

        internal void TestStartupAndStop()
        {
            StartBatchManagerService();
        }

        protected override void OnStop()
        {
            foreach (var processId in ManagedProcessesIds)
            {
                KillProcess(processId);
            }
        }

        private void StartBatchManagerService()
        {
            EventLog.WriteEntry("StartBatchManagerService");
            List<BatchProcess> BatchProcesses = GetBatchServiceArguments();
            LaunchLogitudeBatchServices(BatchProcesses);
        }

        private List<BatchProcess> GetBatchServiceArguments()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string ConfigFilePath = path + "BatchManagerConfig.xml";
            string xmlString = File.ReadAllText(ConfigFilePath);
            BatchManagerConfigurations Configs = xmlString.ParseXML<BatchManagerConfigurations>(); 
            return Configs.Processes;
        }

        private void KillProcess(int processId)
        {
            var IsProcessRunning = Process.GetProcesses().Any(x => x.Id == processId);
            if (IsProcessRunning)
            {
                Process CurrentProcess = Process.GetProcessById(processId);
                CurrentProcess.WaitForExit(2000);
                CurrentProcess.Kill();
            } 
        }

        private void LaunchLogitudeBatchServices(List<BatchProcess> Processes)
        {
            EventLog.WriteEntry("Start LaunchLogitudeBatchServices");
            foreach (var Process in Processes)
            {
                StartProcessWithArgs(Process);
            }

        }

        private void StartProcessWithArgs(BatchProcess batchProcess)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = path + "\\LogitudeBatchServices.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = GetArgumentFromBatchProcess(batchProcess);
            //MainBatchServices.Main(new string[] { startInfo.Arguments });
            //EventLog.WriteEntry("Start Process With Arg " + Argument);
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    ManagedProcessesIds.Add(exeProcess.Id);
                    startInfo.Arguments = startInfo.Arguments + "*" + exeProcess.Id + "*";
                    ManagedProcessesInfos.Add(startInfo);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Exception : " + e.ToString(), EventLogEntryType.Error);
            }
        }

        private string GetArgumentFromBatchProcess(BatchProcess batchProcess)
        {
            var Arguments = " -MMMB:" + batchProcess.MaxMemoryMB + " -MPWTIM:" + batchProcess.MaxWorkingTimeInMinutes;
            if (batchProcess.AllServices && !string.IsNullOrEmpty(batchProcess.Ignore))
            {
                Arguments += " -Ignore:" + batchProcess.Ignore;
            }
            else if (!batchProcess.AllServices)
            {
                var IncludedServices = " -Include:";
                foreach (var Service in batchProcess.Services)
                {
                    IncludedServices += Service.Code + "-MWTIM~" + Service.MaxWorkingTimeInMinutes + ";";
                }
                Arguments += IncludedServices.TrimEnd(';');
            }
            return Arguments;
        }
    }
     
}
