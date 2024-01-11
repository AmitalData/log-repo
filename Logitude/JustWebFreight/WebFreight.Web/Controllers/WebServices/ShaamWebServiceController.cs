using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.WebServices.Services;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class ShaamWebServiceController : ApiController
    {
        ShaamService shaamService = new ShaamService();
        AllocateInvoiceService allocateInvoiceService = new AllocateInvoiceService();

        [HttpGet]
        [Route("linkToCodeForToken")]
        public HttpResponseMessage LinkToCodeForToken(string user)
        {
            return TryCatchWrapper((tenant) =>
            {
                HttpClienResponse apiToShaamRes = shaamService.LinkToCodeForToken(tenant.Value, user);
                return apiToShaamRes;
            }, true);
        }

        [HttpGet]
        [Route("tokens")]
        public HttpResponseMessage Tokens(int pageSize, int page)
        {
            try
            {
                int? tenant = GetTenantFromToken();
                if (tenant == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                HttpClienResponse apiToShaamRes = shaamService.Tokens(tenant.Value, pageSize, page);

                if (apiToShaamRes.Res.StatusCode != HttpStatusCode.OK)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception(apiToShaamRes.Content)));

                var json = JsonConvert.DeserializeObject<object>(apiToShaamRes.Content);
                return Request.CreateResponse(HttpStatusCode.OK, json);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("newRefreshToken")]
        public HttpResponseMessage NewRefreshToken([FromBody] NewRefreshTokenBody body)
        {
            return TryCatchWrapper((tenant) =>
            {
                HttpClienResponse apiToShaamRes = shaamService.NewRefreshToken(body.tenant, body.user, body.code);
                return apiToShaamRes;
            }, false, false);
        }

        [HttpPost]
        [Route("createConfirmationNumber")]
        public HttpResponseMessage CreateConfirmationNumber([FromBody] dynamic body)
        {
            return TryCatchWrapper((tenant) =>
            {
                string invoiceJson = Convert.ToString(body);
                HttpClienResponse apiToShaamRes = allocateInvoiceService.CreateConfirmationNumber(invoiceJson, tenant.Value);
                return apiToShaamRes;
            });
        }

        [HttpPut]
        [Route("UpdateSettings")]
        public HttpResponseMessage UpdateSettings([FromBody] UpdateSettingsData body)
        {
            return TryCatchWrapper((tenant) =>
            {                
                HttpClienResponse apiToShaamRes = shaamService.UpdateSettings(body, tenant.Value);
                return apiToShaamRes;
            });
        }

        [HttpGet]
        [Route("settings")]
        public HttpResponseMessage Settings()
        {
            return TryCatchWrapper((tenant) =>
            {                
                HttpClienResponse apiToShaamRes = shaamService.GetSettings(tenant.Value);
                return apiToShaamRes;
            });
        }

        private HttpResponseMessage TryCatchWrapper(Func<int?, HttpClienResponse> func, bool returnContent = false, bool authorize = true)
        {
            try
            {
                int? tenant = GetTenantFromToken();
                if (tenant == null && authorize)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                HttpClienResponse apiToShaamRes = func(tenant);

                if (apiToShaamRes.Res.StatusCode != HttpStatusCode.OK)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(new Exception(apiToShaamRes.Content)));

                return Request.CreateResponse(HttpStatusCode.OK, returnContent ? apiToShaamRes.Content : null);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private int? GetTenantFromToken()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                return authToken.Tenant;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public class NewRefreshTokenBody
        {
            public string user { get; set; }
            public string code { get; set; }
            public int tenant { get; set; }
        }

        public class UpdateSettingsData
        {
            public string clientId { get; set; }
            public string secret { get; set; }
            public string companyName { get; set; }
        }
    }
}