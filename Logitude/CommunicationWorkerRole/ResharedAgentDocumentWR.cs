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

namespace CommunicationWorkerRole
{
    public class ResharedAgentDocumentWR : WorkerEntryPoint
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

                        string documentTypeCode = string.Empty;
                        string entityId = string.Empty;
                        string securityId = string.Empty;
                        int destinationAgentTenant =0 ;
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (response.MessageId != null)
                        {
                            if (response.MessageValues.Keys.Contains("EntityId")) entityId = response.MessageValues["EntityId"].ToString();
                            if (response.MessageValues.Keys.Contains("Tenant")) int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                            if (response.MessageValues.Keys.Contains("DocumentTypeCode")) documentTypeCode = response.MessageValues["DocumentTypeCode"].ToString();
                            if (response.MessageValues.Keys.Contains("SecurityId")) securityId = response.MessageValues["SecurityId"].ToString();

                            if (string.IsNullOrEmpty(entityId) || !response.MessageValues.Keys.Contains("Tenant") || string.IsNullOrEmpty(documentTypeCode) || string.IsNullOrEmpty(securityId))
                            {
                                queueservice.Complete();
                                continue;
                            }

                            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                            string shipmentNumber = "";
                            Shipment shipment = shipmentRepository.GetSingleShipmentwithOutIncludes(entityId, tenant);
                            if(shipment == null)
                            {
                                queueservice.Complete();
                                continue;
                            }

                            shipmentNumber = shipment.ShipmentNumber;
                            if (shipment.ShipmentLevelCode == "H")
                            {
                                 shipment = shipmentRepository.GetSingleShipmentwithOutIncludes(shipment.MasterShipmentDataId, tenant);
                            }
                            if (shipment == null || string.IsNullOrEmpty(shipment.AgentId))
                            {
                                queueservice.Complete();
                                continue;
                            }

                            AgentRepository agentRepository = new AgentRepository(tenant);
                            AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
                            Agent currentAgent = agentRepository.GetSingleAgent(tenant, shipment.AgentId);

                            if(currentAgent == null)
                            {
                                queueservice.Complete();
                                continue;
                            }

                            AgentSharedLogisticsKey agentSharedLogisticsKey = agentSharedLogisticsKeyRepository.GetSingleAgentSharedLogisticsKey(currentAgent.AgentSharedLogisticsKey);

                            if (agentSharedLogisticsKey == null)
                            {
                                queueservice.Complete();
                                continue;
                            }

                            destinationAgentTenant = agentSharedLogisticsKey.Agent1Tenant == tenant ? agentSharedLogisticsKey.Agent2Tenant : agentSharedLogisticsKey.Agent1Tenant;
                         
                            AgentSharedDocumentRepository agentSharedDocumentRepository = new AgentSharedDocumentRepository(destinationAgentTenant);
                            AgentSharedDocument agentSharedDocument =  agentSharedDocumentRepository.GetAgentSharedDocumentByAgentRef(shipmentNumber, destinationAgentTenant);

                            if(agentSharedDocument == null)
                            {
                                queueservice.Complete();
                                continue;
                            }

                            DocumentSL documentSL = new DocumentSL();
                            List<DocumentDetails> documentLists = new List<DocumentDetails>();

                            documentLists.Add(new DocumentDetails() {DocumentCode = documentTypeCode,SecurityKey = securityId });

                            documentSL.DocumentLists = documentLists;
                            documentSL.AgentId = shipment.AgentId;
                            documentSL.AgentReference = shipmentNumber;
                            documentSL.AgentSharedManifestRef = agentSharedDocument.AgentSharedManifestRef;
                            documentSL.ShipmentLevelCode = agentSharedDocument.ShipmentLevelCode;

                            TenantRepository tenantRepository = new TenantRepository(tenant);
                            Tenant sourceAgentTenantPOCO = tenantRepository.GetSingleTenant(tenant);
                            Tenant destinationAgentTenantPOCO = tenantRepository.GetSingleTenant(destinationAgentTenant);

                            if (sourceAgentTenantPOCO != null && sourceAgentTenantPOCO != null)
                            {
                                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                                ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                                ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);
                                ContactRepository contactRepository = new ContactRepository(commonContext);
                                string email = "system@tenant" + tenant + ".com";
                                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

                                using (TransactionScope scope = TransactionFactory.GetTransaction())
                                {
                                    byte[] documentXML = LogitudeXmlSerializer.SerializeObject(documentSL);

                                    CommunicationsParams logParams = new CommunicationsParams()
                                    {
                                        From = sourceAgentTenantPOCO.Company + " " + sourceAgentTenantPOCO.Id,
                                        To = destinationAgentTenantPOCO.Company + " " + destinationAgentTenantPOCO.Id,
                                        Tenant = tenant,
                                        CommunicationLogTypeCode = "Q",
                                        QueueName = "AgentsSharedDocumentAnalyzeQueue",
                                        Priority = 1,
                                        InOut = "O",
                                        Status = "W",
                                        LoggingUserId = loggedContact!=null ? loggedContact.Id:"",
                                        LoggingObjectTableId = table.Id,
                                        LoggingEntityId = entityId,
                                        Subject = "Shared Documents",
                                        FolderName = "AgentsSharedDocumentAnalyzeQueue",
                                        ByteData = documentXML,

                                    };

                                    logParams.QueueParameters = new Dictionary<string, string>() {
                                            { "Tenant", tenant.ToString() }, { "AgentTenant", agentSharedDocument.Tenant.ToString() } };
                                    Communications.AddCommunicationLog(logParams);

                                    scope.Complete();
                                }

                            }

                            queueservice.Complete();
                            LogDoneItemInMemory();

                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "ResharedAgentDocumentWR worker role start", null, null);
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
            BatchServiceCode = "ResharedAgentDocument";

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        DbQueueService queueservice;
        string queueName = "ResharedAgentDocumentQueue";
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
