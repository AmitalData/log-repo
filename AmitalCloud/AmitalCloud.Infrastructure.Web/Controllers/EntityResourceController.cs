using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [RoutePrefix("api/EntityResource")]
    public class EntityResourceController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetEntityResourceByTableName(string objectTableName, int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            ObjectTable objectTable = new ObjectTableRepository(tenant).GetObjectTableByName(objectTableName, tenant, false);
            return Request.CreateResponse(HttpStatusCode.OK, objectTable?.EntityResource);
        }
    }
}
