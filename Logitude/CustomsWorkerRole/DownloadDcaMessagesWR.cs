using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logitude.SystemLogs;
using CustomsWorkerRole.DCA;
using Microsoft.ServiceBus.Messaging;
using System.IO;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
namespace CustomsWorkerRole
{
#if false
    

    public //public for E:\users\itzik\documents\visual studio 2012\Projects\CustomsWorkerRoleTester\CustomsWorkerRoleTester
        class DownloadDcaMessagesWR : WorkerEntryPoint
    {

        
        

        public DownloadDcaMessagesWR()
        {
           
            
        }
        public override void Run()
        {

            while (true)
            {

                if (!General.IsUpdating())
                {

                    
                    try
                    {
                        WorkOnce();
                        
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "DownloadDcaMessagesWR : Run() Method", null);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }

                
            }

        }

      
   
        public override bool OnStart()
        {
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

        public override void WorkOnce()
        {
             
            var myDcaService = new DcaService();
            var debugIIGMessageId ="";
            if (this.DebugObject != null)
            {
                debugIIGMessageId = this.DebugObject.ToString();
            }
            myDcaService.DownloadAll(debugIIGMessageId);

        }
    }
#endif
}
