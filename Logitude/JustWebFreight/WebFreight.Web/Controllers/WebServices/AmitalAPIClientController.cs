using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.AmitalAPI;
using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Controllers.WebServices
{
    public class AmitalAPIClientController : AmitalAPIControllerBase<AmitalApiClientApi, AmitalApiClient> {
        AmitalApiClientApi clientApi = new AmitalApiClientApi();

        [HttpGet]
        public override HttpResponseMessage GetAll()
        {
            int? tenant = HeaderHelper.GetTenantFromToken();
            if (tenant == null || !HeaderHelper.IsGlobaUser())
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            string token = GetAmitalApiToken(tenant.Value);
            var res = clientApi.GetAll(token);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }
}