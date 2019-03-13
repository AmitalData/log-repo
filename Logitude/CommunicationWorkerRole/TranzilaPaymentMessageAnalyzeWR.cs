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
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace CommunicationWorkerRole
{
    class TranzilaPaymentMessageAnalyzeWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        private string queueName;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                        AnalyzeQueue analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("TranzilaPayment");

                        //LastActivity = DateTime.UtcNow;
                        if (analyzeQueue != null)
                        {
                            //analyzeQueue.Status = "In progress";
                            //analyzeQueueRepository.Update(analyzeQueue);
                            var MsgBody = System.Text.Encoding.UTF8.GetString(analyzeQueue.MessageBody);
                            if (MsgBody.Contains("Response=000"))
                            {
                                AnalyzeMessage(analyzeQueue);
                                analyzeQueue.Status = "D";
                            }
                            else
                            {
                                analyzeQueue.Status = "F";
                            }
                            analyzeQueueRepository.Update(analyzeQueue);
                            analyzeQueueRepository.SubmitChanges();
                        }

                        else
                        {
                            Thread.Sleep(500);
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "TranzilaPaymentMessageAnalyzeWR : Run() Method", null);
                        Thread.Sleep(5000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void AnalyzeMessage(AnalyzeQueue analyzeQueue)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(analyzeQueue.Tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            var MsgBody = System.Text.Encoding.UTF8.GetString(analyzeQueue.MessageBody);
            List<QueueTask> tasks = new List<QueueTask>();
            tasks.Add(new QueueTask()
            {
                Action = analyzeQueue.From + "Payment",
                Parameters = new List<Logitude.Server.Tools.Parameter>()
                                {
                                      new Logitude.Server.Tools.Parameter { Name = "Response", Value = MsgBody}
                                }
            });
            var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = ByteData.Length,
                Tenant = analyzeQueue.Tenant,
                Id = IdCounter.GetNumber("Document", analyzeQueue.Tenant),
                HasFile = true,
                Folder = "ExternalTasksQueue",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            var commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", analyzeQueue.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant),
                InOut = "O",
                //EntityId = OceanInsightsRequest.Id,
                ObjectTableId = null,
                Subject = "Tranzila Payment",
                Tenant = analyzeQueue.Tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "externaltasksqueue" + analyzeQueue.Tenant + 1,
                Priority = 1,

            };

            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            string filename = document.Id + "." + document.Extension;
            string filePath = "tenant" + commLog.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = analyzeQueue.Tenant,
                FileSize = ByteData.Length,

            };

            storageservice.Write(ByteData, fileInfo);
            if (!string.IsNullOrEmpty(commLog.QueueName))
            {
                SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, commLog.Tenant);
                //Communications.UpdateCommunicationLogStatus(commLog.Id, commLog.Tenant, null, "D", "after adding message to queue  TranzilaPayment " + DateTime.Now.ToString(), null);
            }
        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue Forwarder Shipment", null, null);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "TranzilaPaymentMessageAnalyze";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            //queueName = "ChampAnalyzer";
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
