using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                string cahce_key = "ContactPassword_" + email;
                if (CacheManager.CacheWrapper.Get(cahce_key) != null) CacheManager.CacheWrapper.Remove(cahce_key);


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

        public void ValidationPassword(string email, string newPassword, string currentPassword = null, bool checkCurrentPassword = false)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new Exception(TextCodesTranslator.TranslateText("User.M.PasswordCantBeEmpty", 0));
            }

            ValidatePasswordComplexitySeries(newPassword);
            ValidateStrongPassword(newPassword);


            if (checkCurrentPassword)
            {
                if (string.IsNullOrEmpty(currentPassword)) throw new Exception("Current Password can't be empty!");
                PasswordCheckService passwordCheckService = new PasswordCheckService();
                bool result = passwordCheckService.CheckUserPassword(currentPassword, null, 0, email);
                if (!result) throw new Exception(TextCodesTranslator.TranslateText("User.M.CurrentPasswordDoesntMatchYourInput", 0));
            }


            if (!string.IsNullOrEmpty(currentPassword))
            {
                if (newPassword == currentPassword) throw new Exception(TextCodesTranslator.TranslateText("User.M.NewPasswordCantBeSameAsCurrentOne", 0));
            }

        }


        private void ValidateStrongPassword(string password)
        {
            if (!this.IsContainsLowerUpperCase(password))
            {
                throw new Exception("Your password must include an uppercase and lowercase letter.");
            }

            if (!this.IsContainsNumber(password))
            {

                throw new Exception("Your password must include a number.");
            }
            if (password.Length < 8)
            {
                throw new Exception(TextCodesTranslator.TranslateText("User.M.PasswordsMinimumLengthIs8Characters", 0));
            }

        }

        private bool IsContainsLowerUpperCase(string str)
        {
            bool result = false;

            bool isLower = str.ToArray().Where(d => char.IsLower(d)).Any();
            bool isUpper = str.ToArray().Where(d => char.IsUpper(d)).Any();

            if (isLower && isUpper) result = true;
            return result;

        }

        private bool IsContainsNumber(string str)
        {
            bool isNumber = str.ToArray().Where(d => char.IsNumber(d)).Any();

            return isNumber;

        }
        public void ValidatePasswordComplexitySeries(string password)
        {
            bool result = false;

            List<int> passwordNumnberList = new List<int>();
            if (!string.IsNullOrEmpty(password)) password = password.ToUpper();

            for (var i = 0; i < password.Length; i++)
            {
                char character = password[i];
                var number = 0;
                string numbers = "0123456789";
                if (numbers.Contains(character)) number = Int32.Parse(character.ToString());
                else number = (int)character;
                passwordNumnberList.Add(number);
            }

            result = IsSeries(passwordNumnberList,"+");
            if(!result) result = IsSeries(passwordNumnberList, "-");
            if(result) throw new Exception("Password should not contain more than 3 following characters");

            if (!result)
            {
                result = IsSeries(passwordNumnberList, "Same");
                if (result) throw new Exception("Password should not contain more than 3 consecutive repeating characters");
            }

        }

       public bool IsSeries(List<int> passwordNumnberList , string operatorCode)
        {
            bool result = false ;
            int seriesNumnberCount = 0;
            List<int> seriesNumnberList = new List<int>();
            passwordNumnberList.ForEach((item) =>
            {
                var IsNotSeriesNumnber = false;
                if (item <= 9 || ((item >= 65 && item <= 90)))
                {
                    if (seriesNumnberList.Count() == 0) seriesNumnberList.Add(item);
                    else
                    {
                        if (operatorCode == "+")
                        {
                            if (seriesNumnberList[seriesNumnberList.Count() - 1] + 1 == item)
                            {
                                seriesNumnberList.Add(item);
                                seriesNumnberCount += 1;
                            }
                            else IsNotSeriesNumnber = true;
                        }
                        else if (operatorCode == "-")
                        {
                            if (seriesNumnberList[seriesNumnberList.Count() - 1] - 1 == item)
                            {
                                seriesNumnberList.Add(item);
                                seriesNumnberCount += 1;
                            }
                            else IsNotSeriesNumnber = true;
                        }
                        else if (operatorCode == "Same")
                        {
                            if (seriesNumnberList[seriesNumnberList.Count() - 1]  == item)
                            {
                                seriesNumnberList.Add(item);
                                seriesNumnberCount += 1;
                            }
                            else IsNotSeriesNumnber = true;
                        }

                    }

                }
                else IsNotSeriesNumnber = true;


                if (seriesNumnberCount == 3)
                {
                    result = true;
                    return;
                }

                if (IsNotSeriesNumnber)
                {
                    seriesNumnberCount = 0;
                    seriesNumnberList = new List<int>();
                    seriesNumnberList.Add(item);
                }

            });
            return result;
        }

        

    }
}