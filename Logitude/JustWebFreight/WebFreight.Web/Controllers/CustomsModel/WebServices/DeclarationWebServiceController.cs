using Logitude.AmitalMessaging.Customs.CustomFile;

using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrection;
using Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Logitude.CustomsMessaging.ResponseServices;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class DeclarationWebServiceController : ApiController
    {
        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
        AmitalContext GetAmitalContext(int tenant)
        {
            var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
            if (tenantAmitalContext == null)
            {

                tenantAmitalContext = AmitalContext.GetContext(tenant);
                _AmitalContextList.Add(tenantAmitalContext);
            }
            return tenantAmitalContext;
        }

        private Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService supplierInvoiceQuery;

        public HttpResponseMessage GetDeclarationConstraintsByDeclrationId(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationConstraintQueryService query = new DeclarationConstraintQueryService(customContext);
                List<DeclarationConstraintPM> list = query.GetDeclarationConstraintsByDeclrationId(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationErrors(string declarationId, string listVersionId, string courierFilter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService query = new DeclarationQueryService(customContext);
                List<DeclarationErrorView> list
                    = query.GetDeclarationErrors(declarationId == "undefined" ? null : declarationId
                                        , tenant, listVersionId == "undefined" ? null : listVersionId, courierFilter);

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleCustomsCollateral(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsCollateralQueryService customsCollateralQuery = new CustomsCollateralQueryService(customContext);
                PaymentOrderQueryService paymentOrderQueryService = new PaymentOrderQueryService(customContext);
                CustomsCollateralPM customsCollateral = customsCollateralQuery.GetSingle(id, true, false);
                CustomsCollateralsAnswerPM answer = customsCollateral.CustomsCollateralsAnswers.Where(d => d.PaymentOrderId != null).FirstOrDefault();
                if (answer != null)
                {
                    PaymentOrderPM paymentOrder = paymentOrderQueryService.GetSingle(answer.PaymentOrderId, false, false);
                    customsCollateral.PaymentNumber = paymentOrder.PaymentNumber;
                    customsCollateral.PaymentOrderId = paymentOrder.Id;
                }
                return Request.CreateResponse(HttpStatusCode.OK, customsCollateral);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCheckIfDocumentPointerExistsForConstraint(string constraintNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsDocumentPointerQueryService customsDocumentPointerQuery = new CustomsDocumentPointerQueryService(customContext);

                var exist = customsDocumentPointerQuery.CheckIfDocumentPointerExistsForConstraint(constraintNumber, "Constraint", tenant);

                return Request.CreateResponse(HttpStatusCode.OK, exist);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendDeclarationConstraint(ConstraintApprovalRequestParams requestParamsData)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            string loggedUserEmail = authToken.Email;

            try
            {
                ConstraintApprovalRequestParams requestParams = requestParamsData;
                INF_MSG_GenericResponseData responseData = null;
                if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
                {
                    responseData = new INF_MSG_GenericResponseData();
                    switch (requestParams.TestCase.Code)
                    {
                        case "Send Succeeded":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;
                                break;
                            }
                        case "Send Failed":
                            {
                                responseData.HasException = true;
                                responseData.Succeeded = false;
                                responseData.UserMessage = "this is a test fail exception for send declaration constraint!";
                                break;
                            }

                    }

                }
                else
                {
                    // use messageing service
                    var messagingService = new EV_NG_8214_MSG23001_ConstraintApprovalRequestMessagingService();
                    responseData = messagingService.Send(requestParams);
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSupplierInvoiceBySequenceNumber(string declarationId, int invoiceSequence, int skip, int take)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                supplierInvoiceQuery = new Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService(customContext);
                SupplierInvoicePM supplierInvoice = supplierInvoiceQuery.GetSupplierInvoiceBySequenceNumber(declarationId, invoiceSequence, skip, take);

                return Request.CreateResponse(HttpStatusCode.OK, supplierInvoice);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSupplierInvoiceWithItemBySequenceNumber(string declarationId, int invoiceSequence, int itemSequence, int skip, int take, string type=null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                supplierInvoiceQuery = new Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService(customContext);
                SupplierInvoicePM supplierInvoice = supplierInvoiceQuery.GetSupplierInvoiceWithSpecificItemBySequenceNumber(declarationId, invoiceSequence, itemSequence, type);


                return Request.CreateResponse(HttpStatusCode.OK, supplierInvoice);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    
        public HttpResponseMessage PostSendCollateralAnswers(CollateralRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;


                // use messageing service

                var service = new COLT_NG_8212_MSG10041_CollateralAnswerMsgMessagingService();
                responseData = service.Send(requestParamsData);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage PostSendDeclarationConstraintAgentObjection(ConstraintAgentObjectionRequestParams requestParams)
        {
            try
            {

                ConstraintAgentAnswerResponseData responseData = null;
                if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
                {
                    responseData = new ConstraintAgentAnswerResponseData();
                    switch (requestParams.TestCase.Code)
                    {
                        case "Send Succeeded":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;

                                break;
                            }
                        case "Send Failed":
                            {
                                responseData.HasException = true;
                                responseData.Succeeded = false;
                                responseData.UserMessage = "this is a test fail exception for send declaration constraint!";
                                break;
                            }

                    }

                }
                else
                {
                    // use messageing service
                    var messagingService = new EV_NG_8216_MSG23003_ConstraintAgentAnswerMessagingService();
                    responseData = messagingService.Send(requestParams);
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


        // Certificates - Tickets
        public HttpResponseMessage GetCertificateTickets(string declarationId, string reqConfirmationType, string invoiceNumber, int? invoiceCounterKey, string demandState)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                reqConfirmationType = reqConfirmationType == "null" ? null : reqConfirmationType;
                invoiceNumber = invoiceNumber == "null" ? null : invoiceNumber;
                demandState = demandState == "null" ? null : demandState;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
                List<CertificateTicket> tickets = queryService.GetDeclarationCertificateTicket(declarationId, reqConfirmationType, invoiceNumber, invoiceCounterKey, demandState, tenant).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, tickets);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDeclarationInvoicesNumbers(string declarationId)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);

                SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
                List<CertificateConnectedItems> numbers = queryService.GetDeclarationInvoicesNumbers(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, numbers);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostNewAmendmentDeclaration(GenericRequestParams requestParams)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
 
                DF_MSG10000_ImportDeclarationRequestService _dF_MSG10000_ImportDeclarationRequestService = new DF_MSG10000_ImportDeclarationRequestService();
                var request = _dF_MSG10000_ImportDeclarationRequestService.GetRequest(requestParams);
                string error="";
                DF_NG_2754_MSG10004_ImportFixedDeclarationResponseService dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService = new DF_NG_2754_MSG10004_ImportFixedDeclarationResponseService();

                DeclarationPM declarationPM =    dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService.MapResponseToDeclaration(request.Declaration, requestParams.Tenant, true , out error);

                XmlSerializer xsSubmit = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration));
 

                if (declarationPM != null)
                return Request.CreateResponse(HttpStatusCode.OK, declarationPM);

                return Request.CreateResponse(HttpStatusCode.BadRequest, error);


            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage PostSendDeclaration(GenericRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData;
                var messagingService = new DF_MSG10000_ImportDeclarationMessagingService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage PostSendDeclarationAmendment(GenericRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData;
                var messagingService = new DF_MSG10000_ImportDeclarationMessagingService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


        public HttpResponseMessage PostSendManifest(MANIFESTRequestRequestParams requestParamsData)
        {
            try
            {
                MANIFESTRequestResponseData responseData = null;
                var messagingService = new MN_MSG1_MANIFESTMessagingService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSendDeclarationChecksAndPrecalculations(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext myContext = CustomContext.GetContext(tenant);
                DeclarationUpdateService updateService = new DeclarationUpdateService(myContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                SendDeclarationChecksResult result = updateService.DoSendDeclarationChekcs(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRequiredFieldsForDeclaration(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, errors);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            
        }

        public HttpResponseMessage GetRequiredFieldsForCourierDeclaration(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForCourierDeclaration(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, errors);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetMAWBCourierMasterByDeclaration(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CourierDeclarationQueryService courierDeclarationQueryService = new CourierDeclarationQueryService(tenant);
                var MAWBCourierMaster = courierDeclarationQueryService.GetMAWBCourierMasterByDeclarationId(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, MAWBCourierMaster);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetCustomsPartnersItemsForSelection(string vendorId, string CustomerId)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsPartnersItemQueryService customsPartnersItemQuery = new CustomsPartnersItemQueryService(customContext);
                List<CustomsPartnersItemList> customsPartnersItems = customsPartnersItemQuery.GetItemsByVendorOrCustomer(vendorId, CustomerId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, customsPartnersItems);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //begin uni
        public HttpResponseMessage GetGTBITEMPartnersItemList(string vendorId, string customerCode, string search, int top)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (search != null && (search.ToLower() == "undefined" || search.ToLower() == "null"))
                    search = null;

                #region get data from Unifri

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }


                var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
                var itemRepo = new GTBITEMRepository(GetAmitalContext(tenant));

                string partner = GetDefault("ISRAEL", "CIM_SIVUG_103", "NON", customerCode, tenant); // S=Supplier I=Client
                if (partner == "S") // If Supplier get Unifreight card
                {
                    customerCode = GetDefaultAccountNumber("ISRAEL", "CEX_CUS_SUP", "NON", customerCode, tenant);
                }

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }


                var cardDetails = cardRepo.GetAll().Where(rec => rec.CARDID == customerCode).FirstOrDefault();
                var q =
                from itm in itemRepo
                    .GetAll()
                    .Select(rec => new CustomsPartnersItemList()
                    {
                        Id = rec.ITEMID,
                        ClassificationCode = rec.PRATID,
                        ItemCode = rec.ITEMID,
                        CustomerId = rec.PARTNERID,
                        CustomerName = rec.PARTNERID,
                        Name = rec.DESCRIPTION,
                        VendorId = rec.PARTNERID,
                        SearchFields = rec.SEARCHENG
                    })
                select new { itm };
                if (!string.IsNullOrWhiteSpace(customerCode))
                {
                    q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
                }
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToUpper();
                    q = q.Where(rec => rec.itm.ItemCode.Contains(search) | rec.itm.SearchFields.Contains(search));
                }
                q = q.Distinct();
                q = q.Take(top);

                var aynList = q.ToList();
                var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();

                #endregion

                return Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            var myGDFDATAQueryService = new GDFDATAQueryService(GetAmitalContext(tenant));

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }
        private string GetDefaultAccountNumber(string DISTRID, string DEFID, string BRANCHID, string SHORTDEFDATA, int tenant)
        {
            var myGDFDATAQueryService = new GDFDATAQueryService(GetAmitalContext(tenant));

            if (DISTRID == null || DEFID == null || BRANCHID == null || SHORTDEFDATA == null)
            {
                return ("");
            }

            string accountNumber = myGDFDATAQueryService.GetCardIdByDefaultValue(DISTRID, DEFID, BRANCHID, SHORTDEFDATA);

            return (accountNumber);
        }
        private CustomsPartnersItemList GetCustomsPartnersItemList(CustomsPartnersItemList item, GNDCARD card, int tenant)
        {
            var crd = card ?? new GNDCARD();
            item.VendorName = crd.NAMEENG;

            if(!string.IsNullOrEmpty(item.OriginCountryCode))
            {
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(item.OriginCountryCode, false, true);
                if(country != null)
                {
                    item.OriginCountryName = country.LocalName;
                }
                else
                {
                    item.OriginCountryCode = null;
                    item.OriginCountryName = null;
                }
            }
            return item;

        }
        //END inu

        
        public HttpResponseMessage GetGITITEMPartnersItemList(string vendorId, string customerCode, string search, int top, bool searchNULLVendor = true)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (search != null && (search.ToLower() == "undefined" || search.ToLower() == "null"))
                {
                    search = null;
                }

                if (vendorId != null && (vendorId.ToLower() == "undefined" || vendorId.ToLower() == "null"))
                {
                    vendorId = null;
                }

                #region get data from Unifri

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
                //var vendorRepo = new CTBCUSTSUPRepository(GetAmitalContext(tenant));
                var itemRepo = new GITITEMRepository(GetAmitalContext(tenant));

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var cardDetails = cardRepo.GetAll().Where(rec => rec.CARDID == customerCode).FirstOrDefault();
                var q =
                from itm in itemRepo
                    .GetAll().Where(rec => rec.ITEMCANCELLED != "T")
                    .Select(rec => new CustomsPartnersItemList()
                    {
                        Id = rec.COUNTER.ToString(),
                        ClassificationCode = rec.PRATID,
                        ItemCode = rec.ITEMNO,
                        CustomerId = rec.PARTNERID,
                        CustomerName = rec.PARTNERID,
                        Name = rec.NAMEENG,
                        VendorId = rec.SAPAKID,
                        SearchFields = rec.SEARCHENG,
                        OriginCountryCode = rec.ORIGINCOUNTRY,
                        InvoiceQuantityType = rec.UNITID,
                    })
                select new { itm };
                if (!string.IsNullOrWhiteSpace(customerCode))
                {
                    q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
                }

                if (searchNULLVendor)
                {
                    if (!string.IsNullOrWhiteSpace(vendorId))
                    {
                        q = q.Where(rec => rec.itm.VendorId.Equals(vendorId) || rec.itm.VendorId.Equals("NULL"));
                    }
                    else
                    {
                        q = q.Where(rec => rec.itm.VendorId.Equals("NULL"));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(vendorId))
                {
                    q = q.Where(rec => rec.itm.VendorId.Equals(vendorId));
                }
                
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToUpper();
                    q = q.Where(rec => rec.itm.ItemCode.Contains(search) ||
                    rec.itm.ClassificationCode.Contains(search) ||
                    rec.itm.SearchFields.Contains(search) ||
                    rec.itm.VendorId.Contains(search));
                }
                q = q.Distinct();
                q = q.Take(top);

                var aynList = q.ToList();
                var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();

                #endregion

                return Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetGITITEMPartnersItemListByItemCode(string vendorId, string customerCode, string itemCode, int top, bool searchNULLVendor = true)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (itemCode == null && (itemCode != null && (itemCode.ToLower() == "undefined" || itemCode.ToLower() == "null")))
                {
                    return null;
                }
                if (vendorId != null && (vendorId.ToLower() == "undefined" || vendorId.ToLower() == "null"))
                {
                    vendorId = null;
                }              

                #region get data from Unifri

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
                var itemRepo = new GITITEMRepository(GetAmitalContext(tenant));

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var cardDetails = cardRepo.GetAll().Where(rec => rec.CARDID == customerCode).FirstOrDefault();
                var q =
                from itm in itemRepo
                    .GetAll().Where(rec => rec.ITEMCANCELLED != "T")
                    .Select(rec => new CustomsPartnersItemList()
                    {
                        Id = rec.COUNTER.ToString(),
                        ClassificationCode = rec.PRATID,
                        ItemCode = rec.ITEMNO,
                        CustomerId = rec.PARTNERID,
                        CustomerName = rec.PARTNERID,
                        Name = rec.NAMEENG,
                        VendorId = rec.SAPAKID,
                        SearchFields = rec.SEARCHENG,
                        OriginCountryCode = rec.ORIGINCOUNTRY,
                        InvoiceQuantityType = rec.UNITID,
                    })
                select new { itm };
                if (!string.IsNullOrWhiteSpace(customerCode))
                {
                    q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
                }

                if (!string.IsNullOrWhiteSpace(vendorId))
                {
                    if (searchNULLVendor)
                    {
                        q = q.Where(rec => rec.itm.VendorId.Equals(vendorId) || rec.itm.VendorId.Equals("NULL"));
                    }
                    else
                    {
                        q = q.Where(rec => rec.itm.VendorId.Equals(vendorId));
                    }
                }
                else
                {
                    q = q.Where(rec => rec.itm.VendorId.Equals("NULL"));
                }

                q = q.Where(rec => rec.itm.ItemCode.Equals(itemCode)); // Contains
                q = q.Distinct();
                q = q.Take(top);

                var aynList = q.ToList();
                var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();

                #endregion

                return Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetGITITEMPartnersItemListByName(string vendorId, string customerCode, string name, int top)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (string.IsNullOrWhiteSpace(name) && (name != null && (name.ToLower() == "undefined" || name.ToLower() == "null")))
                {
                    return null;
                }
                if (vendorId != null && (vendorId.ToLower() == "undefined" || vendorId.ToLower() == "null"))
                {
                    vendorId = null;
                }

                #region get data from Unifri

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
                var itemRepo = new GITITEMRepository(GetAmitalContext(tenant));

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var cardDetails = cardRepo.GetAll().Where(rec => rec.CARDID == customerCode).FirstOrDefault();

                var q =
                from itm in itemRepo
                    .GetAll().Where(rec => rec.ITEMCANCELLED != "T")
                    .Select(rec => new CustomsPartnersItemList()
                    {
                        Id = rec.COUNTER.ToString(),
                        ClassificationCode = rec.PRATID,
                        ItemCode = rec.ITEMNO,
                        CustomerId = rec.PARTNERID,
                        CustomerName = rec.PARTNERID,
                        Name = rec.NAMEENG,
                        VendorId = rec.SAPAKID,
                        SearchFields = rec.SEARCHENG,
                        OriginCountryCode = rec.ORIGINCOUNTRY,
                        InvoiceQuantityType = rec.UNITID,
                    })
                select new { itm };
                if (!string.IsNullOrWhiteSpace(customerCode))
                {
                    q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
                }

                if (!string.IsNullOrWhiteSpace(vendorId))
                {
                    q = q.Where(rec => rec.itm.VendorId.Equals(vendorId) || rec.itm.VendorId.Equals("NULL"));
                }
                else
                {
                    q = q.Where(rec => rec.itm.VendorId.Equals("NULL"));
                }

                name = name.ToUpper();
                q = q.Where(rec => rec.itm.SearchFields.Contains(name));
                q = q.Distinct();
                q = q.Take(top);

                var aynList = q.ToList();
                var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();

                #endregion

                return Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        //public HttpResponseMessage GetGITITEMPartnersItemListByItemCode(string vendorId, string customerCode, string itemCode, int top)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;

        //        if (itemCode == null && (itemCode != null && (itemCode.ToLower() == "undefined" || itemCode.ToLower() == "null")))
        //        {
        //            return null;
        //        }

        //        #region get data from Unifri

        //        if (string.IsNullOrWhiteSpace(customerCode))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest);
        //        }

        //        var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
        //        var itemRepo = new GITITEMRepository(GetAmitalContext(tenant));

        //        if (string.IsNullOrWhiteSpace(customerCode))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest);
        //        }

        //        var cardDetails = cardRepo.GetAll().Where(rec => rec.CARDID == customerCode).FirstOrDefault();
        //        var q =
        //        from itm in itemRepo
        //            .GetAll()
        //            .Select(rec => new CustomsPartnersItemList()
        //            {
        //                Id = rec.COUNTER.ToString(),
        //                ClassificationCode = rec.PRATID,
        //                ItemCode = rec.ITEMNO,
        //                CustomerId = rec.PARTNERID,
        //                CustomerName = rec.PARTNERID,
        //                Name = rec.NAMEENG,
        //                VendorId = rec.SAPAKID,
        //                SearchFields = rec.SEARCHENG
        //            })
        //        select new { itm };
        //        if (!string.IsNullOrWhiteSpace(customerCode))
        //        {
        //            q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
        //        }

        //        if (!string.IsNullOrWhiteSpace(vendorId))
        //        {
        //            q = q.Where(rec => rec.itm.VendorId.Equals(vendorId) || rec.itm.VendorId.Equals("NULL"));
        //        }
        //        else
        //        {
        //            q = q.Where(rec => rec.itm.VendorId.Equals("NULL"));
        //        }

        //        q = q.Where(rec => rec.itm.ItemCode.Contains(itemCode));
        //        q = q.Distinct();
        //        q = q.Take(top);

        //        var aynList = q.ToList();
        //        var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();

        //        #endregion

        //        return Request.CreateResponse(HttpStatusCode.OK, l);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}

        //public HttpResponseMessage GetGITITEMPartnersItemListByName(string vendorId, string customerCode, string name, int top)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;

        //        if (string.IsNullOrWhiteSpace(name) && (name != null && (name.ToLower() == "undefined" || name.ToLower() == "null")))
        //        {
        //            return null;
        //        }

        //        #region get data from Unifri

        //        if (string.IsNullOrWhiteSpace(customerCode))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest);
        //        }

        //        var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
        //        var itemRepo = new GITITEMRepository(GetAmitalContext(tenant));

        //        if (string.IsNullOrWhiteSpace(customerCode))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest);
        //        }

        //        var cardDetails = cardRepo.GetAll().Where(rec => rec.CARDID == customerCode).FirstOrDefault();
        //        var q =
        //        from itm in itemRepo
        //            .GetAll()
        //            .Select(rec => new CustomsPartnersItemList()
        //            {
        //                Id = rec.COUNTER.ToString(),
        //                ClassificationCode = rec.PRATID,
        //                ItemCode = rec.ITEMNO,
        //                CustomerId = rec.PARTNERID,
        //                CustomerName = rec.PARTNERID,
        //                Name = rec.NAMEENG,
        //                VendorId = rec.SAPAKID,
        //                SearchFields = rec.SEARCHENG
        //            })
        //        select new { itm };
        //        if (!string.IsNullOrWhiteSpace(customerCode))
        //        {
        //            q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
        //        }

        //        if (!string.IsNullOrWhiteSpace(vendorId))
        //        {
        //            q = q.Where(rec => rec.itm.VendorId.Equals(vendorId) || rec.itm.VendorId.Equals("NULL"));
        //        }
        //        else
        //        {
        //            q = q.Where(rec => rec.itm.VendorId.Equals("NULL"));
        //        }

        //        q = q.Where(rec => rec.itm.Name.Contains(name));
        //        q = q.Distinct();
        //        q = q.Take(top);

        //        var aynList = q.ToList();
        //        var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();

        //        #endregion

        //        return Request.CreateResponse(HttpStatusCode.OK, l);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}

        public HttpResponseMessage GetSupplierInvoiceWithSpecificItemByCounterKey( string declarationId, int counterKey, int itemSequence)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                supplierInvoiceQuery = new Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService(customContext);
                SecurityUtility.AuthenticationOnTenant(tenant);
           
                SupplierInvoicePM supplierInvoice = supplierInvoiceQuery.GetSupplierInvoiceWithSpecificItemByCounterKey(declarationId, counterKey, itemSequence,"certificate");

                supplierInvoice.IsAccumalated = false;
                return Request.CreateResponse(HttpStatusCode.OK, supplierInvoice);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCheckCertificateStatus(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext myContext = CustomContext.GetContext(tenant);
                DeclarationQueryService queryService = new DeclarationQueryService(myContext);
                SecurityUtility.AuthenticationOnTenant(tenant);
                bool exist =  SecurityUtility.CheckFeature("Customs.Declaration", "IKEA", tenant);

                CustomsRequiredFieldErrors result = queryService.CheckCertificateStatus(declarationId, tenant, exist);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentDeclarationId(string DeclarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsDocumentQueryService customsDocumentQuery = new CustomsDocumentQueryService(customContext);
                string DeclarationVersion = null;
                string DocumentDeclarationId  = customsDocumentQuery.GetDocumentDeclarationId(DeclarationId, tenant,out DeclarationVersion);

               

                return Request.CreateResponse(HttpStatusCode.OK,
                    new
                    {
                        DocumentDeclarationId = DocumentDeclarationId,
                        DeclarationVersion = DeclarationVersion
                    });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationDocumentList(string parentEntityId, string parentEntityCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsDocumentQueryService customsDocumentQuery = new CustomsDocumentQueryService(customContext);
                List<CustomsDocumentPM> documents = customsDocumentQuery.GetDeclarationDocumentList(parentEntityId, parentEntityCode, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, documents);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationMandatoryTicketList(string parentEntityId, string parentEntityCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                //CustomsDocumentQueryService customsDocumentQuery = new CustomsDocumentQueryService(customContext);
                //List<CustomsDocumentPM> documents = customsDocumentQuery.GetDeclarationMandatoryTicketList(parentEntityId, parentEntityCode, tenant);

                //return Request.CreateResponse(HttpStatusCode.OK, documents);
                var query = new DeclarationQueryService(customContext);
                var myCustomsDocumentsTicketPMList =query.GetDeclarationMandatoryTicket(parentEntityId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myCustomsDocumentsTicketPMList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCheckFreightAmountsByIncoterm(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext myContext = CustomContext.GetContext(tenant);
                DeclarationQueryService queryService = new DeclarationQueryService(myContext);
                var result = queryService.CheckFreightAmountsByIncoterm(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCheckFreightAmountsByIncotermWithDefault(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext myContext = CustomContext.GetContext(tenant);
                DeclarationQueryService queryService = new DeclarationQueryService(myContext);

                string isNoIncotermCheck = GetDefault("ISRAEL", "CGG_NO_INC_CHK", "NON", "NON", tenant); 
                if (isNoIncotermCheck == "Y")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                var result = queryService.CheckFreightAmountsByIncoterm(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationClosureMethod(string declarationId, int tenant)
        {
            try
            {
                var mess = DeclarationUpdateService.DeclarationClosure(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, mess);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCancelDeclarationClosureMethod(string declarationId, int tenant)
        {
            try
            {
                var mess = DeclarationUpdateService.CancelDeclarationClosure(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, mess);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //declaration payment
        public HttpResponseMessage GetSingleDeclarationPaymentPMandDefaultExplain(string id, string CustomerCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                
                var declarationPaymentPM = GetSingleDeclarationPaymentPM(id, tenant);
                if (!string.IsNullOrWhiteSpace(CustomerCode))
                {
                    if (declarationPaymentPM == null || declarationPaymentPM.DeclarationPaymentProtests == null || declarationPaymentPM.DeclarationPaymentProtests.Count == 0 || string.IsNullOrWhiteSpace(declarationPaymentPM.DeclarationPaymentProtests.FirstOrDefault().CustomsAgentExplanation))
                    {
                        var declarationQS = new DeclarationQueryService(customContext);

                        string customsAgentExplanationDefault = declarationQS.GetDefault("ISRAEL", "CIM_PROTEST_PAY", "NON", CustomerCode, tenant);

                        if (!string.IsNullOrWhiteSpace(customsAgentExplanationDefault))
                        {
                            if (declarationPaymentPM == null)
                            {
                                declarationPaymentPM = new DeclarationPaymentPM()
                                {
                                    //DeclarationId = id,
                                    //Tenant = tenant,
                                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                    CustomsAgentExplanationDefault = customsAgentExplanationDefault,
                                };
                            }

                            else if (declarationPaymentPM.DeclarationPaymentProtests == null || declarationPaymentPM.DeclarationPaymentProtests.Count() == 0 || string.IsNullOrWhiteSpace(declarationPaymentPM.DeclarationPaymentProtests.FirstOrDefault().CustomsAgentExplanation))
                            {
                                declarationPaymentPM.CustomsAgentExplanationDefault = customsAgentExplanationDefault;
                            }
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, declarationPaymentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private DeclarationPaymentPM GetSingleDeclarationPaymentPM(string id, int tenant)
        {
            ICustomContext customContext = CustomContext.GetContext(tenant);
            DeclarationPaymentQueryService declarationPaymentQuery = new DeclarationPaymentQueryService(customContext);
            DeclarationPaymentPM declarationPayment = declarationPaymentQuery.GetSingle(id, true, false);
            return declarationPayment;
        }
        public HttpResponseMessage GetCustomBanksForCard(string cardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);

                CustomBankQueryService customBankQuery = new CustomBankQueryService(customContext);
                List<CustomBankList> customBanks = customBankQuery.GetCustomBanksByCard(cardId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, customBanks);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //send
        public HttpResponseMessage PostSendPaymentWithCheckCustomFileCredit(CustomFileCreditRequestParams requestParamsCredit)
        {
            try
            {
                CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();

                DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsCredit.Tenant);
                DeclarationPM declarationPM = declarationQueryService.GetSingleDeclarationById(requestParamsCredit.AppicationId, requestParamsCredit.Tenant);
                if (declarationPM != null && declarationPM.IsConnectedToUnifreight)
                {
                    try
                    {
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשה לבדיקת אשראי");
                        var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
                        CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "ניתוח תשובה בקרת אשראי");
                        responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
                        //if (creditResponseData.CustomFileCredit[0].ErrorMessage != null || creditResponseData.CustomFileCredit[0].ErrorMessage != "")
                        if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
                        {
                            responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
                            responseData.HasException = true;
                        }
                        responseData.Succeeded = true;
                        responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
                        responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
                        responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
                        responseData.IsTRansGove = false;
                    }
                    catch (Exception e)
                    {
                        responseData.CreditStatus = "0";
                        responseData.Succeeded = true;
                        responseData.HasException = true;
                        responseData.UserMessage = e.ToString();
                    }
                }
                else
                {
                    responseData.CreditStatus = "5";
                    responseData.IsTRansGove = false;
                }

                if (responseData.CreditStatus == "5" | responseData.CreditStatus == "3")
                {
                    ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "תחילת שליחה למכס- הגשת תשלום");
                    GenericRequestParams submitRequestParams = new GenericRequestParams();
                    submitRequestParams.AppicationId = requestParamsCredit.AppicationId;
                    submitRequestParams.InterfaceTypeCode = requestParamsCredit.InterfaceTypeCode;
                    submitRequestParams.PBId = requestParamsCredit.PBId;
                    submitRequestParams.CustomsRequestsSheetId = requestParamsCredit.CustomsRequestsSheetId;
                    submitRequestParams.Tenant = requestParamsCredit.Tenant;
                    submitRequestParams.RequestVIA = requestParamsCredit.RequestVIA;
                    submitRequestParams.LoggingUserId = requestParamsCredit.LoggingUserId;
                    submitRequestParams.ForcePersonalSign = requestParamsCredit.ForcePersonalSign;

                    submitRequestParams.LoggingEntityId = requestParamsCredit.LoggingEntityId;
                    submitRequestParams.LoggingEntityId2 = requestParamsCredit.LoggingEntityId2;
                    submitRequestParams.LoggingObjectTableId = requestParamsCredit.LoggingObjectTableId;
                    submitRequestParams.LoggingObjectTableId2 = requestParamsCredit.LoggingObjectTableId2;

                    var messagingService = new 
                        DF_NG_2755_MSG12001_SubmitDeclarationMessagingService();
                    INF_MSG_GenericResponseData submitResponseData = messagingService.Send(submitRequestParams);
                    responseData.Succeeded = submitResponseData.Succeeded;
                    responseData.HasException = submitResponseData.HasException;
                    responseData.UserMessage = submitResponseData.UserMessage;
                    responseData.ContinueProcessInBackground = submitResponseData.ContinueProcessInBackground;
                }
                else if (responseData.CreditStatus == "1")
                {
                    responseData.IsTRansGove = true;
                }
                else if (responseData.CreditStatus == "6")
                {
                    responseData.IsReTRansGove = true;
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetAllRequiredFieldsForDeclarationPayment(string declarationId)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclarationPayment(declarationId, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, errors);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage PostSendTransferRequest(CustomFileCreditRequestParams requestParamsCredit)
        {
            try
            {
                CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();

                DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsCredit.Tenant);
                DeclarationPM declarationPM = declarationQueryService.GetSingleDeclarationById(requestParamsCredit.AppicationId, requestParamsCredit.Tenant);
                if (declarationPM != null && declarationPM.IsConnectedToUnifreight)
                {
                    try
                    {
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשת העברה לגובה");
                        var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
                        CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשת העברה לגובה");
                        responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
                        //if (creditResponseData.CustomFileCredit[0].ErrorMessage != null || creditResponseData.CustomFileCredit[0].ErrorMessage != "")
                        if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
                        {
                            responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
                            responseData.HasException = true;
                        }
                        responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
                        responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
                        responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
                        responseData.IsTRansGove = false;
                    }
                    catch (Exception e)
                    {
                        responseData.CreditStatus = "0";
                        responseData.Succeeded = false; ;
                        responseData.HasException = true;
                        responseData.UserMessage = e.ToString();
                    }
                }
                else
                {
                    responseData.CreditStatus = "5";
                    responseData.IsTRansGove = false;
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage GetDeclarationCorrection(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService query = new DeclarationQueryService(customContext);
                DeclarationCorrectionView correction = query.GetDeclarationCorrection(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, correction);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationByTapagConnectionConnection(string tapagId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService query = new DeclarationQueryService(customContext);
                List<DeclarationList> declarationList = query.GetTapagDeclarations(tapagId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, declarationList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage //GetCustomFileCredit(string DeclarationNumber, string DeclarationId, string LoggingUserId,int Tenant)
            PostCheckCustomFileCreditOnly(CustomFileCreditRequestParams requestParamsCredit)
        {
            try
            {
                CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();

                DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsCredit.Tenant);
                DeclarationPM declarationPM = declarationQueryService.GetSingleDeclarationById(requestParamsCredit.AppicationId, requestParamsCredit.Tenant);
                if (declarationPM != null && declarationPM.IsConnectedToUnifreight)
                {
                    try
                    {
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשה לבדיקת אשראי");
                        var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
                        CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "ניתוח תשובה בקרת אשראי");
                        responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
                        //if (creditResponseData.CustomFileCredit[0].ErrorMessage != null || creditResponseData.CustomFileCredit[0].ErrorMessage != "")
                        if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
                        {
                            responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
                            responseData.HasException = true;
                        }
                        responseData.Succeeded = true;
                        responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
                        responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
                        responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
                        responseData.IsTRansGove = false;
                    }
                    catch (Exception e)
                    {
                        responseData.CreditStatus = "0";
                        responseData.Succeeded = true;
                        responseData.HasException = true;
                        responseData.UserMessage = e.ToString();
                    }
                }
                else
                {
                    responseData.CreditStatus = "5";
                    responseData.IsTRansGove = false;
                }

                if (responseData.CreditStatus == "5" | responseData.CreditStatus == "3")
                {
                    // to do booom !!!! in angular 
                }
                else if (responseData.CreditStatus == "1")
                {
                    responseData.IsTRansGove = true;
                }
                else if (responseData.CreditStatus == "6")
                {
                    responseData.IsReTRansGove = true;
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendPaymentOnly(CustomFileCreditRequestParams requestParamsCredit)
        {
            try
            {

                CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();

                //ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "תחילת שליחה למכס- הגשת תשלום");
                GenericRequestParams submitRequestParams = new GenericRequestParams();
                submitRequestParams.AppicationId = requestParamsCredit.AppicationId;
                submitRequestParams.InterfaceTypeCode = requestParamsCredit.InterfaceTypeCode;
                submitRequestParams.PBId = requestParamsCredit.PBId;
                submitRequestParams.CustomsRequestsSheetId = requestParamsCredit.CustomsRequestsSheetId;
                submitRequestParams.Tenant = requestParamsCredit.Tenant;
                submitRequestParams.RequestVIA = requestParamsCredit.RequestVIA;
                submitRequestParams.LoggingUserId = requestParamsCredit.LoggingUserId;
                submitRequestParams.ForcePersonalSign = requestParamsCredit.ForcePersonalSign;

                submitRequestParams.LoggingEntityId = requestParamsCredit.LoggingEntityId;
                submitRequestParams.LoggingEntityId2 = requestParamsCredit.LoggingEntityId2;
                submitRequestParams.LoggingObjectTableId = requestParamsCredit.LoggingObjectTableId;
                submitRequestParams.LoggingObjectTableId2 = requestParamsCredit.LoggingObjectTableId2;

                var messagingService = new
                    DF_NG_2755_MSG12001_SubmitDeclarationMessagingService();
                INF_MSG_GenericResponseData submitResponseData = messagingService.Send(submitRequestParams);
                responseData.Succeeded = submitResponseData.Succeeded;
                responseData.HasException = submitResponseData.HasException;
                responseData.UserMessage = submitResponseData.UserMessage;
                responseData.ContinueProcessInBackground = submitResponseData.ContinueProcessInBackground;



                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDeclarationCollateralsList(string declarationId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsCollateralQueryService queryService = new CustomsCollateralQueryService(customContext);
                List<CustomsCollateralPM> customsCollateralList = queryService.GetDeclarationCollateralsList(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, customsCollateralList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationCargoSplitByDeclarationIdList(string declarationId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationCargoSplitQueryService queryService = new DeclarationCargoSplitQueryService(customContext);
                List<DeclarationCargoSplitPM> declarationCargoSplitList = queryService.GetDeclarationCargoSplitsList(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, declarationCargoSplitList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendCargoSplit(CargoSplitRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;


                // use messageing service

                var service = new MN_MSG8370_CargoSplitMessagingService();
                responseData = service.Send(requestParamsData);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        //DeclarationMamanSpecialAction
        public HttpResponseMessage GetDeclarationMamanSpecialAction(string declarationId, int tenant, string actionCode, string mamanSpecialActionCode)
        {
            try
            {
                ICustomContext myContext = CustomContext.GetContext(tenant);
                ICourierGWMessageECSpcRequestService courierGWMessageECSpclRequestService = null;

                DeclarationQueryService declarationQueryService = new DeclarationQueryService(myContext);
                DeclarationPM declaration = declarationQueryService.GetSingle(declarationId, true, false);
                if (declaration != null && declaration.Consignments != null && declaration.Consignments.Count() > 0)
                {
                    var amitalContext = AmitalContext.GetContext(tenant);
                    var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);
                    var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", false, true);

                    if (def.DEFDATA.Contains("ILMMN") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILMMN") // Maman
                    {
                        courierGWMessageECSpclRequestService = new CourierGWMessageECSpclMamanRequestService();
                    }
                    else if (def.DEFDATA.Contains("ILOVL") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL") // OVS
                    {
                        courierGWMessageECSpclRequestService = new Logitude.Customs.BL.Messaging.ILOVS.CourierOVSSpecialActionRequestService();
                    }
                }

                string actionResultString = "";
                if (courierGWMessageECSpclRequestService != null)
                {
                    MamanActionCodeUpdateOrCancel mamanActionCode = MamanActionCodeUpdateOrCancel.Upsert;
                    MamanSpecialCode mamanSpecialCode = MamanSpecialCode.ReceivingDelayCertificate_DelayIt;
                    if (actionCode == "C")
                    {
                        mamanActionCode = MamanActionCodeUpdateOrCancel.Cancel;
                    }
                    switch (mamanSpecialActionCode)
                    {
                        case "2":
                            mamanSpecialCode = MamanSpecialCode.ReceivingDelayCertificate_DelayIt;
                            break;
                        case "4":
                            mamanSpecialCode = MamanSpecialCode.StickerPrinting;
                            break;
                        case "5":
                            mamanSpecialCode = MamanSpecialCode.PrintDocuments;
                            break;
                        case "6":
                            mamanSpecialCode = MamanSpecialCode.Sban;
                            break;
                    }

                    actionResultString = courierGWMessageECSpclRequestService.BuildQueueSendWebAPI(declarationId, tenant, mamanActionCode, mamanSpecialCode);
                }
                else
                {
                    actionResultString = "לא קיימת הרשאה";
                }
                return Request.CreateResponse(HttpStatusCode.OK, actionResultString);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
    
}