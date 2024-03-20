using Logitude.BL.Helpers.ExportServer;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace WebFreight.Web.Controllers
{
    public class TaxesController : ApiController
    {
        public HttpResponseMessage GetLinkLogin()
        {
            AuthenticationToken authToken = GetTenantFromToken();
            if (authToken == null) return Request.CreateResponse(HttpStatusCode.Unauthorized);

            int tenant = authToken.Tenant;
            string email = authToken.Email;
            string exportToken = ExportServerService.GetTokenForConfirmationNumber(tenant, email);
            string link = ExportServerLogin.GetLinkToLogin(tenant, email, exportToken);
            return Request.CreateResponse(HttpStatusCode.OK, link);
        }

        [HttpGet]
        public HttpResponseMessage CloudSettings()
        {
            AuthenticationToken authToken = GetTenantFromToken();
            if (authToken == null) return Request.CreateResponse(HttpStatusCode.Unauthorized);

            ExportServerSettings exportServerSettings = ExportServerService.GetSettings(authToken.Tenant);

            return Request.CreateResponse(HttpStatusCode.OK, exportServerSettings);
        }

        [HttpPut]
        public HttpResponseMessage UpdateSettings([FromBody] ExportServerSettings newSettings)
        {
            AuthenticationToken authToken = GetTenantFromToken();
            if (authToken == null) return Request.CreateResponse(HttpStatusCode.Unauthorized);

            ExportServerService.UpdateSettings(authToken.Tenant, newSettings);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]        
        public HttpResponseMessage CreateConfirmationNumber([FromBody] dynamic body)
        {
            AuthenticationToken authToken = GetTenantFromToken();
            if (authToken == null) return Request.CreateResponse(HttpStatusCode.Unauthorized);

            return ExportServerService.CreateConfirmationNumber(authToken.Tenant, authToken.Email, Convert.ToString(body)).Res;
        }

        private static AuthenticationToken GetTenantFromToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            
            return authToken;
        }
    }
}
