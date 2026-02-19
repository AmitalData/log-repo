using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace WebFreight.Web.Controllers.ShipmentsModel.Generated.PMControllers
{
    public partial class ContainersController : ApiController
    {
        public HttpResponseMessage GetViewsGraphData()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Container", "READ", authToken.Tenant);
                var data = new ContainerQuery(authToken.Tenant).GetViewsGraphData(authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}