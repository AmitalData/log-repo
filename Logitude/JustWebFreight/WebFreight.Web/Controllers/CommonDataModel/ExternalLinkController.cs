using System.Web.Http;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using System;
using System.Net.Http;
using System.Net;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace WebFreight.Web.Controllers.CommonDataModel
{
    public class ExternalLinkController : ApiController
    {
        public Response GetExternalLink(string Ref, string param)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            Response response = new Response();

            try
            {
                string link = new ExternalLinkQuery(authToken.Tenant).GetExternalLink(Ref, param, authToken.Tenant);
                response.HasError = false;
                response.Result = AddOrigin(link);
            }
            catch (Exception e)
            {
                response.InnerErrorMessage = e.InnerException?.Message;
                response.ErrorMessage = e.Message;
                response.HasError = true;
            }

            return response;
        }

        public HttpResponseMessage GetForward(string token)
        {
            AuthenticationToken authToken = null;

            try
            {
                authToken = new AuthenticationTokenRepository().GetSingleToken(token);
            }
            catch (Exception e)
            {
                if (e is AutenticationException)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(e));
            }

            if (authToken == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Token is not valid");

            if (authToken?.ExpirationDate < DateTime.Now)
                throw new AutenticationException("Session expired. Please log in again");

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(AddOrigin(authToken.Params));

            return response;
        }

        private static string AddOrigin(string link)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme;
            string host = HttpContext.Current.Request.Url.Host;
            if (host == "localhost")
            {
                host += ":4200";
                link = link.Replace("/Angular/index.html", "");
            }

            return $"{protocol}://{host}{link}";
        }
    }
}