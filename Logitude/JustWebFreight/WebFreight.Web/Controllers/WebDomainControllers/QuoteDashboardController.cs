using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.DataContracts;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.QuoteModel.EntityQueries.Charts;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class QuoteDashboardController : ApiController
    {
        public HttpResponseMessage GetQuoteDashboardValues(QuoteDashboardArguments quoteDashboardArgs)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Quote", "READ", authToken.Tenant);

                DashboardQuery dashboardQuery = new DashboardQuery(tenant);
                List<ChartingDataClass> myResult = dashboardQuery.GetChartValues(quoteDashboardArgs);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}