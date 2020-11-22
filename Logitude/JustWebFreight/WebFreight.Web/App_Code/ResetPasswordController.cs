using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using Logitude.SystemLogs;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class ResetPasswordController : ApiController
    {
        public UserData PostResetPassword(ResetPasswordParameters resetPasswordParameters)
        {
            try
            {
                ResetPasswordHelper resetPasswordHelper = new ResetPasswordHelper();
                UserData userData = resetPasswordHelper.ForgetPassword(resetPasswordParameters);
                return userData;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, resetPasswordParameters.Email, "", "ResetPasswordController : PostResetPassword", null);
                UserData data = new UserData();
                data.HasError = true;
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += Environment.NewLine + e.InnerException.Message;
                }
                data.ExceptionMessage = message;
                return data;
            }

        }
    }
}