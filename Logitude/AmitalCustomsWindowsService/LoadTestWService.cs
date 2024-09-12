using AmitalCustomsWindowsService.Tester.LoadTest;
using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService
{
    partial class LoadTestWService : ServiceBase, IServiceStartMe
    {
        private LoadTestWorkerService _LoadTestWorkerService;

        public LoadTestWService()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _LoadTestWorkerService = new LoadTestWorkerService();
            ///teset - 
            ///tk2 delete changes
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var err = e.ExceptionObject.ToString();
            NetCommonHelper.Logger.DevLog.Instance.WriteError("CurrentDomain_UnhandledException!!!" + e.IsTerminating.ToString() + "Err:" + e.ToString() + err);
            var featureCheckMaxPoolSizeWasReachedThenRetart = ConfigurationManager.AppSettings["20180219.CheckMaxPoolSizeWasReachedThenRetart"] == "1";
            if (featureCheckMaxPoolSizeWasReachedThenRetart) { }
            Environment.Exit(-1);
        }
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            Task.Factory.StartNew(() =>
            {
                StartMe();
            });
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("OnStop()");
            _LoadTestWorkerService.StopThreads();
        }

        public void StartMe()
        {
            //throw new NotImplementedException();

            Program.ThreadStartStaticIsMustB4UsingTheDB();
            _LoadTestWorkerService.EnshureThreadWorking(true);
        }
    }
}
