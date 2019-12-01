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

namespace LogitudeBatchServicesManager
{
    public partial class BatchManagerService : ServiceBase
    {
        private static List<int> ManagedProcessesIds = new List<int>();
        public BatchManagerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ManagedProcessesIds = new List<int>();
            var BatchManager = new BackgroundWorker();
            BatchManager.DoWork += BatchManager_DoWork;
            BatchManager.RunWorkerAsync();
            
        }

        private void BatchManager_DoWork(object sender, DoWorkEventArgs e)
        {
            EventLog.WriteEntry("BatchManager_DoWork");
            StartMe();// ;// throw new NotImplementedException();
        }

        internal void TestStartupAndStop()
        {
            StartMe();
        }

        protected override void OnStop()
        {
            foreach (var processId in ManagedProcessesIds)
            {
                Process CurrentProcess = Process.GetProcessById(processId);
                CurrentProcess.WaitForExit(2000);
                CurrentProcess.Kill();
            }
        }

        void LaunchLogitudeBatchServices(List<string> args)
        { 
            foreach (var arg in args)
            {
                
                string path = System.AppDomain.CurrentDomain.BaseDirectory; 
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.FileName = path + "\\LogitudeBatchServices.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = arg;
                EventLog.WriteEntry("Path : " + startInfo.FileName);
                try
                { 
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        ManagedProcessesIds.Add(exeProcess.Id); 
                    }
                }
                catch(Exception e)
                {
                    EventLog.WriteEntry("Exception : " + e.ToString(),EventLogEntryType.Error);
                }
            }
            
        }

        private void StartMe()
        {
            EventLog.WriteEntry("StartMe");
            List<string> BatchServices = new List<string>(); 
            var iAppSettings = ConfigurationManager.AppSettings;
            if (iAppSettings != null)
            {
                if (iAppSettings["BatchServices"] != null)
                {
                    BatchServices = iAppSettings["BatchServices"].Split(';').ToList();
                }
            }
            LaunchLogitudeBatchServices(BatchServices);
        }
    }

    //public class BatchServiceDefenition
    //{
    //    public List<string> BatchCodes { get; set; }
    //    public int BatchMode { get; set; }
    //}

    //public static class BatchInstanseMode
    //{
    //    public const int RunAll = 0;
    //    public const int RunExceptList = 1;
    //    public const int RunOnlyList = 2; 
    //}
}
