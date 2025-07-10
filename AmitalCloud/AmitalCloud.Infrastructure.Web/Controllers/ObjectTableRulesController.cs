using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class ObjectTableRulesController : ApiController
    {
        public HttpResponseMessage GetObjectTableRulePMsByTenant(int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var result = new ObjectTableRuleQuery(tenant).GetObjectTableRulePMsByTenant();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}