
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Transactions;
using System.Threading.Tasks;
using Logitude.Customs.BL.AzureSearch;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Common.RequestParams;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CB_CustomsItemExtendedController : ApiController
    {

        public HttpResponseMessage GetCustomsBookMainView(string customsBookType, int Tenant)
        {
            try
            {
                Filters filters = new Filters();
                filters.CustomsBookType = customsBookType;
                filters.Tenant = Tenant;

                string token = HttpContext.Current.Request.Headers["Token"];
                if (token == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Token is missing")));

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(Tenant);

                CB_CustomsItemComputedDataQueryService customsItemComputedDataQueryService = new CB_CustomsItemComputedDataQueryService(Tenant);
                List<CB_CustomsItemComputedDataList> result = customsItemComputedDataQueryService.GetCustomsBookMainView(filters.CustomsBookType, filters.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        public HttpResponseMessage GetCustomsBookMainViewSearchByClassification([FromBody] Filters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                if (token == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Token is missing")));

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                CB_CustomsItemComputedDataQueryService customsItemComputedDataQueryService = new CB_CustomsItemComputedDataQueryService(authToken.Tenant);
                List<CB_CustomsItemComputedDataList> result = customsItemComputedDataQueryService.GetCustomsBookMainViewSearchByClassification(filters.CustomsBookType,
                    filters.SearchFields, filters.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        public HttpResponseMessage GetCustomsBookMainViewSearchByText([FromBody] Filters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                if (token == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception("Token is missing")));

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                CB_CustomsItemComputedDataQueryService customsItemComputedDataQueryService = new CB_CustomsItemComputedDataQueryService(authToken.Tenant);
                List<CB_CustomsItemComputedDataList> result = customsItemComputedDataQueryService.GetCustomsBookMainViewSearchByText(filters.SearchFields,
                    filters.CustomsBookType, filters.CustomsItemHierarchic, filters.Reamarks, filters.Rules, filters.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage AddNEWRemarksClassification(RemarksClassificationPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        ICustomContext MyContext = CustomContext.GetContext(entityPM.Tenant);
                        RemarksClassificationUpdateService service = new RemarksClassificationUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        service.Update(entityPM, true);

                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage EditRemarksClassification(RemarksClassificationPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                        ICustomContext MyContext = CustomContext.GetContext(entityPM.Tenant);
                        RemarksClassificationUpdateService service = new RemarksClassificationUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        service.Update(entityPM, true);

                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteRemarksClassification(RemarksClassificationPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                        ICustomContext MyContext = CustomContext.GetContext(entityPM.Tenant);
                        RemarksClassificationUpdateService service = new RemarksClassificationUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        service.Update(entityPM, true);

                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage GetAllCommentsByCustomsItemId(int customsItemId, int tenant)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                RemarksClassificationQueryService remarksClassificationQuery = new RemarksClassificationQueryService(MyContext);
                remarksClassificationQuery.InitializeSettings();

                List<RemarksClassificationList> remarksClassificationPMList = remarksClassificationQuery.GetAllCommentsByCustomsItemId(customsItemId, tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, remarksClassificationPMList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetCustomItemClassifGuidance(int customsItemId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                GetCustomItemClassifGuidanceRequestParams requestParamsData = new GetCustomItemClassifGuidanceRequestParams()
                { 
                    CustomItemId = customsItemId,
                    ValidToDate = DateTime.Now,
                    Tenant = tenant
                };
                DCAInGet_CB_MSG_8317_CustomItemClassifGuidanceMessagingService messagingService = new DCAInGet_CB_MSG_8317_CustomItemClassifGuidanceMessagingService();
                 CustomItemClassifGuidanceResponseData responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetClassifGuidanceDetails(string classificationGuidanceNumber, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                GetClassifGuidanceDetailsRequestParams requestParamsData = new GetClassifGuidanceDetailsRequestParams()
                {
                    ClassificationGuidanceNumber = classificationGuidanceNumber,
                    Tenant = tenant
                };
                DCAInGet_CB_MSG_8323_ClassifGuidanceDetailsMessagingService messagingService = new DCAInGet_CB_MSG_8323_ClassifGuidanceDetailsMessagingService();
                GetClassifGuidanceDetailsResponseData responseData = messagingService.Send(requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public async Task<HttpResponseMessage> GetFromTypesense(string searchValue, string customsBookType)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            RemarkAndCustomsBook res = await CustomsBookAzureSearchService.SearchItmesAndRemark(searchValue, customsBookType, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        public HttpResponseMessage GetTenantFromCustomsSettings()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                
                CustomsSettingQueryService customsSettingQueryService = new CustomsSettingQueryService(authToken.Tenant);
               int tenant = customsSettingQueryService.GetTheFirstTenantWithCustomsAgentId();
                return Request.CreateResponse(HttpStatusCode.OK, tenant);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [TokenAutherize]
        public async Task<HttpResponseMessage> GetClassifications()
        {
            Dictionary<string, Dictionary<int, ClassificationCustomsBook>> res = await CustomsBookAzureSearchService.GetClassifications();

            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }


    public class Filters
    {
        public string CustomsBookType { get; set; } = "1";
        public int Tenant { get; set; } = 0;
        public string SearchFields { get; set; } = null;
        public string CustomsItemHierarchic { get; set; } = null;
        public bool Reamarks { get; set; } = false;
        public bool Rules { get; set; } = false;

    }
}