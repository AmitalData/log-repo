using Logitude.XSD;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.WebServices
{
    public class InboundEmailWebServiceController : ApiController
    {
        public HttpResponseMessage GetMessageResult(string recepient, int tenant, string subject, string body, string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int mytenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(mytenant);
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    InboundEmailWebService myService = new InboundEmailWebService();
                    myService.SendInboundEmail(recepient, subject, body, mytenant, entityId);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}