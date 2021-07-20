//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Http;
//using System.Web.Security;
//using Simplog.Data.CommonDataModel;
//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using Simplog.Data.Helpers;
//using Simplog.Data.InfrastructureModel;
//using Simplog.Global.Data.GlobalModel;
//using Simplog.Global.Data.GlobalModel.EntityPOCOs;
//using Simplog.Server.Infrastructure.Helpers;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using WebFreight.Web.DataContracts;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Security;
//using WebFreight.Web.WebServices;
//using System.Text.RegularExpressions;
//using System.Web.UI;

//using Simplog.Data.CommonDataModel.Repositories;
//using Logitude.Server.Tools.Counters;
//using Simplog.Global.Data.GlobalModel.Repositories;
//using Simplog.Server.Infrastructure;
//using Logitude.Server.Tools.Helpers;
//using Logitude.Server.Tools.StorageService;
//using Simplog.Server.Infrastructure.Azure;
//using Microsoft.Practices.Unity;
//using System.IO;
//using Logitude.Server.Tools;
//using Logitude.SystemLogs;
//using System.ServiceModel;
//using Microsoft.WindowsAzure.Storage.Blob;
//using System.Diagnostics;
//using Logitude.BL.GlobalModel.EntityQueries;
//using Logitude.BL.GlobalModel.EntityPMs;
//using System.ServiceModel.Web;
//using Logitude.Server.Tools.QueueService;
//using Simplog.Data.InfrastructureModel.Repositories;
//using Simplog.Data.InfrastructureModel.EntityPOCOs;
//using System.Net;
//using System.Text;
//using System.Net.Http;
//using WebFreight.Web.App_Code;
//using System.Threading;
//using Logitude.SystemLogs.POCOs;
//using Logitude.SystemLogs.Repositories;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Drawing.Drawing2D;
//using WebFreight.Web.Helpers.DataProviderHelpers;
//using Logitude.BL.Interfaces;
//namespace WebFreight.Web.Services
//{
//    public class LogitudeAuthenticationService: ILogitudeAuthenticationService
//    {
//        public LogitudeAuthenticationService()
//        {

//        }
//        public UserData ValidateUser()
//        {
//            TenantManagmentPrivateLabelsPM privatelabel = null;
//            var url = SecurityUtility.getLoggedDomain();
//            //if (LogitudeSettings.DeploymentStage.ToLower() == "test2")
//            //{
//            url = url.Split(':')[0];
//            //}
//            if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
//            {
//                TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
//                privatelabel = query.GetSingleActivePMByUrl(url);
//            }
//            DateTime DateBeforePostUserValidation = DateTime.Now;
//            string email = loginParameters.Email.Trim();
//            string password = loginParameters.Password;
//            string Techology = "";
//            UserData data = new UserData();
//            List<CompanyLogin> loginsList = new List<CompanyLogin>();
//            IGlobalContext globalObjectContext = GlobalContext.GetContext();
//            ContactPassword contactPassword = null;

//            data = CheckCaptchaState(loginParameters);

//            if (!data.HasError)
//            {
//                data = CheckUserState(email.ToLower(), password, ref contactPassword, loginParameters.ByToken, loginParameters.ClientType);
//            }

//            if (!data.HasError)
//            {

//                bool customerCare = false;
//                bool distributor = false;
//                User logitudeUser = null;

//                GlobalContact zeroContact = globalObjectContext.GlobalContacts.Where(d => d.GlobalTenantId == 0 && d.Email == email && d.InActive == false).FirstOrDefault(); //mohammad

//                if (zeroContact != null)
//                {
//                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
//                    logitudeUser = (from a in commonDataContext.Users
//                                    where a.Id == zeroContact.Id
//                                    select a).FirstOrDefault();



//                    if (logitudeUser != null)
//                    {
//                        Techology = logitudeUser.Technology;
//                        if (logitudeUser.Tenant == 0)
//                        {
//                            distributor = logitudeUser.IsDistributor;
//                            customerCare = !logitudeUser.IsDistributor;
//                        }
//                    }

//                }
//                if (customerCare || distributor)
//                {
//                    if (distributor)
//                    {
//                        List<int> distributorTenants = (from a in globalObjectContext.TenantManagements
//                                                        where (a.DistributorCode == logitudeUser.DistributorCode && a.Id != 0 && a.IsDistributorSupportEnabled)
//                                                        select a.Id).ToList();

//                        List<GlobalTenant> globalTenants = (from t in globalObjectContext.GlobalTenants
//                                                            where distributorTenants.Contains(t.Id) && t.IsActive //&& t.Version != -1 Islam: customer care didn't see this tenant
//                                                            select t).ToList();

//                        foreach (GlobalTenant globalTenant in globalTenants)
//                        {
//                            CompanyLogin company = new CompanyLogin()
//                            {
//                                Email = zeroContact.Email,
//                                CompanyName = globalTenant.CompanyName + " (" + globalTenant.Id + ")",
//                                IsUser = true,
//                                Tenant = globalTenant.Id,
//                                CardId = null,
//                                CardType = null,
//                                ContactId = zeroContact.Id,
//                                LicensedUser = true,
//                                PrivateLabelId = globalTenant.PrivateLabelId
//                            };

//                            loginsList.Add(company);
//                        }
//                    }
//                    else if (customerCare)
//                    {
//                        List<int> custCareTenants = (from a in globalObjectContext.TenantManagements
//                                                     where a.IsSystemSupportEnabled || a.Id == 0
//                                                     select a.Id).ToList();

//                        List<GlobalTenant> globalTenants = (from t in globalObjectContext.GlobalTenants
//                                                            where custCareTenants.Contains(t.Id) && t.IsActive //&& t.Version != -1 Islam: customer care didn't see this tenant
//                                                            select t).ToList();

//                        foreach (GlobalTenant globalTenant in globalTenants)
//                        {
//                            CompanyLogin company = new CompanyLogin()
//                            {
//                                Email = zeroContact.Email,
//                                CompanyName = globalTenant.CompanyName + " (" + globalTenant.Id + ")",
//                                IsUser = true,
//                                Tenant = globalTenant.Id,
//                                CardId = null,
//                                CardType = null,
//                                ContactId = zeroContact.Id,
//                                LicensedUser = true,
//                                PrivateLabelId = globalTenant.PrivateLabelId
//                            };

//                            loginsList.Add(company);
//                        }
//                    }

//                    List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.GlobalTenant.IsActive == true && m.InActive == false && (m.InternetAccess == true)).ToList();

//                    // Techology = GetUserTechology(contacts);

//                    foreach (GlobalContact contact in contacts)
//                    {
//                        ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
//                        GlobalTenant globalTenant = (from t in globalObjectContext.GlobalTenants
//                                                     where t.Id == contact.GlobalTenantId
//                                                     select t).Include("TenantManagement").FirstOrDefault();
//                        if (contact.InternetAccess)
//                        {
//                            //List<CardContact> cardcontactsList = commonDataContext.CardContacts.Where(c => c.ContactId == contact.Id && c.InternetAccess == true && c.Tenant == contact.GlobalTenantId).ToList();
//                            //foreach (CardContact cardContact in cardcontactsList)
//                            //{
//                            //    Card card = commonDataContext.Cards.Where(c => c.Id == cardContact.CardId && c.Tenant == cardContact.Tenant).FirstOrDefault(); ;
//                            //    CompanyLogin companyAccess = new CompanyLogin()
//                            //    {
//                            //        Email = contact.Email,
//                            //        CompanyName = globalTenant.CompanyName + "-" + card.EnglishName + " (" + contact.GlobalTenantId + ")",
//                            //        Tenant = contact.GlobalTenantId,
//                            //        CardId = card.Id,
//                            //        CardType = card.PartnerTypeId,
//                            //        IsUser = false,
//                            //        ContactId = contact.Id,
//                            //    };

//                            //    loginsList.Add(companyAccess);


//                            //}

//                            TenantRepository tenantRep = new TenantRepository(0);
//                            Tenant currentTenant = tenantRep.GetSingleTenant(contact.GlobalTenantId);
//                            bool enableLogin = false;
//                            if (loginParameters.IsMobileLogin)
//                            {
//                                enableLogin = currentTenant.IsMobileActivated;
//                            }
//                            else
//                            {
//                                enableLogin = currentTenant.IsWebAccessActivated;
//                            }
//                            if (enableLogin)
//                            {
//                                List<CardContact> cardcontactsList = commonDataContext.CardContacts.Where(c => c.ContactId == contact.Id && c.InternetAccess == true && c.Tenant == contact.GlobalTenantId).ToList();
//                                foreach (CardContact cardContact in cardcontactsList)
//                                {
//                                    Card card = commonDataContext.Cards.Where(c => c.Id == cardContact.CardId && c.Tenant == cardContact.Tenant).FirstOrDefault();
//                                    CompanyLogin companyAccess = new CompanyLogin()
//                                    {
//                                        Email = contact.Email,
//                                        CompanyName = globalTenant.CompanyName + "-" + card.EnglishName + " (" + contact.GlobalTenantId + ")",
//                                        Tenant = contact.GlobalTenantId,
//                                        CardId = card.Id,
//                                        CardType = card.PartnerTypeId,
//                                        IsUser = false,
//                                        ContactId = contact.Id,
//                                        InternetAccess = contact.InternetAccess,
//                                        CustomerName = card.EnglishName,
//                                        PrivateLabelId = globalTenant.PrivateLabelId
//                                    };

//                                    loginsList.Add(companyAccess);


//                                }
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.GlobalTenant.IsActive == true && m.InActive == false && (m.IsUser == true || m.InternetAccess == true)).ToList();


//                    //Techology = GetUserTechology(contacts);

//                    foreach (GlobalContact contact in contacts)
//                    {
//                        ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
//                        GlobalTenant globalTenant = (from t in globalObjectContext.GlobalTenants
//                                                     where t.Id == contact.GlobalTenantId
//                                                     select t).Include("TenantManagement").FirstOrDefault();

//                        if (contact.IsUser && !loginParameters.IsCargoTracking)
//                        {
//                            bool Licensed = true;
//                            if (globalTenant.TenantManagement.ManageLicencesPerUser)
//                            {
//                                User user = (from a in commonDataContext.Users
//                                             where a.Id == contact.Id
//                                             select a).FirstOrDefault();

//                                Licensed = user.LicencedUser;

//                            }

//                            CompanyLogin company = new CompanyLogin()
//                            {
//                                Email = contact.Email,
//                                CompanyName = globalTenant.CompanyName + " (" + contact.GlobalTenantId + ")",
//                                IsUser = true,
//                                Tenant = contact.GlobalTenantId,
//                                CardId = null,
//                                CardType = null,
//                                ContactId = contact.Id,
//                                LicensedUser = Licensed,
//                                InternetAccess = contact.InternetAccess,
//                                PrivateLabelId = globalTenant.PrivateLabelId
//                            };

//                            loginsList.Add(company);
//                        }
//                        if (contact.InternetAccess)
//                        {
//                            TenantRepository tenantRep = new TenantRepository(0);
//                            Tenant currentTenant = tenantRep.GetSingleTenant(contact.GlobalTenantId);
//                            bool enableLogin = false;
//                            if (loginParameters.IsMobileLogin)
//                            {
//                                enableLogin = currentTenant.IsMobileActivated;
//                            }
//                            else
//                            {
//                                enableLogin = currentTenant.IsWebAccessActivated;
//                            }
//                            if (enableLogin)
//                            {
//                                List<CardContact> cardcontactsList = commonDataContext.CardContacts.Where(c => c.ContactId == contact.Id && c.InternetAccess == true && c.Tenant == contact.GlobalTenantId).ToList();
//                                foreach (CardContact cardContact in cardcontactsList)
//                                {
//                                    Card card = commonDataContext.Cards.Where(c => c.Id == cardContact.CardId && c.Tenant == cardContact.Tenant).FirstOrDefault();
//                                    CompanyLogin companyAccess = new CompanyLogin()
//                                    {
//                                        Email = contact.Email,
//                                        CompanyName = globalTenant.CompanyName + "-" + card.EnglishName + " (" + contact.GlobalTenantId + ")",
//                                        Tenant = contact.GlobalTenantId,
//                                        CardId = card.Id,
//                                        CardType = card.PartnerTypeId,
//                                        IsUser = false,
//                                        ContactId = contact.Id,
//                                        InternetAccess = contact.InternetAccess,
//                                        CustomerName = card.EnglishName,
//                                        PrivateLabelId = globalTenant.PrivateLabelId
//                                    };

//                                    loginsList.Add(companyAccess);


//                                }
//                            }
//                        }


//                    }
//                }

//                List<CompanyLogin> unliscened = loginsList.Where(s => s.LicensedUser == false && s.IsUser == true).ToList();

//                loginsList = loginsList.Where(s => s.LicensedUser == true || s.IsUser == false).OrderBy(c => c.CompanyName).ToList();

//                List<string> logboxAccessiblePrivateLabelTenantsIds = GetLogboxAccessiblePrivateLabelTenantsIds(url);
//                MapHasLogboxAccessPrivateLabelTenants(loginsList, logboxAccessiblePrivateLabelTenantsIds);

//                if (loginsList.Count == 1)
//                {
//                    CompanyLogin companyAccess = loginsList.First();
//                    if (privatelabel != null && companyAccess.PrivateLabelId == privatelabel.Id)
//                    {
//                        data = PostLoginData(new LoginParameters()
//                        {

//                            Email = email,
//                            Password = password,
//                            IsUser = companyAccess.IsUser,
//                            CardId = companyAccess.CardId,
//                            CardType = companyAccess.CardType,
//                            ByToken = loginParameters.ByToken,
//                            IsMobileLogin = loginParameters.IsMobileLogin,
//                            GetToken = loginParameters.GetToken,
//                            IsAngularLogin = loginParameters.IsAngularLogin,
//                            InternalLoginValidationCall = true,
//                            ClientType = loginParameters.ClientType,
//                        }, companyAccess.Tenant);
//                    }
//                    else
//                    {
//                        if (privatelabel == null && (!string.IsNullOrEmpty(companyAccess.PrivateLabelId) && !companyAccess.HasLogboxAccess))
//                        {
//                            data = new UserData()
//                            {
//                                UserName = email,
//                                CompanyLogins = new List<CompanyLogin>(),
//                                HasError = true,
//                                // InValidMailOrPassword = (temp.Count() == 0 ? true : false),
//                                ContactsCount = 0,
//                                InActive = true,
//                                Unlicensed = (unliscened.Count() > 0 ? true : false),
//                                PrivateLablehasZeroTenant = true
//                            };
//                        }
//                        else if (privatelabel != null && string.IsNullOrEmpty(companyAccess.PrivateLabelId))
//                        {
//                            data = new UserData()
//                            {
//                                UserName = email,
//                                CompanyLogins = new List<CompanyLogin>(),
//                                HasError = true,
//                                // InValidMailOrPassword = (temp.Count() == 0 ? true : false),
//                                ContactsCount = 0,
//                                InActive = true,
//                                Unlicensed = (unliscened.Count() > 0 ? true : false),
//                                PrivateLablehasZeroTenant = true
//                            };
//                        }
//                        else
//                        {
//                            data = PostLoginData(new LoginParameters()
//                            {

//                                Email = email,
//                                Password = password,
//                                IsUser = companyAccess.IsUser,
//                                CardId = companyAccess.CardId,
//                                CardType = companyAccess.CardType,
//                                ByToken = loginParameters.ByToken,
//                                IsMobileLogin = loginParameters.IsMobileLogin,
//                                GetToken = loginParameters.GetToken,
//                                IsAngularLogin = loginParameters.IsAngularLogin,
//                                InternalLoginValidationCall = true,
//                                ClientType = loginParameters.ClientType,
//                            }, companyAccess.Tenant);
//                        }
//                    }



//                    data.CompanyLogins = loginsList;
//                    //email, password, companyAccess.Tenant, companyAccess.IsUser, companyAccess.CardId, companyAccess.CardType);
//                    data.ContactsCount = 1;
//                }
//                else
//                {
//                    var temp = new List<CompanyLogin>();
//                    if (privatelabel != null)
//                    {
//                        temp = loginsList.Where(a => a.PrivateLabelId == privatelabel.Id).ToList();
//                    }
//                    else
//                    {
//                        temp = loginsList.Where(a => a.PrivateLabelId == null || a.HasLogboxAccess).ToList();
//                    }
//                    if (temp.Count == 1)
//                    {
//                        CompanyLogin companyAccess = temp.First();
//                        data = PostLoginData(new LoginParameters()
//                        {

//                            Email = email,
//                            Password = password,
//                            IsUser = companyAccess.IsUser,
//                            CardId = companyAccess.CardId,
//                            CardType = companyAccess.CardType,
//                            ByToken = loginParameters.ByToken,
//                            IsMobileLogin = loginParameters.IsMobileLogin,
//                            GetToken = loginParameters.GetToken,
//                            IsAngularLogin = loginParameters.IsAngularLogin,
//                            InternalLoginValidationCall = true,
//                            ClientType = loginParameters.ClientType,
//                        }, companyAccess.Tenant);

//                        data.CompanyLogins = temp;
//                        //email, password, companyAccess.Tenant, companyAccess.IsUser, companyAccess.CardId, companyAccess.CardType);
//                        data.ContactsCount = 1;
//                    }
//                    else
//                    {
//                        data = new UserData()
//                        {
//                            UserName = email,
//                            CompanyLogins = temp,
//                            HasError = (temp.Count() == 0 ? true : false),
//                            // InValidMailOrPassword = (temp.Count() == 0 ? true : false),
//                            ContactsCount = temp.Count(),
//                            InActive = (privatelabel != null ? false : (temp.Count() == 0 ? true : false)),
//                            Unlicensed = (unliscened.Count() > 0 ? true : false),
//                            PrivateLablehasZeroTenant = (privatelabel != null ? (temp.Count() == 0 ? true : false) : false)
//                        };
//                    }

//                }
//            }

//            if (loginParameters.IsMobileLogin && !data.HasError)
//            {
//                data.CompanyLogins = data.CompanyLogins.Where(c => c.InternetAccess = true).ToList();

//                TenantRepository tenantRepository = new TenantRepository(0);
//                List<int> tenants = (from a in data.CompanyLogins
//                                     select a.Tenant).ToList();

//                List<Tenant> mobileTenants = (from t in tenantRepository.context.Tenants
//                                              where tenants.Contains(t.Id)
//                                              select t).ToList();

//                List<CompanyLogin> mobileActivatedTenants = new List<CompanyLogin>();
//                foreach (CompanyLogin login in data.CompanyLogins)
//                {
//                    Tenant tenant = mobileTenants.FirstOrDefault(t => t.Id == login.Tenant);
//                    if (tenant != null)
//                    {
//                        if (tenant.IsMobileActivated)
//                        {

//                            List<string> LogoInfo = GetTenantLogoUri(login.Tenant, loginParameters.MobileVersion);

//                            if (LogoInfo != null && LogoInfo.Count > 0) login.URL = LogoInfo[0];
//                            if (LogoInfo != null && LogoInfo.Count > 1) login.Extension = LogoInfo[1];


//                            mobileActivatedTenants.Add(login);

//                        }
//                    }


//                }

//                data.CompanyLogins = mobileActivatedTenants;
//                data.InActive = (mobileActivatedTenants.Count() == 0 ? true : false);
//                data.Unlicensed = (mobileActivatedTenants.Count() == 0 ? true : false);
//                data.HasError = (mobileActivatedTenants.Count() == 0 ? true : false);
//                data.ContactsCount = mobileActivatedTenants.Count();
//                data.InvalidMobileAccessPermission = (mobileActivatedTenants.Count() == 0 ? true : false);


//            }

//            int executionTime = (int)((DateTime.Now.Ticks - DateBeforePostUserValidation.Ticks) / TimeSpan.TicksPerMillisecond);
//            AddServerTimeToHeaderRespose(executionTime);

//            #region PasswordExpirationDate

//            if (data != null && !data.HasError && loginParameters.ClientType == "Web")
//            {
//                if (contactPassword == null)
//                {
//                    contactPassword = globalObjectContext.ContactPasswords.Where(c => c.Email.ToLower() == email).FirstOrDefault();
//                }

//                if (contactPassword != null && contactPassword.PasswordExpirationDate != null)
//                {
//                    DateTime nowDate = DateTime.Now;
//                    DateTime expirationDate = (DateTime)contactPassword.PasswordExpirationDate;

//                    if (nowDate.Date > expirationDate.Date)
//                    {
//                        data.HasError = true;
//                        data.MustChangePassword = true;
//                    }
//                    else
//                    {
//                        var days = (expirationDate - nowDate).TotalDays;
//                        if (days.ToString().Contains("."))
//                        {
//                            days = Int32.Parse(days.ToString().Split('.')[0]) + 1;
//                        }
//                        if (days <= 10)
//                        {
//                            data.HasError = true;
//                            data.PasswordExpirationDateMessage = "Your password will expire in " + days + " days. Do you want to change it now?";
//                        }
//                    }

//                }
//            }
//            #endregion


//            if (data.HasError)
//            {
//                if (data.InValidMailOrPassword || data.IsLocked || data.IpRestricted)
//                {
//                    AddFailedLoginLog(data, loginParameters);
//                    // int sleepTime = data.NumberOfRetries > 0 ? data.NumberOfRetries : 1;

//                    if (!data.IpRestricted && loginParameters.ClientType == "Web")
//                    {
//                        bool isLoadContactPasswords = false;
//                        if (contactPassword == null)
//                        {
//                            contactPassword = globalObjectContext.ContactPasswords.Where(c => c.Email.ToLower() == email).FirstOrDefault();
//                            isLoadContactPasswords = true;
//                        }

//                        if (contactPassword != null)
//                        {
//                            if (contactPassword.NumberOfRetries++ >= 5)
//                            {
//                                CaptchaHelper captchaHelper = new CaptchaHelper();
//                                captchaHelper.AddCaptchaKey(loginParameters.Email, data, "Login");
//                                if (!isLoadContactPasswords) contactPassword = globalObjectContext.ContactPasswords.Where(c => c.Email.ToLower() == loginParameters.Email).FirstOrDefault();
//                                if (contactPassword != null)
//                                {
//                                    contactPassword.CaptchaKey = data.CaptchaKey;
//                                    globalObjectContext.SaveChanges();
//                                }
//                            }
//                        }
//                    }

//                    //  Thread.Sleep(sleepTime);
//                }



//            }

//            return data;
//        }
//    }
//}