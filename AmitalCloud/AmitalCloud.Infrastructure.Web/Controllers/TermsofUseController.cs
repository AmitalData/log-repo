using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [RoutePrefix("api/TermsofUse")]
    public class TermsofUseController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetCheckIfGoToTermUseComponent(int tenant, string userId)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();

                if (string.IsNullOrEmpty(userId))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "missing user id");
                }

                var result = new TermsofUseQuery(tenant).CheckIfGoToTermUseComponent(userId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}