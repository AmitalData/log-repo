using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Services;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;

using WebFreight.Web.CommonDataModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Text;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for PasswordCheckService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PasswordCheckService : System.Web.Services.WebService
    {
        [WebMethod]
        public bool CheckUserPassword(string password, string contactId, int tenant, string emailContact = null)
        {
            bool isValid = false;
          
            GlobalContact contact = null;
            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            if (!string.IsNullOrEmpty(contactId))
            {
                contact = globalObjectContext.GlobalContacts.Where(c => c.Id == contactId && c.GlobalTenantId == tenant && (c.IsUser == true || c.InternetAccess == true) && c.InActive == false).FirstOrDefault();
            }
           
            if (contact != null || !string.IsNullOrEmpty(emailContact))
            {
                string email = contact!=null? contact.Email != null ? contact.Email.ToLower() : "": emailContact;

                ContactPassword contactPassword = AuthenticationUtil.VerifyContactPassword(email, password, globalObjectContext);

                if (contactPassword != null)
                {
                    isValid = true;

                }
            }
            return isValid;
        }

        [WebMethod]
        public bool CheckIfUserIsLocked(string email, ref bool inValidEmail)
        {
            bool isLocked = false;

            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            List<GlobalContact> globalContacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.InActive == false && (m.IsUser == true || m.InternetAccess == true)).ToList();// || m.InternetAccess
            inValidEmail = true;
            foreach (GlobalContact globalContact in globalContacts)
            {
                inValidEmail = false;

                isLocked = globalObjectContext.ContactPasswords.Where(c => c.Email == email).FirstOrDefault().IsLocked;

                //ICommonDataContext commonDataContext = CommonDataContext.GetContext(globalContact.GlobalTenantId);
                //List<Contact> contacts = commonDataContext.Contacts.Where(m => m.Email == email && m.InActive == false).ToList();
                //if (contacts.Count != 0)
                //{
                //    isLocked = contacts.Where(f => f.IsLocked == true).Any();
                //}
                //else
                //{
                //    inValidEmail = true;
                //}
            }

            // CommonDataContext commonDataContext = new CommonDataContext();
            //List< Contact> contacts = commonDataContext.Contacts.Where(m => m.Email == email && m.InActive == false).ToList();
            //if (contacts.Count != 0)
            // {
            //     isLocked = contacts.Where(f => f.IsLocked == true).Any();
            // }
            // else
            // {
            //     InValidEmail = true;
            // }

            return isLocked;
        }

        [WebMethod]
        public bool CheckIfUserIsLockedOrPasswordReset(string email, int tenant, string password, ref bool mustChangePassword, ref bool isLocked, ref bool isValidPassword, ref bool isIpRestricted)
        {
            //ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            IGlobalContext globalContext = GlobalContext.GetContext();
            string hashedPassword = PasswordGenerator.GetHashedPassword(email, password);
            GlobalContact contact = globalContext.GlobalContacts.Where(c => c.GlobalTenantId == tenant && c.Email == email && c.InActive == false).FirstOrDefault();
            ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == email).FirstOrDefault();
            bool isValidMail = false;
            if (contactPassword != null && contact != null)
            {
                bool customerCare = false;
                bool distributor = false;
                User logitudeUser = null;
                GlobalContact contactZero = globalContext.GlobalContacts.Where(d => d.GlobalTenantId == 0 && d.Email == email).FirstOrDefault(); //mohammad
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


                isValidMail = true;
                isValidPassword = (contactPassword.Password == hashedPassword && !contactPassword.IsBCrypt);
                if (!isValidPassword)
                {
                    isValidPassword = PasswordGenerator.VerifyBCryptHashedPassword(email, password, contactPassword.Password);
                }


                isLocked = contactPassword.IsLocked;
                mustChangePassword = contactPassword.MustChangePassword;

                isIpRestricted = false;
                if (customerCare)
                {
                    string ipstring = LogitudeSettings.CustomerCareIP;//System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
                    string[] authenticatedIPs = ipstring.Split(',');
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }

                    if (!authenticatedIPs.Contains(currentIP))
                    {
                        isIpRestricted = true;
                    }
                }
            }


            return isValidMail;
        }

        [WebMethod]
        public bool ResetUserPassword(string email, int tenant)
        {
            bool succeeded = false;
            IGlobalContext globalContext = GlobalContext.GetContext();
            GlobalContact contact = globalContext.GlobalContacts.Where(c => c.GlobalTenantId == tenant && c.Email == email && c.InActive == false).FirstOrDefault();
            ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == email).FirstOrDefault();
            if (contactPassword != null && contact != null)
            {
                string newPassword = PasswordGenerator.Generate(8);
                string newHashedPassword = PasswordGenerator.GetBCryptHashedPassword(email, newPassword);
                UpdateContactWithNewHashedPassword(globalContext, contactPassword, newHashedPassword);
                ResetPasswordHtmlArgs resetPasswordHtmlArgs = GetNewResetPasswordArgs(newPassword);
                resetPasswordHtmlArgs = IsLogboxEnvironment() ? GetResetPasswordArgsForLogboxEnvironment(newPassword) : resetPasswordHtmlArgs;
                StringBuilder HtmlTemplate = BuildResetPasswordSentSuccessfullyHtmlTemplate(resetPasswordHtmlArgs);
                ResetPassswordEmailCommunicationArgs resetPassswordEmailCommunicationArgs = new ResetPassswordEmailCommunicationArgs
                {
                    FromEmail = resetPasswordHtmlArgs.FromEmail,
                    ToEmail = email,
                    Tenant = tenant,
                    LoggedUserId = contact.Id,
                    HtmlTemplate = HtmlTemplate
                };
                AddPasswordResetEmailCommunicationLog(resetPassswordEmailCommunicationArgs);
                succeeded = true;
            }
            return succeeded;
        }

        private void UpdateContactWithNewHashedPassword(IGlobalContext globalContext, ContactPassword contactPassword, string newHashedPassword)
        {
            contactPassword.Password = newHashedPassword;
            contactPassword.IsLocked = false;
            contactPassword.MustChangePassword = true;
            contactPassword.IsBCrypt = true;
            globalContext.SaveChanges();
        }

        private ResetPasswordHtmlArgs GetNewResetPasswordArgs(string newPassword)
        {
            ResetPasswordHtmlArgs resetPasswordHtmlArgs = new ResetPasswordHtmlArgs
            {
                NewPassword = newPassword,
                TeamName = (LogitudeSettings.WorkEnvironment == "cloud" ? "Amital" : LogitudeSettings.ProductName) + " Team",
                SiteUri = LogitudeSettings.WorkEnvironment == "cloud" ? "https://cloud.amital.co.il/" : ("www." + LogitudeSettings.DomainName),
                SiteLogin = LogitudeSettings.LogitudeURL,
                FromEmail = "no-reply@" + (LogitudeSettings.WorkEnvironment == "cloud" ? "amital.co.il" : "LogitudeWorld.com")
            };

            return resetPasswordHtmlArgs;
        }

        private void AddPasswordResetEmailCommunicationLog(ResetPassswordEmailCommunicationArgs resetPassswordEmailCommunicationArgs)
        {
            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                Subject = "Password reset",
                From = resetPassswordEmailCommunicationArgs.FromEmail,
                To = resetPassswordEmailCommunicationArgs.ToEmail,
                CC = null,
                BCC = null,
                EmailBody = resetPassswordEmailCommunicationArgs.HtmlTemplate.ToString(),
                Tenant = resetPassswordEmailCommunicationArgs.Tenant,
                LoggingUserId = resetPassswordEmailCommunicationArgs.LoggedUserId,
                IsBodySecured = true,
            };
            Communications.AddEmailCommunicationLogQueue(emailParams, resetPassswordEmailCommunicationArgs.Tenant);
        }

        private ResetPasswordHtmlArgs GetResetPasswordArgsForLogboxEnvironment(string newPassword)
        {
            ResetPasswordHtmlArgs resetPasswordHtmlArgs;
            string url = SecurityUtility.getLoggedDomain();
            if (IsPrivateLableUrl(url))
            {
                resetPasswordHtmlArgs = GetResetPasswordArgsForPrivateLable(newPassword, url);
            }
            else
            {
                resetPasswordHtmlArgs = new ResetPasswordHtmlArgs
                {
                    NewPassword = newPassword,
                    TeamName = "LogBox Team",
                    SiteUri = "system.logbox.co.il",
                    SiteLogin = "http://system.logbox.co.il",
                    FromEmail = "no-reply@logbox.co.il"
                };
            }
            return resetPasswordHtmlArgs;
        }

        private ResetPasswordHtmlArgs GetResetPasswordArgsForPrivateLable(string newPassword, string url)
        {
            ResetPasswordHtmlArgs resetPasswordHtmlArgs = new ResetPasswordHtmlArgs();
            TenantManagmentPrivateLabelsQuery tenantManagmentPrivateLabelsQuery = new TenantManagmentPrivateLabelsQuery(0);
            TenantManagmentPrivateLabelsPM privatelabel = tenantManagmentPrivateLabelsQuery.GetSingleActivePMByUrl(url);
            if (privatelabel != null)
            {
                resetPasswordHtmlArgs.NewPassword = newPassword;
                resetPasswordHtmlArgs.TeamName = privatelabel.PrivateLabelShortName + " Team";
                resetPasswordHtmlArgs.SiteUri = privatelabel.PrivateLabelUrl;
                resetPasswordHtmlArgs.SiteLogin = "http://" + privatelabel.PrivateLabelUrl;
                resetPasswordHtmlArgs.FromEmail = "no-reply@" + privatelabel.PrivateLabelDomain;
            }

            return resetPasswordHtmlArgs;
        }

        private StringBuilder BuildResetPasswordSentSuccessfullyHtmlTemplate(ResetPasswordHtmlArgs resetPasswordHtmlArgs)
        {
            StringBuilder HtmlTemplate = new StringBuilder();
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("Hi,");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Your password has been reset successfully.");
            HtmlTemplate.Append("</P>");
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("Your new password is: " + resetPasswordHtmlArgs.NewPassword);
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("To access your account please <a href='" + resetPasswordHtmlArgs.SiteLogin + "'>login</a>");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Thanks,");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append(resetPasswordHtmlArgs.TeamName);
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<a href='http://" + resetPasswordHtmlArgs.SiteUri + "'>" + resetPasswordHtmlArgs.SiteUri + "<a>");
            HtmlTemplate.Append("<br /><span  style='font-size:13px;text-align:left'>Please do not reply directly to this message</span>");
            HtmlTemplate.Append("</P>");
            HtmlTemplate.Append("");

            return HtmlTemplate;
        }

        private bool IsPrivateLableUrl(string url)
        {
            return !url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il");
        }

        private bool IsLogboxEnvironment()
        {
            return LogitudeSettings.DeploymentStage != null && (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2");
        }

        //public Document BuildHtmlDocument(int tenant, string body)
        //{
        //    DocumentRepository documentRep = new DocumentRepository();           
        //    Document document = new Document()
        //    {
        //        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
        //        Extension = "html",
        //        FileSize = Convert.ToInt32(htmlData.Length),
        //        Tenant = Convert.ToInt32(tenant),
        //        Id = IdCounter.GetNumber("Document").ToString(),
        //        Folder = "docsout",
        //    };
        //    documentRep.Add(document);
        //    documentRep.SubmitChanges();
        //    try
        //    {
        //        string filename = document.Id + ".html";
        //        CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
        //        var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));
        //        using (Stream memstream = blobfile.OpenWrite())
        //        {
        //            memstream.Write(htmlData, 0, htmlData.Length);
        //            memstream.Close();
        //            //memstream.Write(htmlData, 0, htmlData.Length);
        //            //blobfile.UploadFromStream(memstream);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return document;
        //}

        [WebMethod]
        public bool ChangeUserPassword(string email, string newPassword)
        {
            PasswordChangeHelper passwordChangeHelper = new PasswordChangeHelper();
            bool succeeded = passwordChangeHelper.ChangePassword(email, newPassword);
           
            return succeeded;
        }

        [WebMethod]
        public bool RequestResetUserPassword(ResetPasswordParameters resetPasswordParameters, string appMobileEnvironment, string tenant = null)
        {
            bool exists = false;

            IGlobalContext globalContext = GlobalContext.GetContext();
            ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == resetPasswordParameters.Email).FirstOrDefault();

            if (contactPassword != null)
            {
                exists = true;

                ResetUserPasswordService resetUserPasswordService = new ResetUserPasswordService();
                resetUserPasswordService.ResetUserPassword(resetPasswordParameters,tenant);
            }

            return exists;
        }
        
        [WebMethod]
        public bool CheckIfUserIsExists(string email, int tenant, ref bool hasPassword, ref bool hasContact)
        {

            bool hasUser = false;

            IGlobalContext globalContext = GlobalContext.GetContext();
            GlobalContact contact = globalContext.GlobalContacts.Where(c => c.GlobalTenantId == tenant && c.Email == email).FirstOrDefault();
            ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == email).FirstOrDefault();

            if (contactPassword != null)
            {
                hasPassword = true;
            }
            if (contact != null)
            {
                hasContact = true;
                hasUser = contact.IsUser;
            }


            return hasUser;
        }


        //[WebMethod]
        //public bool SetAllowInternetAccess(string contactId,string cardId, int tenant)
        //{
        //    bool succeeded = false;
        //    //using (TransactionScope scope = TransactionFactory.GetTransaction())
        //    //{
        //    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
        //    Contact contact = (from c in commonDataContext.Contacts
        //                       where c.Id == contactId && c.Tenant == tenant
        //                       select c).FirstOrDefault();

        //    CardContact cardcontact = (from c in commonDataContext.CardContacts
        //                       where c.ContactId == contactId&& c.CardId == cardId && c.Tenant == tenant
        //                       select c).FirstOrDefault();

        //   // SharedLogisticContactPM
        //    if (contact != null && cardcontact != null)
        //    {
        //        string newPassword = PasswordGenerator.Generate(8);
        //        string newHashedPassword = PasswordGenerator.GetHashedPassword(contact.Email, newPassword);

        //        contact.Password = newHashedPassword;
        //        contact.IsLocked = false;
        //        contact.MustChangePassword = true;

        //        IGlobalContext globalObjectContext = GlobalContext.GetContext();
        //        GlobalContact globalContact = (from g in globalObjectContext.GlobalContacts
        //                                       where g.Id == contactId && g.InActive == false
        //                                       select g).FirstOrDefault();

        //        globalContact.Password = newHashedPassword;
        //        globalContact.InternetAccess = true;
        //        globalObjectContext.SaveChanges();

        //        cardcontact.InternetAccess = true;
        //        commonDataContext.SaveChanges();

        //        Tenant tenantCompany = commonDataContext.Tenants.Where(t => t.Id == tenant).FirstOrDefault();
 
        //        succeeded = true;
        //    }
        //    else
        //    {
        //        succeeded = false;
        //    }
        //    //}
        //    return succeeded;
        //}

        [WebMethod]
        public bool SetUserLastLogin(string password, string contactId, int tenant, string computerId)
        {
            bool isValid = false;

            //ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            //ContactRepository contactRep = new ContactRepository(tenant);

            //Contact contact = ContactRepository.GetSingleContact(contactId, tenant, false);
            IGlobalContext globalContext = GlobalContext.GetContext();
            GlobalContact contact = globalContext.GlobalContacts.Where(c => c.GlobalTenantId == tenant && c.Id == contactId && c.IsUser == true && c.InActive == false).FirstOrDefault();
            if (contact == null)
            {
                contact = globalContext.GlobalContacts.Where(c => c.GlobalTenantId == 0 && c.Id == contactId && c.IsUser == true && c.InActive == false).FirstOrDefault();
            }

            if (contact != null)
            {
               
                ContactPassword contactPassword = AuthenticationUtil.VerifyContactPassword(contact.Email, password, globalContext);

                //IGlobalContext globalObjectContext = GlobalContext.GetContext();
                //IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);
                ////GlobalContact member = globalObjectContext.GlobalContacts.Where(m => m.Email == contact.Email && m.Password == hashedPassword && m.InActive == false && (m.IsUser == true) && m.GlobalTenantId == tenant).FirstOrDefault();// || m.InternetAccess
                ////if (member == null)
                ////{
                ////    member = globalObjectContext.GlobalContacts.Where(m => m.Email == contact.Email && m.Password == password && m.InActive == false && (m.IsUser == true) && m.GlobalTenantId == tenant).FirstOrDefault();// || m.InternetAccess
                ////}

                ////if (member == null)
                ////{
                ////    hashedPassword = PasswordGenerator.GetOldHashedPassword(password);
                ////    member = globalObjectContext.GlobalContacts.Where(m => m.Email == contact.Email && m.Password == hashedPassword && m.InActive == false && (m.IsUser == true) && m.GlobalTenantId == tenant).FirstOrDefault();// || m.InternetAccess
                ////}

                if (contactPassword != null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
                        string userAgent = HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500);
                        //if (user.IsUser)
                        //{
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        UserLoginLog userLog = new UserLoginLog()
                        {
                            Id = IdCounter.GetNumber("UserLoginLog", tenant).ToString(),
                            Tenant = tenant,
                            Browser = HttpContext.Current.Request.Browser.Type,
                            IP = currentIP,
                            UserId = contact.Id,
                            GMTDateTime = DateTime.Now,
                            LocalDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                            UserAgent = userAgent,
                            ComputerId = computerId,
                        };

                        UserLastLogin lastLogin = commonDataContext.UserLastLogins.Where(u => u.Id == contact.Id).FirstOrDefault();
                        if (lastLogin == null)
                        {
                            lastLogin = new UserLastLogin()
                            {
                                Id = contact.Id,

                                Tenant = tenant,
                                LoginDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                            };

                            commonDataContext.UserLastLogins.Add(lastLogin);
                        }

                        lastLogin.ComputerId = computerId;
                        lastLogin.WorkEnvironment = LogitudeSettingConfigration.GetWorkEnvironment();
                        commonDataContext.UserLoginLogs.Add(userLog);
                        commonDataContext.SaveChanges();
                        isValid = true;

                        scope.Complete();
                    }
                }
            }
            else
            {
                throw new Exception("Contact not found!");
            }
            return isValid;
        }
    }
    public class ResetPasswordHtmlArgs
    {
        public string NewPassword { get; set; }
        public string TeamName { get; set; }
        public string SiteUri { get; set; }
        public string SiteLogin { get; set; }
        public string FromEmail { get; set; }
    }

    public class ResetPassswordEmailCommunicationArgs
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public int Tenant { get; set; }
        public string LoggedUserId { get; set; }
        public StringBuilder HtmlTemplate { get; set; }
    }
}
