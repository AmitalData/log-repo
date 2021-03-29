

using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.APIDataContract.ApiV1;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class GLAccountChequeDetailsController : ApiController
    {
        public HttpResponseMessage GetSingleGLAccountChequeDetails(string number)
        {
            try
            {
                AuthenticationToken authToken = GetAuthenticationToken();
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                GLAccountChequeDetails gLAccountChequeDetails = GetGLAccountChequeDetails(authToken.Tenant,number);
                return CreateSuccessfulResponse(gLAccountChequeDetails);
             
            }
            catch (Exception ex)
            {
                return CreateFailedResponse(ex);
            }
        }

        private GLAccountChequeDetails GetGLAccountChequeDetails(int tenant, string number)
        {
            GLAccountChequeDetailsInstanceCreator helper = new GLAccountChequeDetailsInstanceCreator(tenant);
            return helper.CreateGLAccountChequeDetailsInstance(number);
        }
        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private HttpResponseMessage CreateSuccessfulResponse(GLAccountChequeDetails gLAccountChequeDetails)
        {
                return Request.CreateResponse(HttpStatusCode.OK, gLAccountChequeDetails);
        }
        private HttpResponseMessage CreateFailedResponse(Exception exception)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(exception);
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);

        }
    }
}