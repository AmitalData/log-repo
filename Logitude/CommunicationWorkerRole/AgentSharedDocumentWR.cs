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

namespace CommunicationWorkerRole
{
    public class AgentSharedDocumentWR : WorkerEntryPoint
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
                        string entityId = string.Empty;
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (response.MessageId != null)
                        {



                            if (response.MessageValues.Keys.Contains("EntityId")) entityId = response.MessageValues["EntityId"].ToString();
                            if (response.MessageValues.Keys.Contains("Tenant")) int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                            if (response.MessageValues.Keys.Contains("AgentTenant")) int.TryParse(response.MessageValues["AgentTenant"].ToString(), out agentTenant);

                            if (string.IsNullOrEmpty(entityId) || !response.MessageValues.Keys.Contains("Tenant") || !response.MessageValues.Keys.Contains("AgentTenant"))
                            {
                                queueservice.Complete();
                                continue;
                            }


                       

                            ICommonDataContext agentContext = CommonDataContext.GetContext(agentTenant);
                            AgentSharedDocumentRepository agentSharedDocumentRepository = new AgentSharedDocumentRepository(agentTenant);
                            AgentSharedDocument agentSharedDocument = agentSharedDocumentRepository.GetSingleAgentSharedDocumentWithOutInCluded(entityId, agentTenant);

                            if (agentSharedDocument == null || string.IsNullOrEmpty(agentSharedDocument.DocumentXML))
                            {
                                queueservice.Complete();
                                continue;
                            }

                            DocumentSL documentSL = LogitudeXmlSerializer.DeserializeObject<DocumentSL>(agentSharedDocument.DocumentXML);
                            if (documentSL == null)
                            {
                                queueservice.Complete();
                                continue;
                            }

                                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                                ContactRepository agentContactRepository = new ContactRepository(agentContext);
                                AgentRepository agentRepository = new AgentRepository(agentContext);
                                DocumentTypeRepository agentDocumentTypeRepository = new DocumentTypeRepository(agentContext);
                                List<DocumentType> agentDocumentTypeList = agentDocumentTypeRepository.GetDocumentTypes(agentTenant).ToList();

                                AgentSharedManifestQuery agentSharedManifestQuery = new AgentSharedManifestQuery(agentTenant);
                                ShipmentRepository shipmentRepository = new ShipmentRepository(agentTenant);

                                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                                DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(tenant);
                                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                                DocumentRepository documentRepository = new DocumentRepository(tenant);
                                List<DocumentType> tenantDocumentTypeList = new List<DocumentType>();
                                List<DocumentsFilingList> documentsFilingLists = new List<DocumentsFilingList>();
                                List<string> documentCodeLists = new List<string>();
                                List<string> securityKeyLists = new List<string>();

                                foreach (DocumentDetails documentDetails in documentSL.DocumentLists)
                                {
                                    if (!documentCodeLists.Contains(documentDetails.DocumentCode)) documentCodeLists.Add(documentDetails.DocumentCode);
                                    if (!securityKeyLists.Contains(documentDetails.SecurityKey)) securityKeyLists.Add(documentDetails.SecurityKey);

                                }

                                if (!documentCodeLists.Contains("GMS")) documentCodeLists.Add("GMS");
                                tenantDocumentTypeList = documentTypeRepository.GetDocumentTypesByCodeLists(documentCodeLists, tenant);
                                if (securityKeyLists.Count > 0) documentsFilingLists = documentsFilingQuery.GetDocumentsFilingListsBysecurityIds(securityKeyLists, tenant);




                                //Agent destinationAgent = agentRepository.GetSingleAgentBySharedKey(manifestSL.AgentSharedKey, tenant);
                                Contact agentSystemContact = agentContactRepository.GetSingleContactByEmail("system@tenant" + agentTenant + ".com", agentTenant, false);

                                DocumentsFilingService documentsFilingService = new DocumentsFilingService(agentContext, agentTenant);
                                ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);
                                ObjectTable objectTable = objectTableRepository.GetObjectTableByName("Shipment", 0, false);

                                Shipment agentShipment = shipmentRepository.GetShipmentByAgentSharedManifestRef(agentSharedDocument.AgentSharedManifestRef, agentTenant);

                                TenantRepository sourceTenantRepository = new TenantRepository(tenant);
                                Tenant sourceAgentTenantPOCO = sourceTenantRepository.GetSingleTenant(tenant);
                                TenantRepository destinationTenantRepository = new TenantRepository(tenant);
                                Tenant destinationAgentTenantPOCO = destinationTenantRepository.GetSingleTenant(agentTenant);


                                    foreach (DocumentDetails details in documentSL.DocumentLists)
                                    {
                                        string fileName = "";
                                        DocumentsFilingList sharedDocumentFilngList = documentsFilingLists.First(t => t.SecurityId == details.SecurityKey);
                                        DocumentType sharedDocumentType = tenantDocumentTypeList.First(t => t.Id == sharedDocumentFilngList.DocumentTypeId);
                                        Document document = null;
                                        if (sharedDocumentFilngList.HasFile)
                                        {
                                            if (sharedDocumentFilngList.DirectionCode == "I")
                                            {
                                                document = documentRepository.GetSingleDocument(tenant, sharedDocumentFilngList.DocumentId);
                                            }
                                            else
                                            {

                                                DocumentOutCopyList documentOutCopylist = documentOutCopyQuery.GeDocumentOutCopyBydocumentTypeCopyAndDocumentOutId(sharedDocumentType.SharedDocumentTypeCopyId, sharedDocumentFilngList.Id, tenant);

                                                if (documentOutCopylist != null)
                                                {
                                                    document = documentRepository.GetSingleDocument(tenant, documentOutCopylist.DocumentId);
                                                }

                                                //docType.SharedDocumentTypeCopyId
                                            }

                                    if (document != null)
                                    {

                                        if (sharedDocumentFilngList.DirectionCode!= "I")
                                        {
                                            if (!string.IsNullOrEmpty(document.CalculatedFileName)) fileName = document.CalculatedFileName;
                                        }

                                        BlobFileInfo documentFileInfo = new BlobFileInfo()
                                                {
                                                    FileName = document.Id,
                                                    FolderName = document.Folder,
                                                    Extension = document.Extension,
                                                    Tenant = document.Tenant,
                                                };

                                                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                                                byte[] documentFileData = storageservice.Read(documentFileInfo);
                                                DocumentType agentDocumentType = agentDocumentTypeList.FirstOrDefault(t => t.Code == sharedDocumentType.Code);
                                                if (agentDocumentType == null)
                                                    agentDocumentType = agentDocumentTypeList.FirstOrDefault(t => t.Code == "GMS");

                                                DocumentsFilingPM documentInPM = new DocumentsFilingPM()
                                                {
                                                    DirectionCode = "I",
                                                    Tenant = agentTenant,
                                                    //Code = documentDataPM.Code,
                                                    EntityId = agentShipment.Id,
                                                    //ChildEntityId = documentDataPM.ChildEntityId,
                                                    //ChildEntityReference = documentDataPM.ChildEntityReference,
                                                    DocumentTypeId = agentDocumentType.Id,
                                                    ObjectTableId = objectTable.Id,

                                                    CreatedByUserId = agentSystemContact.Id,
                                                    CreateDate = sharedDocumentFilngList.ReceivedDate != null ? sharedDocumentFilngList.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(agentTenant),
                                                    Notes = agentShipment.Notes,
                                                    OwnerId = agentSystemContact.Id,
                                                    UpdatedByUserId = agentSystemContact.Id,
                                                    UpdateDate = sharedDocumentFilngList.ReceivedDate != null ? sharedDocumentFilngList.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(agentTenant),
                                                    Description = sharedDocumentFilngList.Description,
                                                    //ExternalEntityName = documentDataPM.ExternalEntityName,
                                                    //ExternalEntityReference = documentDataPM.ExternalEntityReference,
                                                    EntityReference = agentShipment.ShipmentNumber,
                                                    FileExtension = document.Extension,
                                                    FileSize = documentFileData != null ? Convert.ToInt32(documentFileData.Length) : 0,//Convert.ToInt32(documentDataPM.FileData.Length),
                                                    Folder = "docsin",
                                                    HasFile = true,
                                                    FileName = !string.IsNullOrEmpty(fileName) ? fileName : !string.IsNullOrEmpty(document.FileName) ? document.FileName : sharedDocumentType.Name,
                                                    FileData = documentFileData,
                                                    Received = true,
                                                    ReceivedDate = sharedDocumentFilngList.ReceivedDate != null ? sharedDocumentFilngList.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(agentTenant),
                                                    ReceivedByUserId = agentSystemContact.Id,
                                                    IsSharedIn = true,
                                                   
                                                };


                                                documentsFilingService.Create(documentInPM, documentFileData, agentSystemContact.Id);

                                            }
                                        }
                                    }

                       

                                byte[] documentXML = LogitudeXmlSerializer.SerializeObject(documentSL);

                                CommunicationsParams logParams = new CommunicationsParams()
                                {
                                    Tenant = agentTenant,
                                    From = sourceAgentTenantPOCO.Company + " " + sourceAgentTenantPOCO.Id,
                                    To = destinationAgentTenantPOCO.Company + " " + destinationAgentTenantPOCO.Id,
                                    CommunicationLogTypeCode = "Q",
                                    Priority = 1,
                                    InOut = "I",
                                    Status = "D",
                                    LoggingUserId = agentSystemContact.Id,
                                    LoggingObjectTableId = objectTable.Id,
                                    LoggingEntityId = agentShipment.Id,
                                    Subject = "Shared Documents",
                                    FolderName = "AgentsSharedDocumentAnalyzeQueue",
                                    ByteData = documentXML,
                                };

                                Communications.AddCommunicationLog(logParams);

                                agentSharedDocument.StatusCode = "COMP";
                                agentSharedDocumentRepository.Update(agentSharedDocument);
                                agentSharedDocumentRepository.SubmitChanges();
                            ActivityLog.SendTotangoContactActivity(agentSystemContact.Email, "(A) Agents Shared Logistics", "(A) Receiving Shared Documents", agentTenant, false, null,null);
                            queueservice.Complete();
                            LogDoneItemInMemory();

                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "AgentsSharedLogistics worker role start", null, null);
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
            BatchServiceCode = "AgentsSharedDocuments";

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        DbQueueService queueservice;
        string queueName = "AgentsSharedDocumentQueue";
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
