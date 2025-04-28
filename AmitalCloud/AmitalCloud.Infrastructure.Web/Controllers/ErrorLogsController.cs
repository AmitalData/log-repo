using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using AmitalCloud.Infrastructure.Application.Helpers;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [RoutePrefix("api/ErrorLogs")]
    public class ErrorLogsController : ApiController
    {
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post(ErrorLog entity)
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant(entity.Tenant);
                entity = new ErrorLogQuery(tenant).AddErrorLog(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}