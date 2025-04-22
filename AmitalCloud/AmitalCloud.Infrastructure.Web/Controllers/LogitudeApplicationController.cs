using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [RoutePrefix("api/LogitudeApplication")]
    public class LogitudeApplicationController : ApiController
    {
        [Route("")]
        public HttpResponseMessage GetCheckIsupgradingSystem()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                bool isBlocking = GetIsBlockingFromDB(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, isBlocking);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        private bool GetIsBlockingFromDB(int tenant)
        {
            bool isBlocking = new GlobalDBQueryService(tenant).GetFirst().IsBlocking;

            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = HttpContext.Current.Request.UserHostAddress;
            }

            string[] authenticatedIPs = AmitalCloudSettings.CustomerCareIP.Split(',');

            if (!authenticatedIPs.Contains(currentIP))
            {
                isBlocking = false;
            }
            return isBlocking;
        }
    }
}