using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.Helpers;


using WebFreight.Web.Security;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.Server.Tools.TreeFilterQuery;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.BL.DataProviders;

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
                var dataProvicer = new DataProviderService();
                var result = dataProvicer.GetData(widget);
                return Request.CreateResponse(result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}