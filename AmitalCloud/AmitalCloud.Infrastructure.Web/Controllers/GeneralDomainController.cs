using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Web.Helpers;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers 
{
    public class GeneralDomainController : ApiController
    {
        public HttpResponseMessage GetObjectFieldModificationForLoggedTenant()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant(); ;
                List<ObjectFieldModificationPM> result = new ObjectFieldModificationQueryService(tenant).GetMulti(a=> a.Tenant == tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}