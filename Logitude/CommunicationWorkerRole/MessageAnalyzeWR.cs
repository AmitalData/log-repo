using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Threading;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Transactions;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.WindowsAzure.Storage;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using System.Security.Cryptography;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using CHAMP;
using Logitude.XSD.Analyzers.CHAMPAnalyzer;
using Logitude.Server.Tools.QueueService;

namespace CommunicationWorkerRole
{
    class MessageAnalyzeWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        private string queueName;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    //try
                    //{
                    //    AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                    //    AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("Champ");
                    //    LastActivity = DateTime.UtcNow;
                    //    if (analyzeQueue != null)
                    //    {
                    //        CHAMPAnalyzer analyzer = new CHAMPAnalyzer(analyzeQueue, analyzeQueueRepository);
                    //        analyzer.Run();
                    //        LogDoneItemInMemory();
                    //    }

                    //    else
                    //    {
                    //        Thread.Sleep(3000);
                    //    }
                    //}

                    try
                    {
                        bool isUsingDbQueueService = false;

                        var iAppSettings = System.Configuration.ConfigurationManager.AppSettings;
                        if (iAppSettings != null)
                        {
                            if (iAppSettings["ChampDbQueueService"] != null)
                            {
                                string iValueText = iAppSettings["ChampDbQueueService"].ToString();
                                if (!string.IsNullOrEmpty(iValueText))
                                {
                                    if(iValueText.ToLower() == "true")
                                    {
                                        isUsingDbQueueService = true;
                                    }
                                }
                            }
                        }

                        if (isUsingDbQueueService)
                        {
                            queueservice = queueservice = new DbQueueService(queueName, 0);
                            QueueResponse iQueueResponse = queueservice.Receive(new TimeSpan(0, 0, 0, 10));

                            string AnalyzeQueueId = iQueueResponse.MessageValues["AnalyzeQueueId"].ToString();
                            if (!string.IsNullOrEmpty(AnalyzeQueueId))
                            {
                                queueservice.Complete();

                                AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                                AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetSingleAnalyzeQueue(AnalyzeQueueId);
                                if (analyzeQueue != null)
                                {
                                    CHAMPAnalyzer analyzer = new CHAMPAnalyzer(analyzeQueue, analyzeQueueRepository);
                                    analyzer.Run();
                                }
                            }
                        }

                        else
                        {
                            AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                            AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("Champ");
                            LastActivity = DateTime.UtcNow;
                            if (analyzeQueue != null)
                            {
                                CHAMPAnalyzer analyzer = new CHAMPAnalyzer(analyzeQueue, analyzeQueueRepository);
                                analyzer.Run();
                                LogDoneItemInMemory();
                            }

                            else
                            {
                                Thread.Sleep(3000);
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "MessageAnalyzeWR : Run() Method", null);
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
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "MessageAnalyze";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            queueName = "ChampAnalyzer";
            ConnectClient();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        public void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 0);
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "MessageAnalyzeWR Connect client", null, null);
            }
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
