using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers; 
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security; 
using WebFreight.Web.WebServices;
using System.Text.RegularExpressions; 
using System.Web.UI; 

using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Counters;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using System.IO;
using Logitude.Server.Tools;
using Logitude.SystemLogs;
using System.ServiceModel;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.ServiceModel.Web;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Net;
using System.Text;
using System.Net.Http;
using WebFreight.Web.App_Code;
using System.Threading;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using WebFreight.Web.Helpers.DataProviderHelpers;

namespace WebFreight.Web
{

    public class AuthenticationController : ApiController
    {
        public AuthenticationController()
        {

        }
        private static readonly string SimplogGuid = Guid.NewGuid().ToString("N");

        public UserData PostLoginUsingAuthenticaionToken(LoginTokenParameter logintokenparam, string dummy)
        {
            UserData userdata = GetUserDataByToken(logintokenparam, dummy);
            userdata.Token = logintokenparam.Token;
            return userdata;
        }
         
        public UserData PostLoginUsingAuthenticaionToken(LoginTokenParameter logintokenparam, bool isAngular)
        {
            UserData userData = null;
            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
            AuthenticationToken auttoken = authenticationTokenRepository.GetSingleToken(logintokenparam.Token);
            if (auttoken != null)
            {
                LoginParameters loginParameters = new LoginParameters() { Email = auttoken.Email, GetToken = true, IsUser = true, Password = auttoken.Password + "@HashPassword" };
                if (!string.IsNullOrEmpty(logintokenparam.CardId))
                {
                    loginParameters.CardId = logintokenparam.CardId;
                    loginParameters.IsUser = false;
                }
                AuthenticationController authenticationController = new AuthenticationController();
                userData = authenticationController.PostLoginData(loginParameters, auttoken.Tenant);
            }
            return userData;
        }
         
          
        public UserData PostTrayLoginUsingAuthenticaionToken(LoginTokenParameter logintokenparam, bool fromTray, bool useTenant)
        {
            UserData userdata;
            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
            AuthenticationToken auttoken = authenticationTokenRepository.GetSingleToken(logintokenparam.Token);
            if (auttoken != null)
            {
                // userdata =  PostLoginToken(auttoken.Email, auttoken.Password, true);

                LoginParameters loginParameters = new LoginParameters()
                {
                    Email = auttoken.Email,
                    Password = auttoken.Password,
                    ByToken = true,
                    IsUser = true
                };

                userdata = PostUserValidation(loginParameters);

                if (!userdata.HasError)
                {
                    CompanyLogin company = null;
                    company = userdata.CompanyLogins.Where(c => c.Tenant == auttoken.Tenant).FirstOrDefault();

                    if (company != null)
                    {
                        userdata.SelectedCompanyLogin = company;
                        loginParameters.CardId = company.CardId;
                        loginParameters.CardType = company.CardType;

                        UserData data = PostLoginData(loginParameters, company.Tenant);
                        if (data.HasError)
                        {
                            userdata = data;
                        }

                        userdata.CurrentTenant = company.Tenant;


                        if (userdata.CompanyLogins.Count > 1)
                        {


                            CompanyLogin companyLogin = userdata.SelectedCompanyLogin;

                            if (companyLogin != null)
                            {
                                ICommonDataContext commonDataContext = CommonDataContext.GetContext(logintokenparam.Tenant);
                                string via = "Mobile";
                                if (!logintokenparam.IsMobileLogin)
                                {
                                    via = "PC";
                                }
                                string computerId = Guid.NewGuid().ToString("N");
                                string userAgent = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : null;

                                ContactLoginLog contactLog = new ContactLoginLog()
                                {
                                    Id = IdCounter.GetNumber("ContactLoginLog", companyLogin.Tenant).ToString(),
                                    Tenant = companyLogin.Tenant,
                                    Browser = HttpContext.Current.Request.Browser.Type,
                                    IP = AuthenticationUtil.GetIP4Address(),//HttpContext.Current.Request.UserHostAddress,
                                    ContactId = companyLogin.ContactId,
                                    GMTDateTime = DateTime.Now,
                                    LocalDateTime = TenantServerConfigration.GetCurrentDateTime(companyLogin.Tenant),
                                    ContactAgent = userAgent,
                                    ComputerId = computerId,
                                    Via = via,
                                };
                                commonDataContext.ContactLoginLogs.Add(contactLog);
                                commonDataContext.SaveChanges();

                            }
                        }

                    }
                    else
                    {
                        userdata.HasError = true;
                        userdata.InvalidMobileAccessPermission = true;
                    }

                }


            }

            else
            {
                userdata = new UserData()
                {
                    HasError = true,
                    InvalidToken = true,
                };
            }
            return userdata;
        }

        public UserData PostLoginUsingAuthenticaionTokenInAngular(string dummy, int tenant, LoginTokenParameter logintokenparam)
        {
            UserData userdata = GetUserDataByToken(logintokenparam, dummy);

            return userdata;
        }

        private UserData GetUserDataByToken(LoginTokenParameter logintokenparam, string dummy)
        {
            DateTime DateBeforePostLoginUsingAuthenticaionToken = DateTime.Now;


            UserData userdata;
            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
            AuthenticationToken auttoken = authenticationTokenRepository.GetSingleToken(logintokenparam.Token);
            if (auttoken != null)
            { 
                // userdata =  PostLoginToken(auttoken.Email, auttoken.Password, true);

                LoginParameters loginParameters = new LoginParameters()
                {
                    Email = auttoken.Email,
                    Password = auttoken.Password,
                    ByToken = true,
                    IsMobileLogin = logintokenparam.IsMobileLogin,
                    MobileVersion = logintokenparam.MobileVersion,
                };
                if (LogitudeSettings.IsCostomsDeploy)
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    var contactPasswordRepository = new ContactPasswordRepository(globalContext);

                    var dbcontact =
                        //globalContext.ContactPasswords.Where(c => c.Email == auttoken.Email).FirstOrDefault();
                        contactPasswordRepository.GetSingleContactPassword(auttoken.Email);
                    if (dbcontact != null)
                    {
                        dbcontact = dbcontact ?? new ContactPassword();
                        if (!String.IsNullOrWhiteSpace(dbcontact.Password))
                        {
                            if (dbcontact.Password != loginParameters.Password)
                            {
                                Debug.WriteLine(@"ihab(@Itzik):SSO:the Hash Password from token irrelevant Allow Login even though HashPass  not match");
                                loginParameters.Password = dbcontact.Password;
                            }
                        }
                        if (dbcontact.MustChangePassword)
                        {
                            dbcontact.MustChangePassword = false;
                            contactPasswordRepository.Update(dbcontact);
                            globalContext.SaveChanges();
                        }
                    }
                }

                userdata = PostUserValidation(loginParameters);

                if (!userdata.HasError)
                {
                    CompanyLogin company = null;


                    if (dummy == "user")
                    {
                        company = userdata.CompanyLogins.Where(c => c.IsUser == true && c.Tenant == logintokenparam.Tenant).FirstOrDefault();
                        loginParameters.IsUser = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(logintokenparam.CardId))
                        {
                            company = userdata.CompanyLogins.Where(c => c.IsUser == false && c.Tenant == auttoken.Tenant && c.CardId == logintokenparam.CardId).FirstOrDefault();
                        }
                        else
                        {
                            company = userdata.CompanyLogins.Where(c => c.IsUser == false && c.Tenant == auttoken.Tenant).FirstOrDefault();
                        }

                        if (!string.IsNullOrEmpty(logintokenparam.CardId) && company == null)
                        {
                            company = userdata.CompanyLogins.Where(c => c.IsUser == false && c.Tenant == auttoken.Tenant).FirstOrDefault();
                        }



                    }

                    if (company != null)
                    {
                        userdata.SelectedCompanyLogin = company;
                        loginParameters.CardId = company.CardId;
                        loginParameters.CardType = company.CardType;

                        UserData data = PostLoginData(loginParameters, company.Tenant);
                        if (data.HasError)
                        {
                            userdata = data;
                        }

                        userdata.HtmlVersion = userdata.HtmlVersion ?? data.HtmlVersion;

                        userdata.CurrentTenant = company.Tenant;


                        if (userdata.CompanyLogins.Count > 1)
                        {


                            CompanyLogin companyLogin = userdata.SelectedCompanyLogin;

                            if (companyLogin != null)
                            {
                                ICommonDataContext commonDataContext = CommonDataContext.GetContext(logintokenparam.Tenant);
                                string via = "Mobile";
                                if (!logintokenparam.IsMobileLogin)
                                {
                                    via = "PC";
                                }
                                string computerId = Guid.NewGuid().ToString("N");
                                string userAgent = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : null;

                                ContactLoginLog contactLog = new ContactLoginLog()
                                {
                                    Id = IdCounter.GetNumber("ContactLoginLog", companyLogin.Tenant).ToString(),
                                    Tenant = companyLogin.Tenant,
                                    Browser = HttpContext.Current.Request.Browser.Type,
                                    IP = AuthenticationUtil.GetIP4Address(),//HttpContext.Current.Request.UserHostAddress,
                                    ContactId = companyLogin.ContactId,
                                    GMTDateTime = DateTime.Now,
                                    LocalDateTime = TenantServerConfigration.GetCurrentDateTime(companyLogin.Tenant),
                                    ContactAgent = userAgent,
                                    ComputerId = computerId,
                                    Via = via,
                                };
                                commonDataContext.ContactLoginLogs.Add(contactLog);
                                commonDataContext.SaveChanges();

                            }
                        }

                        //Abed log
                    }
                    else
                    {
                        userdata.HasError = true;
                        userdata.InvalidMobileAccessPermission = true;
                    }

                }


            }

            else
            {
                userdata = new UserData()
                {
                    HasError = true,
                    InvalidToken = true,
                };
            }



            int executionTime = (int)((DateTime.Now.Ticks - DateBeforePostLoginUsingAuthenticaionToken.Ticks) / TimeSpan.TicksPerMillisecond);
            AddServerTimeToHeaderRespose(executionTime);
            return userdata;
        }

        private List<string> GetTenantLogoUri(int companyId, int mobileVersion)
        {
            try
            {

                List<string> ImageInfo = new List<string>();

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = "verysmalllogo" + companyId,
                    FolderName = "logos",
                    Extension = "png",
                    Tenant = companyId,

                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                byte[] datainByte = storageservice.Read(fileInfo);

                if (datainByte == null)
                {
                    fileInfo.Extension = "jpg";
                    datainByte = storageservice.Read(fileInfo);
                }

                if (datainByte == null)
                {
                    fileInfo.FileName = "smalllogo" + companyId;
                    datainByte = storageservice.Read(fileInfo);
                }

                if (datainByte != null)
                {
                    if (mobileVersion > 1)
                    {
                        ImageInfo.Add(GetUrlImage(datainByte, fileInfo.Extension));
                        ImageInfo.Add(fileInfo.Extension);
                    }
                    else
                    {
                        ImageInfo.Add(GetUrlImage(datainByte, "jpg"));
                        ImageInfo.Add("jpg");
                    }
                    return ImageInfo;
                }

                else return null;


            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, companyId, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method", null);
                return null;
            }


        }

        private static string GetUrlImage(byte[] datainByte, string extension)
        {
            string base64String = System.Convert.ToBase64String(datainByte, 0, datainByte.Length);


            string uri = "data:image/" + extension + ";base64," + base64String;

            return uri;
        }

        //private static string GetUrlImage(Logitude.Server.Tools.BlobServiceReference.Response response)
        //{
        //    using (MemoryStream memstream = new MemoryStream())
        //    {
        //        byte[] datainByte = response.Result as byte[];//memstream.ToArray();
        //        string base64String = System.Convert.ToBase64String(datainByte,
        //               0,
        //               datainByte.Length);
        //        string uri = "data:image/jpg;base64," + base64String;
        //        return uri;
        //    }
        //}

        //public UserData PostLoginToken(string email, string password, bool byToken)
        //{
        //    UserData data;
        //    List<CompanyLogin> loginsList = new List<CompanyLogin>();
        //    IGlobalContext globalObjectContext = GlobalContext.GetContext();
        //    ContactPassword contactPassword = null;
        //    data = CheckUserState(email, password, ref contactPassword, byToken);

        //    if (!data.HasError)
        //    {


        //        List<GlobalTenant> globalTenants = globalObjectContext.GlobalTenants.ToList();
        //        List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.GlobalTenant.IsActive == true && m.InActive == false && (m.IsUser == true || m.InternetAccess == true)).ToList();
        //        foreach (GlobalContact contact in contacts)
        //        {
        //            ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
        //            GlobalTenant globalTenant = globalTenants.Where(t => t.Id == contact.GlobalTenantId).FirstOrDefault();
        //            if (contact.IsUser)
        //            {
        //                CompanyLogin company = new CompanyLogin()
        //                {
        //                    Email = contact.Email,
        //                    CompanyName = globalTenant.CompanyName + " (" + contact.GlobalTenantId + ")",
        //                    IsUser = true,
        //                    Tenant = contact.GlobalTenantId,
        //                    CardId = null,
        //                    CardType = null,
        //                    ContactId = contact.Id,
        //                };

        //                loginsList.Add(company);
        //            }
        //            if (contact.InternetAccess)
        //            {
        //                List<CardContact> cardcontactsList = commonDataContext.CardContacts.Where(c => c.ContactId == contact.Id && c.InternetAccess == true && c.Tenant == contact.GlobalTenantId).ToList();
        //                foreach (CardContact cardContact in cardcontactsList)
        //                {
        //                    Card card = commonDataContext.Cards.Where(c => c.Id == cardContact.CardId && c.Tenant == cardContact.Tenant).FirstOrDefault(); ;
        //                    CompanyLogin companyAccess = new CompanyLogin()
        //                    {
        //                        Email = contact.Email,
        //                        CompanyName = globalTenant.CompanyName + "-" + card.EnglishName + " (" + contact.GlobalTenantId + ")",
        //                        Tenant = contact.GlobalTenantId,
        //                        CardId = card.Id,
        //                        CardType = card.PartnerTypeId,
        //                        IsUser = false,
        //                        ContactId = contact.Id,
        //                    };

        //                    loginsList.Add(companyAccess);


        //                }
        //            }

        //            loginsList = loginsList.OrderBy(c => c.CompanyName).ToList();
        //        }

        //        if (loginsList.Count == 1)
        //        {
        //            CompanyLogin companyAccess = loginsList.First();
        //            data = PostLoginData(new LoginParameters()
        //            {

        //                Email = email,
        //                Password = password,
        //                IsUser = companyAccess.IsUser,
        //                CardId = companyAccess.CardId,
        //                CardType = companyAccess.CardType
        //            }, companyAccess.Tenant);//PostLoginData(email, password, companyAccess.Tenant, companyAccess.IsUser, companyAccess.CardId, companyAccess.CardType);
        //            data.ContactsCount = 1;
        //            data.CompanyLogins = loginsList;
        //        }
        //        else
        //        {
        //            data = new UserData()
        //            {
        //                UserName = email,
        //                CompanyLogins = loginsList,
        //                HasError = (loginsList.Count() == 0 ? true : false),
        //                InValidMailOrPassword = (loginsList.Count() == 0 ? true : false),
        //                ContactsCount = loginsList.Count(),
        //            };
        //        }

        //        if (!data.HasError)
        //        {
        //            string token = Guid.NewGuid().ToString();
        //            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
        //            AuthenticationToken authentication = new AuthenticationToken() { CreateDate = DateTime.Now, Email = email, Password = contactPassword.Password, Token = token };
        //            authenticationTokenRepository.Add(authentication);
        //            authenticationTokenRepository.SubmitChanges();
        //            data.Token = token;
        //        }
        //    }

        //    return data;
        //}

        //public string PostSignUpContactData(string name, string email, string company)
        //{
        //    IGlobalContext globalContext = GlobalContext.GetContext();
        //    LogitudeLeadRepository leadRepository = new LogitudeLeadRepository(globalContext);
        //    email = email.ToLower();
        //    LogitudeLead lead = new LogitudeLead()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        ContactName = name,
        //        CompanyName = company,
        //        Email = email,
        //        CreateDate = DateTime.Now,
        //        Country = "Palestine",
        //        LastUpdateDate = DateTime.Now

        //    };

        //    globalContext.LogitudeLeads.Add(lead);

        //    globalContext.SaveChanges();

        //    return lead.Id;
        //}

        public bool GetLogOutData(string userEmail)
        {


            FormsAuthentication.SignOut();

            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            //if (HttpContext.Current.Session != null)
            //{
            //    HttpContext.Current.Session.Abandon();
            //}


            //// clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            //HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            //if (cookie2 != null)
            //{
            //    cookie2.Expires = DateTime.Now.AddYears(-1);
            //    HttpContext.Current.Response.Cookies.Add(cookie2);
            //}


            //FormsAuthentication.RedirectToLoginPage();




            return true;
        }


        public UserData PostUserValidation(LoginParameters loginParameters)
        {

            try
            {


                TenantManagmentPrivateLabelsPM privatelabel = null;
                var url = SecurityUtility.getLoggedDomain();
                if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                {
                    TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
                    privatelabel = query.GetSingleActivePMByUrl(url);
                }
                DateTime DateBeforePostUserValidation = DateTime.Now;
                string email = loginParameters.Email.Trim();
                string password = loginParameters.Password;
                string Techology = "";
                UserData data = new UserData();
                List<CompanyLogin> loginsList = new List<CompanyLogin>();
                IGlobalContext globalObjectContext = GlobalContext.GetContext();
                ContactPassword contactPassword = null;
          
               data = CheckCaptchaState(loginParameters);

                if (!data.HasError)
                {
                    data = CheckUserState(email.ToLower(), password, ref contactPassword, loginParameters.ByToken, loginParameters.ClientType);
                }

                if (!data.HasError)
                {

                    bool customerCare = false;
                    bool distributor = false;
                    User logitudeUser = null;

                    GlobalContact zeroContact = globalObjectContext.GlobalContacts.Where(d => d.GlobalTenantId == 0 && d.Email == email && d.InActive == false).FirstOrDefault(); //mohammad

                    if (zeroContact != null)
                    {
                        ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
                        logitudeUser = (from a in commonDataContext.Users
                                        where a.Id == zeroContact.Id
                                        select a).FirstOrDefault();



                        if (logitudeUser != null)
                        {
                            Techology = logitudeUser.Technology;
                            if (logitudeUser.Tenant == 0)
                            {
                                distributor = logitudeUser.IsDistributor;
                                customerCare = !logitudeUser.IsDistributor;
                            }
                        }

                    }
                    if (customerCare || distributor)
                    {
                        if (distributor)
                        {
                            List<int> distributorTenants = (from a in globalObjectContext.TenantManagements
                                                            where (a.DistributorCode == logitudeUser.DistributorCode && a.Id != 0 && a.IsDistributorSupportEnabled)
                                                            select a.Id).ToList();

                            List<GlobalTenant> globalTenants = (from t in globalObjectContext.GlobalTenants
                                                                where distributorTenants.Contains(t.Id) && t.IsActive //&& t.Version != -1 Islam: customer care didn't see this tenant
                                                                select t).ToList();

                            foreach (GlobalTenant globalTenant in globalTenants)
                            {
                                CompanyLogin company = new CompanyLogin()
                                {
                                    Email = zeroContact.Email,
                                    CompanyName = globalTenant.CompanyName + " (" + globalTenant.Id + ")",
                                    IsUser = true,
                                    Tenant = globalTenant.Id,
                                    CardId = null,
                                    CardType = null,
                                    ContactId = zeroContact.Id,
                                    LicensedUser = true,
                                    PrivateLabelId = globalTenant.PrivateLabelId
                                };

                                loginsList.Add(company);
                            }
                        }
                        else if (customerCare)
                        {
                            List<int> custCareTenants = (from a in globalObjectContext.TenantManagements
                                                         where a.IsSystemSupportEnabled || a.Id == 0
                                                         select a.Id).ToList();

                            List<GlobalTenant> globalTenants = (from t in globalObjectContext.GlobalTenants
                                                                where custCareTenants.Contains(t.Id) && t.IsActive //&& t.Version != -1 Islam: customer care didn't see this tenant
                                                                select t).ToList();

                            foreach (GlobalTenant globalTenant in globalTenants)
                            {
                                CompanyLogin company = new CompanyLogin()
                                {
                                    Email = zeroContact.Email,
                                    CompanyName = globalTenant.CompanyName + " (" + globalTenant.Id + ")",
                                    IsUser = true,
                                    Tenant = globalTenant.Id,
                                    CardId = null,
                                    CardType = null,
                                    ContactId = zeroContact.Id,
                                    LicensedUser = true,
                                    PrivateLabelId = globalTenant.PrivateLabelId
                                };

                                loginsList.Add(company);
                            }
                        }

                        List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.GlobalTenant.IsActive == true && m.InActive == false && (m.InternetAccess == true)).ToList();

                        // Techology = GetUserTechology(contacts);

                        foreach (GlobalContact contact in contacts)
                        {
                            ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
                            GlobalTenant globalTenant = (from t in globalObjectContext.GlobalTenants
                                                         where t.Id == contact.GlobalTenantId
                                                         select t).Include("TenantManagement").FirstOrDefault();
                            if (contact.InternetAccess)
                            {
                                //List<CardContact> cardcontactsList = commonDataContext.CardContacts.Where(c => c.ContactId == contact.Id && c.InternetAccess == true && c.Tenant == contact.GlobalTenantId).ToList();
                                //foreach (CardContact cardContact in cardcontactsList)
                                //{
                                //    Card card = commonDataContext.Cards.Where(c => c.Id == cardContact.CardId && c.Tenant == cardContact.Tenant).FirstOrDefault(); ;
                                //    CompanyLogin companyAccess = new CompanyLogin()
                                //    {
                                //        Email = contact.Email,
                                //        CompanyName = globalTenant.CompanyName + "-" + card.EnglishName + " (" + contact.GlobalTenantId + ")",
                                //        Tenant = contact.GlobalTenantId,
                                //        CardId = card.Id,
                                //        CardType = card.PartnerTypeId,
                                //        IsUser = false,
                                //        ContactId = contact.Id,
                                //    };

                                //    loginsList.Add(companyAccess);


                                //}

                                TenantRepository tenantRep = new TenantRepository(0);
                                Tenant currentTenant = tenantRep.GetSingleTenant(contact.GlobalTenantId);
                                bool enableLogin = false;
                                if (loginParameters.IsMobileLogin)
                                {
                                    enableLogin = currentTenant.IsMobileActivated;
                                }
                                else
                                {
                                    enableLogin = currentTenant.IsWebAccessActivated;
                                }
                                if (enableLogin)
                                {
                                    List<CardContact> cardcontactsList = commonDataContext.CardContacts.Where(c => c.ContactId == contact.Id && c.InternetAccess == true && c.Tenant == contact.GlobalTenantId).ToList();
                                    foreach (CardContact cardContact in cardcontactsList)
                                    {
                                        Card card = commonDataContext.Cards.Where(c => c.Id == cardContact.CardId && c.Tenant == cardContact.Tenant).FirstOrDefault();
                                        CompanyLogin companyAccess = new CompanyLogin()
                                        {
                                            Email = contact.Email,
                                            CompanyName = globalTenant.CompanyName + "-" + card.EnglishName + " (" + contact.GlobalTenantId + ")",
                                            Tenant = contact.GlobalTenantId,
                                            CardId = card.Id,
                                            CardType = card.PartnerTypeId,
                                            IsUser = false,
                                            ContactId = contact.Id,
                                            InternetAccess = contact.InternetAccess,
                                            CustomerName = card.EnglishName,
                                            PrivateLabelId = globalTenant.PrivateLabelId
                                        };

                                        loginsList.Add(companyAccess);


                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.GlobalTenant.IsActive == true && m.InActive == false && (m.IsUser == true || m.InternetAccess == true)).ToList();


                        //Techology = GetUserTechology(contacts);

                        foreach (GlobalContact contact in contacts)
                        {
                            ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
                            GlobalTenant globalTenant = (from t in globalObjectContext.GlobalTenants
                                                         where t.Id == contact.GlobalTenantId
                                                         select t).Include("TenantManagement").FirstOrDefault();

                            if (contact.IsUser)
                            {
                                bool Licensed = true;
                                if (globalTenant.TenantManagement.ManageLicencesPerUser)
                                {
                                    User user = (from a in commonDataContext.Users
                                                 where a.Id == contact.Id
                                                 select a).FirstOrDefault();

                                    Licensed = user.LicencedUser;

                                }

                                CompanyLogin company = new CompanyLogin()
                                {
                                    Email = contact.Email,
                                    CompanyName = globalTenant.CompanyName + " (" + contact.GlobalTenantId + ")",
                                    IsUser = true,
                                    Tenant = contact.GlobalTenantId,
                                    CardId = null,
                                    CardType = null,
                                    ContactId = contact.Id,
                                    LicensedUser = Licensed,
                                    InternetAccess = contact.InternetAccess,
                                    PrivateLabelId = globalTenant.PrivateLabelId
                                };

                                loginsList.Add(company);
                            }
                            if (contact.InternetAccess)
                            {
                                TenantRepository tenantRep = new TenantRepository(0);
                                Tenant currentTenant = tenantRep.GetSingleTenant(contact.GlobalTenantId);
                                bool enableLogin = false;
                                if (loginParameters.IsMobileLogin)
                                {
                                    enableLogin = currentTenant.IsMobileActivated;
                                }
                                else
                                {
                                    enableLogin = currentTenant.IsWebAccessActivated;
                                }
                                if (enableLogin)
                                {
                                    List<CardContact> cardcontactsList = commonDataContext.CardContacts.Where(c => c.ContactId == contact.Id && c.InternetAccess == true && c.Tenant == contact.GlobalTenantId).ToList();
                                    foreach (CardContact cardContact in cardcontactsList)
                                    {
                                        Card card = commonDataContext.Cards.Where(c => c.Id == cardContact.CardId && c.Tenant == cardContact.Tenant).FirstOrDefault();
                                        CompanyLogin companyAccess = new CompanyLogin()
                                        {
                                            Email = contact.Email,
                                            CompanyName = globalTenant.CompanyName + "-" + card.EnglishName + " (" + contact.GlobalTenantId + ")",
                                            Tenant = contact.GlobalTenantId,
                                            CardId = card.Id,
                                            CardType = card.PartnerTypeId,
                                            IsUser = false,
                                            ContactId = contact.Id,
                                            InternetAccess = contact.InternetAccess,
                                            CustomerName = card.EnglishName,
                                            PrivateLabelId = globalTenant.PrivateLabelId
                                        };

                                        loginsList.Add(companyAccess);


                                    }
                                }
                            }


                        }
                    }

                    List<CompanyLogin> unliscened = loginsList.Where(s => s.LicensedUser == false && s.IsUser == true).ToList();

                    loginsList = loginsList.Where(s => s.LicensedUser == true || s.IsUser == false).OrderBy(c => c.CompanyName).ToList();

                    if (loginsList.Count == 1)
                    {
                        CompanyLogin companyAccess = loginsList.First();
                        if (privatelabel != null && companyAccess.PrivateLabelId == privatelabel.Id)
                        {
                            data = PostLoginData(new LoginParameters()
                            {

                                Email = email,
                                Password = password,
                                IsUser = companyAccess.IsUser,
                                CardId = companyAccess.CardId,
                                CardType = companyAccess.CardType,
                                ByToken = loginParameters.ByToken,
                                IsMobileLogin = loginParameters.IsMobileLogin,
                                GetToken = loginParameters.GetToken,
                                IsAngularLogin = loginParameters.IsAngularLogin,
                                InternalLoginValidationCall = true,
                                ClientType = loginParameters.ClientType,
                            }, companyAccess.Tenant);
                        }
                        else
                        {
                            if (privatelabel == null && !string.IsNullOrEmpty(companyAccess.PrivateLabelId))
                            {
                                data = new UserData()
                                {
                                    UserName = email,
                                    CompanyLogins = new List<CompanyLogin>(),
                                    HasError = true,
                                    // InValidMailOrPassword = (temp.Count() == 0 ? true : false),
                                    ContactsCount = 0,
                                    InActive = true,
                                    Unlicensed = (unliscened.Count() > 0 ? true : false),
                                    PrivateLablehasZeroTenant = true
                                };
                            }
                            else if (privatelabel != null && string.IsNullOrEmpty(companyAccess.PrivateLabelId))
                            {
                                data = new UserData()
                                {
                                    UserName = email,
                                    CompanyLogins = new List<CompanyLogin>(),
                                    HasError = true,
                                    // InValidMailOrPassword = (temp.Count() == 0 ? true : false),
                                    ContactsCount = 0,
                                    InActive = true,
                                    Unlicensed = (unliscened.Count() > 0 ? true : false),
                                    PrivateLablehasZeroTenant = true
                                };
                            }
                            else
                            { 
                                data = PostLoginData(new LoginParameters()
                                {

                                    Email = email,
                                    Password = password,
                                    IsUser = companyAccess.IsUser,
                                    CardId = companyAccess.CardId,
                                    CardType = companyAccess.CardType,
                                    ByToken = loginParameters.ByToken,
                                    IsMobileLogin = loginParameters.IsMobileLogin,
                                    GetToken = loginParameters.GetToken,
                                    IsAngularLogin = loginParameters.IsAngularLogin,
                                    InternalLoginValidationCall = true,
                                    ClientType = loginParameters.ClientType,
                                }, companyAccess.Tenant);
                            }
                        }



                        data.CompanyLogins = loginsList;
                        //email, password, companyAccess.Tenant, companyAccess.IsUser, companyAccess.CardId, companyAccess.CardType);
                        data.ContactsCount = 1;
                    }
                    else
                    {
                        var temp = new List<CompanyLogin>();
                        if (privatelabel != null)
                        {
                            temp = loginsList.Where(a => a.PrivateLabelId == privatelabel.Id).ToList();
                        }
                        else
                        {
                            temp = loginsList.Where(a => a.PrivateLabelId == null).ToList();
                        }
                        if (temp.Count == 1)
                        {
                            CompanyLogin companyAccess = temp.First();
                            data = PostLoginData(new LoginParameters()
                            {

                                Email = email,
                                Password = password,
                                IsUser = companyAccess.IsUser,
                                CardId = companyAccess.CardId,
                                CardType = companyAccess.CardType,
                                ByToken = loginParameters.ByToken,
                                IsMobileLogin = loginParameters.IsMobileLogin,
                                GetToken = loginParameters.GetToken,
                                IsAngularLogin = loginParameters.IsAngularLogin,
                                InternalLoginValidationCall = true,
                                ClientType = loginParameters.ClientType,
                            }, companyAccess.Tenant);

                            data.CompanyLogins = temp;
                            //email, password, companyAccess.Tenant, companyAccess.IsUser, companyAccess.CardId, companyAccess.CardType);
                            data.ContactsCount = 1;
                        }
                        else
                        {
                            data = new UserData()
                            {
                                UserName = email,
                                CompanyLogins = temp,
                                HasError = (temp.Count() == 0 ? true : false),
                                // InValidMailOrPassword = (temp.Count() == 0 ? true : false),
                                ContactsCount = temp.Count(),
                                InActive = (privatelabel != null ? false : (temp.Count() == 0 ? true : false)),
                                Unlicensed = (unliscened.Count() > 0 ? true : false),
                                PrivateLablehasZeroTenant = (privatelabel != null ? (temp.Count() == 0 ? true : false) : false)
                            };
                        }

                    }
                }

                if (loginParameters.IsMobileLogin && !data.HasError)
                {
                    data.CompanyLogins = data.CompanyLogins.Where(c => c.InternetAccess = true).ToList();

                    TenantRepository tenantRepository = new TenantRepository(0);
                    List<int> tenants = (from a in data.CompanyLogins
                                         select a.Tenant).ToList();

                    List<Tenant> mobileTenants = (from t in tenantRepository.context.Tenants
                                                  where tenants.Contains(t.Id)
                                                  select t).ToList();

                    List<CompanyLogin> mobileActivatedTenants = new List<CompanyLogin>();
                    foreach (CompanyLogin login in data.CompanyLogins)
                    {
                        Tenant tenant = mobileTenants.FirstOrDefault(t => t.Id == login.Tenant);
                        if (tenant != null)
                        {
                            if (tenant.IsMobileActivated)
                            {

                                List<string> LogoInfo = GetTenantLogoUri(login.Tenant, loginParameters.MobileVersion);

                                if (LogoInfo != null && LogoInfo.Count > 0) login.URL = LogoInfo[0];
                                if (LogoInfo != null && LogoInfo.Count > 1) login.Extension = LogoInfo[1];


                                mobileActivatedTenants.Add(login);

                            }
                        }


                    }

                    data.CompanyLogins = mobileActivatedTenants;
                    data.InActive = (mobileActivatedTenants.Count() == 0 ? true : false);
                    data.Unlicensed = (mobileActivatedTenants.Count() == 0 ? true : false);
                    data.HasError = (mobileActivatedTenants.Count() == 0 ? true : false);
                    data.ContactsCount = mobileActivatedTenants.Count();
                    data.InvalidMobileAccessPermission = (mobileActivatedTenants.Count() == 0 ? true : false);


                }

                int executionTime = (int)((DateTime.Now.Ticks - DateBeforePostUserValidation.Ticks) / TimeSpan.TicksPerMillisecond);
                AddServerTimeToHeaderRespose(executionTime);

                #region PasswordExpirationDate

                if (data != null && !data.HasError && loginParameters.ClientType == "Web")
                {
                    if (contactPassword == null)
                    {
                        contactPassword = globalObjectContext.ContactPasswords.Where(c => c.Email.ToLower() == email).FirstOrDefault();
                    }

                    if (contactPassword != null && contactPassword.PasswordExpirationDate != null)
                    {
                        DateTime nowDate = DateTime.Now;
                        DateTime expirationDate = (DateTime)contactPassword.PasswordExpirationDate;

                        if (nowDate.Date > expirationDate.Date)
                        {
                            data.HasError = true;
                            data.MustChangePassword = true;
                        }
                        else
                        {
                            var days = (expirationDate - nowDate).TotalDays;
                            if (days.ToString().Contains("."))
                            {
                                days = Int32.Parse(days.ToString().Split('.')[0]) + 1;
                            }
                            if (days <= 10)
                            {
                                data.HasError = true;
                                data.PasswordExpirationDateMessage = "Your password will expire in " + days + " days. Do you want to change it now?";
                            }
                        }

                    }
                }
                #endregion


                if (data.HasError)
                {
                    if (data.InValidMailOrPassword || data.IsLocked || data.IpRestricted)
                    {
                        AddFailedLoginLog(data , loginParameters);
                       // int sleepTime = data.NumberOfRetries > 0 ? data.NumberOfRetries : 1;

                        if (!data.IpRestricted && loginParameters.ClientType == "Web")
                        {
                            bool isLoadContactPasswords = false;
                            if (contactPassword == null)
                            {
                                contactPassword = globalObjectContext.ContactPasswords.Where(c => c.Email.ToLower() == email).FirstOrDefault();
                                isLoadContactPasswords = true;
                            } 

                            if (contactPassword != null)
                            {
                                if (contactPassword.NumberOfRetries++ >= 5)
                                {
                                    CaptchaHelper captchaHelper = new CaptchaHelper();
                                    captchaHelper.AddCaptchaKey(loginParameters.Email, data, "Login");
                                    if (!isLoadContactPasswords) contactPassword = globalObjectContext.ContactPasswords.Where(c => c.Email.ToLower() == loginParameters.Email).FirstOrDefault();
                                    if (contactPassword != null)
                                    {
                                        contactPassword.CaptchaKey = data.CaptchaKey;
                                        globalObjectContext.SaveChanges();
                                    }
                                }
                            }
                        }

                      //  Thread.Sleep(sleepTime);
                    }

          

                }
            
                return data;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, loginParameters.Email, "", "AuthenticationController : PostUserValidation", null);
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
        
        private UserData CheckCaptchaState(LoginParameters loginParameters , bool withoutCheckUsed = false)
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            UserData data = new UserData();
            if (!loginParameters.IsMobileLogin && loginParameters.ClientType == "Web")
            {
                bool isCheckCaptchaCode = !string.IsNullOrEmpty(loginParameters.CaptchaCode) && !string.IsNullOrEmpty(loginParameters.CaptchaKey) ? true : false;
                ContactPassword contactPassword = null;
                IGlobalContext globalContext = null;
                if (!isCheckCaptchaCode)
                {
                     globalContext = GlobalContext.GetContext();
                     contactPassword = globalContext.ContactPasswords.Where(c => c.Email.ToLower() == loginParameters.Email).FirstOrDefault();

                    if (contactPassword!=null)
                    {
                        if (contactPassword.NumberOfRetries++ >= 5)
                        {
                            DateTime dateNowBefor5Minutes = DateTime.Now.AddMinutes(-5);
                            if (contactPassword.LockDateTime > dateNowBefor5Minutes) isCheckCaptchaCode = true;
                            if (!isCheckCaptchaCode)
                            {
                                int countCaptchaKey = globalContext.CaptchaKeys.Where(a => a.Email == loginParameters.Email && a.Activity == "Login" && a.CreateDate >= dateNowBefor5Minutes).Count();
                                if (countCaptchaKey >= 5) isCheckCaptchaCode = true;
                            }
                        }
                    }
                }

                else
                {
                    globalContext = GlobalContext.GetContext();
                    contactPassword = globalContext.ContactPasswords.Where(c => c.Email.ToLower() == loginParameters.Email).FirstOrDefault();
                }

                string userCaptchaKey = contactPassword != null ? contactPassword.CaptchaKey : null;

                if (isCheckCaptchaCode && !captchaHelper.CheckCaptchaCodeValidated(loginParameters.CaptchaCode, loginParameters.CaptchaKey, userCaptchaKey, withoutCheckUsed))
                {
                    captchaHelper.AddCaptchaKey(loginParameters.Email, data, "Login");
                    if (contactPassword != null)
                    {
                        contactPassword.CaptchaKey = data.CaptchaKey;
                        globalContext.SaveChanges();
                    }

                }

            }
            return data;
        }

    

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";//"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@!#$%^&*";//"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void AddFailedLoginLog(UserData data, LoginParameters loginParameters)
        {

            FailedLoginLogRepository failedLoginLogRepository = new FailedLoginLogRepository();
            FailedLoginLog failedLoginLog = new FailedLoginLog()
            {
                Id = IdCounter.GetNumber("FailedLoginLog", data.Tenant).ToString(),
                Browser = HttpContext.Current.Request.Browser.Type,
                IP = AuthenticationUtil.GetIP4Address(),
                GMTDateTime = DateTime.Now,
                UserAgent = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : null,
                Email = data.UserName,
            };

            if (data.Param1) failedLoginLog.Reason = "Wrong Email address";
            else if (data.InValidMailOrPassword) failedLoginLog.Reason = "Wrong Password";
            else if (data.IpRestricted) failedLoginLog.Reason = "Unauthorized IP address";
            else if (data.InValidCaptcha) failedLoginLog.Reason = "Valid Captcha";
            else if (data.IsLocked) failedLoginLog.Reason = "Locked User";

            data.Param1 = false;
            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (!string.IsNullOrEmpty(currentIP)) failedLoginLog.Browser = failedLoginLog.Browser.ToUpper();

            failedLoginLogRepository.Add(failedLoginLog);
            failedLoginLogRepository.SubmitChanges();

        }


        private void SetSessionPolicy(UserData data)
        {
            SessionPolicyRepository sessionPolicyRepository = new SessionPolicyRepository();
            SessionPolicy sessionPolicy = sessionPolicyRepository.GetSingleSessionPolicy();
            if (sessionPolicy != null)
            {
                data.WebTokenLifeTimeInMinutes = sessionPolicy.WebTokenLifeTimeInMinutes;
                data.WebTokenExpirationWarningInMinutes = sessionPolicy.WebTokenExpirationWarningInMinutes;

            }
        }

        bool OneTimePassword = false;
        public UserData PostLoginData(LoginParameters parameters, int tenant)
        {


            try
            {
                DateTime DateBeforePostLoginData = DateTime.Now;
                if (!string.IsNullOrEmpty(parameters.Email)) parameters.Email = parameters.Email.ToLower();

                string email = parameters.Email;
                string password = parameters.Password;
                bool isUser = parameters.IsUser;
                string cardId = parameters.CardId;
                string cardType = parameters.CardType;

                string userData = string.Empty;

                string computerId = Guid.NewGuid().ToString("N");
                IGlobalContext globalContext = GlobalContext.GetContext();
                ContactPassword contactPassword = null;

                var passResult = ResolvePassword(password);
                if (passResult != null)
                {
                    this.OneTimePassword = passResult.IsOneTimePassword;
                }

                UserData user = CheckCaptchaState(parameters , true);

                if (!user.InValidCaptcha)
                {
                    user = null;
                    bool customerCare = false;
                    bool distributor = false;
                    User logitudeUser = null;
                    GlobalContact contact = globalContext.GlobalContacts.Where(d => d.GlobalTenantId == 0 && d.Email.ToLower() == parameters.Email.ToLower()).FirstOrDefault(); //mohammad
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
                    if (contact != null)
                    {

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
                    else
                    {
                        contact = globalContext.GlobalContacts.Where(c => c.Email == email && c.InActive == false && c.GlobalTenantId == tenant).FirstOrDefault();

                    }
                    if (contact != null)
                    {


                        logitudeUser = (from a in commonDataContext.Users
                                        where a.Id == contact.Id
                                        select a).FirstOrDefault();



                        int currentTenant = tenant;//contact.GlobalTenantId;
                        string customData = currentTenant + "," + computerId;

                        string via = "";
                        if (parameters.IsMobileLogin)
                        {
                            via = "Mobile";
                        }
                        else
                        {
                            via = "PC";
                        }



                        user = ValidateUser(email, password, customData, out userData, isUser, cardId, cardType, parameters.ByToken, via, parameters.IsAngularLogin, parameters.ClientType);



                        if (user != null)
                        {

                            if (user.IsUser == false)
                            {
                                string logindata = user.UserName + ":" + user.Id + ":" + user.CurrentTenant + ":" + computerId + ":" + user.IsAuthenticated;

                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(/* version */ 1, email,
                                                                             DateTime.Now, DateTime.Now.AddMinutes(20160),
                                                                             false,
                                                                            logindata,
                                                                             FormsAuthentication.FormsCookiePath);

                                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                                HttpContext.Current.Response.Cookies.Add(authCookie);

                            }

                        }
                        else
                        {
                            contactPassword = null;
                            user = CheckUserState(email, password, ref contactPassword, parameters.ByToken, parameters.ClientType);
                        }
                    }
                    else
                    {
                        user = new UserData() { HasError = true, InValidMailOrPassword = true };
                    }

                    TenantManagement tenantManagement = globalContext.TenantManagements.Where(d => d.GlobalTenant.Id == user.CurrentTenant).FirstOrDefault();
                    if (tenantManagement != null && tenantManagement.EnableBranding) user.IsBrandingEnabled = tenantManagement.HideSharedlogistics;

                    user.Technology = "AG";//GetUserTechology(logitudeUser, tenantManagement, user.CurrentTenant);

                    #region KeepUserLoggedIn
                    TenantLoginPolicyRepository securityPolicyRepository = new TenantLoginPolicyRepository(tenant);
                    TenantLoginPolicy securityPolicy = securityPolicyRepository.GetSingleTenantLoginPolicy(tenant);

                    if (securityPolicy != null)
                    {
                        user.KeepUserLoggedIn = securityPolicy.KeepUserLoggedIn;

                        if (parameters.ClientType == "Web")
                        {
                            if (!securityPolicy.KeepUserLoggedIn)
                            {
                                user.SessionTimeout = securityPolicy.SessionTimeout;
                                SetSessionPolicy(user);
                            }
                        }
                    }
                    else
                    {
                        if (parameters.ClientType == "Web") SetSessionPolicy(user);
                    }
                    #endregion

                    if (!user.HasError && (parameters.IsMobileLogin || parameters.GetToken))
                    {
                        bool IsTwoFactorAuthenticationRequired = false;
                        if (!parameters.IsAngularLogin && !parameters.IsMobileLogin && parameters.IsUser)
                        {
                            IsTwoFactorAuthenticationRequired = CheckLoginSecurityPolicy(tenant, user, logitudeUser, commonDataContext);
                        }

                        if (!IsTwoFactorAuthenticationRequired)
                        {
                            if (!parameters.ByToken)
                            {
                                string hashedPassword = "";
                                contactPassword = AuthenticationUtil.VerifyContactPassword(parameters.Email, parameters.Password, globalContext);
                                if (contactPassword != null)
                                {
                                    hashedPassword = contactPassword.Password;
                                }

                                if (passResult != null)
                                {
                                    hashedPassword = passResult.Password;

                                }

                                // if (!parameters.InternalLoginValidationCall || parameters.IsMobileLogin)
                                //{
                                string token = AuthenticationUtil.GenerateToken();// Guid.NewGuid().ToString();
                                AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
                                AuthenticationToken authentication = new AuthenticationToken() { CreateDate = DateTime.Now, Email = email, Password = hashedPassword, Token = token, Tenant = user.CurrentTenant, ClientType = parameters.IsMobileLogin ? "Mobile" : parameters.ClientType };
                                if (!user.KeepUserLoggedIn && authentication.ClientType == "Web" && user.WebTokenLifeTimeInMinutes != 0) authentication.ExpirationDate = DateTime.Now.AddMinutes(user.WebTokenLifeTimeInMinutes);

                                AuthenticationToken authenticationDocument = new AuthenticationToken()
                                {
                                    CreateDate = DateTime.Now,
                                    ExpirationDate = DateTime.Now.AddMinutes(15),
                                    Email = email,
                                    Password = hashedPassword,
                                    Token = AuthenticationUtil.GenerateToken(),
                                    Tenant = user.CurrentTenant,
                                    ClientType = "DocumentDownload"
                                };
                                authenticationTokenRepository.Add(authenticationDocument);
                                authenticationTokenRepository.Add(authentication);

                                authenticationTokenRepository.SubmitChanges();
                                user.Token = token;
                                user.DocumentDownloadToken = authenticationDocument.Token;
                                //}
                            }
                        }
                        else
                        {
                            user.IsTwoFactorAuthenticationRequired = true;
                        }
                    }

                    if (logitudeUser != null)
                    {
                        user.UserId = logitudeUser.Id;
                        user.Tenant = logitudeUser.Tenant;
                    }

                    user.HtmlVersion = GetHtmlVersion();
                }

                int executionTime = (int)((DateTime.Now.Ticks - DateBeforePostLoginData.Ticks) / TimeSpan.TicksPerMillisecond);
                AddServerTimeToHeaderRespose(executionTime);

                //if (user.HasError)
                //{
                //    if (user.InValidMailOrPassword ||  ((user.IsLocked && parameters.ClientType!="Web") || (user.InValidCaptcha && parameters.ClientType == "Web")) || user.IpRestricted)
                //    {
                //        int sleepTime = user.NumberOfRetries > 0 ? user.NumberOfRetries : 1;
                //        Thread.Sleep(sleepTime);
                //    }
                //}

                return user;
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, parameters.Email, "", "AuthenticationController : PostLoginData", null);
                UserData user = new UserData();
                user.HasError = true;
                user.ExceptionMessage = e.Message;
                return user;
            }
        }

        private bool CheckLoginSecurityPolicy(int tenant, UserData user, User logitudeUser, ICommonDataContext commonDataContext)
        {
            string ipAddress = AuthenticationUtil.GetIP4Address();
            string deviceDescription = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : "";
            //System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
            //if (host != null)
            //    deviceDescription = host.HostName;
            //string deviceDescription = Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).HostName;
            string TwoFactorkey = HttpContext.Current.Request.Headers["TwoFactorkey"];


            bool IsTwoFactorAuthenticationRequired = false;
            TenantLoginPolicyRepository securityPolicyRepository = new TenantLoginPolicyRepository(commonDataContext);
            TwoFactorAuthenticationDeviceRepository deviceRepository = new TwoFactorAuthenticationDeviceRepository(commonDataContext);
            TenantLoginPolicy securityPolicy = securityPolicyRepository.GetSingleTenantLoginPolicy(tenant);
            if (securityPolicy != null && securityPolicy.LoginPolicyCode != "NOREST")
            {
                if (securityPolicy.LoginPolicyCode == "TFAUTH")
                {
                    if (!user.IsUser)
                        return false;

                    if (securityPolicy.IsEnabledForSpecificUsers)
                    {
                        if (!logitudeUser.IsTwoFactorAuthenticationEnabled)
                        {
                            return false;
                        }
                    }

                    if (securityPolicy.ExcludeInternalIPs)
                    {
                        if (!string.IsNullOrEmpty(securityPolicy.TwoFactorInternalIPs))
                        {
                            if (securityPolicy.TwoFactorInternalIPs.Contains(ipAddress))
                            {
                                return false;
                            }
                        }
                    }

                    Contact loggedContact = (from a in commonDataContext.Contacts
                                             where a.Id == logitudeUser.Id
                                             select a).FirstOrDefault();



                    TwoFactorAuthenticationDevice device = null;
                    if (!string.IsNullOrEmpty(TwoFactorkey))
                    {
                        List<TwoFactorAuthenticationDevice> devices = deviceRepository.GetAllTwoFactorAuthenticationDevicesByUser(loggedContact.Id, tenant).ToList();

                        device = devices.FirstOrDefault(d => TwoFactorkey.Contains(d.TwoFactorkey));
                    }

                    if (device == null || (device != null && device.InActive))
                    {
                        Random generator = new Random();
                        string authCode = generator.Next(0, 1000000).ToString("D6");
                        string key = StringHelper.GetRandomString(40);
                        device = new TwoFactorAuthenticationDevice()
                        {
                            Id = IdCounter.GetNumber("TwoFactorAuthenticationDevice", tenant),
                            TwoFactorkey = key,
                            Tenant = tenant,
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                            UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                            UserId = logitudeUser.Id,
                            CodeExpirationDate = TenantServerConfigration.GetCurrentDateTime(tenant).AddMinutes(10),
                            AuthenticationCode = authCode,
                            //LastLoginIP = ipAddress,
                            DeviceDescription = deviceDescription,
                        };

                        IsTwoFactorAuthenticationRequired = true;
                        user.IsTwoFactorAuthenticationRequired = true;
                        user.CodeExpirationDate = device.CodeExpirationDate;
                        user.TwoFactorkey = device.TwoFactorkey;
                        user.UserMobileNumber = GetContactMaskedMobileNumber(loggedContact);

                        deviceRepository.Add(device);
                        deviceRepository.SubmitChanges();

                        AddVerificationCodeSMSLog(loggedContact, device, commonDataContext);
                    }
                    else
                    {
                        if (device.IsVerified)
                        {
                            //Browser = HttpContext.Current.Request.Browser.Type,
                            //IP = AuthenticationUtil.GetIP4Address()
                            device.DeviceDescription = deviceDescription;
                            device.LastLoginIP = ipAddress;
                            device.LastLoginDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            deviceRepository.Update(device);
                            deviceRepository.SubmitChanges();
                        }
                        else
                        {
                            if (device.CodeExpirationDate < TenantServerConfigration.GetCurrentDateTime(tenant))
                            {
                                Random generator = new Random();
                                string authCode = generator.Next(0, 1000000).ToString("D6");
                                device.AuthenticationCode = authCode;
                                device.CodeExpirationDate = TenantServerConfigration.GetCurrentDateTime(tenant).AddMinutes(10);
                                device.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                deviceRepository.Update(device);
                                deviceRepository.SubmitChanges();

                                AddVerificationCodeSMSLog(loggedContact, device, commonDataContext);

                            }

                            IsTwoFactorAuthenticationRequired = true;
                            user.UserMobileNumber = GetContactMaskedMobileNumber(loggedContact);
                            user.IsTwoFactorAuthenticationRequired = true;
                            user.CodeExpirationDate = TenantServerConfigration.GetCurrentDateTime(tenant).AddMinutes(10);
                            user.TwoFactorkey = device.TwoFactorkey;
                        }
                    }

                    //securityPolicy.IsEnabledForSpecificUsers
                    //ENFEXIPO
                    //ENABLED
                }
                else if (securityPolicy.LoginPolicyCode == "COMPIP")
                {
                    if (!string.IsNullOrEmpty(securityPolicy.AllowedIPs))
                    {
                        string ipstring = securityPolicy.AllowedIPs;
                        string[] authenticatedIPs = ipstring.Split(',');
                        bool isIpAuthenticated = false;
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        if (!authenticatedIPs.Contains(currentIP))
                        {
                            if (Environment.CommandLine.ToLower().Contains("iisexpress.exe") &&
                                currentIP == "::1") ///localhost !!!
                            {
                                isIpAuthenticated = true;//iisexpress
                            }
                            else
                            {
                                isIpAuthenticated = false;
                            }

                        }
                        else
                            isIpAuthenticated = true;

                        if (!isIpAuthenticated)
                        {
                            user.IpRestricted = true;
                            user.HasError = true;
                        }
                    }
                }
            }
            return IsTwoFactorAuthenticationRequired;
        }

        private string AddVerificationCodeSMSLog(Contact loggedContact, TwoFactorAuthenticationDevice device, ICommonDataContext commonDataContext)
        {
            if (!string.IsNullOrEmpty(loggedContact.Mobile) && loggedContact.Mobile.Length > 7)
            {
                int tenant = device.Tenant;


                //user.UserMobileNumber = GetContactMaskedMobileNumber(loggedContact);

                DocumentRepository documentRepository = new DocumentRepository(commonDataContext);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonDataContext);

                ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("TwoFactorAuthenticationDevice", 0, true);
                string myObjectTableId = null;
                if (objectTable != null)
                {
                    myObjectTableId = objectTable.Id;
                }

                string body = "Please use the code " + device.AuthenticationCode + " to verify your Logitude Account";
                byte[] bytearray = Encoding.ASCII.GetBytes(body);

                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "txt",
                    FileSize = bytearray.Length,
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "MobileSMS",
                };

                documentRepository.Add(document);
                documentRepository.SubmitChanges();

                string subject = "Two-Factor Authentication Verification SMS";
                CommunicationLog commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    LastStatusDateUTC = DateTime.UtcNow,
                    To = loggedContact.Mobile,
                    InOut = "O",
                    From = "Two-Factor Authentication",
                    Subject = "Two-Factor Authentication Verification SMS",
                    Tenant = tenant,
                    CommunicationLogTypeCode = "SMS",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CommunicationStatusTypeCode = "W",
                    CreatedByUserId = loggedContact.Id,
                    DocumentId = document.Id,
                    SearchFields = loggedContact.Mobile + "," + "SMS" + "," + "O" + "," + subject,
                    CreateDateUTC = DateTime.UtcNow,
                    EntityId = device.TwoFactorkey,
                    ObjectTableId = myObjectTableId,
                    IsSecured = true,
                };

                communicationLogRepository.Add(commLog);
                communicationLogRepository.SubmitChanges();
                //"Please use the code " + device.AuthenticationCode + " to verify your Logitude Account"

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = bytearray.Length,
                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                storageservice.Write(bytearray, fileInfo);

                DbQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("MobileSMS", 0);
                Dictionary<string, string> param = new Dictionary<string, string>() { { "LogId", commLog.Id }, { "Tenant", tenant.ToString() } };
                queueservice.Send(param);

                return GetContactMaskedMobileNumber(loggedContact);
            }

            return null;
        }

        private string GetContactMaskedMobileNumber(Contact loggedContact)
        {
            if (!string.IsNullOrEmpty(loggedContact.Mobile))
            {
                var firstDigits = loggedContact.Mobile.Substring(0, 4);
                var lastDigits = loggedContact.Mobile.Substring(loggedContact.Mobile.Length - 2, 2);
                var requiredMask = new String('*', loggedContact.Mobile.Length - firstDigits.Length - lastDigits.Length);
                var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);

                return maskedString;
            }
            else
                return string.Empty;
        }

        public bool PostAuthenticationDeviceVerificationCode(string deviceKey, string verificationCode, int tenant)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            TwoFactorAuthenticationDeviceRepository deviceRepository = new TwoFactorAuthenticationDeviceRepository(commonDataContext);
            TwoFactorAuthenticationDevice device = deviceRepository.GetSingleTwoFactorAuthenticationDeviceByKey(deviceKey, tenant);
            if (!string.IsNullOrEmpty(deviceKey) && !string.IsNullOrEmpty(verificationCode))
            {
                deviceKey = deviceKey.Trim();
                verificationCode = verificationCode.Trim();

                if (device != null && device.CodeExpirationDate > TenantServerConfigration.GetCurrentDateTime(tenant) && device.AuthenticationCode == verificationCode && device.InActive == false)
                {
                    string ipAddress = AuthenticationUtil.GetIP4Address();

                    //string deviceDescription = HttpContext.Current.Request.UserHostName;
                    //System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
                    //if (host != null)
                    //    deviceDescription = host.HostName;

                    string deviceDescription = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : "";

                    device.IsVerified = true;
                    device.LastLoginIP = ipAddress;
                    device.DeviceDescription = deviceDescription;
                    device.LastLoginDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    deviceRepository.Update(device);
                    deviceRepository.SubmitChanges();

                    return true;
                }
            }

            return false;
        }

        public string PostResendAuthenticationDeviceVerificationCode(string deviceKey, string userId, int tenant)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            TwoFactorAuthenticationDeviceRepository deviceRepository = new TwoFactorAuthenticationDeviceRepository(commonDataContext);
            TwoFactorAuthenticationDevice device = null;
            if (!string.IsNullOrEmpty(deviceKey))
            {
                device = deviceRepository.GetSingleTwoFactorAuthenticationDeviceByUser(deviceKey, userId, tenant);
            }

            Contact loggedContact = (from a in commonDataContext.Contacts
                                     where a.Id == userId
                                     select a).FirstOrDefault();

            //string deviceDescription = HttpContext.Current.Request.UserHostName;
            //System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
            //if (host != null)
            //    deviceDescription = host.HostName;

            string deviceDescription = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : "";

            Random generator = new Random();
            string authCode = generator.Next(0, 1000000).ToString("D6");


            deviceKey = deviceKey.Trim();
            if (device != null)
            {
                device.AuthenticationCode = authCode;
                device.CodeExpirationDate = TenantServerConfigration.GetCurrentDateTime(tenant).AddMinutes(10);
                device.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                deviceRepository.Update(device);
                deviceRepository.SubmitChanges();




            }
            else
            {
                string key = StringHelper.GetRandomString(40);
                device = new TwoFactorAuthenticationDevice()
                {
                    Id = IdCounter.GetNumber("TwoFactorAuthenticationDevice", tenant),
                    TwoFactorkey = key,
                    Tenant = tenant,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UserId = loggedContact.Id,
                    CodeExpirationDate = TenantServerConfigration.GetCurrentDateTime(tenant).AddMinutes(10),
                    AuthenticationCode = authCode,
                    DeviceDescription = deviceDescription,
                };

                deviceRepository.Add(device);
                deviceRepository.SubmitChanges();

            }

            return AddVerificationCodeSMSLog(loggedContact, device, commonDataContext);

        }

        private string GetUserTechology(User logitudeUser, TenantManagement tenantManagement, int tenant)
        {
            string result = "";
            string Browser = HttpContext.Current.Request.Browser.Type;

            if (logitudeUser != null)
            {
                if (logitudeUser.SetAngularAsDefault) result = "AG";
                else result = logitudeUser.Technology;
            }

            if (tenantManagement != null)
            {
                if ((!string.IsNullOrEmpty(result) && result == "DE") || string.IsNullOrEmpty(result)) result = tenantManagement.Technology;

                //if ((tenantManagement.PackageCode == "EAWB" || tenantManagement.PackageCode == "BUBK"))
                //{
                //    if (Browser.Contains("Chrome") || Browser.Contains("Firefox") || Browser.Contains("Opera")) result = "AG";
                //}

            }

            //if (tenant == 65)
            //{
            //    result = "AG";
            //    if (Browser.Contains("InternetExplorer") || Browser.Contains("Internet Explorer") || Browser.Contains("IE")) result = "SL";
            //}


            return result;
        }

        private UserData ValidateUser(string name, string password, string customData, out string userData, bool isUser, string cardId, string cardType, bool byToken, string via, bool isAngularLogin,string clientType)
        {
            ContactPassword contactPassword = null;
            UserData user = null;
            userData = null;

            name = name.ToLower();

            if (name.Contains("system@"))
            {
                return user;
            }

            GlobalContact member = null;
            //Card card = null;
            //CardContact cardContact = null;
            int tenant = -1;

            string computerId = null;
            if (customData != null)
            {
                string[] customdataArray = customData.Split(',');

                int.TryParse(customdataArray[0], out tenant);
                computerId = customdataArray[1];
            }
            else
            {
            }


            string hashedPassword = hashedPassword = byToken ? password : PasswordGenerator.GetHashedPassword(name, password);

            bool isHashPassword = byToken;

            if (!isHashPassword)
            {
                var passResult = ResolvePassword(password);
                if (passResult != null)
                {
                    hashedPassword = passResult.Password;
                    isHashPassword = passResult.isHashPassword;
                }
            }


            IGlobalContext globalObjectContext = GlobalContext.GetContext();




            bool customerCare = false;
            bool distributor = false;
            User logitudeUser = null;
            GlobalContact contact = globalObjectContext.GlobalContacts.Where(d => d.GlobalTenantId == 0 && d.InActive == false && d.Email == name).FirstOrDefault(); //mohammad




            if (contact != null)
            {
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
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
            else
            {
                List<GlobalContact> globalcontacts = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.InActive == false && m.GlobalTenantId == tenant).ToList();
                if (globalcontacts.Count > 1)
                {
                    contact = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.InActive == false && m.GlobalTenantId == tenant && m.IsUser == isUser).FirstOrDefault();
                }
                else
                {
                    contact = globalcontacts.FirstOrDefault();
                }
            }
            if (contact != null)
            {
                bool isIpAuthenticated = true;
                if (customerCare)//(contact.Email == "customercare@logitudeworld.com")
                {
                    isIpAuthenticated = IscustomerCareIpAuthenticated();
                }

                if (isIpAuthenticated)
                {

                    string pass = isHashPassword ? hashedPassword : password;
                    contactPassword = AuthenticationUtil.VerifyContactPassword(name, pass, globalObjectContext, isHashPassword);


                    if (contactPassword != null)
                    {
                        CheckLockedUser(contactPassword, globalObjectContext , clientType);

                        if ((!contactPassword.IsLocked  || clientType == "Web") && (!contactPassword.MustChangePassword || this.OneTimePassword))
                        {
                            member = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.GlobalTenantId == 0).FirstOrDefault();
                            if (member == null)
                            {
                                List<GlobalContact> globalcontacts = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.InActive == false && (m.IsUser == true || m.InternetAccess == true) && m.GlobalTenantId == tenant).ToList();//|| m.InternetAccess == true
                                if (globalcontacts.Count > 1)
                                {
                                    if (isUser)
                                    {
                                        member = globalcontacts.Where(m => m.Email == name && m.InActive == false && m.IsUser == true && m.GlobalTenantId == tenant).FirstOrDefault();//|| m.InternetAccess == true
                                    }
                                    else
                                    {
                                        member = globalcontacts.Where(m => m.Email == name && m.InActive == false && m.InternetAccess == true && m.GlobalTenantId == tenant).FirstOrDefault();//|| m.InternetAccess == true

                                    }
                                }
                                else
                                {
                                    member = globalcontacts.FirstOrDefault();
                                }
                            }

                            if (member != null)
                            {
                                if (contactPassword.NumberOfRetries > 0)
                                {
                                    contactPassword.NumberOfRetries = 0;
                                    contactPassword.LockDateTime = null;
                                    globalObjectContext.SaveChanges();
                                }

                                RoleQuery roleQuery = new RoleQuery(tenant);
                                List<RolePM> allRoles = roleQuery.GetRolesForContact(contact.Id, contact.GlobalTenantId).ToList();
                                List<RolePM> allCustomRoles = allRoles.Where(d => d.IsCustomRole == true).ToList();

                                List<string> allRolesIds = allRoles.Select(s => s.Id).ToList();

                                foreach (RolePM item in allCustomRoles)
                                {
                                    if (allRolesIds.Contains(item.ParentRoleId))
                                    {
                                        allRolesIds.Remove(item.ParentRoleId);
                                    }
                                }

                                ContactInfo myContactInfo = new ContactInfo()
                                {
                                    Tenant = tenant,
                                    ContactEmail = contact.Email,
                                    RolesIds = allRolesIds,
                                };

                                CacheManager.CacheWrapper.Insert(contact.Email + tenant, myContactInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                            else
                            {

                                contactPassword.NumberOfRetries++;
                                if (contactPassword.NumberOfRetries >= 5)
                                {
                                    contactPassword.IsLocked = true;
                                    contactPassword.LockDateTime = DateTime.Now;
                                }

                                globalObjectContext.SaveChanges();
                            }
                        }
                    }
                }

                if (member != null)
                {
                    if (!string.IsNullOrEmpty(cardId))
                    {
                        ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == member.Id && d.CardId == cardId).FirstOrDefault();
                        if (cardContact == null)
                        {
                            GlobalContact member2 = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.InActive == false && (m.IsUser == true || m.InternetAccess == true) && m.GlobalTenantId == tenant).FirstOrDefault();
                            if (member2 != null)
                            {
                                member = member2;
                            }
                        }
                    }

                    user = new UserData()
                    {
                        UserName = name,
                        Id = member.Id,
                        Name = name,
                        CurrentTenant = tenant,//member.GlobalTenantId,
                        IsUser = isUser,
                    };
                }

                if (user != null)
                {

                    Card card = null;
                    string userAgent = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : null;

                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);

                    if (isUser)
                    {
                        if (isAngularLogin)
                        {
                            ActivityLog.SendTotangoContactActivity(contact.Email, "(A) Miscellaneous", "(A) Login", tenant, false, cardId, via);
                        }
                        else
                        {

                            ActivityLog.SendTotangoContactActivity(contact.Email, "Miscellaneous", "Login", tenant, false, cardId, via);
                        }

                        UserLoginLog userLog = new UserLoginLog()
                        {
                            Id = IdCounter.GetNumber("UserLoginLog", tenant).ToString(),
                            Tenant = tenant,
                            Browser = HttpContext.Current.Request.Browser.Type,
                            IP = AuthenticationUtil.GetIP4Address(),// HttpContext.Current.Request.UserHostAddress,
                            UserId = user.Id,
                            GMTDateTime = DateTime.Now,
                            LocalDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                            UserAgent = userAgent,
                            ComputerId = computerId,
                        };
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (!string.IsNullOrEmpty(currentIP))
                        {
                            userLog.Browser = userLog.Browser.ToUpper();
                        }
                        UserLastLogin lastLogin = (from a in commonDataContext.UserLastLogins
                                                   where a.Id == user.Id
                                                   select a).FirstOrDefault();
                        if (lastLogin == null)
                        {
                            lastLogin = new UserLastLogin()
                            {
                                Id = user.Id,
                                Tenant = tenant,
                                ComputerId = computerId,
                            };

                            commonDataContext.UserLastLogins.Add(lastLogin);
                        }

                        user.LastLoginDateTime = lastLogin.LoginDateTime;

                        lastLogin.LoginDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        lastLogin.Tenant = tenant;
                        commonDataContext.UserLoginLogs.Add(userLog);
                        commonDataContext.SaveChanges();
                    }
                    else
                    {

                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == member.Id && d.CardId == cardId).FirstOrDefault();
                        if (cardContact != null)
                        {
                            card = commonDataContext.Cards.Where(d => d.Id == cardId).FirstOrDefault();
                            card.SharedLogisticsInvitationStatusCode = 3;
                            card.LastLoginDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            cardContact.LastLoginDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            user.CardId = card.Id;
                            user.CardType = card.PartnerTypeId;


                            if (via == "Mobile")
                            {
                                card.IsActiveForMobile = true;
                            }

                        }

                        string activity = card.PartnerTypeId == "CS" ? "Customer Access" : "Agent Access";

                        ActivityLog.SendTotangoContactActivity(contact.Email, "System Login", activity, tenant, true, cardId, via);
                        //Abed    Log
                        ContactLoginLog contactLog = new ContactLoginLog()
                        {
                            Id = IdCounter.GetNumber("ContactLoginLog", tenant).ToString(),
                            Tenant = tenant,
                            Browser = HttpContext.Current.Request.Browser.Type,
                            IP = AuthenticationUtil.GetIP4Address(),// HttpContext.Current.Request.UserHostAddress,
                            ContactId = user.Id,
                            GMTDateTime = DateTime.Now,
                            LocalDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                            ContactAgent = userAgent,
                            ComputerId = computerId,
                            Via = via,
                        };




                        ContactLastLogin lastLogin = (from a in commonDataContext.ContactLastLogins
                                                      where a.Id == user.Id
                                                      select a).FirstOrDefault();
                        if (lastLogin == null)
                        {
                            lastLogin = new ContactLastLogin()
                            {
                                Id = user.Id,
                                Tenant = tenant,
                            };

                            commonDataContext.ContactLastLogins.Add(lastLogin);
                        }

                        lastLogin.ComputerId = computerId;
                        lastLogin.LoginDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        commonDataContext.ContactLoginLogs.Add(contactLog);
                        commonDataContext.SaveChanges();
                        // add a record to contact last login table
                    }
                    //contact.Email != "customercare@logitudeworld.com" && customercare to be replaced with tenant 0 users that are not distributors

                    if (logitudeUser != null)
                    {
                        distributor = logitudeUser.IsDistributor;
                        customerCare = !logitudeUser.IsDistributor;
                    }

                    if (!distributor && !customerCare)
                    {
                        globalObjectContext.TenantManagements.Where(t => t.Id == tenant).FirstOrDefault().LastLoginDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        globalObjectContext.SaveChanges();
                    }

                    if (card != null)
                    {
                        userData = member.Email + ":" + member.Id + ":" + card.Id + ":" + card.PartnerTypeId + SimplogGuid;
                    }
                    else
                    {
                        if (member != null)
                        {
                            userData = member.Email + ":" + member.Id + ":" + SimplogGuid;
                        }
                    }


                }
            }


            if (contactPassword != null)
            {
                user.NumberOfRetries = contactPassword.NumberOfRetries;
            }
            return user;
        }

        private static bool IscustomerCareIpAuthenticated()
        {
            bool isIpAuthenticated = false;
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
                    if (Environment.CommandLine.ToLower().Contains("iisexpress.exe") &&
                        HttpContext.Current.Request.UserHostAddress == "::1") ///localhost !!!
                    {
                        isIpAuthenticated = true;//iisexpress
                    }
                    else
                    {
                        isIpAuthenticated = false;
                    }

                }
                else
                {
                    isIpAuthenticated = true;//authenticatedIPs.Contains(HttpContext.Current.Request.UserHostAddress))
                }
            }
            else
            {
                isIpAuthenticated = true;///authenticatedIPs.Contains("*"))
            }
            return isIpAuthenticated;
        }

        PasswordCheckService passwordChkService = new PasswordCheckService();

        private UserData CheckUserState(string email, string password, ref ContactPassword contactPassword, bool byToken, string clientType)
        {

            if (!string.IsNullOrEmpty(email)) email = email.ToLower();
            UserData userData = new UserData()
            {
                UserName = email,

            };
            IGlobalContext globalContext = GlobalContext.GetContext();
            string hashedPassword = byToken ? password : PasswordGenerator.GetHashedPassword(email, password);

            bool IsOneTimePassword = false;

            bool isHashPassword = byToken;
            if (!isHashPassword)
            {
                var passResult = ResolvePassword(password);
                if (passResult != null)
                {
                    hashedPassword = passResult.Password;
                    IsOneTimePassword = passResult.IsOneTimePassword;
                    isHashPassword = passResult.isHashPassword;
                }
            }

            string pass = isHashPassword ? hashedPassword : password;
            contactPassword = AuthenticationUtil.VerifyContactPassword(email, pass, globalContext, isHashPassword);

            if (contactPassword != null)
            {
                CheckLockedUser(contactPassword, globalContext, clientType);

                GlobalContact contact = globalContext.GlobalContacts.Where(d => d.GlobalTenantId == 0 && d.InActive == false && d.Email.ToLower() == email).FirstOrDefault(); //mohammad
                bool customerCare = false;
                bool distributor = false;
                User logitudeUser = null;
                if (contact != null)
                {
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
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

                    userData.Technology = logitudeUser.Technology;
                    userData.IsUser = contact.IsUser;
                }


                userData.IsLocked = contactPassword.IsLocked;
                userData.MustChangePassword = IsOneTimePassword ? false : contactPassword.MustChangePassword;



                if (customerCare)
                {
                    //string ipstring = LogitudeSettings.CustomerCareIP;//System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
                    //string[] authenticatedIPs = ipstring.Split(',');
                    //if (!authenticatedIPs.Contains("*"))
                    //{
                    //    if (!authenticatedIPs.Contains(HttpContext.Current.Request.UserHostAddress))
                    //    {
                    //        userData.IpRestricted = true;
                    //    }
                    //}
                    userData.IpRestricted = !IscustomerCareIpAuthenticated();
                }
            }
            else
            {
                contactPassword = globalContext.ContactPasswords.Where(c => c.Email.ToLower() == email).FirstOrDefault();
                if (contactPassword != null)
                {
                    contactPassword.NumberOfRetries++;
                    if (contactPassword.NumberOfRetries >= 5)
                    {
                       contactPassword.IsLocked = true;
                       contactPassword.LockDateTime = DateTime.Now;
                    }
                    globalContext.SaveChanges();
                }
                else userData.Param1 = true;
                userData.InValidMailOrPassword = true;

                if (byToken)
                {
                    userData.InValidMailOrPassword = false;
                }
            }


            userData.HasError = (userData.InValidMailOrPassword || userData.IpRestricted || (userData.IsLocked && clientType != "Web") || userData.MustChangePassword);

            if (contactPassword != null)
            {
                userData.NumberOfRetries = contactPassword.NumberOfRetries;
            }

            return userData;
        }

        private void CheckLockedUser(ContactPassword contact, IGlobalContext globalContext , string clientType)
        {
            if (contact.IsLocked)
            {
                if (contact.LockDateTime != null)
                {
                    TimeSpan timeElapsed = (DateTime.Now - contact.LockDateTime.Value);
                    if (timeElapsed.TotalMinutes > 30 || clientType == "Web")
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
            else if (clientType == "Web")
            {
                contact.IsLocked = false;
                contact.LockDateTime = null;
                contact.NumberOfRetries = 0;
                contact.CaptchaKey = null;
                globalContext.SaveChanges();
            }
        }

        public UserData GetRequestResetUserPassword(string email, bool ischamplogin)
        {
            ResetPasswordHelper resetPasswordHelper = new ResetPasswordHelper();
            UserData userData = resetPasswordHelper.ForgetPassword(email.ToLower(), ischamplogin, false,false);

            return userData;
        }
       
     

        private  string ResolveEmail(ref string email)
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

        public UserData GetRequestResetUserPassword(bool ismobile, string email)
        {
            ResetPasswordHelper resetPasswordHelper = new ResetPasswordHelper();
            UserData userData = resetPasswordHelper.ForgetPassword(email.ToLower(), false, ismobile, false);

            return userData;
        }


        public UserData GetRequestResetUserPassword(string email, string appEnvironment)//New Method
        {
            ResetPasswordHelper resetPasswordHelper = new ResetPasswordHelper();
            UserData userData = resetPasswordHelper.ForgetPassword(email.ToLower(), false, true, false,null,null, appEnvironment);

            return userData;
        }

        
        public HttpResponseMessage PostChangePassword(ResetPasswordParameters param, string email)
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
                        //ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);

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



        #region Mobile

        public SuccessMobile PostChangePassword(bool ismobile, ResetPasswordParameters param)
        {
            ResetPasswordHelper resetPasswordHelper = new ResetPasswordHelper();
            SuccessMobile successMobile = new SuccessMobile();
            string log = "";
            string currentpassword = "";
            bool isValid = true;
            ContactPasswordRepository contactPasswordRepository = new ContactPasswordRepository();

            if (!string.IsNullOrEmpty(param.Email)) param.Email = param.Email.ToLower();


            string newPassword = param.NewPassword;
            string oldPassword = param.OldPassword;

            PasswordChangeHelper passwordChangeHelper = new PasswordChangeHelper();
 
            try
            {
                passwordChangeHelper.ValidationPassword(param.Email, newPassword, param.OldPassword);
            }
            catch (Exception ex)
            {
                successMobile.ExceptionMessage = ex.Message;
                successMobile.IsScceed = false;
                isValid = false;
            }
            
            if (isValid)
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                if (param.MobilePageType == "ChangePassword")
                {
                    ContactPassword contactPassword = AuthenticationUtil.VerifyContactPassword(param.Email, oldPassword, globalContext);

                    if (contactPassword != null)
                    {
                        successMobile.IsScceed = passwordChangeHelper.ChangePassword(param.Email, newPassword);
                        currentpassword = contactPassword.Password;
                        log = "(Change password) Change password successfully";

                    }
                    else
                    {
                        successMobile.IsScceed = false;
                        successMobile.ExceptionMessage = "The old Password entered was invalid";

                        currentpassword = PasswordGenerator.GetHashedPassword(param.Email, oldPassword); ;
                        log = "(Change password) Change password failure( the old Password entered was invalid )";


                    }

                }
                else if (param.MobilePageType == "ForgetPassword")
                {
                    PasswordResetRequestRepository passwordResetRequestRepository = new PasswordResetRequestRepository();
                    IQueryable<PasswordResetRequest> passwordResetRequestList = passwordResetRequestRepository.GetPasswordResetRequestByEmail(param.Email, param.VerificationCode);

                    PasswordResetRequest passwordResetRequest = passwordResetRequestList.Where(d => d.VerificationCode == param.VerificationCode).FirstOrDefault();

                    if (passwordResetRequest != null)
                    {
                        if (DateTime.UtcNow <= passwordResetRequest.ExpirationDate)
                        {
                            successMobile.IsScceed = passwordChangeHelper.ChangePassword(param.Email, newPassword);

                            if (successMobile.IsScceed == true)
                            {

                                foreach (PasswordResetRequest item in passwordResetRequestList)
                                {
                                    if (DateTime.UtcNow <= item.ExpirationDate || item == passwordResetRequest)
                                    {

                                        item.DoneDate = DateTime.UtcNow;
                                        item.IsDone = true;
                                        passwordResetRequestRepository.Update(item);

                                    }

                                }

                                passwordResetRequestRepository.SubmitChanges();

                            }


                        }
                        else
                        {
                            successMobile.ExceptionMessage = "The code you entered is Expired";
                            successMobile.IsScceed = false;

                        }

                    }
                    else
                    {
                        successMobile.ExceptionMessage = "The number you entered doesn't match your code. Please try again.";
                        successMobile.IsScceed = false;
                    }
                    currentpassword = contactPasswordRepository.GetOldPasswordByEmail(param.Email);

                    if (successMobile.IsScceed)
                    {

                        log = "(Forget Password ) Forget password successfully";
                    }

                    else log = "(Forget Password ) Forget password failure ( " + successMobile.ExceptionMessage + " )";



                }
                else
                {
                    currentpassword = contactPasswordRepository.GetOldPasswordByEmail(param.Email);
                    successMobile.IsScceed = passwordChangeHelper.ChangePassword(param.Email, newPassword);

                    if (successMobile.IsScceed) log = "(Change password) Change password successfully";

                    else log = "(Change password)  Change password failure ( the old Password entered was invalid )";
                }
            }


            resetPasswordHelper.CreateChangePasswordLog(log, currentpassword, PasswordGenerator.GetBCryptHashedPassword(param.Email, newPassword), param.Email);
            return successMobile;
        }

        public bool ValidatePassword(string password)
        {
            if (password == null) throw new ArgumentNullException();

            bool hashCharacter = false;
            bool hasDecimalDigit = false;


            foreach (char c in password)
            {
                if (char.IsUpper(c) || char.IsLower(c) || char.IsSymbol(c)) hashCharacter = true;

                else if (char.IsDigit(c)) hasDecimalDigit = true;
            }


            bool isValid = hashCharacter && hasDecimalDigit;

            return isValid;

        }


    
        #endregion

        // GET api/<controller>

        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        public string GetSettingsLoginCode(int myDummyInteger, string myDummyString)
        {
            string myResult = "";

            IGlobalContext globalContext = GlobalContext.GetContext();
            Setting mySettings = globalContext.Settings.FirstOrDefault();

            if (mySettings != null)
            {
                myResult = mySettings.LogoCode;
            }

            return myResult;
        }



        private string GetHtmlVersion()
        {
            string myResult = "";

            IGlobalContext globalContext = GlobalContext.GetContext();
            Setting mySettings = globalContext.Settings.FirstOrDefault();

            if (mySettings != null && !string.IsNullOrEmpty(mySettings.HtmlVersion)) myResult = mySettings.HtmlVersion;

            return myResult;
        }


        private void AddServerTimeToHeaderRespose(int executionTime)
        {
            if (HttpContext.Current.Response.Headers["ServerTime"] != null)
            {
                HttpContext.Current.Response.Headers["ServerTime"] = executionTime.ToString();
            }
            else HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

        }



        //   [OperationContract]
        //   [WebGet(UriTemplate = "getcontactloginlog/{tenant}/{contactId}/{via}")]

        //   public bool GetContactLoginLog(int tenant, string contactId, string via)
        //   {

        //       string computerId = Guid.NewGuid().ToString("N");
        //       string userAgent = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : null;
        //       //ContactLoginLogRepository   
        //       ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
        //       ContactLoginLog contactLog = new ContactLoginLog()
        //       {
        //           Id = IdCounter.GetNumber("ContactLoginLog", tenant).ToString(),
        //           Tenant = tenant,
        //           Browser = HttpContext.Current.Request.Browser.Type,
        //           IP = HttpContext.Current.Request.UserHostAddress,
        //           ContactId = contactId,
        //           GMTDateTime = DateTime.Now,
        //           LocalDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
        //           ContactAgent = userAgent,
        //           ComputerId = computerId,
        //           Via = "Mobile"

        //       };


        //       commonDataContext.ContactLoginLogs.Add(contactLog);
        //       commonDataContext.SaveChanges();

        //       return true;
        //   }
        //}


        public HttpResponseMessage GetSignOut()//New Method
        {
            try
            {
                bool result = false;
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                if (authToken != null)
                {
                    authToken.InActive = true;
                    authToken.InActiveDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                    authToken.InActiveReason = "Sign Out";
                    AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(authToken.Tenant);
                    authenticationTokenRepository.Update(authToken);
                    authenticationTokenRepository.SubmitChanges();


                    string entityName = "Token" + authToken.Token;
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Remove(entityName);
                    }


                    result = true;
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }


        private PasswordParameter ResolvePassword(string password)
        {
            PasswordParameter result = null;

            if (!string.IsNullOrEmpty(password)) {
                if (password.Contains("@OneTimePassword") || password.Contains(@"HashPassword"))
                {
                    var passwordarray = password.Contains("@OneTimePassword") ? password.Split(new string[] { "@OneTimePassword" }, StringSplitOptions.None) : password.Split(new string[] { "@HashPassword" }, StringSplitOptions.None);
                    if (passwordarray.Length > 0)
                    {
                        result = new PasswordParameter();
                        result.Password = passwordarray[0];
                        if (password.Contains("@OneTimePassword")) result.IsOneTimePassword = true;
                        result.isHashPassword = true;
                    }
                }
            }
            return result;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getcontactloginlog/{tenant}/{contactId}/{via}")]


        public bool GetContactLoginLog(int tenant, string contactId, string via)
        {

            string computerId = Guid.NewGuid().ToString("N");
            string userAgent = !string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent) ? (HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500)) : null;
            //ContactLoginLogRepository   
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
            ContactLoginLog contactLog = new ContactLoginLog()
            {
                Id = IdCounter.GetNumber("ContactLoginLog", tenant).ToString(),
                Tenant = tenant,
                Browser = HttpContext.Current.Request.Browser.Type,
                IP = AuthenticationUtil.GetIP4Address(),//HttpContext.Current.Request.UserHostAddress,
                ContactId = contactId,
                GMTDateTime = DateTime.Now,
                LocalDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                ContactAgent = userAgent,
                ComputerId = computerId,
                Via = "Mobile"

            };


            commonDataContext.ContactLoginLogs.Add(contactLog);
            commonDataContext.SaveChanges();

            return true;
        }




        public HttpResponseMessage GetDocumentDownloadToken(string documentToken)//New Method
        {
            try
            {
                string result = "";
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                if (authToken != null)
                {
                    AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(authToken.Tenant);

                    if (!string.IsNullOrEmpty(documentToken))
                    {
                        AuthenticationToken documentAuthenticationToken = AuthenticationTokenRepository.GetSingleTokenFromCache(documentToken);
                        if (documentAuthenticationToken != null)
                        {
                            DateTime nowDate = DateTime.Now;
                            DateTime endDate = (DateTime)documentAuthenticationToken.ExpirationDate;
                            if (endDate.AddMinutes(-5) > nowDate)
                            {
                                result = documentAuthenticationToken.Token;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(result))
                    {
                        AuthenticationToken authenticationDocument = new AuthenticationToken()
                        {
                            CreateDate = DateTime.Now,
                            ExpirationDate = DateTime.Now.AddMinutes(15),
                            Email = authToken.Email,
                            Password = authToken.Password,
                            Token = AuthenticationUtil.GenerateToken(),
                            Tenant = authToken.Tenant
                            ,
                            ClientType = "DocumentDownload"
                        };

                        authenticationTokenRepository.Add(authenticationDocument);
                        authenticationTokenRepository.SubmitChanges();
                        result = authenticationDocument.Token;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }




    }
    public class LoginTokenParameter
    {
        public string Token { get; set; }
        public int Tenant { get; set; }
        public string ContactId { get; set; }
        public string CardId { get; set; }
        public bool IsMobileLogin { get; set; }
        public int MobileVersion { get; set; }
    }


    public class PasswordParameter
    {
        public string Password { get; set; }
        public bool isHashPassword { get; set; }
        public bool IsOneTimePassword { get; set; }

    }




}
