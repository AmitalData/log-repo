using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.Messaging.Maman;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CourierMasterController : ApiController
    {

        public HttpResponseMessage GetIfCourierMasterExists(string Id, string airlineId, string HAWB, string MAWB)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);


                CourierMasterRepository rep = new CourierMasterRepository(customContext);
                bool exist = rep.ChcekIfCourierExists(Id, airlineId, HAWB, MAWB, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, exist);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetStatistic(string CourierMasterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(customContext);
                var keyValuePairList = new List<KeyValuePair<string, int>>();
                courierMasterQueryService.GetStatistic(CourierMasterId, tenant, out keyValuePairList);

                return Request.CreateResponse(HttpStatusCode.OK, keyValuePairList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetSendALLCorrectManifest(string CourierMasterId, string HAWB, string CourierDeclarationStatusCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB1170_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null,
                    new SendALLCorrectRequestParams()
                    {
                        CourierMasterId = CourierMasterId,
                        HAWB = HAWB,
                        CourierDeclarationStatusCode = CourierDeclarationStatusCode
                    }
                    //CourierMasterId, HAWB, CourierDeclarationStatusCode
                    );

                return Request.CreateResponse(HttpStatusCode.OK,

                    sts
                    );
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendALLCorrectManifest(SendALLCorrectRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB1170_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null,
                    //requestParamsData.CourierMasterId, requestParamsData.HAWB, requestParamsData.CourierDeclarationStatusCode, requestParamsData.Declarations);
                    requestParamsData);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSendPayReadyLow2755(string CourierMasterId, string HAWB, string InternalBankId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new
                    DCAInUCB2755_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, CourierMasterId, HAWB, InternalBankId);

                return Request.CreateResponse(HttpStatusCode.OK,

                    sts
                    );
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendPayReadyLow2755(SendPayReadyLowRequestParams requestParamsData) 
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB2755_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData.CourierMasterId, requestParamsData.HAWB, requestParamsData.InternalBankId, requestParamsData.Declarations);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSendALLCorrectDec(string CourierMasterId, string HAWB, string CourierDeclarationStatusCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB2750_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null,
                    new SendALLCorrectRequestParams()
                    {
                        CourierMasterId =
                    CourierMasterId,
                        HAWB = HAWB,
                        CourierDeclarationStatusCode = CourierDeclarationStatusCode
                    });

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendALLCorrectDec(SendALLCorrectRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB2750_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSendALLDeclarationsStatusRequest(string CourierMasterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB8250_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, CourierMasterId);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCourierConnectedDeclarations([FromUri] ApiQueryFilters filters)
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
                    ObjectTableName = "Customs.Declaration",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.Declaration",
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
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                CourierMasterQueryService queryService = new CourierMasterQueryService(customContext);
                IQueryable<DeclarationPM> declarations = queryService.GetCourierConnectedDeclarations(queryOperations, tenant);

                ServiceResponse response = new ServiceResponse();
                response.Count = declarations.Count();
                response.Result = declarations;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetNotConnectedDeclarations([FromUri] ApiQueryFilters filters)
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
                    ObjectTableName = "Customs.Declaration",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.Declaration",
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
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                CourierMasterQueryService queryService = new CourierMasterQueryService(customContext);
                IQueryable<DeclarationPM> declarations = queryService.GetNotConnectedDeclaratins(queryOperations, tenant);

                ServiceResponse response = new ServiceResponse();
                response.Count = declarations.Count();
                response.Result = declarations;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage getCourierMasterByDeclarationId(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                CourierDeclarationQueryService courierDeclarationQueryService = new CourierDeclarationQueryService(customContext);

                CourierDeclarationPM courierDeclaration = courierDeclarationQueryService.GetCourierDeclarationByDeclarationId(declarationId, tenant);
                CourierMasterPM courierMaster = null;
                if (courierDeclaration != null)
                {
                    CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(customContext);
                    courierMaster = courierMasterQueryService.GetSingle(courierDeclaration.CourierMasterId, false, false);


                }

                return Request.CreateResponse(HttpStatusCode.OK, courierMaster);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRequiredFieldsForCourierMaster(string courierMasterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldsForCourierMaster(courierMasterId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, errors);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSendFTPMamanRequest(string courierMasterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CourierMasterMamanService courierMasterMamanService = new CourierMasterMamanService();
                var response = courierMasterMamanService.SendFTPMamanRequest(courierMasterId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetSendECTHRDataMaman(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                var courierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanRequestService();
                var response = courierGWMessageECTHRDataMamanService.BuildQueueSendWebAPI(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}