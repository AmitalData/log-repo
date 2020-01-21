using Logitude.Accounting.BL.APIDataContract.ApiV1;
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
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class GLAccountMoreDataController : ApiController
    {
        public HttpResponseMessage GetSingleGLAccountMoreData(string number )
        {
            try
            {
                AuthenticationToken authToken = GetAuthenticationToken();
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                GLAccount account= GetGLAccountByNumber(number, tenant);
                GLAccountMoreData gLAccountMoreData = GetSingleGLAccountMoreDataByGLAccountId(account);
                return Request.CreateResponse(HttpStatusCode.OK, gLAccountMoreData);
            }
            catch (Exception ex)
            {
                return CreateResponse(ex, null);
            }
        }
        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private HttpResponseMessage CreateResponse(Exception exception, string message)
        {
            if (exception == null && message != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(exception);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        private GLAccount GetGLAccountByNumber(string internalNumber, int tenant)
        {
            GLAccountQueryService Service = new GLAccountQueryService(tenant);
            GLAccount account= Service.GetGLAccountByInternalNumber(internalNumber, tenant);
            if(account == null)
            {
                throw new Exception("There is no GLAccount with number" + internalNumber);
            }
            else { return account; }
        }
        private GLAccountMoreData GetSingleGLAccountMoreDataByGLAccountId(GLAccount account)
        {
            GLAccountMoreDataQueryService accountMoreDataQueryService = new GLAccountMoreDataQueryService(account.Tenant);
            GLAccountMoreData accountMoreData = accountMoreDataQueryService.GetGLAccountMoreDataByAccountId(account.Id, account.Tenant);
            if(accountMoreData == null)
            {
                throw new Exception("GLAccount with number " + account.InternalNumber + " has no GLAccount more data record");
            }
            else {
                accountMoreData.AccountId = null;
                return accountMoreData;
            }

        }
    }
}