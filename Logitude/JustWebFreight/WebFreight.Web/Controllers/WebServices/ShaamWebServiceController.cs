using Logitude.XSD.CW_API.ABM;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebServices
{
    public class ShaamWebServiceController : ApiController
    {
        [HttpGet]
        [Route("Logs")]
        public HttpResponseMessage Logs()
        {
            try
            {
                int tenant = AuthorizedToken();

                string subject = "Get confirmation number";
                //GetShareManifestCommunicationLogByEntityIdAndQueueNameAndSubject


                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("Tokens")]
        public HttpResponseMessage Tokens()
        {
            try
            {
                int tenant = AuthorizedToken();
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static int AuthorizedToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken.Tenant;
        }
    }
}