using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Hosting;

namespace HighCPUIISRecycleService
{
    public partial class HighCPUIISRecycleService : ServiceBase
    {
        int totalHits = 0;
        int totalRecycles = 0;
        DateTime? FirstRecycleTime = null;
        DateTime? CurrentRecycleTime = null;
        TimeSpan TenMin = new TimeSpan(0,10,0);
        TimeSpan AnHour = new TimeSpan(1,0,0);
        public HighCPUIISRecycleService()
        {
            InitializeComponent();
        }


       
        protected override void OnStart(string[] args)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Myworker_DoWork);
            worker.RunWorkerAsync();
        }

        protected override void OnStop()
        {
        }

        void Myworker_DoWork(object sender, DoWorkEventArgs e)
        {
            EventLog.WriteEntry("HighCPUIISRecycleService started");
            while (true)
            {
                try
                {
                    //EventLog.WriteEntry("CPU Consumption is more than 90% in the last minute");
                    float cpuPercent = getCPUCounter();
                    if (cpuPercent >= 90)
                    {
                        totalHits = totalHits + 2;
                        if (totalHits >= 300)
                        {
                            EventLog.WriteEntry("CPU Consumption is more than 90% in the last minute");
                            if (FirstRecycleTime == null)
                            {
                                FirstRecycleTime = DateTime.Now;
                            }
                            if (CurrentRecycleTime == null)
                            {
                                CurrentRecycleTime = DateTime.Now;
                            }
                            if (CurrentRecycleTime.Value.Subtract(FirstRecycleTime.Value) <= AnHour && totalRecycles < 5)
                            {
                                RecycleApplicationPool("Default Web Site"); 
                                Thread.Sleep(TenMin);
                                CurrentRecycleTime = DateTime.Now;
                                totalRecycles++;
                            }
                            else
                            {
                                if (CurrentRecycleTime.Value.Subtract(FirstRecycleTime.Value) > AnHour && totalRecycles < 5)
                                {
                                    EventLog.WriteEntry("IIS is now Healthy..");
                                    FirstRecycleTime = CurrentRecycleTime;
                                    totalRecycles = 0;
                                }
                                else
                                {

                                    EventLog.WriteEntry("Recycled Too Many Times in one hour, the service stopped..");
                                    return;
                                }
                               
                            }
                            totalHits = 0;
                            
                        }
                    }
                    else
                    {
                        totalHits = 0;
                    }
                    Thread.Sleep(2000);

                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("HighCPUIISRecycleService Error " + ex.Message);
                    Thread.Sleep(60000);
                }
            }

        }


        private float getCPUCounter()
        {

            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            // will always start at 0
            float firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            // now matches task manager reading
            float secondValue = cpuCounter.NextValue();

            return secondValue;

        }

        public bool RecycleApplicationPool(string siteName = null)
        {
            if (siteName == null) siteName = HostingEnvironment.ApplicationHost.GetSiteName();
            using (ServerManager iisManager = new ServerManager())
            {
                SiteCollection sites = iisManager.Sites;
                foreach (Site site in sites)
                {
                    if (site.Name == siteName)
                    {
                        iisManager.ApplicationPools[site.Applications["/"].ApplicationPoolName].Recycle();
                        EventLog.WriteEntry(siteName + " Recycled due to high CPU Consumption");
                        return true;
                    }
                }
            }
            return false;
        }




        internal void TestStartupAndStop()
        {
            this.OnStart(null);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
