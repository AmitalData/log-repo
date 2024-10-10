
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.CustomsMessaging.MessagingServices;
using static WebFreight.Web.Controllers.CustomsModel.Extended.CourierMasterController;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class MultiCertificatesController : ApiController
    {

        public HttpResponseMessage GetCertificateConnectedItems(string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);

                List<CertificateConnectedItems> connectedItems = queryService.GetCertificateConnectedItemsList(declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, connectedItems);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostCertificateTicket([FromBody]CertificateTicket entity) // entity  string declarationId, string invoiceNumber, string attachmentTypeCode, string certificateNumber, string resConfirmationTypeCode, string certificateExemptionTypeCode, string reqConfirmationTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
                List<CertificateConnectedItems> connectedItems = null;
                SupplierInvioceItemCertificatUpdateService updateService = new SupplierInvioceItemCertificatUpdateService(customContext);

                if (entity.IsAllSelected)
                {
                    connectedItems = queryService.GetCertificateConnectedItemsList(entity.DeclarationId, entity.oldAttachment, entity.ReqConfirmationTypeCode, entity.oldCertificateExempt, entity.oldCertificateNumber, entity.oldResConfirmation, tenant, entity.SearchFields);

                    if (!string.IsNullOrEmpty( entity.ExcludedItemsKeys))
                    {
                        string[] items = entity.ExcludedItemsKeys.Split(',');

                        foreach(string item in items)
                        {
                            string[] keys = item.Split(';');
                            var connetedItem = connectedItems.Where(d => d.DeclarationId == keys[0] && d.InvoiceCounterKey.ToString() == keys[1] && d.LineNumber.ToString() == keys[2] && d.ItemCertificateCounterKey.ToString() == keys[3]).FirstOrDefault();

                            connectedItems.Remove(connetedItem);


                        }
                    }
                    updateService.UpdateCertificateConnectedItems(connectedItems, entity.DeclarationId, entity.InvoiceNumber, entity.AttachmentTypeCode, entity.CertificateNumber, entity.ResConfirmationTypeCode, entity.CertificateExemptionTypeCode, entity.ReqConfirmationTypeCode, tenant);

                }

             
                else
                {
                    string[] values = entity.ConnectedItemsKeys.Split(',');

                    updateService.UpdateCertificateConnectedItems(values, entity.DeclarationId, entity.InvoiceNumber, entity.AttachmentTypeCode, entity.CertificateNumber, entity.ResConfirmationTypeCode, entity.CertificateExemptionTypeCode, entity.ReqConfirmationTypeCode, tenant);

                }


                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        // this method is not used, replaced with code in client (MultiCertificateWindow). Abdullah
        public HttpResponseMessage GetUpdateCertificatesBySearchFields(
            string declarationId,
            int invoiceCounterKey,
            string externalRequestTypeCode,
            string approvalRequestNumber,
             
            string reqConfirmationTypeCode,
            string attachmentTypeCode,
            string certificateNumber,
            string certificateExemptionTypeCode,
            string resConfirmationTypeCode
            )
        {
            try
            {
                if (certificateNumber == "null" || certificateNumber == "undefined") certificateNumber = null;
                if (certificateExemptionTypeCode == "null" || certificateExemptionTypeCode == "undefined") certificateExemptionTypeCode = null;
                if (resConfirmationTypeCode == "null" || resConfirmationTypeCode == "undefined") resConfirmationTypeCode = null;

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                    SupplierInvioceItemCertificatUpdateService updateService = new SupplierInvioceItemCertificatUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                    SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);

                    List<SupplierInvioceItemCertificatPM> certs = queryService.GetCertificatesBySearchFields(declarationId, invoiceCounterKey,externalRequestTypeCode, approvalRequestNumber, tenant);
                    
                    if(certs.Count > 0)
                    {
                        certs.ForEach(cert =>
                        {
                            // 1- update cert 
                            cert.ReqConfirmationTypeCode = reqConfirmationTypeCode;
                            cert.AttachmentTypeCode = attachmentTypeCode;
                            cert.CertificateNumber = certificateNumber;
                            cert.CertificateExemptionTypeCode = certificateExemptionTypeCode;
                            cert.ResConfirmationTypeCode = resConfirmationTypeCode;
                            cert.ChangeSetOp = ChangeSetOperation.Update;
                            updateService.Update(cert, true);

                            // 2- update supp invoice item
                           string status= updateService.UpdateCertificateStatus(cert, tenant);

                        });
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, certs.Count);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        } 


        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters, string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customs.CustomsRequestsSheet",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.CustomsRequestsSheets",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {

                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);


                       
                    }
                }
                //queryOperations.QueryFilterItems = filters;
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);


                SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
                List<CertificateConnectedItems> connectedItems = queryService.GetCertificateConnectedItems(queryOperations, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount && filters.PageIndex ==0)
                {
                    int count = queryService.GetListCount(queryOperations, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant);

                    response.Count = count;
                }


                response.Result = connectedItems;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCountByFilters([FromUri] ApiQueryFilters filters, string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customs.CustomsRequestsSheet",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.CustomsRequestsSheets",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {

                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);



                    }
                }
                //queryOperations.QueryFilterItems = filters;
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);


                SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
               
                ServiceResponse response = new ServiceResponse();
              
                    int count = queryService.GetListCount(queryOperations, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant);

                   
               


              
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, count);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage PutSupplierInvoiceItemCatalogNumber(CertificateConnectedItems connectedItem) 
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                customContext = CustomContext.GetContext(tenant);
                SupplierInvoiceItemQueryService itemQueryService = new SupplierInvoiceItemQueryService(customContext);
                itemQueryService.LoadComposition = true;
                SupplierInvoiceItemPM itemPM = itemQueryService.GetSingle(connectedItem.DeclarationId, connectedItem.InvoiceCounterKey, connectedItem.LineNumber, true, false);
                SupplierInvoiceItemsProdIdentUpdateService updateService = new SupplierInvoiceItemsProdIdentUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                SupplierInvoiceItemUpdateService itemupdateService = new SupplierInvoiceItemUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                
                var identification = itemPM.SupplierInvoiceItemsProdIdents.Where(d => d.TypeCode == "MN").FirstOrDefault();
                if (connectedItem?.CatalogNumber != null) {
                    if (identification != null)
                    {
                        identification.Identification = connectedItem.CatalogNumber;
                        identification.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        updateService.Update(identification, true);
                    }
                    else
                    {
                        int line = 0;
                        if (itemPM.SupplierInvoiceItemsProdIdents.Count > 0)
                        {

                            line = itemPM.SupplierInvoiceItemsProdIdents.Max(d => d.LineNumber);

                        }

                        line += 1;
                        var item = new SupplierInvoiceItemsProdIdentPM();

                        item.DeclarationId = connectedItem.DeclarationId;
                        item.Tenant = tenant;
                        item.InvoiceCounterKey = connectedItem.InvoiceCounterKey;
                        item.InvoiceItemLineNumber = itemPM.LineNumber;
                        item.LineNumber = line;
                        item.Identification = connectedItem.CatalogNumber;
                        item.TypeCode = "MN";
                        item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        //  updateService.Update(identification, true);
                        itemPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        itemPM.SupplierInvoiceItemsProdIdents.Add(item);
                        itemupdateService.Update(itemPM, true);

                    }
                }
               

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage  GetDeclarationHasInvoices(string declarationId )
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                SupplierInvoiceQueryService itemQueryService = new SupplierInvoiceQueryService(customContext);
                SupplierInvoiceItemQueryService invoiceItemQuery = new SupplierInvoiceItemQueryService(customContext);

                int invoiceCount = itemQueryService.GetSupplierInvoiceCountForDeclaration(declarationId, tenant);
                int  invoiceItemPMsCount = invoiceItemQuery.GetDeclarationCountOfSupplierInvoiceItems(tenant, declarationId);
                bool hasInvoices;
                if (invoiceCount == 0 || invoiceItemPMsCount == 0)
                {
                    hasInvoices = false;
                }
                else
                {
                    hasInvoices = true;
                }

                return Request.CreateResponse(HttpStatusCode.OK, hasInvoices);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetUpdateAllCertificateWithoutResponse(string declarationId,string customFileNo)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                //SupplierInvioceItemCertificatUpdateService updateService = new SupplierInvioceItemCertificatUpdateService(customContext);

                //var count = updateService.UpdateAllCertificateWithoutResponse(declarationId, tenant);
                var messagingService = new DCAInUCBUpdateAllCertificateWithoutResponse_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, declarationId, customFileNo);
               
                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCreateCertificateForInvoiceItems(string declarationId, string customFileNo, string attachmentTypeCode,
            string reqConfirmationTypeCode, string resConfirmationTypeCode, string certificateNumber, string certificateExemptionTypeCode, string selectedInvoiceItemsKeys)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                
                var messagingService = new DCAInUCBCreateCertificateForInvoiceItems_MsgMessagingService();
                string RequestInProgressList;
                var sts = messagingService.CreateCRS(
                    tenant, null, declarationId, customFileNo, attachmentTypeCode, reqConfirmationTypeCode, resConfirmationTypeCode, certificateNumber, certificateExemptionTypeCode, selectedInvoiceItemsKeys, out RequestInProgressList);
                DataResult result = new DataResult();
                result.RequestInProgressList = RequestInProgressList;
                result.Message = sts;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}