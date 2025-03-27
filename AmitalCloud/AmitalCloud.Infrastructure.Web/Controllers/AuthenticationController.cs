using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Web.DataContracts;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class AuthenticationController : ApiController
    {
        public string GetSettingsLoginCode(int myDummyInteger, string myDummyString)
        {
            string myResult = "";
            IGlobalContext globalContext = GlobalContext.GetContext();
            Setting mySettings = globalContext.Settings.FirstOrDefault();
            if (mySettings == null)
            {
                return myResult;
            }
            myResult = mySettings.LogoCode;
            return myResult;
        }

        public HttpResponseMessage PostUserValidation(LoginParameters loginParameters)
        {
            try
            {
                Authentication authentication = new Authentication(loginParameters.Tenant);
                UserData data = authentication.AuthenticateUser(loginParameters);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, loginParameters.Email, "", "AuthenticationController : PostUserValidation", null);
                string message = "";
                if (ex.InnerException != null)
                {
                    message += ex.InnerException.Message + Environment.NewLine;
                }
                message += ex.Message;
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [Route("api/Authentication")]
        public HttpResponseMessage PostLoginData(LoginParameters parameters, int tenant, bool? isFromCTool = false)
        {
            try
            {
                Authentication authentication = new Authentication(tenant);
                UserData user = authentication.LoginUser(parameters, tenant, isFromCTool);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, parameters.Email, "", "AuthenticationController : PostLoginData", null);
                string errorMessage = "";
                if (ex.InnerException != null)
                {
                    errorMessage = ex.InnerException.Message + Environment.NewLine;
                }
                errorMessage += ex.Message;
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, errorMessage);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}