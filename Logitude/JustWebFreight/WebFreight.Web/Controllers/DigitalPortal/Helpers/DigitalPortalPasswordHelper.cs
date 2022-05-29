using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class DigitalPortalPasswordHelper
    {
        public UserData ForgetPassword(ResetPasswordParameters resetPasswordParameters, bool useCaptcha)
        {
            UserData userData = new UserData();

            string email = resetPasswordParameters.Email;
            string tenant = ResolveEmail(ref email);
            resetPasswordParameters.Email = email;

            if (useCaptcha)
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

        private UserData RequestToResetUserPassword(ResetPasswordParameters resetPasswordParameters, List<GlobalContact> contacts, string tenant)
        {
            UserData userData = new UserData();
            bool hasActiveContact = HasActiveContact(contacts);
            bool isValidContact = CheckIsValidContactByEmail(resetPasswordParameters.Email);
            string logMessage;

            if (hasActiveContact && isValidContact)
            {
                PerformUserPasswordReset(resetPasswordParameters, tenant);
                logMessage = "(ForgetPassword) Email has been sent successfully";
            }
            else
            {
                userData = GetErrorUserDataDetails(resetPasswordParameters.Email, hasActiveContact);
                logMessage = "(ForgetPassword) Email is not sent successfully";
            }

            CreateChangePasswordLog(logMessage, "", "", resetPasswordParameters.Email);
            return userData;
        }

        private bool HasActiveContact(List<GlobalContact> contacts)
        {
            var activeContacts = contacts.Where(m => m.GlobalTenant.IsActive == true && m.InActive == false).ToList();

            if (activeContacts.Count < 0)
            {
                return false;
            }

            return true;
        }

        private bool CheckIsValidContactByEmail(string email)
        {
            IGlobalContext globalContext = GlobalContext.GetContext();
            ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == email).FirstOrDefault();

            if (contactPassword == null)
            {
                return false;
            }

            return true;
        }

        private void PerformUserPasswordReset(ResetPasswordParameters resetPasswordParameters, string tenant)
        {
            if (string.IsNullOrEmpty(resetPasswordParameters.AppEnvironment))
                resetPasswordParameters.AppEnvironment = "Unifreight";

            DigitalPortalResetUserPasswordService resetUserPasswordService = new DigitalPortalResetUserPasswordService();
            resetUserPasswordService.ResetUserPassword(resetPasswordParameters, tenant);
        }

        private UserData GetErrorUserDataDetails(string email, bool hasActiveContact)
        {
            UserData userData = new UserData();
            userData.HasError = true;

            if (!hasActiveContact)
            {
                userData.InActive = true;
            }
            else
            {
                bool isLocked = CheckIsUserLockedByEmail(email);
                userData.IsLocked = isLocked;
            }

            return userData;
        }

        private bool CheckIsUserLockedByEmail(string email)
        {
            bool isLocked = false;
            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            isLocked = globalObjectContext.ContactPasswords.Where(c => c.Email == email).FirstOrDefault().IsLocked;
            return isLocked;
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
                IP = AuthenticationUtil.GetIP4Address(),
            };

            changePasswordlogRepository.Add(changePasswordLog);
            changePasswordlogRepository.SubmitChanges();
        }
    }
}