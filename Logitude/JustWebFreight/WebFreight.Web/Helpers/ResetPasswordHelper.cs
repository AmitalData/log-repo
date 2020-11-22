using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class ResetPasswordHelper
    {
        public UserData ForgetPassword(ResetPasswordParameters resetPasswordParameters)
        {
            UserData userData = new UserData();

            string email = resetPasswordParameters.Email;
            string tenant = ResolveEmail(ref email);
            resetPasswordParameters.Email = email;

            if (resetPasswordParameters.UseCaptcha)
                userData = CheckForgotPasswordCaptchaCode(resetPasswordParameters);

            if (!userData.InValidCaptcha)
            {
                IGlobalContext globalObjectContext = GlobalContext.GetContext();
                List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == resetPasswordParameters.Email && (m.IsUser == true || m.InternetAccess == true)).Include("GlobalTenant").ToList();
                if (contacts.Count == 0)
                {
                    AddInvalidEmailResetPassword(resetPasswordParameters.Email, 0);
                }
                else
                {
                    userData = RequestToResetUserPassword(resetPasswordParameters, contacts, tenant);
                }
            }

            return userData;
        }

        private UserData RequestToResetUserPassword(ResetPasswordParameters resetPasswordParameters, List<GlobalContact> contacts, string tenant)
        {
            UserData userData = new UserData();
            contacts = contacts.Where(m => m.GlobalTenant.IsActive == true && m.InActive == false).ToList();
            if (contacts.Count > 0)
            {
                CreateChangePasswordLogAfterCheckContactPassword(resetPasswordParameters, tenant);
            }
            else
            {
                userData.HasError = true;
                userData.InActive = true;
                CreateChangePasswordLog("(ForgetPassword) Email is not sent successfully", "", "", resetPasswordParameters.Email);
            }

            return userData;
        }

        private void CreateChangePasswordLogAfterCheckContactPassword(ResetPasswordParameters resetPasswordParameters, string tenant)
        {
            UserData userData = new UserData();
            PasswordCheckService passwordChkService = new PasswordCheckService();
            bool result;

            if (string.IsNullOrEmpty(resetPasswordParameters.AppEnvironment))
                resetPasswordParameters.AppEnvironment = "Unifreight";

            if (!string.IsNullOrEmpty(tenant))
                result = passwordChkService.RequestResetUserPassword(resetPasswordParameters, resetPasswordParameters.AppEnvironment, tenant);
            else
                result = passwordChkService.RequestResetUserPassword(resetPasswordParameters, resetPasswordParameters.AppEnvironment);

            bool inValidEmail = false;
            if (!result)
            {
                userData.HasError = true;
                bool isLocked = passwordChkService.CheckIfUserIsLocked(resetPasswordParameters.Email, ref inValidEmail);
                userData.IsLocked = isLocked;
                // userData.InValidMailOrPassword = inValidEmail;
                CreateChangePasswordLog("(ForgetPassword) Email is not sent successfully", "", "", resetPasswordParameters.Email);
            }
            else
            {
                CreateChangePasswordLog("(ForgetPassword) Email has been sent successfully", "", "", resetPasswordParameters.Email);
            }
        }

        private static UserData CheckForgotPasswordCaptchaCode(ResetPasswordParameters resetPasswordParameters)
        {
            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            CaptchaHelper captchaHelper = new CaptchaHelper();
            UserData userData = new UserData();
            string iP = AuthenticationUtil.GetIP4Address();
            DateTime dateNowBefor5Minutes = DateTime.Now.AddMinutes(-5);
            bool checkCaptcha = false;

            int resetPasswordCount = globalObjectContext.InvalidEmailResetPasswords.Where(a => a.IP == iP && a.CreateDate >= dateNowBefor5Minutes).Count();
            if (resetPasswordCount < 4)
            {
                resetPasswordCount += globalObjectContext.ChangePasswordLogs.Where(a => a.IP == iP && a.CreateDate >= dateNowBefor5Minutes).Count();
            }
            if (!string.IsNullOrEmpty(resetPasswordParameters.CaptchaCode) && !string.IsNullOrEmpty(resetPasswordParameters.CaptchaKey)) 
                checkCaptcha = true;
            if (!checkCaptcha)
            {
                if (resetPasswordCount >= 5) checkCaptcha = true;
                else
                {
                    int countCaptchaKey = globalObjectContext.CaptchaKeys.Where(a => a.IP == iP && a.CreateDate >= dateNowBefor5Minutes && a.Activity == "ResetPassword").Count();
                    if (countCaptchaKey >= 5) checkCaptcha = true;
                }
            }

            if (checkCaptcha && !captchaHelper.CheckCaptchaCodeValidated(resetPasswordParameters.CaptchaCode, resetPasswordParameters.CaptchaKey)) 
                captchaHelper.AddCaptchaKey(resetPasswordParameters.Email, userData, "ResetPassword");
            return userData;
        }

        private string ResolveEmail(ref string email)
        {
            string tenant = "";
            if (email.Contains("^"))
            {
                var data = email.Split('^');

                if (data != null && data.Length > 1)
                {
                    email = data[0];
                    if (!string.IsNullOrEmpty(data[1])) tenant = data[1];
                }
            }

            email = email.ToLower();
            return tenant;
        }

        private void AddInvalidEmailResetPassword(string email, int tenant)
        {
            InvalidEmailResetPasswordRepository invalidEmailResetPasswordRepository = new InvalidEmailResetPasswordRepository();
            InvalidEmailResetPassword invalidEmailResetPassword = new InvalidEmailResetPassword()
            {
                Id = IdCounter.GetNumber("InvalidEmailResetPassword", tenant),
                IP = AuthenticationUtil.GetIP4Address(),
                CreateDate = DateTime.Now,
                Email = email
            };
            invalidEmailResetPasswordRepository.Add(invalidEmailResetPassword);
            invalidEmailResetPasswordRepository.SubmitChanges();

     
        }

        public void CreateChangePasswordLog(string log, string currentpassword, string enteredpassword, string email)
        {
            ChangePasswordlogRepository changePasswordlogRepository = new ChangePasswordlogRepository();

            ChangePasswordLog changePasswordLog = new ChangePasswordLog()
            {
                Id = Guid.NewGuid().ToString(),
                CurrentPassword = currentpassword,
                EnteredPassword = enteredpassword,
                Email = email,
                CreateDate = DateTime.Now,
                log = log,
                IP =AuthenticationUtil.GetIP4Address(),
            };

            changePasswordlogRepository.Add(changePasswordLog);
            changePasswordlogRepository.SubmitChanges();

        }
    }
}