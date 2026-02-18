using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Web.DataContracts;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class AuthenticationController : ApiController
    {
        public string GetSettingsLoginCode(int myDummyInteger, string myDummyString)
        => new SettingQueryService(0).GetMulti(a => true, a => a.LogoCode).FirstOrDefault();

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
                LogException(ex, loginParameters.Email, "PostUserValidation");
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
                LogException(ex, parameters.Email, "PostLoginData", tenant);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        private void LogException(Exception ex, string email, string methodName, int tenant = 0)
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, tenant, email, "", $"AuthenticationController : {methodName}", null);
            string message = ex.InnerException?.Message + Environment.NewLine + ex.Message;
            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, message);
        }
    }
}