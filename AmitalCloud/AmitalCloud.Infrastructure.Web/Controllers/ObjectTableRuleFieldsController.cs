using AmitalCloud.Infrastructure.Domain.EntityPMs;
using System.Net.Http;
using System.Net;
using System;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class ObjectTableRuleFieldsController : ApiController
    {
        public HttpResponseMessage GetObjectTableRuleFieldPMsByTenant(int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var queryService = new ObjectTableRuleFieldQueryService(tenant);
                var result = queryService.GetMulti(a => a.Tenant == tenant || a.Tenant == 0, a => new ObjectTableRuleFieldPM(a)
                {
                    ObjectFieldName = a.ObjectField.FieldName,
                    ObjectTableRuleCode = a.ObjectTableRule.RuleCode,
                    ObjectTableRuleTypeCode = a.ObjectTableRule.RuleTypeCode,
                }, "ObjectField,ObjectTableRule");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}