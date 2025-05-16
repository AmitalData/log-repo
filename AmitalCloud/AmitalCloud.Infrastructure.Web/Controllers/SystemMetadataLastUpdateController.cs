using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class SystemMetadataLastUpdateController : ApiController
    {
        public HttpResponseMessage GetSystemMetadataLastUpdates(int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var metadatalastUpdates = new SystemMetadataLastUpdateQuery(tenant).GetSystemMetadataLastUpdatesCacheHandle();
                return Request.CreateResponse(HttpStatusCode.OK, metadatalastUpdates);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}