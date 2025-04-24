using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Domain.EntityPMs;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class CommonDomainController : ApiController
    {
        public HttpResponseMessage GetLoggedTenantDB()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                TenantPM myResult = new TenantQueryService(tenant).GetSingle(tenant, true, true);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}