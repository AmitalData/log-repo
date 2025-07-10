using AmitalCloud.Infrastructure.Application.EntityListQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class ReportController : ApiController
    {
        public HttpResponseMessage GetReportListsByGroupId(string groupId, int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                List<ReportList> reportLists = new ReportListQueryService(tenant).GetList(tenant).Where(d => d.ReportGroupId == groupId).OrderBy(d => d.Name).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, reportLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}