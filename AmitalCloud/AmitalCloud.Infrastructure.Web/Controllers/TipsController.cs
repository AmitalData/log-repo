using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class TipsController : ApiController
    {
        public HttpResponseMessage GetTipsPMs(int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var tips = new TipQuery(tenant).GetTips();
                return Request.CreateResponse(HttpStatusCode.OK, tips);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}