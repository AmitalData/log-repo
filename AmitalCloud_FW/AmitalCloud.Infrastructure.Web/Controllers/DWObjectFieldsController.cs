using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Application.Helpers;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class DWObjectFieldsController : ApiController
    {
        public HttpResponseMessage GetDWObjectFieldsWithChildren()
        {
            string logKey = PerformanceLogger.LogCurrentTime();
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var dWObjectFieldPM = new DWObjectFieldQuery(tenant).GetDWObjectFieldWithChildrenFieldsPMsByTenant();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, dWObjectFieldPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}