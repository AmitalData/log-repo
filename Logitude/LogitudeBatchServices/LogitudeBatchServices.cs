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
    public partial class LogitudeBatchServices : ServiceBase, IServiceStarter
    {
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
            StartMe(null);
        }

        public void StartMe(string arg)
        {
            try
            {
                EventLog.WriteEntry("worker_DoWork start");
                CommunicationWorkerRole.ThreadedRoleEntryPoint d = new CommunicationWorkerRole.ThreadedRoleEntryPoint();
                EventLog.WriteEntry("ThreadedRoleEntryPoint ");
                d.OnStart();
                EventLog.WriteEntry("OnStart Passed ");
                d.Run();
                EventLog.WriteEntry("Run Passed ");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogitudeBatchServices Error");
                EventLog.WriteEntry(ex.Message);
            }
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

        public void Start()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }
    }
}
