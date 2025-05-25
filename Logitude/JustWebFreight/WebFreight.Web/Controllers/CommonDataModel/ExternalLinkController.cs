using System.Web.Http;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net.Http;
using System.Net;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Infrastructure.BL.Validators;

namespace WebFreight.Web.Controllers.CommonDataModel
{
    public class ExternalLinkController : ApiController
    {
        public Response GetExternalLink(string Ref, string param)
        {            
            AuthenticationToken authToken = HeaderHelper.Authenticate();

            Response response = new Response();

            try
            {
                ExternalLinkQuery externalLinkQuery = new ExternalLinkQuery(authToken.Tenant);
                string link = externalLinkQuery.AddExternalLink(Ref, param, authToken.Tenant);
                response.HasError = false;
                response.Result = link;
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

                if (authToken == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Token is not valid");

                if (authToken?.ExpirationDate < DateTime.Now)
                    throw new AutenticationException("Session expired. Please log in again");

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Redirect);
                string link = new ExternalLinkQuery(authToken.Tenant).GetFormToken(authToken);
                if (string.IsNullOrEmpty(link))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Link not found");

                response.Headers.Location = new Uri(link);

                return response;
            }
            catch (AutenticationException e)
            {
                string msg = e.Message;
                if (msg.Contains("Session expired"))
                {
                    authToken = new AuthenticationTokenRepository().GetSingleToken(token);
                    string supportEmail = new TenantManagementQuery().GetSinglePM(authToken.Tenant)?.EcommerceSupportEmail;
                    msg = InfrastructureTranslateTextsClass.Translate("General.O.GetSupportEmail", authToken.Tenant) + supportEmail;
                }

                return Request.CreateResponse(HttpStatusCode.Unauthorized, msg);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(e));
            }
        }
    }
}