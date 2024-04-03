using Logitude.DashboardModule.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DashboardModel.Extended
{
    public class AnalyticsFactsFieldsMetaDataPMExtendedController : ApiController
    {
        public HttpResponseMessage GetAllByAnalyticsFactsMetaDataId(string analyticsFactsMetaDataId)
        {
            try
            {
                int tenant = AuthenticatToken();
                AnalyticsFactsFieldsMetaDataQueryService dashboardQueryService = new AnalyticsFactsFieldsMetaDataQueryService(tenant);
                var result = dashboardQueryService.GetAllByAnalyticsFactsMetaDataId(analyticsFactsMetaDataId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetPresetFilters()
        {
            try
            {
                int tenant = AuthenticatToken();
                AnalyticsFactsFieldsMetaDataQueryService dashboardQueryService = new AnalyticsFactsFieldsMetaDataQueryService(tenant);
                var result = dashboardQueryService.GetPresetFilters();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static int AuthenticatToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Dashboard", "READ", tenant);
            return tenant;
        }
    }
}