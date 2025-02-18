using System.Linq;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Net;
using System;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;

namespace CommunicationWorkerRole
{
    class WarmWorkerRole : WorkerEntryPoint
    {
        public override void Run()
        {
           
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        LastActivity = DateTime.UtcNow;
                        string url = LogitudeSettings.LogitudeURL;//ConfigurationManager.AppSettings.Get("LogitudeURL");
                        url = url + "/WebServices/WarmWebService.asmx";
                        url = url.Replace("https", "http");
                        WarmServiceReference.WarmWebServiceSoapClient warmService = new WarmServiceReference.WarmWebServiceSoapClient();
                        
                        warmService.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);
                        warmService.StartWarming();
                        Thread.Sleep(10000);
                        warmService.LoadMetaData();
                        Thread.Sleep(300000); // 5 minutes300000
                        LogDoneItemInMemory();
                    }
                    catch (Exception e)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "WarmWorkerRole",null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "WarmSystem";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
