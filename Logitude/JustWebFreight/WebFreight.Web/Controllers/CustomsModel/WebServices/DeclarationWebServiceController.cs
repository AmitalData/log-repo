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
using Logitude.Customs.BL.TraceEvents;
using Unifreight.BL.EntityPMs;
using SupplierInvoicePM = Logitude.Customs.Def.EntityPMs.SupplierInvoicePM;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.AmitalMessaging.Utils;
using Newtonsoft.Json;
using Logitude.Customs.BL.BL;
using WebFreight.Web.CustomWebServices.BL.XLSReports;
using System.Net.Http.Headers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Threading.Tasks;
using Logitude.Customs.BL.Helpers;
using Logitude.Customs.Data.EntityPOCOs;
using System.Data;
using Org.BouncyCastle.Bcpg.Sig;
using Logitude.Customs.Data.EntityMapping;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Reflection;
using System.Web.Script.Serialization;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class DeclarationWebServiceController : ApiController
    {
        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
        enum DeclarationTypeCode { EXPORT, TRANSSHIPMENT }
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

        [HttpPost]

        public HttpResponseMessage GetIsConsignmentConectContainerization(ReqConectContainerization requestParam)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationConstraintQueryService query = new DeclarationConstraintQueryService(customContext);
                List<string> countDeclartions = query.GetIsConsignmentConectContainerization(requestParam.Tenant ,requestParam.ArrayDeclartiosId, requestParam.ContainerizationID, requestParam.CargoTypeCode, requestParam.ManifestNumber, requestParam.SecondCargoID, requestParam.ThirdCargoID);

                return Request.CreateResponse(HttpStatusCode.OK, countDeclartions);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDeclarationErrors(string declarationId, string listVersionId, string courierFilter,bool IsAmendmentErrors,bool IsExportCloseErrors)
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
                                        , tenant, listVersionId == "undefined" ? null : listVersionId, courierFilter , IsAmendmentErrors, IsExportCloseErrors);

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


        public HttpResponseMessage GetAcceptDeclarationAmendment(string declarationId)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);

                DeclarationQueryService queryService = new DeclarationQueryService(customContext);
                 var declaration = queryService.GetAcceptDeclarationAmendment(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, declaration);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetWaitingDeclarationAmendmentByCustomsFile(string customFileNo)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);

                DeclarationQueryService queryService = new DeclarationQueryService(customContext);
                var declaration = queryService.GetWaitingDeclarationAmendmentByCustomsFile(customFileNo, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, declaration);
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
                if (requestParams.RequestName.StartsWith("Export"))
                {
                    DF_NG_2751_MSG10000_ExportDeclarationRequestService _dF_MSG10000_ExportDeclarationRequestService = new DF_NG_2751_MSG10000_ExportDeclarationRequestService();
                    _dF_MSG10000_ExportDeclarationRequestService.IsFromOpenNewAmendment = true;
                    var request = _dF_MSG10000_ExportDeclarationRequestService.GetRequest(requestParams);
                    string error = "";
                    DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService = new DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService();

                    DeclarationPM declarationPM = dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService.MapResponseToDeclaration(request.Declaration, requestParams.Tenant, true, requestParams.AppicationId, out error, user: requestParams.LoggingUserId, isCopy: Convert.ToBoolean(requestParams.LoggingEntityId2));


                    if (declarationPM != null)
                        return Request.CreateResponse(HttpStatusCode.OK, declarationPM);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, error);
                }
                else
                {

                    DF_MSG10000_ImportDeclarationRequestService _dF_MSG10000_ImportDeclarationRequestService = new DF_MSG10000_ImportDeclarationRequestService();
                    _dF_MSG10000_ImportDeclarationRequestService.IsFromOpenNewAmendment = true;
                    var request = _dF_MSG10000_ImportDeclarationRequestService.GetRequest(requestParams);
                    string error = "";
                    DF_NG_2754_MSG10004_ImportAmendmentDeclarationResponseService dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService = new DF_NG_2754_MSG10004_ImportAmendmentDeclarationResponseService();

                    DeclarationPM declarationPM = dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService.MapResponseToDeclaration(request.Declaration, requestParams.Tenant, true, requestParams.AppicationId, out error, user: requestParams.LoggingUserId, isCopy: Convert.ToBoolean(requestParams.LoggingEntityId2));

                    XmlSerializer xsSubmit = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration));


                    if (declarationPM != null)
                        return Request.CreateResponse(HttpStatusCode.OK, declarationPM);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, error);
                }

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendExportDeclaration(GenericRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData;
                var messagingService = new DF_NG_2751_MSG10000_ExportDeclarationMessagingService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpPost]
        public HttpResponseMessage PostActionOnDeclarationBatch([FromBody] SendDeclarationBatchRequestParams sendDeclarationBatchRequestParams, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                UserRepository userRepository = new UserRepository(tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                DeclarationUpdateService DeclarationUpdateService = new DeclarationUpdateService(customContext);
                if (sendDeclarationBatchRequestParams.IsAllSelected && filters != null)
                {
                    filters.GetAll = true;
                    sendDeclarationBatchRequestParams.QueryOperations = PrepareFilters(tenant, filters);
                }

                DataResult result = new DataResult();
                string RequestInProgressList;

                switch (sendDeclarationBatchRequestParams.Action.ToLower())
                {
                    case "sendsigneddeclarationsaction":
                    case "senddeclarationaction":
                        DeclarationUpdateService.UpdateTaxationDateTime(new List<string>(sendDeclarationBatchRequestParams.SelectedIds), tenant);
                        bool signDeclaration = sendDeclarationBatchRequestParams.Action.ToLower() == "sendsigneddeclarationsaction";

                        var sendMessagingService = new DCAInUCB2751_MsgMessagingService();
                        var sendDeclarationSts = sendMessagingService.CreateCRS(tenant, sendDeclarationBatchRequestParams, signDeclaration, out RequestInProgressList);
                        result.RequestInProgressList = RequestInProgressList;
                        result.Message = sendDeclarationSts;
                        break;

                    case "senddeclarationpaymentsaction":
                        DeclarationUpdateService.UpdateTaxationDateTime(new List<string>(sendDeclarationBatchRequestParams.SelectedIds), tenant);

                        var paymentMessagingService = new DCAInUCB2755E_MsgMessagingService();
                        var paymentDeclarationSts = paymentMessagingService.CreateCRS(tenant, sendDeclarationBatchRequestParams, out RequestInProgressList);
                        result.RequestInProgressList = RequestInProgressList;
                        result.Message = paymentDeclarationSts;
                        break;

                    default:
                        throw new Exception("Action not supported");
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendTransshipmenDeclaration(GenericRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData;
                var messagingService = new SaveDF_MSG2751_2757_TransshipmentDeclarationRequestMessagingService();
                responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
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
                if (requestParamsData.RequestName.StartsWith("Transshipment"))
                {
                    var serializedParent = JsonConvert.SerializeObject(requestParamsData);
                    AmendmentRequestParams requestParams = JsonConvert.DeserializeObject<AmendmentRequestParams>(serializedParent);

                    var messagingService = new DF_MSG8235_TransshipmentDeclarationAmendmentMessagingService();
                    responseData = messagingService.Send(requestParams);
                }
                else if (requestParamsData.RequestName.StartsWith("Export"))
                {
                    //var messagingService = new DF_MSG8235_TransshipmentDeclarationAmendmentMessagingService();
                    var serializedParent = JsonConvert.SerializeObject(requestParamsData);
                    AmendmentRequestParams requestParams = JsonConvert.DeserializeObject<AmendmentRequestParams>(serializedParent);

                    var messagingService = new DF_MSG8235_ExportDeclarationAmendmentMessagingService();
                    responseData = messagingService.Send(requestParams);
                }
                else
                {
                    var messagingService = new DF_MSG2892_ImportDeclarationAmendmentMessagingService();
                    responseData = messagingService.Send(requestParamsData);
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage PostSendDeclarationClosingAmendment(AmendmentRequestParams requestParamsData)
        {
            try
            {
                ExportDeclarationAmendmentResponseData responseData = requestParamsData.IsTransShipment ?
                    new DF_MSG8235_TransshipmentDeclarationAmendmentMessagingService().Send(requestParamsData) :
                    new DF_MSG8235_ExportDeclarationAmendmentMessagingService().Send(requestParamsData);

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

        public HttpResponseMessage GetWarningFieldsForExportDeclaration(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetWarningFieldsForExportDeclaration(declarationId, tenant);
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

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {
                    var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
                    var itemRepo = new GTBITEMRepository(GetAmitalContext(tenant));

                    DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
                    string partner = defaultValueQueryService.GetDefault("ISRAEL", "CIM_SIVUG_103", "NON", customerCode, tenant); // S=Supplier I=Client
                    if (partner == "S") // If Supplier get Unifreight card
                    {
                        customerCode = defaultValueQueryService.GetDefaultAccountNumber("ISRAEL", "CEX_CUS_SUP", "NON", customerCode, tenant);
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
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
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

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {
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
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetGTBPTYPEItemList(string application, string search, int top, bool searchNULLVendor = true)
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

                if (application != null && (application.ToLower() == "undefined" || application.ToLower() == "null"))
                {
                    application = null;
                }

                #region get data from Unifri

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {
                    var TarifsRepo = new GTBPTYPERepository(GetAmitalContext(tenant));
                    var q =
                    from itm in TarifsRepo
                        .GetAll().Where(rec => rec.APPLICATION == application).Select(o => new
                        {
                            Name=o.NAMEENG,
                            PriceType=o.PRICETYPE,
                            application = o.APPLICATION,
                        })
                    select new { itm };

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        search = search.ToUpper();
                        q = q.Where(rec => rec.itm.PriceType.Contains(search));
                    }
                    q = q.Distinct();
                    q = q.Take(top);

                    var TarifList = q.ToList();
                    #endregion
                   
                    return Request.CreateResponse(HttpStatusCode.OK, TarifList);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
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

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {

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
                            TariffID = rec.TARIFFID,
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
                   
                    var itemCrRepo = new GITITEMCRRepository(GetAmitalContext(tenant));
                    foreach (var item in aynList)
                    {
                        var crkeys = new Unifreight.Data.AmitalModel.EntityKeys.GITITEMKeys() { COUNTER = item.itm.Id };
                        /*
                        var aq =
                    from itmcr in itemCrRepo
                        .GetMulti(crkeys)
                        .Select(rec => new GITITEMCR()
                        {
                            COUNTER = rec.COUNTER,
                            REMARKS = rec.REMARKS,
                            REQCERT = rec.REQCERT,
                            
                        })
                    select new { itmcr };
                        */
                        var list = itemCrRepo.GetMulti(crkeys);
                  //     item.itm.GITITEMCRs = list;

                    }

                    /*
                    var aq =
                    from itmcr in itemCrRepo
                        .GetMulti(aynList.Select(r => r.itm.Id)
                        );
                        
                    select new { itmcr };
                    */
                    var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();

                    

                    #endregion

                    return Request.CreateResponse(HttpStatusCode.OK, l);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
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
                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {


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
                return Request.CreateResponse(HttpStatusCode.OK);
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


        public HttpResponseMessage GetDeclarationDocumentWithConnectNotValid(string parentEntityId, string parentEntityCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsDocumentQueryService customsDocumentQuery = new CustomsDocumentQueryService(customContext);
                List<CustomsDocumentPM> documents = customsDocumentQuery.GetDeclarationDocumentWithConnectNotValid(parentEntityId, parentEntityCode, tenant);

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
                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {
                    DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
                    string isNoIncotermCheck = defaultValueQueryService.GetDefault("ISRAEL", "CGG_NO_INC_CHK", "NON", "NON", tenant);
                    if (isNoIncotermCheck == "Y")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, false);
                    }
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

        public HttpResponseMessage GetGoldPaymentDefaults(string CustomerCode)
        {
            string CustomerDefaultGoldPay_CIM_GOLD_PAY = null;
            string CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY = null;
            string CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C = null;
            
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ICustomContext customContext = CustomContext.GetContext(tenant);
                //CIM_GOLD_PAY CGG_MAX_AGT_PAY
                DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
                if (!string.IsNullOrWhiteSpace(CustomerCode))
                {
                    ///דיפולט באינדקס לקוח "תשלום בניצול העברת זהב לקוח "
                    CustomerDefaultGoldPay_CIM_GOLD_PAY = defaultValueQueryService.GetDefault("ISRAEL", "CIM_GOLD_PAY", "NON", CustomerCode, tenant);

                }
                ///דיפולט ברמת חברה "סכום מיסים מקסימלי לתשלום במס"ב סוכן
                CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY = defaultValueQueryService.GetDefault("ISRAEL", "CGG_MAX_AGT_PAY", "NON", "NON", tenant);

                //סכום שמעל יבוצע תשלום בקופה סוכן"
                CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C = defaultValueQueryService.GetDefault("ISRAEL", "CGG_ABOVE_AGT_C", "NON", "NON", tenant);

                return Request.CreateResponse(HttpStatusCode.OK, new {
                    CustomerDefaultGoldPay_CIM_GOLD_PAY = CustomerDefaultGoldPay_CIM_GOLD_PAY,
                    CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY= CompanyDefaultMaxPayMASAV_CGG_MAX_AGT_PAY,
                    CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C= CompanyDefaultaboveamountagentCash_CGG_ABOVE_AGT_C
                });
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
                        DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
                        string customsAgentExplanationDefault = defaultValueQueryService.GetDefault("ISRAEL", "CIM_PROTEST_PAY", "NON", CustomerCode, tenant);

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
 
                CustomsSettingQueryService settingService = new CustomsSettingQueryService(requestParamsCredit.Tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(requestParamsCredit.Tenant);

                if (setting.IsConnectedToUniFreight)
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

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(requestParamsCredit.Tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(requestParamsCredit.Tenant);

                if (setting.IsConnectedToUniFreight)
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



        public HttpResponseMessage GetDeclarationCorrection(string declarationId,bool isExportClose = false)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService query = new DeclarationQueryService(customContext);
                DeclarationCorrectionView correction = query.GetDeclarationCorrection(declarationId, tenant,isExportClose);

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


                CustomsSettingQueryService settingService = new CustomsSettingQueryService(requestParamsCredit.Tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(requestParamsCredit.Tenant);

                if (setting.IsConnectedToUniFreight)
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

                submitRequestParams.TestCase = requestParamsCredit.TestCase;

                submitRequestParams.RequestName = requestParamsCredit.RequestName;

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

        public HttpResponseMessage PostSendTransshipmentPaymentOnly(CustomFileCreditRequestParams requestParamsCredit) 
        {
            try
            {
                CustomFileCreditResponseData responseData = SubmitPayment(requestParamsCredit, DeclarationTypeCode.TRANSSHIPMENT);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage PostSendExportPaymentOnly(CustomFileCreditRequestParams requestParamsCredit)
        {
            try
            {
                CustomFileCreditResponseData responseData = SubmitPayment(requestParamsCredit, DeclarationTypeCode.EXPORT);

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

        public HttpResponseMessage GetDeclarationCargoSealLists(string declarationId, int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                CargoSealIdentifierQueryService queryService = new CargoSealIdentifierQueryService(customContext);
                List<CargoSealIdentifierPM> cargoSealIdentifierPMList = queryService.GetDeclarationCargoSealIdentifierList(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, cargoSealIdentifierPMList);
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
                    else if (def.DEFDATA.Contains("ILSWS") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILSWS") // OVS
                    {
                        courierGWMessageECSpclRequestService = new Logitude.Customs.BL.Messaging.ILSWS.CourierSWSSpecialActionRequestService();
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

        public HttpResponseMessage PostSendDeclarationCancellation(GenericRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;

                // use messageing service
                var service = new SaveDF_MSG5002_DeclarationCancellationRequestMsgService();
                responseData = service.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


        public HttpResponseMessage GetIsDeclarationCancellationAttachmentNumberIsMoreThenAllow(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(customContext);

                //Get DeclarationCancellation Attachments
                bool isAttachmentNumberIsMoreThenAllow = false;
                List<CustomsDocumentPM> customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = declarationId, ParentEntityCode = "DeclarationCancellation" }, tenant);

                customsDocumentPMList = customsDocumentPMList.Where(x => x.DocumentTypeCode == "IL_679" && ! string.IsNullOrEmpty(x.CustomsDocId) ).ToList();
                if (customsDocumentPMList != null && customsDocumentPMList.Count() > 0)
                {
                    isAttachmentNumberIsMoreThenAllow = true;
                }

                return Request.CreateResponse(HttpStatusCode.OK, isAttachmentNumberIsMoreThenAllow);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PostSendCargoSealsRequest(CargoSealsRequestParams requestParamsData)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;

                // use messageing service
                var service = new SE_6001_SealUpdateMessagingService();
                responseData = service.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


        public HttpResponseMessage GetCLSHWBEventHandle(string DeclerationID,String UserID ,int a_Tenent , int  a_mode)
        {
            try
            {

                DeclarationUpdateService.SetCLSHWB(DeclerationID, UserID, a_Tenent, a_mode == 1 ? UnifreightEventMode.@new : UnifreightEventMode.del);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        [HttpGet]
        public HttpResponseMessage SendPRIVEventPrivacyProtection(int tenant, string declarationId, string customFileNo)
        {
            try
            {
                var privacyProtection = new PrivacyProtection();
                privacyProtection.SendPRIVEventPrivacyProtectionMethod(tenant,declarationId, customFileNo);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        [HttpGet]
        public HttpResponseMessage CheckIfError12195ExistInCustomfileno(int tenant, string declarationId, string customFileNo)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService query = new DeclarationQueryService(customContext);
                var isErrorExist = query.CheckIfDeclarationHasError12195(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, isErrorExist);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        [HttpGet]
        public HttpResponseMessage UpdateSupplierInvoiceItemsWhoHasError12195(int tenant, string declarationId, string customFileNo)
        {
            try
            {
                var mess = DeclarationUpdateService.UpdateSupplierInvoiceItemsWhoHasError12195(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, mess);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        //RaiseCLSHWBEvent

        [HttpGet]
        public HttpResponseMessage DeclarationConsignment(string exportFile)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                DeclarationConsignments res = new DeclarationRepository(authToken.Tenant).GetDeclarationConsignment(exportFile);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        [HttpGet]
        public HttpResponseMessage ExportStorageConnectToDeclaration(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                ExportStorageConnectToDeclaration res = new DeclarationQueryService(authToken.Tenant).GetExportStorageConnectToDeclaration(id, authToken.Tenant);
                    
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static CustomFileCreditResponseData SubmitPayment(CustomFileCreditRequestParams requestParamsCredit, DeclarationTypeCode declarationTypeCode)
        {
            CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();
            GenericRequestParams submitRequestParams = new GenericRequestParams();
            submitRequestParams.AppicationId = requestParamsCredit.AppicationId;
            submitRequestParams.InterfaceTypeCode = "2755E";
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

            submitRequestParams.TestCase = requestParamsCredit.TestCase;

            INF_MSG_GenericResponseData submitResponseData =
                declarationTypeCode == DeclarationTypeCode.EXPORT ?
                new DF_NG_2755_MSG12001_SubmitExportDeclarationMessagingService().Send(submitRequestParams) :
                new SaveDF_MSG2755_2757_SubmitTransshipmenDeclarationRequesMessagingService().Send(submitRequestParams);
            responseData.Succeeded = submitResponseData.Succeeded;
            responseData.HasException = submitResponseData.HasException;
            responseData.UserMessage = submitResponseData.UserMessage;
            responseData.ContinueProcessInBackground = submitResponseData.ContinueProcessInBackground;
            return responseData;
        }
        public HttpResponseMessage GetDeclarationExportStoragesByDeclarationIdAndExportFile(string declarationId,string exportFile ,int tenant)
        {
            try
            {
                ICustomContext customContext = CustomContext.GetContext(tenant);
                ExportStorageQueryService queryService = new ExportStorageQueryService(customContext);
                List<ExportStoragePM> declarationExportStoragesList = queryService.GetDeclarationExportStoragesList(declarationId, exportFile, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, declarationExportStoragesList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetExportReport2Excel(string tenant, string ExportFromDate, string ExportToDate)
        {
            try
            {
                var lastMileReport = new ExportReport(tenant);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var item = lastMileReport.GetExportReport(ExportFromDate, ExportToDate);

                response.Content = new StreamContent(new MemoryStream(item));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = ExportFromDate.ToString() + " - " + ExportToDate.ToString() + ".xls";
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        [HttpGet]
        public HttpResponseMessage DeleteCourierMawbsFromExcel(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                new CourierHawbFromExcelRepository(authToken.Tenant).DeleteByUserAndTenant(tenant, userId);
                return new HttpResponseMessage(HttpStatusCode.OK);
              
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> ImportCourierMawbsFromExcel(string userid,int tenant)
        {
            try
            {
                if (!Request. Content.IsMimeMultipartContent())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid request format");
                }

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.Contents)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');

                    using (var stream = await file.ReadAsStreamAsync())
                    {
                        IWorkbook workbook;
                        if (fileName.EndsWith(".xlsx"))
                        {
                            workbook = new XSSFWorkbook(stream);
                        }
                        else if (fileName.EndsWith(".xls"))
                        {
                            workbook = new HSSFWorkbook(stream);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Unsupported file format");
                        }

                        var sheet = workbook.GetSheetAt(0); // Assuming the first sheet

                        var values = new List<string>();
                        for (var row = 0; row <= sheet.LastRowNum; row++)
                        {
                            var cell = sheet.GetRow(row)?.GetCell(0);
                            if (cell != null)
                            {
                                string cellValue = null;

                                if (cell.CellType == CellType.String)
                                {
                                    cellValue = cell.StringCellValue;
                                }
                                else if (cell.CellType == CellType.Numeric)
                                {
                                    cellValue = cell.NumericCellValue.ToString();
                                }

                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    values.Add(cellValue.TrimEnd());
                                }
                            }
                        }
                        var errorWithMawbs=new List<CourierHawbFromExcel>();
                        try
                        {
                            CustomsStoredProcedures.UpdateCourierHawbFromExcel(tenant, userid, values, out errorWithMawbs);
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception from UpdateCourierHawbFromExcel
                            var errorResponse = new
                            {
                                ErrorMessage = "An error occurred in UpdateCourierHawbFromExcel",
                                ExceptionMessage = ex.Message
                            };
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
                        }
                        if (values.Count > 0)
                        {
                            var firstValue = values[0];
                            return Request.CreateResponse(HttpStatusCode.OK, errorWithMawbs);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NoContent, "No strings found in the uploaded Excel file.");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
		public HttpResponseMessage PostNewAmendmentDeclarationWithSend(GenericRequestParams requestParams)
		{

			try
			{
				string token = HttpContext.Current.Request.Headers["Token"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService _pc_NG_2280_MSG01_CertificateOfOriginRequestResponseService = new PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService();
				DF_NG_2751_MSG10000_ExportDeclarationRequestService _dF_MSG10000_ExportDeclarationRequestService = new DF_NG_2751_MSG10000_ExportDeclarationRequestService();

				ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);
				CertificateOfOriginQueryService certificateOfOriginQuery = new CertificateOfOriginQueryService(MyContext);
				CertificateOfOriginPM certificateOfOriginPM = certificateOfOriginQuery.GetSingle(requestParams.LoggingEntityId2, true, false);


				var request = _dF_MSG10000_ExportDeclarationRequestService.GetRequest(requestParams);
				string error = "";
				DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService = new DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService();

				DeclarationPM declarationPM = dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService.MapResponseToDeclaration(request.Declaration, requestParams.Tenant, true, requestParams.AppicationId, out error, user: requestParams.LoggingUserId, isCopy: false, from2280:true);


				if (declarationPM != null) { 
                    
					bool IsUpdated = _pc_NG_2280_MSG01_CertificateOfOriginRequestResponseService.UpdateCooNumberInDeclaration(declarationPM.Id, certificateOfOriginPM);

					INF_MSG_GenericResponseData responseData = _pc_NG_2280_MSG01_CertificateOfOriginRequestResponseService.SendAmendmentDeclaration(declarationPM);
					
				   return Request.CreateResponse(HttpStatusCode.OK, declarationPM);
				}

				return Request.CreateResponse(HttpStatusCode.BadRequest, error);		
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}

        private QueryOperations PrepareFilters(int tenant, ApiQueryFilters filters)
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = "Customs.Declaration",
                PageIndex = filters.PageIndex,
                PageSize = filters.PageSize,
                QuerySection = "Customs.Declarations",
                SortByColumnName = filters.SortBy,
                SortDirectin = filters.SortDirection,
                GetAll = filters.GetAll,
            };

            List<ObjectField> DeclarationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.Declaration", tenant);
            List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
            for (int i = 1; i <= 10; i++)
            {
                object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                object filterValue2 = null;

                if (filterNameProp != null)
                {
                    string filterName = filterNameProp.ToString();
                    string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                    //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                    //{
                    //string[] values = filterValue1.ToString().Split(',');
                    //if (values.Count() > 1)
                    //{
                    //filterValue1 = values[0];
                    //filterValue2 = values[1];
                    //}
                    //}
                    //ToDo: Get object field by name and set the remained filter properties
                    ObjectField field = DeclarationObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                    if (field != null)
                    {
                        string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                        //queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);

                    }
                    else
                        queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                }
            }

            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                foreach (QueryFilterItem filter in filters_list)
                {
                    ObjectField field = DeclarationObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                    if (field != null)
                    {


                        string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                        //queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);

                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }
            return queryOperations;
        }
    }

    internal class CustomsPartnersItemCRList
    {
        public CustomsPartnersItemCRList()
        {
        }

        public string Id { get; set; }
        public string REMARKS { get; set; }
        public string REQCERT { get; set; }
    }
    public class Request
    {
        public string[] ArrayDeclartiosId { get; set; }

    }
   public class ReqConectContainerization
   {
        public string[] ArrayDeclartiosId { get; set; }
        public string ContainerizationID { get; set; }
        public string CargoTypeCode { get; set; }
        public string ManifestNumber { get; set; }
        public string SecondCargoID { get; set; }
        public string ThirdCargoID { get; set; }
        public int  Tenant { get; set; }
   }

    public class DataResult
    {
        public string RequestInProgressList { get; set; }
        public string Message { get; set; }
    }
}