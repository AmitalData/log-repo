using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace LogitudeActiveServer
{
    public partial class LogitudeActiveServer : ServiceBase
    {
        public LogitudeActiveServer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();

        }

        protected override void OnStop()
        {
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            EventLog.WriteEntry("worker_DoWork start");
            while (true)
            {
                try
                {
                    string ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                    ServerManager m = new ServerManager();
                    var MyPath = m.Sites["default web site"].Applications["/"].VirtualDirectories["/"].PhysicalPath;
                    if (!File.Exists(MyPath + "\\" + ServerName + ".txt"))
                    {
                        EventLog.WriteEntry(System.Environment.CurrentDirectory);
                        EventLog.WriteEntry(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                        //if (!File.Exists(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + ServerName + ".txt"))
                        //{
                        //    File.Create(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + ServerName + ".txt");
                        //}
                        //File.Copy(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + ServerName + ".txt", MyPath + "\\" + ServerName + ".txt");
                        File.Create(MyPath + "\\" + ServerName + ".txt").Dispose(); ;
                    }

                    Thread.Sleep(60000);

                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("LogitudeActiveServer Error " + ex.Message);
                    Thread.Sleep(60000);
                }
            }

        }

        internal void TestStartupAndStop()
        {
            this.OnStart(null);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
