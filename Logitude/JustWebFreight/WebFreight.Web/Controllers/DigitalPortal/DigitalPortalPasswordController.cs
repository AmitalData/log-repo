using System;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using Logitude.SystemLogs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPortalPasswordController : ApiController
    {
        [ActionName("PostDigitalPortalResetPassword")]
        public UserData PostDigitalPortalResetPassword(ResetPasswordParameters resetPasswordParameters)
        {
            try
            {
                DigitalPortalPasswordHelper resetPasswordHelper = new DigitalPortalPasswordHelper();
                UserData userData = resetPasswordHelper.ForgetPassword(resetPasswordParameters, true);
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