using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
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

    public class BluesnapPageAnalyzerWorkerRole : WorkerEntryPoint
    {
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating() && LogitudeSettings.WorkerRoleName.ToLower() != "staging")
                {
                    try
                    {
                        AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                        AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("Bluesnap");
                        LastActivity = DateTime.UtcNow;

                        if (analyzeQueue != null)
                        {
                            BluesnapAnalyzer analyzer = new BluesnapAnalyzer(analyzeQueue, analyzeQueueRepository);

                            analyzer.Run();
                            LogDoneItemInMemory();
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "BluesnapPageAnalyzeWR : Run() Method", null);
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
            BatchServiceCode = "BluesnapPageAnalyzerWorkerRole";
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
