using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Web.DataContracts;
using System;
using System.Linq;
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

        [ActionName("PostUserValidation")]
        public UserData PostUserValidation(LoginParameters loginParameters)
        {
            try
            {
                Authentication authentication = new Authentication(loginParameters.Tenant);
                UserData data = authentication.AuthenticateUser(loginParameters);
                return data;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, loginParameters.Email, "", "AuthenticationController : PostUserValidation", null);
                UserData data = new UserData();
                data.HasError = true;
                string message = "";

                if (e.InnerException != null)
                {
                    message += e.InnerException.Message;
                }
                message += Environment.NewLine + e.Message;
                //todo
                //NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e, message);
                data.ExceptionMessage = message;
                return data;
            }
        }

        public UserData PostLoginData(LoginParameters parameters, int tenant, bool? isFromCTool = false)
        {
            try
            {
                Authentication authentication = new Authentication(tenant);
                UserData user = authentication.LoginUser(parameters, tenant, isFromCTool);
                return user;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, parameters.Email, "", "AuthenticationController : PostLoginData", null);
                string errorMessage = "";

                if (e.InnerException != null)
                {
                    errorMessage = e.InnerException.Message + Environment.NewLine;
                }

                errorMessage += e.Message;

                //todo
                //NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e, errorMessage);
                UserData user = new UserData();
                user.HasError = true;
                user.ExceptionMessage = errorMessage;
                return user;
            }
        }
    }
}