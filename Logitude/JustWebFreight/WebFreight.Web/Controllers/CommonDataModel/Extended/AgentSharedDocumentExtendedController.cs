using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class AgentSharedDocumentExtendedController : ApiController
    {
        public HttpResponseMessage PostSharedDocuments(List<ShipmentShareDocumentsData>shipmentShareDocumentsDataLists)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("AgentSharedDocument", "UPDATE", authToken.Tenant);

                
                if(shipmentShareDocumentsDataLists != null)
                {
                   
                    shipmentShareDocumentsDataLists = shipmentShareDocumentsDataLists.Where(d => !string.IsNullOrEmpty(d.AgentSharedManifestRef)).ToList();

                    if (shipmentShareDocumentsDataLists.Count > 0)
                    {
                        int tenantAgent = 0;
                        string agentId = string.Empty;
                        tenantAgent = shipmentShareDocumentsDataLists[0].TenantAgent;

                        ICommonDataContext commonContext = CommonDataContext.GetContext(authToken.Tenant);
                        AgentRepository agentRepository = new AgentRepository(commonContext);
                        AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
                     

                        string agentSharedLogisticsKey = agentRepository.GetSharedKeyByAgentId(shipmentShareDocumentsDataLists[0].AgentId, authToken.Tenant);
                        if (!string.IsNullOrEmpty(agentSharedLogisticsKey))
                        {
                            agentId = agentRepository.GetAgentIdBySharedKey(agentSharedLogisticsKey, tenantAgent);
                        }

                        AgentSharedDocumentService agentSharedDocumentService = new AgentSharedDocumentService(commonContext, authToken.Tenant);

                        List<string> documentFilingSecurityKeyLists = new List<string>();
                        List<DocumentSL> documentSLLists = new List<DocumentSL>();


                        foreach (ShipmentShareDocumentsData shipmentShareDocumentsData in shipmentShareDocumentsDataLists)
                        {

                            DocumentSL documentSL = new DocumentSL();
                            List<DocumentDetails> documentLists = new List<DocumentDetails>();
                            foreach (ShareDocument shareDocument in shipmentShareDocumentsData.ShareDocuments)
                            {
                                documentLists.Add(new DocumentDetails() { DocumentCode = shareDocument.DocumentTypeCode, SecurityKey = shareDocument.SecurityId });
                            }
                            documentSL.DocumentLists = documentLists;
                            documentSL.AgentId = agentId;
                            documentSL.AgentReference = shipmentShareDocumentsData.ShipmentNumber;
                            documentSL.AgentSharedManifestRef = shipmentShareDocumentsData.AgentSharedManifestRef;
                            documentSL.ShipmentLevelCode = shipmentShareDocumentsData.ShipmentLevelCode;

                            documentSLLists.Add(documentSL);

                        }

                        #region Communications Log with Queue
                        if (documentSLLists.Count > 0)
                        {
                            ObjectTableQuery tablesQuery = new ObjectTableQuery(authToken.Tenant);
                            ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);

                            ContactRepository contactRepository = new ContactRepository(commonContext);
                            string email = HttpContext.Current.User.Identity.Name;
                            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, authToken.Tenant);


                            TenantRepository tenantRepository = new TenantRepository(authToken.Tenant);
                            Tenant sourceAgentTenantPOCO = tenantRepository.GetSingleTenant(authToken.Tenant);
                            Tenant destinationAgentTenantPOCO = tenantRepository.GetSingleTenant(tenantAgent);

                            if (sourceAgentTenantPOCO != null && sourceAgentTenantPOCO != null)
                            {
                                foreach (DocumentSL documentSL in documentSLLists)
                                {
                                    string entityId = "";
                                    ShipmentShareDocumentsData shipmentShareDocumentsData = shipmentShareDocumentsDataLists.Where(d => d.AgentSharedManifestRef == documentSL.AgentSharedManifestRef).FirstOrDefault();
                                    if (shipmentShareDocumentsData != null)
                                    {
                                        entityId = shipmentShareDocumentsData.EntityId;
                                    }
                                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                                    {
                                        byte[] documentXML = LogitudeXmlSerializer.SerializeObject(documentSL);

                                        CommunicationsParams logParams = new CommunicationsParams()
                                        {
                                            From = sourceAgentTenantPOCO.Company + " " + sourceAgentTenantPOCO.Id,
                                            To = destinationAgentTenantPOCO.Company + " " + destinationAgentTenantPOCO.Id,
                                            Tenant = authToken.Tenant,
                                            CommunicationLogTypeCode = "Q",
                                            QueueName = "AgentsSharedDocumentAnalyzeQueue",
                                            Priority = 1,
                                            InOut = "O",
                                            Status = "W",
                                            LoggingUserId = loggedContact.Id,
                                            LoggingObjectTableId = table.Id,
                                            LoggingEntityId = entityId,
                                            Subject = "Shared Documents",
                                            FolderName = "AgentsSharedDocumentAnalyzeQueue",
                                            ByteData = documentXML,

                                        };

                                        logParams.QueueParameters = new Dictionary<string, string>() {
                                        { "Tenant", authToken.Tenant.ToString() }, { "AgentTenant", tenantAgent.ToString() } };
                                        Communications.AddCommunicationLog(logParams);

                                        scope.Complete();
                                    }
                                }
                            }


                        }
                        #endregion


                    }

                }



             

                return Request.CreateResponse(HttpStatusCode.OK, "");

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
    }
}