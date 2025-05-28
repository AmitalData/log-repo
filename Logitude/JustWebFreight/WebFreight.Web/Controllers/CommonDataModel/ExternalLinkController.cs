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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.Controllers.CommonDataModel
{
    public class ExternalLinkController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetExternalLink(string Ref, string param)
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

            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult GetForward(string token)
        {
            AuthenticationToken authToken = null;

            try
            {
                authToken = new AuthenticationTokenRepository().GetSingleToken(token);

                if (authToken == null)
                    return Unauthorized();

                if (authToken?.ExpirationDate < DateTime.Now)
                {
                    authToken = new AuthenticationTokenRepository().GetSingleToken(token);
                    string supportEmail = new TenantManagementQuery().GetSinglePM(authToken.Tenant)?.EcommerceSupportEmail;
                    string msg = new TextCodeRepository(authToken.Tenant).GetTextCodeByTenantAndCode("General.O.GetSupportEmail", authToken.Tenant)?.LocalDefaultText;
                    return Content(HttpStatusCode.BadGateway, $"{msg} {supportEmail}");
                }

                string link = new ExternalLinkQuery(authToken.Tenant).GetFormToken(authToken);
                if (string.IsNullOrEmpty(link))
                    return BadRequest("Link not found");

                return Ok(link);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}