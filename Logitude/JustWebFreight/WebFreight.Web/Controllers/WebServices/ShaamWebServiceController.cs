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

        [HttpGet]
        [Route("linkToCodeForToken")]
        public HttpResponseMessage LinkToCodeForToken(string user)
        {
            return TryCatchWrapper((tenant) =>
            {
                HttpClienResponse apiToShaamRes = shaamService.LinkToCodeForToken(tenant, user);
                return apiToShaamRes;
            }, true);
        }

        [HttpGet]
        [Route("tokens")]
        public HttpResponseMessage Tokens(int pageSize, int page)
        {
            try
            {
                int tenant = GetTenantFromToken();

                HttpClienResponse apiToShaamRes = shaamService.Tokens(tenant, pageSize, page);

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
                HttpClienResponse apiToShaamRes = shaamService.NewRefreshToken(tenant, body.user, body.code);
                return apiToShaamRes;
            });           
        }

        [HttpPost]
        [Route("createConfirmationNumber")]
        public HttpResponseMessage CreateConfirmationNumber([FromBody] dynamic body, int? confirmationTokenLogId = null)
        {
            return TryCatchWrapper((tenant) =>
            {
                string invoiceJson = Convert.ToString(body);
                HttpClienResponse apiToShaamRes = shaamService.CreateConfirmationNumber(invoiceJson, tenant, confirmationTokenLogId);
                return apiToShaamRes;
            });
        }

        private HttpResponseMessage TryCatchWrapper(Func<int, HttpClienResponse> func, bool returnContent = false)
        {
            try
            {
                int tenant = GetTenantFromToken();

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

        private int GetTenantFromToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken.Tenant;
        }

        public class NewRefreshTokenBody
        {
            public string user { get; set; }
            public string code { get; set; }
        }
    }
}