using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using System.Linq;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class ObjectTableLastUpdateController : ApiController
    {
        public HttpResponseMessage GetLastUpdatedTables(int tenant, DateTime sinceDate, string clientEmail)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                List<ObjectTableLastUpdatePM> list = new ObjectTableLastUpdateQueryService(tenant).GetMulti(a => a.LastUpdateDate > sinceDate && (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTable.CacheOnClient && !a.ObjectTable.IsClosed, a => new ObjectTableLastUpdatePM(a)
                {
                    ObjectTableName = a.ObjectTable.Name,
                }, "ObjectTable").OrderByDescending(d => d.LastUpdateDate).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}