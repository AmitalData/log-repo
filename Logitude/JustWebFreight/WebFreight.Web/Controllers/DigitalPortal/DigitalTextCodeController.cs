using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalTextCodeController : ApiController
    {
        [HttpGet]
        [Route("DigitalText/GetTextCodesByFilters")]
        public HttpResponseMessage GetTextCodesByFilters(string cardId, string objectTableId = null)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
                var textCodeQuery = new DigitalTextCodeQuery();
                var codes = textCodeQuery.GetDigitalTextCodesQuery(tenant, objectTableId);
                return Request.CreateResponse(HttpStatusCode.OK, codes);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}