using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.BL.DataProviders;
using Logitude.DashboardModule.BL.APIDataContract;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class DashboardAnalyticsController : ApiController
    {
        public HttpResponseMessage PostGetDataAnalytic(WidgetPM widget)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var dataProvider = new DataProviderFactory().GetDataProviderService(widget);
                var result = dataProvider.GetChartData();
                return Request.CreateResponse(result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PostGetDataAnalyticPart(WidgetPartArguments widgetPartArguments)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var dataProvider = new DataProviderFactory().GetDataProviderService(widgetPartArguments.Widget);
                var result = dataProvider.GeChartDataPart(widgetPartArguments);
                return Request.CreateResponse(result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}