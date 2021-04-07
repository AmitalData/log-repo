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
using Logitude.Customs.BL.Messaging.ILOVS;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Reflection;
using Logitude.Customs.Data.EntityLists;
using WebFreight.Web.CustomWebServices.BL.XLSExport;
using System.IO;
using System.Net.Http.Headers;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CourierMasterController : ApiController
    {


        public HttpResponseMessage GetExportCourierMaster2Excel(string CourierMasterId, int tenant)
        {
            try
            {

                ICustomContext customContext = CustomContext.GetContext(tenant);


                var o = new CourierMasterWSheetExport();
                var result = o.ExportReport(CourierMasterId, tenant);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new StreamContent(new MemoryStream(result));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName =
                    Guid.NewGuid().ToString() + "_" + CourierMasterId + ".xls";
                return response;

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
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


        public HttpResponseMessage GetIfAllowToCancelCourierMaster(string CourierMasterId)
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
                string error = courierMasterQueryService.CheckIfAllowToCancelCourierMaster(tenant, CourierMasterId);

                return Request.CreateResponse(HttpStatusCode.OK, error);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




        public HttpResponseMessage GetPending(string CourierMasterId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);
                var declarationCourierStatusRepository = new DeclarationCourierStatusRepository(MyContext);
                List<string> result = declarationCourierStatusRepository.GetPendingByMasterID(authToken.Tenant, CourierMasterId);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, result);
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
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendALLChangeStorageSiteCode(SendALLStorageSiteRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCBStorageSite_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData);

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

        public HttpResponseMessage PostSendALLTerminal(SendALLCorrectRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);


                var messagingService = new DCAInUCBCTML_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSendUnCorrectDocuments(SendUnCorrectDocumentsRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB2715_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSendALLDeclarationsStatusRequest(string CourierMasterId, string testerSendOption)
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
                var sts = messagingService.CreateCRS(tenant, null, CourierMasterId, testerSendOption);

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
                IQueryable<DeclarationPM> querydeclarations = queryService.GetNotConnectedDeclaratins(queryOperations, tenant);
                querydeclarations = querydeclarations.OrderBy(r => r.Id);
                if (!queryOperations.GetAll)
                {
                    int skippedPorts = queryOperations.PageIndex;
                    querydeclarations = querydeclarations.Skip(skippedPorts);
                    querydeclarations = querydeclarations.Take(queryOperations.PageSize);
                }
                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    string CourierSearchField = "";
                    QueryFilterItem CourierSearchFieldFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CourierSearchFields").FirstOrDefault();
                    if (CourierSearchFieldFilter != null)
                    {
                        CourierSearchField = CourierSearchFieldFilter.FieldValue.ToString();
                    }
                    DeclarationRepository declarationRep = new DeclarationRepository(tenant);
                    var declarationsAll = declarationRep.GetNotConnectedDeclarations(tenant);
                    if (!string.IsNullOrWhiteSpace(CourierSearchField))
                    {
                        declarationsAll = declarationsAll.Where(r => r.CourierSearchFields.Contains(CourierSearchField));
                    }
                    response.Count = declarationsAll.Count();
                }
                response.Result = querydeclarations.ToList();
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

        public HttpResponseMessage GetRequiredFieldsForCourierMasterIncludeManifest(string courierMasterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldsForCourierMaster(courierMasterId, tenant, true);
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
                string response = "";

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
                DeclarationPM declaration = declarationQueryService.GetSingle(declarationId, true, false);
                if (declaration != null && declaration.Consignments != null && declaration.Consignments.Count() > 0)
                {
                    var amitalContext = AmitalContext.GetContext(tenant);
                    var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);
                    var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", false, true);

                    if (def.DEFDATA.Contains("ILMMN") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILMMN") // Maman
                    {
                        var courierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanRequestService();
                        response = courierGWMessageECTHRDataMamanService.BuildQueueSendWebAPI(declarationId, tenant);
                    }
                    else if (def.DEFDATA.Contains("ILOVL") && declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL") // OVS
                    {
                        var courierGWMessageECTHRDataMamanService = new CourierOVSECTHMessageRequestService();
                        response = courierGWMessageECTHRDataMamanService.BuildQueueSendWebAPI(declarationId, tenant);
                    }
                    else
                    {
                        response = "לא קיימת הרשאה";
                    }
                }
                else
                {
                    response = "לא קיימת הרשאה";
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostGatepassRequestMessage(GatepassRequestMessageRequestParams requestParams)
        {
            try
            {
                // use messageing service
                var service = new GP_1030_GatepassRequestMessageMessagingService();
                var responseData = service.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Customs.CourierMaster", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customs.CourierMaster",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.CourierMaster",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };


                List<ObjectField> CourierMasterObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CourierMaster", tenant);
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

                        ObjectField field = CourierMasterObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
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
                        ObjectField field = CourierMasterObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                CourierMasterListQueryService courierMasterListQueryService = new CourierMasterListQueryService(MyContext);
                List<CourierMasterList> entityLists = courierMasterListQueryService.GetList(queryOperations, tenant);

                //CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(MyContext);
                //CourierPendingReasonPM courierPendingReasonPM_902 = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode("902", tenant);
                //CourierPendingReasonPM courierPendingReasonPM_900 = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode("900", tenant);
                //string courierPendingReasonPM_902_Id = courierPendingReasonPM_902 != null ? courierPendingReasonPM_902.Id : "902";
                //string courierPendingReasonPM_900_Id = courierPendingReasonPM_900 != null ? courierPendingReasonPM_900.Id : "900";

               // entityLists = courierMasterListQueryService.AddCalcFields(entityLists, courierPendingReasonPM_900_Id, courierPendingReasonPM_902_Id);
                entityLists = courierMasterListQueryService.AddCalcFields(entityLists);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = courierMasterListQueryService.GetListCount(queryOperations, tenant);
                    response.Count = count;
                }

                response.Result = entityLists;//.OrderBy(d => d.IsSeenByAssignee).ThenBy(d => d.DueDate);
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}