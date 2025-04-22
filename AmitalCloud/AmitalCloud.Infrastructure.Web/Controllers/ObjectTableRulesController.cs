using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Data.Queries;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class ObjectTableRulesController : ApiController
    {
        public HttpResponseMessage GetObjectTableRulePMsByTenant(int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                var result = new ObjectTableRuleQuery(tenant).GetObjectTableRulePMsByTenant(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}