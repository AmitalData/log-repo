using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [RoutePrefix("api/EntityResource")]
    public class EntityResourceController : ApiController
    {
        [Route("")]
        public HttpResponseMessage GetEntityResourceByTableName(string objectTableName, int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            ObjectTable objectTable = new ObjectTableRepository(tenant).GetObjectTableByName(objectTableName, tenant, false);
            return Request.CreateResponse(HttpStatusCode.OK, objectTable?.EntityResource);
        }
    }
}
