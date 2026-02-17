using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using System.Linq;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Model.EntityClasses;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [RoutePrefix("api/UserLastLogins")]
    public class UserLastLoginsController : ApiController
    {
        [HttpGet]
        [Route("GetUserLastLogin")]
        public HttpResponseMessage GetUserLastLogin(string userId, int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                return Request.CreateResponse(HttpStatusCode.OK, new UserLastLoginQueryService(tenant).GetMulti(a => a.Tenant == tenant && a.Id == userId).FirstOrDefault());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put(UserLastLogin entity)
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant(entity.Tenant);
                entity = new UserLastLoginQuery(tenant).UpdateUserLastLogins(entity, AmitalCloudSettingConfigration.GetWorkEnvironment());
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}