using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using WebFreight.Web.Helpers.TicketAnalyzer;

namespace CommunicationWorkerRole
{
    public class ContainerStatusesConnecterWR : WorkerEntryPoint
    {
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                        AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("ContainerStatusesReceiver");
                        LastActivity = DateTime.UtcNow;

                        if (analyzeQueue != null)
                        {
                            //ContainerStatusesConnecterAnalyzer analyzer = new ContainerStatusesConnecterAnalyzer(analyzeQueue, analyzeQueueRepository);

                            //analyzer.Run();
                            LogDoneItemInMemory();
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ContainerStatusesConnecterWR : Run() Method", null);
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
            BatchServiceCode = "ContainerStatusesConnecterWR";
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

