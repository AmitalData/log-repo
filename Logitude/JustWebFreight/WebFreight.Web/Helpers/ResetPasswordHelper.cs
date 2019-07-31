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
        public UserData ForgetPassword(string email, bool ischamplogin, bool ismobile, bool usecaptcha, string captchaCode =null, string captchaKey = null, string appEnvironment = "Unifreight")
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            PasswordCheckService passwordChkService = new PasswordCheckService();

            string tenant = ResolveEmail(ref email);
            UserData userData = new UserData();

            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            string iP = AuthenticationUtil.GetIP4Address();
            DateTime dateNowBefor5Minutes = DateTime.Now.AddMinutes(-5);
            int resetPasswordCount = 0;
            bool checkCaptcha = false;


            if (usecaptcha)
            {
                resetPasswordCount = globalObjectContext.InvalidEmailResetPasswords.Where(a => a.IP == iP && a.CreateDate >= dateNowBefor5Minutes).Count();
                if (resetPasswordCount < 4)
                {
                    resetPasswordCount += globalObjectContext.ChangePasswordLogs.Where(a => a.IP == iP && a.CreateDate >= dateNowBefor5Minutes).Count();
                }

                if (!string.IsNullOrEmpty(captchaCode) && !string.IsNullOrEmpty(captchaKey)) checkCaptcha = true;
                if (!checkCaptcha)
                {
                    if (resetPasswordCount >= 5) checkCaptcha = true;
                    else
                    {
                        int countCaptchaKey = globalObjectContext.CaptchaKeys.Where(a => a.IP == iP && a.CreateDate >= dateNowBefor5Minutes && a.Activity == "ResetPassword").Count();
                        if (countCaptchaKey >= 5) checkCaptcha = true;
                    }
                }


                if (checkCaptcha && !captchaHelper.CheckCaptchaCodeValidated(captchaCode, captchaKey)) captchaHelper.AddCaptchaKey(email, userData, "ResetPassword");
            }

            if (!userData.InValidCaptcha)
            {
                List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && (m.IsUser == true || m.InternetAccess == true)).Include("GlobalTenant").ToList();
                if (contacts.Count == 0)
                {
                    //userData.HasError = true;
                    AddInvalidEmailResetPassword(email, 0);

                    // userData.InValidMailOrPassword = true;

                    //if (usecaptcha)
                    //{
                    //    if (checkCaptcha || (resetPasswordCount + 1) >= 5)
                    //    {
                    //        captchaHelper.AddCaptchaKey(email, userData, "ResetPassword");
                    //    }
                    //}
                }
                else
                {
                    contacts = contacts.Where(m => m.GlobalTenant.IsActive == true && m.InActive == false).ToList();
                    if (contacts.Count > 0)
                    {
                        bool result = false;
                        if (!string.IsNullOrEmpty(tenant))
                        {
                            result = passwordChkService.RequestResetUserPassword(email, ischamplogin, ismobile, appEnvironment, tenant);
                        }
                        else
                        {
                            result = passwordChkService.RequestResetUserPassword(email, ischamplogin, ismobile, appEnvironment);
                        }

                        bool inValidEmail = false;
                        bool isLocked = false;
                        if (!result)
                        {
                            userData.HasError = true;
                            isLocked = passwordChkService.CheckIfUserIsLocked(email, ref inValidEmail);
                            userData.IsLocked = isLocked;
                           // userData.InValidMailOrPassword = inValidEmail;
                            CreateChangePasswordLog("(ForgetPassword) Email is not sent successfully", "", "", email);
                        }
                        else
                        {
                            CreateChangePasswordLog("(ForgetPassword) Email has been sent successfully", "", "", email);
                        }



                    }
                    else
                    {
                        userData.HasError = true;
                        userData.InActive = true;
                        CreateChangePasswordLog("(ForgetPassword) Email is not sent successfully", "", "", email);
                    }
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