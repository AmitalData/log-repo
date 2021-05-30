using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.Helpers;

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

                        DbQueueService queueservice = queueservice = new DbQueueService("ContainerStatusesCommunicationLogQueue", 0);
                        QueueResponse iQueueResponse = queueservice.Receive(new TimeSpan(0, 0, 0, 10));

                        if (iQueueResponse.MessageId != null)
                        {
                            string communicationLogId = iQueueResponse.MessageValues["CommunicationLogId"].ToString();
                            int.TryParse(iQueueResponse.MessageValues["Tenant"].ToString(), out tenant);
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
