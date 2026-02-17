using System;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;

using WebFreight.Web.GlobalModel;

using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure;
using Logitude.SystemLogs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Data.Helpers;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Logitude.Server.Tools.StorageService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Text;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.Web;

namespace CommunicationWorkerRole
{
    public class AgentSharedDocumentAnalyzeWR : WorkerEntryPoint
    {


        public override void Run()
        {


            while (IsRunning)
            {

                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        int agentTenant = 0;
                        string communicationLogId = string.Empty;
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (response.MessageId != null)
                        {



                            if (response.MessageValues.Keys.Contains("CommunicationLogId")) communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
                            if (response.MessageValues.Keys.Contains("Tenant")) int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                            if (response.MessageValues.Keys.Contains("AgentTenant")) int.TryParse(response.MessageValues["AgentTenant"].ToString(), out agentTenant);

                            if (string.IsNullOrEmpty(communicationLogId) ||  !response.MessageValues.Keys.Contains("Tenant") || !response.MessageValues.Keys.Contains("AgentTenant"))
                            {
                                queueservice.Complete();
                                continue;
                            }


                            ICommonDataContext context = CommonDataContext.GetContext(tenant);
                            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                            CommunicationLog commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                            bool processEnebled = true;

                            if (processEnebled)
                            {
                                if (commLog != null)
                                {
                                    if (commLog.CommunicationStatusTypeCode == "D")
                                    {
                                        queueservice.Complete();
                                    }
                                    else
                                    {

                                        if (commLog.Document != null)
                                        {
                                            string filename = commLog.DocumentId + "." + commLog.Document.Extension;


                                            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                                            {
                                                FileName = commLog.Document.Id,
                                                FolderName = commLog.Document.Folder,
                                                Extension = commLog.Document.Extension,
                                                Tenant = commLog.Document.Tenant,
                                                FileSize = commLog.Document.FileSize,
                                            };
                                            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                                            byte[] datainByte = storageservice.Read(fileInfo);



                                            if (datainByte != null)
                                            {
                                                byte[] documentsXML = datainByte;
                                                DocumentSL documentSL = LogitudeXmlSerializer.DeserializeObject<DocumentSL>(datainByte);


                                                AgentSharedDocumentPM agentSharedDocumentPM = new AgentSharedDocumentPM();
                                                agentSharedDocumentPM.Tenant = agentTenant;
                                                agentSharedDocumentPM.AgentId = documentSL.AgentId;
                                                agentSharedDocumentPM.AgentReference = documentSL.AgentReference;
                                                agentSharedDocumentPM.AgentSharedManifestRef = documentSL.AgentSharedManifestRef;
                                                agentSharedDocumentPM.ShipmentLevelCode = documentSL.ShipmentLevelCode;
                                                agentSharedDocumentPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(agentTenant);
                                                agentSharedDocumentPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(agentTenant);
                                                agentSharedDocumentPM.StatusCode = "WAIT";

                                                List<string> documentFilingSecurityKeyLists = new List<string>();
                                                foreach (DocumentDetails dcumentSL in documentSL.DocumentLists)
                                                {
                                                    documentFilingSecurityKeyLists.Add(dcumentSL.SecurityKey);
                                                }

                                                agentSharedDocumentPM.DocumentXML = LogitudeXmlSerializer.SerializeObjectToXmlString(documentSL);



                                                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                                                AgentSharedDocumentService agentSharedDocumentService = new AgentSharedDocumentService(commonContext, tenant);
                                                agentSharedDocumentService.Create(agentSharedDocumentPM);



                                                #region Update DocumentsFiling LastShareDate
                                                if (documentFilingSecurityKeyLists.Count > 0)
                                                {

                                                    ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                                                    ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);

                                                    DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(tenant);
                                                    List<DocumentsFiling> documentsFilingLists = documentsFilingRepository.GetDocumentsFilingsBySecurityIdsAndObjectTable(documentFilingSecurityKeyLists, table.Id, tenant);

                                                    foreach (DocumentsFiling documentsFiling in documentsFilingLists)
                                                    {
                                                        documentsFiling.LastShareDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                                        documentsFiling.IsSharedOut = true;
                                                        documentsFilingRepository.Update(documentsFiling);
                                                    }

                                                    documentsFilingRepository.SubmitChanges();
                                                }
                                                #endregion


                                                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                                                if (shipmentQuery.CheckIfShipmentExistByAgentSharedManifestRef(agentSharedDocumentPM.AgentSharedManifestRef, agentSharedDocumentPM.Tenant))
                                                {
                                                    IQueueService queueservice = new DbQueueService();
                                                    queueservice.InitializeQueue("AgentsSharedDocumentQueue", tenant);
                                                    queueservice.Send(new Dictionary<string, string>() { { "EntityId", agentSharedDocumentPM.Id }, { "Tenant", tenant.ToString() }, { "AgentTenant", agentSharedDocumentPM.Tenant.ToString() } }, null, null, null, null);

                                                }


                                            }


                                            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                                            commLog.CommunicationStatusTypeCode = "D";
                                            commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                                            commLog.DoneDateUTC = DateTime.UtcNow;
                                            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                                            commLog.LastStatusDateUTC = DateTime.UtcNow;
                                            commLogrepository.Update(commLog);
                                            commLogrepository.SubmitChanges();



                                        }

                                        queueservice.Complete();
                                        LogDoneItemInMemory();

                                    }
                                }
                                else
                                {
                                    if (response.RetryNumber <= 11)
                                    {
                                        if (response.RetryNumber < 3)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 1));
                                        }

                                        if (response.RetryNumber >= 3 && response.RetryNumber <= 5)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 5 && response.RetryNumber <= 10)
                                        {

                                            queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                            AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                                + ",at utc time:" + DateTime.UtcNow + ",at AgentsSharedLogistics worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(3000);
                                        }
                                        if (response.RetryNumber == 11)
                                        {

                                            queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                                            AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at AgentsSharedLogistics worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(10000);

                                        }
                                    }
                                    else
                                    {
                                        queueservice.Complete();
                                        AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at AgentsSharedLogistics worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "AgentSharedDocumentAnalyzeWR worker role start", null, null);
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

            ConnectClient();
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            //ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "AgentsSharedDocumentAnalyzeWR";

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        DbQueueService queueservice;
        string queueName = "AgentsSharedDocumentAnalyzeQueue";
        public void ConnectClient()
        {
            try
            {


                queueservice = new DbQueueService(queueName, 0);
                //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
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
