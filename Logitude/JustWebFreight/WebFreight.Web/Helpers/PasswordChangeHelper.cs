using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class PasswordChangeHelper
    {
        public bool ChangePassword(string email, string newPassword, string oldPassword=null , string token = null)
        {
            bool result = false;

            IGlobalContext globalContext = GlobalContext.GetContext();
            ContactPassword contactPassword = AuthenticationUtil.VerifyContactPassword(email, oldPassword, globalContext);
           
            if (contactPassword != null)
                {
                    contactPassword.Password = PasswordGenerator.GetBCryptHashedPassword(email, newPassword);
                    contactPassword.IsLocked = false;
                    contactPassword.MustChangePassword = false;
                    contactPassword.IsBCrypt = true;
                    contactPassword.PasswordExpirationDate = null;
                    globalContext.SaveChanges();

                    if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken != null)
                        {
                            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(authToken.Tenant);
                            authToken.Password = contactPassword.Password;
                            authenticationTokenRepository.Update(authToken);
                            authenticationTokenRepository.SubmitChanges();

                            string entityName = "Token" + authToken.Token;
                            if (CacheManager.CacheWrapper.Get(entityName) != null)
                            {
                                CacheManager.CacheWrapper.Remove(entityName);
                            }

                        }

                    }


                result = true;

                }
            
             return result;

        }

        public bool ChangePassword(string email, string newPassword)
        {
            bool result = false;
            IGlobalContext globalContext = GlobalContext.GetContext();
            ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == email).FirstOrDefault();

            if (contactPassword != null)
            {
                contactPassword.Password = PasswordGenerator.GetBCryptHashedPassword(email, newPassword);
                contactPassword.IsLocked = false;
                contactPassword.MustChangePassword = false;
                contactPassword.IsBCrypt = true;
                contactPassword.PasswordExpirationDate = null;
                globalContext.SaveChanges();
                result = true;

            }

            return result;

        }

        public void ValidationPassword(string email, string newPassword , string currentPassword = null, bool checkCurrentPassword = false)
        {
            if (checkCurrentPassword)
            {
                if (string.IsNullOrEmpty(currentPassword)) throw new Exception("Current Password can't be empty!");
                PasswordCheckService passwordCheckService = new PasswordCheckService();
                bool result = passwordCheckService.CheckUserPassword(currentPassword, null, 0, email);
                if (!result) throw new Exception(TextCodesTranslator.TranslateText("User.M.CurrentPasswordDoesntMatchYourInput", 0));
            }

            if (string.IsNullOrEmpty(newPassword)) throw new Exception("New Password can't be empty!");
            if (newPassword.Length < 8) throw new Exception(TextCodesTranslator.TranslateText("User.M.PasswordsMinimumLengthIs8Characters", 0));
            if (newPassword.Length > 16) throw new Exception(TextCodesTranslator.TranslateText("User.M.PasswordsMaximumLlengthIs16Characters", 0));

            ValidatePassword(newPassword);


        }


        private void ValidatePassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            int i = 0;
            if (hasLowerChar.IsMatch(password)) i += 1;
            if (hasUpperChar.IsMatch(password)) i += 1;
            if (hasNumber.IsMatch(password)) i += 1;
            if (hasSymbols.IsMatch(password)) i += 1;

            if (i < 3)
            {
                throw new Exception("Password is not strong enough. Please use at least three of the four characters types possible.");
            }

        }


    }
}