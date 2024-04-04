using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.AmitalAPI;
using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Controllers.WebServices
{
    public class AmitalAPISchemaController : AmitalAPIControllerBase<AmitalApiSchemaApi, AmitalApiSchema>
    {
        AmitalApiSchemaApi schemaApi = new AmitalApiSchemaApi();

        [HttpGet]
        public HttpResponseMessage GetSettings()
        {
            int? tenant = HeaderHelper.GetTenantFromToken();
            if (tenant == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            string token = GetAmitalApiToken(tenant.Value);
            var res = schemaApi.GetSettings(token, tenant.Value); 
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }
}