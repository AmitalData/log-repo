using System;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using Logitude.SystemLogs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using System.Net.Http;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Linq;
using System.Net;
using WebFreight.Web.WebServices;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using WebFreight.Web.Helpers.MixPanel;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Text;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalAuthenticationController : ApiController
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

        [ActionName("PostDigitalPortalChangePassword")]
        public HttpResponseMessage PostDigitalPortalChangePassword(ResetPasswordParameters param, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email)) email = email.ToLower();
                string newPassword = param.NewPassword;
                bool isResetRequest = param.IsResetRequest;
                string requestNumber = param.RequestNumber;
                bool succeeded = false;

                if (!string.IsNullOrEmpty(email))
                {
                    PasswordChangeHelper passwordChangeHelper = new PasswordChangeHelper();
                    if (isResetRequest && !string.IsNullOrEmpty(requestNumber))
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        GlobalContact contact = globalContext.GlobalContacts.Where(c => c.Email == email && c.InActive == false).FirstOrDefault();

                        PasswordResetRequest request = globalContext.PasswordResetRequests.Where(r => r.RequestNumber == requestNumber && r.Email.ToLower() == email.ToLower() && r.IsDone == false).FirstOrDefault();
                        if (request != null)
                        {
                            passwordChangeHelper.ValidationPassword(email, newPassword);
                            succeeded = passwordChangeHelper.ChangePassword(email, newPassword);
                            if (succeeded)
                            {
                                request.IsDone = true;
                                globalContext.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        passwordChangeHelper.ValidationPassword(email, newPassword, param.OldPassword, true);
                        succeeded = passwordChangeHelper.ChangePassword(email, newPassword, param.OldPassword);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, succeeded);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
       
        [ActionName("PostDigitalPortalCheckPasswordUser")]
        public HttpResponseMessage PostDigitalPortalCheckPasswordUser(ChangePasswordParameter changePasswordParameter)
        {
            try
            {
                PasswordCheckService passwordCheckService = new PasswordCheckService();
                bool result = passwordCheckService.CheckUserPassword(changePasswordParameter.CurrentPassword, changePasswordParameter.ContactId, 0, changePasswordParameter.Email);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }    
}