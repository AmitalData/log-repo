using Logitude.SystemLogs;
using Logitude.XSD.Analyzers.GLSHKAnalyzer;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CommunicationWorkerRole
{
    public class GLSHKMessageAnalyzeWR : WorkerEntryPoint
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
                        AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("GLSHK");
                        LastActivity = DateTime.UtcNow;
                        if (analyzeQueue != null)
                        {
                            GLSHKAnalyzer analyzer = new GLSHKAnalyzer(analyzeQueue, analyzeQueueRepository);
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
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "GLSHKMessageAnalyzeWR : Run() Method", null);
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
            BatchServiceCode = "GLSHKMessageAnalyze";
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
