using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Application.EntityListQueryServices;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class ReportGroupController : ApiController
    {
        public HttpResponseMessage GetReportGroupLists(int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var list = new ReportGroupListQueryService(tenant).GetList(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, list);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}