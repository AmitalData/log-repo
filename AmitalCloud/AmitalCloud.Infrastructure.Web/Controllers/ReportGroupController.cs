using AmitalCloud.Infrastructure.Application.EntityListQueryServices;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class ReportGroupController : ApiController
    {
        public HttpResponseMessage GetReportGroupLists(int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
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