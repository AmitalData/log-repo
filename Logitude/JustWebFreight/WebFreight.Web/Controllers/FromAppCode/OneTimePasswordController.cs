using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class OneTimePasswordController : ApiController
    {




        public HttpResponseMessage GetOneTimePasswordId(string email, int tenant)
        {

            try
            {
                OneTimePasswordResult result = new OneTimePasswordResult();
                string credentialKey = HttpContext.Current.Request.Headers["CredentialKey"];
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(credentialKey))
                {
                    APICredentialsParameters apiCredentialsParam = new APICredentialsParameters() { PrimaryKey = credentialKey, SecondaryKey = credentialKey, Tenant = tenant };
                    AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
                    APICredentialsHelper helper = new APICredentialsHelper();
                    ApiCredential data = helper.CheckUserState(apiCredentialsParam);

                    if (!data.HasError)
                    {

                        ContactPasswordRepository contactPasswordRepository = new ContactPasswordRepository();
                        var contactPassword = contactPasswordRepository.GetSingleContactPassword(email);
                        UserData userData = CheckUserState(email, apiCredentialsParam.Tenant, contactPassword);

                        if (!userData.HasError)
                        {
                            Random generator = new Random();
                            OneTimePasswordRepository oneTimePasswordRepository = new OneTimePasswordRepository();
                            if (!string.IsNullOrEmpty(email))
                            {
                                result.OneTimePasswordId = Guid.NewGuid().ToString() + generator.Next(0, 10000000).ToString().Substring(0, 5); ;

                                OneTimePassword oneTimePassword = new OneTimePassword()
                                {
                                    Id = result.OneTimePasswordId,
                                    UserEmail = email,
                                    CreateDate = DateTime.UtcNow,
                                    ExpirationDate = DateTime.UtcNow.AddMinutes(15),
                                    IsUsed = false,
                                    Tenant = apiCredentialsParam.Tenant,
                                    UserId = "",

                                };
                                oneTimePasswordRepository.Add(oneTimePassword);
                                oneTimePasswordRepository.SubmitChanges();
                            }
                        }

                        else
                        {
                            result.ExceptionMessage = GetMessageException(userData);
                            result.HasError = userData.HasError;
                        }

                    }
                    else
                    {
                        if (data.IpRestricted) result.ExceptionMessage = "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";
                        if (data.InValidKey) result.ExceptionMessage = "Invalid api credentials key";
                        result.HasError = data.HasError;
                    }

                }

                result.OneTimePasswordPage = "Angular" + GetHtmlVersion() + "/index.html";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        private string GetMessageException(UserData userData)
        {
            string exception = "";

            if (userData.InActive) return "Login failed! invalid user name.";
            if (userData.IpRestricted) return "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";
            if (userData.IsLocked) return "Your account has been locked out!" + "<br/>" + "please try again after 30 minutes.";
            return exception;

        }




        private string GetHtmlVersion()
        {
            string myResult = "";

            IGlobalContext globalContext = GlobalContext.GetContext();
            Setting mySettings = globalContext.Settings.FirstOrDefault();

            if (mySettings != null && !string.IsNullOrEmpty(mySettings.HtmlVersion)) myResult = mySettings.HtmlVersion;

            return myResult;
        }

        public UserData GetUserDataByToken(string id)
        {
            UserData userData  = new UserData();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {

                    OneTimePasswordRepository oneTimePasswordRepository = new OneTimePasswordRepository();
                    OneTimePassword oneTimePassword = oneTimePasswordRepository.GetSingleOneTimePassword(id);

                    if (oneTimePassword != null)
                    {
                        if (oneTimePassword.ExpirationDate > DateTime.UtcNow && !oneTimePassword.IsUsed)
                        {
                            oneTimePassword.IsUsed = true;
                            oneTimePasswordRepository.Update(oneTimePassword);
                            oneTimePasswordRepository.SubmitChanges();
                            ContactPasswordRepository contactPasswordRepository = new ContactPasswordRepository();

                            var contactPassword = contactPasswordRepository.GetSingleContactPassword(oneTimePassword.UserEmail);

                            if (contactPassword != null)
                            {
                                LoginParameters loginParameters = new LoginParameters() { Email = oneTimePassword.UserEmail, GetToken = true, IsUser = true, Password = contactPassword.Password + "@OneTimePassword" };
                                AuthenticationController authenticationController = new AuthenticationController();
                                userData = authenticationController.PostLoginData(loginParameters, oneTimePassword.Tenant);
                            }
                        } else userData = FillError(userData, "Can't not use this key (" + id + ") again because you used it before");


                    }    else userData = FillError(userData, "Can't not find this key (" + id + ")");


                } else userData = FillError(userData, "One time password key is empty");


            }
            catch (Exception ex)
            {
                userData = FillError(userData, ex.Message);
            }

            return userData;
        }

        private UserData FillError(UserData userData, string exception)
        {
            userData.HasError = true;
            userData.ExceptionMessage = exception;


            return userData;
        }

        private UserData CheckUserState(string email, int tenant, ContactPassword contactPassword)
        {
            email = email.ToLower();

            UserData userData = new UserData();

            IGlobalContext globalContext = GlobalContext.GetContext();

            if (contactPassword != null)
            {
                CheckLockedUser(contactPassword, globalContext);

                GlobalContact contact = globalContext.GlobalContacts.Where(d => d.GlobalTenantId == tenant && d.InActive == false && d.Email.ToLower() == email.ToLower()).FirstOrDefault(); //mohammad
                bool customerCare = false;
                bool distributor = false;
                User logitudeUser = null;
                if (contact != null)
                {
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    logitudeUser = (from a in commonDataContext.Users
                                    where a.Id == contact.Id
                                    select a).FirstOrDefault();



                    if (logitudeUser != null)
                    {
                        if (logitudeUser.Tenant == 0)
                        {
                            distributor = logitudeUser.IsDistributor;
                            customerCare = !logitudeUser.IsDistributor;
                        }
                    }


                }
                else userData.InActive = true;


                userData.IsLocked = contactPassword.IsLocked;
                userData.MustChangePassword = contactPassword.MustChangePassword;

                if (customerCare)
                {
                    string ipstring = LogitudeSettings.CustomerCareIP;//System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
                    string[] authenticatedIPs = ipstring.Split(',');
                    if (!authenticatedIPs.Contains("*"))
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        if (!authenticatedIPs.Contains(currentIP))
                        {
                            userData.IpRestricted = true;
                        }
                    }
                }
            }
            else
            {
                userData.InActive = true;
            }



            userData.HasError = (userData.IpRestricted || userData.IsLocked || userData.InActive);
            return userData;
        }

        private void CheckLockedUser(ContactPassword contact, IGlobalContext globalContext)
        {
            if (contact.IsLocked)
            {
                if (contact.LockDateTime != null)
                {
                    TimeSpan timeElapsed = (DateTime.Now - contact.LockDateTime.Value);
                    if (timeElapsed.TotalMinutes > 30)
                    {
                        contact.IsLocked = false;
                        contact.LockDateTime = null;
                        contact.NumberOfRetries = 0;

                        globalContext.SaveChanges();
                    }
                }
                else
                {
                    contact.IsLocked = false;
                    contact.LockDateTime = null;
                    globalContext.SaveChanges();
                }


            }
        }




    }




   public  class OneTimePasswordResult{

        public string OneTimePasswordPage { get; set; }
        public string OneTimePasswordId { get; set; }
        public bool HasError { get; set; }
        public string ExceptionMessage { get; set; }
    }

}
