using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers.ExportServer;

namespace WebFreight.Web.Controllers
{
    public class TaxesController : ApiController
    {
        public HttpResponseMessage GetLinkLogin()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            if(authToken == null) return Request.CreateResponse(HttpStatusCode.Unauthorized);

            int tenant = authToken.Tenant;
            string email = authToken.Email;

            string link =  ExportServerLogin.GetLinkToLogin(tenant, email);
            return Request.CreateResponse(HttpStatusCode.OK, link);
        } 
    }
}
