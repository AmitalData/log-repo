using CommunicationWorkerRole.Analyzers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace CommunicationWorkerRole
{
    public class ContainerStatusesMonitorWR : WorkerEntryPoint
    {

        private int tenant;
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        IQueueService  queueservice = new DbQueueService();
                        queueservice.InitializeQueue("ContainerStatusesCommunicationLogQueue", 0);
                        QueueResponse queueResponse = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        if (queueResponse.MessageId != null)
                        {
                            string communicationLogId = queueResponse.MessageValues["CommunicationLogId"].ToString();
                            int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);
                            ContainerStatusesAnalyzer analyzer = new ContainerStatusesAnalyzer(communicationLogId, tenant);
                            analyzer.Run();
                            queueservice.Complete();
                            LogDoneItemInMemory();
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ContainerStatusesMonitorWorkerRole : Run() Method", null);
                        Thread.Sleep(5000);
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
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ContainerStatusesMonitorWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}
