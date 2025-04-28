using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Web.Helpers;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class TipsVisibilityController : ApiController
    {
        public HttpResponseMessage GetTipsVisibilities(int tenant, string userId)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var tipsVisibilityLists = new TipsVisibilityQuery(tenant).GetTipsVisibilities(tenant, userId);
                return Request.CreateResponse(HttpStatusCode.OK, tipsVisibilityLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}