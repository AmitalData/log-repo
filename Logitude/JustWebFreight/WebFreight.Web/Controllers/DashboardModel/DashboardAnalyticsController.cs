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
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

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

                string objectTableName = this.GetObjectTableName(widget.EntityId);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticateDashboardReadFeatures(objectTableName, "READ", authToken.Tenant);
                var dataProvider = new DataProviderFactory().GetDataProviderService(widget, authToken.Tenant);
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

                string objectTableName = this.GetObjectTableName(widgetPartArguments.Widget.EntityId);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticateDashboardReadFeatures(objectTableName, "READ", authToken.Tenant);
                var dataProvider = new DataProviderFactory().GetDataProviderService(widgetPartArguments.Widget, authToken.Tenant);
                var result = dataProvider.GeChartDataPart(widgetPartArguments);
                return Request.CreateResponse(result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostGetKpiData(WidgetPM widget)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string objectTableName = this.GetObjectTableName(widget.EntityId);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticateDashboardReadFeatures(objectTableName, "READ", authToken.Tenant);
                var dataProvider = new DataProviderFactory().GetDataProviderService(widget, authToken.Tenant);
                var result = dataProvider.GetKpiData();
                return Request.CreateResponse(result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string GetObjectTableName(string entityId)
        {
            AnalyticsFactsMetaDataRepository repository = new AnalyticsFactsMetaDataRepository(0);
            AnalyticsFactsMetaData analyticsFacts = repository.GetSingle(entityId, 0);
            return analyticsFacts?.ObjectTableName;
        }
    }
}