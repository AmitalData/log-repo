using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Web.Http;
using Logitude.Accounting.BL.EntityQueryServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class DocumentOutExtendedController : ApiController
    {
        public HttpResponseMessage GetDocumentOutsByTenant(int tenant)
        {
            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                List<DocumentOutPM> result = documentOutQuery.GetDocumentOutPMsByTenant(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentOutsByEntityId(string entityId, int tenant)
        {
            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                List<DocumentOutPM> result = documentOutQuery.GetDocumentOutPMsByEntityId(entityId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentOutsByDocumentType(string docType, int tenant)
        {
            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                List<DocumentOutPM> result = documentOutQuery.GetDocumentOutPMsByTenant(tenant).Where(d => d.DocumentTypeCode == docType && d.Tenant == tenant).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleDocumentOutPM(string id, int tenant)
        {
            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                DocumentOutPM documentOutPM = documentOutQuery.GetSinglePM(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentOutPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentOutsByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, int tenant)
        {
            try
            {
                if (childEntityId == "null" || childEntityId == "undefined") childEntityId = null;
                if (objectTableId == "null" || objectTableId == "undefined") objectTableId = null;
                if (entityId == "null" || entityId == "undefined") entityId = null;
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                List<DocumentOutPM> result = documentOutQuery.GetDocumentOutPMsByEntityIdAndObjectTableAndChildEntityId(entityId, childEntityId, objectTableId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetDocumentOutByDocumentTypeEntityAndChild(string entityId, int tenant, string childEntityId, string documentTypeId)
        {
            try
            {
                if (childEntityId == "null" || childEntityId == "undefined") childEntityId = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, childEntityId, documentTypeId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentOutPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetDocumentOutReturnNewIfNon(string entityId, string childEntityId, string documentTypeId, string childentityreference, string objectTableId, int tenant)
        {
            try
            {
                if (childEntityId == "null" || childEntityId == "undefined") childEntityId = null;
                if (childentityreference == "null" || childentityreference == "undefined") childentityreference = null;

                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                DocumentOutPM documentout = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, childEntityId, documentTypeId, tenant);
                if (documentout == null)
                {
                    DocumentHelper documentHelper = new DocumentHelper();
                    documentout = documentHelper.CreateDocumentOut(documentTypeId, entityId, childEntityId, childentityreference, objectTableId, tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentout);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutDocumentOut(DocumentOutPM currentEntity)
        {

            try
            {
                Authentication(currentEntity.Tenant);
                ICommonDataContext objectContext = CommonDataContext.GetContext(currentEntity.Tenant);

                //DocumentOutCopyRepository documentOutCopyRepository = new DocumentOutCopyRepository(currentEntity.Tenant);
                //foreach (DocumentOutCopyPM item in currentEntity.DocumentOutCopies)
                //{
                //    DocumentOutCopy documentOutCopy = documentOutCopyRepository.GetSingleDocumentOutCopy(item.Id);

                //    if (documentOutCopy != null)
                //    {
                //        if (string.IsNullOrEmpty(item.DocumentId) && !string.IsNullOrEmpty(documentOutCopy.DocumentId))
                //        {
                //            item.DocumentId = documentOutCopy.DocumentId;
                //        }
                //    }

                //    DocumentOutCopy  documentCopy = MapDocumentOutCopyDocumentOutCopyPM(item, documentOutCopy);
                //    documentOutCopyRepository.Update(documentCopy);
                //}

                //documentOutCopyRepository.SubmitChanges();
                DocumentOutService service = new DocumentOutService(objectContext, currentEntity.Tenant);
                service.Update(currentEntity, currentEntity.DocumentOutCopies);
                TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "DocumentOut");

                return Request.CreateResponse(HttpStatusCode.OK, currentEntity);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public DocumentOutCopy MapDocumentOutCopyDocumentOutCopyPM(DocumentOutCopyPM documentOutCopyPM, DocumentOutCopy documentOutCopy)
        {
            documentOutCopy.DocumentId = documentOutCopyPM.DocumentId;
            documentOutCopy.DocumentOutId = documentOutCopyPM.DocumentOutId;
            documentOutCopy.DocumentTypeCopyId = documentOutCopyPM.DocumentTypeCopyId;
            documentOutCopy.Tenant = documentOutCopyPM.Tenant;

            return documentOutCopy;
        }

        public void DeleteDocumentOut(DocumentOut entity)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(entity.Tenant);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(objectContext);
            DocumentOut doc = documentOutRepository.GetSingleDocumentOut(entity.Id, entity.Tenant);
            documentOutRepository.Remove(doc);
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }




        public HttpResponseMessage GetCreateDocumentOut(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, int tenant)
        {
            try
            {
                if (childEntityId == "null" || childEntityId == "undefined") childEntityId = null;
                if (childReference == "null" || childReference == "undefined") childReference = null;

                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, childEntityId, documentTypeId, tenant);
                if (documentOutPM == null)
                {
                    DocumentHelper documentHelper = new DocumentHelper();
                    documentOutPM = documentHelper.CreateDocumentOut(documentTypeId, entityId, childEntityId, childReference, objectTableId, tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentOutPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage PutCreateDocumentOut(CreateDocumentOutArgs createDocumentOutArgs)
        {
            try
            {
                if (createDocumentOutArgs.ChildEntityId == "null" || createDocumentOutArgs.ChildEntityId == "undefined") createDocumentOutArgs.ChildEntityId = null;
                if (createDocumentOutArgs.ChildReference == "null" || createDocumentOutArgs.ChildReference == "undefined") createDocumentOutArgs.ChildReference = null;

                Authentication(createDocumentOutArgs.Tenant);

                DocumentOutQuery documentOutQuery = new DocumentOutQuery(createDocumentOutArgs.Tenant);
                DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(createDocumentOutArgs.EntityId, createDocumentOutArgs.ChildEntityId, createDocumentOutArgs.DocumentTypeId, createDocumentOutArgs.Tenant);
                if (documentOutPM == null)
                {
                    DocumentHelper documentHelper = new DocumentHelper();
                    documentOutPM = documentHelper.CreateDocumentOut(createDocumentOutArgs.DocumentTypeId, createDocumentOutArgs.EntityId, createDocumentOutArgs.ChildEntityId, createDocumentOutArgs.ChildReference, createDocumentOutArgs.ObjectTableId, createDocumentOutArgs.Tenant, null, createDocumentOutArgs.DocumentTypeTemplateId);
                    if (createDocumentOutArgs.SignHSM)
                    {
                        documentHelper.CheckDetailsToHSM(documentOutPM.Id);
                        
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentOutPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        

        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            // SecurityUtility.CheckContactFeature("DocumentOut", "READ", authToken.Tenant);
        }

        public HttpResponseMessage GetEntityPartners(string entityId, string objectTableName, string childEntityId, string childobjectTableName, string gLAccountId = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                Authentication();
                List<EntityPartner> list = new List<EntityPartner>();

                if (objectTableName == "Shipment")
                {
                    #region Shipment
                    ShipmentQuery shipmentQuery = new ShipmentQuery(authToken.Tenant);
                    ShipmentPM shipment = shipmentQuery.GetSinglePM(entityId, authToken.Tenant);
                    int idCounter = 0;

                    if (shipment != null)
                    {
                        if (shipment.CustomerId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.CustomerId,
                                PartnerType = "Customer",
                                PartnerContactId = shipment.CustomerContactId,
                            });
                        }

                        if (shipment.ShipperId != null)
                        {
                            if (shipment.ShipperId != shipment.CustomerId)
                            {
                                list.Add(new EntityPartner()
                                {
                                    Id = idCounter++,
                                    PartnerId = shipment.ShipperId,
                                    PartnerType = "Shipper",
                                    PartnerContactId = shipment.ShipperContactId,
                                });
                            }
                        }

                        if (shipment.ConsigneeId != null)
                        {
                            if (shipment.ConsigneeId != shipment.CustomerId)
                            {
                                list.Add(new EntityPartner()
                                {
                                    Id = idCounter++,
                                    PartnerId = shipment.ConsigneeId,
                                    PartnerType = "Consignee",
                                    PartnerContactId = shipment.ConsigneeContactId,
                                });
                            }
                        }

                        if (shipment.AgentId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.AgentId,
                                PartnerType = "Agent",
                                PartnerContactId = shipment.AgentContactId,
                            });
                        }

                        if (shipment.IssuingCarrierAgentId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.IssuingCarrierAgentId,
                                PartnerType = "Issuing Carrier Agent",
                            });
                        }

                        if (shipment.CustomAgentExportId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.CustomAgentExportId,
                                PartnerType = "Custom Agent Export",
                                PartnerContactId = shipment.CustomAgentExportContactId,
                            });
                        }

                        if (shipment.CustomAgentImportId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.CustomAgentImportId,
                                PartnerType = "Custom Agent Import",
                                PartnerContactId = shipment.CustomAgentImportContactId,
                            });
                        }

                        if (shipment.Notify1Id != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.Notify1Id,
                                PartnerType = "Notify1",
                                PartnerContactId = shipment.Notify1ContactId,
                            });
                        }

                        if (shipment.Notify2Id != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.Notify2Id,
                                PartnerType = "Notify2",
                                PartnerContactId = shipment.Notify2ContactId,
                            });
                        }

                        if (shipment.ShipperNotExporterId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.ShipperNotExporterId,
                                PartnerType = "Shipper Not Exporter",
                                PartnerContactId = shipment.ShipperNotExporterContactId,
                            });
                        }

                        if (shipment.ConsigneeNotImporterId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.ConsigneeNotImporterId,
                                PartnerType = "Consignee Not Importer",
                                PartnerContactId = shipment.ConsigneeNotImporterContactId,
                            });
                        }

                        if (shipment.FreightForwarderId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.FreightForwarderId,
                                PartnerType = "Freight Forwarder",
                                PartnerContactId = shipment.FreightForwarderContactId,
                            });
                        }

                        if (shipment.ColoaderId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.ColoaderId,
                                PartnerType = "Coloader",
                                PartnerContactId = shipment.ColoaderContactId,
                            });
                        }

                        if (shipment.CustomClearancePointId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.CustomClearancePointId,
                                PartnerType = "Custom Clearance Point",
                                PartnerContactId = shipment.CustomClearancePointContactId,
                            });
                        }

                        if (shipment.ConsolidatorId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.ConsolidatorId,
                                PartnerType = "Consolidator",
                                PartnerContactId = shipment.ConsolidatorContactId,
                            });
                        }

                        if (shipment.ReleasingAgentId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = shipment.ReleasingAgentId,
                                PartnerType = "Releasing Agent",
                                PartnerContactId = shipment.ReleasingAgentContactId,
                            });
                        }
                    }
                    #endregion

                    #region ShipmentPickUpDelivery
                    if (!string.IsNullOrEmpty(childEntityId) && childobjectTableName == "ShipmentPickUpDelivery")
                    {
                        ShipmentPickUpDeliveryQuery shipmentPickUpDeliveryQuery = new ShipmentPickUpDeliveryQuery(authToken.Tenant);
                        string carrierId = shipmentPickUpDeliveryQuery.GetShipmentPickUpDeliveryCarrierIdById(childEntityId, authToken.Tenant);

                        if (!string.IsNullOrEmpty(carrierId))
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = carrierId,
                                PartnerType = "Trucker ",
                            });
                        }
                    }
                    #endregion
                }

                else if (objectTableName == "Quote")
                {
                    #region Quote
                    QuoteRepository quoteRepository = new QuoteRepository(authToken.Tenant);
                    QuoteQuery quoteQuery = new QuoteQuery(authToken.Tenant);
                    QuotePM quote = quoteQuery.GetSinglePM(entityId, authToken.Tenant);

                    int idCounter = 0;
                    if (quote != null)
                    {
                        if (quote.CustomerId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = quote.CustomerId,
                                PartnerType = "Customer",
                                PartnerContactId = quote.CustomerContactId,
                            });
                        }

                        if (quote.ShipperId != null)
                        {
                            if (quote.ShipperId != quote.CustomerId)
                            {
                                list.Add(new EntityPartner()
                                {
                                    Id = idCounter++,
                                    PartnerId = quote.ShipperId,
                                    PartnerType = "Shipper",
                                    PartnerContactId = quote.ShipperContactId,
                                });
                            }
                        }

                        if (quote.ConsigneeId != null)
                        {
                            if (quote.ConsigneeId != quote.CustomerId)
                            {
                                list.Add(new EntityPartner()
                                {
                                    Id = idCounter++,
                                    PartnerId = quote.ConsigneeId,
                                    PartnerType = "Consignee",
                                    PartnerContactId = quote.ConsigneeContactId,
                                });
                            }
                        }

                        if (quote.AgentId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = quote.AgentId,
                                PartnerType = "Agent",
                                PartnerContactId = quote.AgentContactId,
                            });
                        }

                        if (quote.MainCarriageCarrierId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = idCounter++,
                                PartnerId = quote.MainCarriageCarrierId,
                                PartnerType = "Carrier",
                            });
                        }
                    }
                    #endregion
                }

                else if (objectTableName == "Opportunity")
                {
                    #region Opp
                    OpportunityQueryService opportunityQuery = new OpportunityQueryService(authToken.Tenant);
                    OpportunityPM entityPM = opportunityQuery.GetSingle(entityId, false, false);
                    if (entityPM != null)
                    {
                        if (!string.IsNullOrEmpty(entityPM.CustomerId))
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = 1,
                                PartnerId = entityPM.CustomerId,
                                PartnerType = "Customer",
                                PartnerContactId = entityPM.ContactId,
                            });
                        }
                    }

                    #endregion
                }

                else if (objectTableName == "Report")
                {
                    #region Report

                    GLAccountConnectedPartnerService gLAccountConnectedPartnerService = new GLAccountConnectedPartnerService(authToken.Tenant);
                    List<ShortPartnersDetails> connectedPartners = gLAccountConnectedPartnerService.GetAllConnectedPartnersByGLAccountId(gLAccountId);


                    list.Add(new EntityPartner()
                    {
                        Id = 1,
                        PartnerId = string.Join(",", connectedPartners.Select(a => a.PartnerId)),
                        PartnerType = "Customer Contacts"
                    });
                    #endregion
                }

                else if (objectTableName == "Customer")
                {
                    #region Customer
                    list.Add(new EntityPartner()
                    {
                        Id = 1,
                        PartnerId = entityId,
                        PartnerType = "Customer Contacts",

                    });
                    #endregion
                }

                else if (objectTableName == "ARInvoice" || objectTableName == "ARPayment" || objectTableName == "APInvoice" || objectTableName == "APPayment")
                {
                    #region
                    IInvoiceContext invoiceContext = InvoiceContext.GetContext(authToken.Tenant);

                    switch (objectTableName)
                    {
                        case "ARInvoice":
                            {
                                ARInvoice iEntity = (from d in invoiceContext.ARInvoices where d.Id == entityId select d).FirstOrDefault();
                                if (iEntity != null)
                                {
                                    if (iEntity.BillToId != null)
                                    {
                                        list.Add(new EntityPartner()
                                        {
                                            Id = 1,
                                            PartnerId = iEntity.BillToId,
                                            PartnerType = "Bill To",
                                        });
                                    }
                                }

                                break;
                            }

                        case "ARPayment":
                            {
                                ARPayment iEntity = (from d in invoiceContext.ARPayments where d.Id == entityId select d).FirstOrDefault();
                                if (iEntity != null)
                                {
                                    if (iEntity.BillToId != null)
                                    {
                                        list.Add(new EntityPartner()
                                        {
                                            Id = 1,
                                            PartnerId = iEntity.BillToId,
                                            PartnerType = "Bill To",
                                        });
                                    }
                                }

                                break;
                            }

                        case "APInvoice":
                            {
                                APInvoice iEntity = (from d in invoiceContext.APInvoices where d.Id == entityId select d).FirstOrDefault();
                                if (iEntity != null)
                                {
                                    if (iEntity.VendorId != null)
                                    {
                                        list.Add(new EntityPartner()
                                        {
                                            Id = 1,
                                            PartnerId = iEntity.VendorId,
                                            PartnerType = "Vendor",
                                        });
                                    }
                                }

                                break;
                            }

                        case "APPayment":
                            {
                                APPayment iEntity = (from d in invoiceContext.APPayments where d.Id == entityId select d).FirstOrDefault();
                                if (iEntity != null)
                                {
                                    if (iEntity.VendorId != null)
                                    {
                                        list.Add(new EntityPartner()
                                        {
                                            Id = 1,
                                            PartnerId = iEntity.VendorId,
                                            PartnerType = "Vendor",
                                        });
                                    }
                                }

                                break;
                            }
                    }

                    #endregion
                }
                else if(objectTableName == "InterestReport")
                {

                    IAccountingContext accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    InterestReport iEntity = (from d in accountingContext.InterestReports where d.Id == entityId select d).FirstOrDefault();
                    if (iEntity != null)
                    {
                        if (iEntity.CustomerId != null)
                        {
                            list.Add(new EntityPartner()
                            {
                                Id = 1,
                                PartnerId = iEntity.CustomerId,
                                PartnerType = "Bill To",
                            });
                        }
                    }

                }

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCalculatedFileNameForDocumentOutCopy(string documentOutId, string documentTypeCopyId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                DocumentOutQuery documentOutQuery = new DocumentOutQuery(authToken.Tenant);
                string fileName = documentOutQuery.GetCalculatedFileNameForDocumentOutCopy(documentOutId, documentTypeCopyId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static void Authentication(int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnEntityTenant("", tenant, authToken.Tenant);
        }




    }
}