using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class SharedLogisticContactHelper
    {
        string ActivityDescription;

        public void InternetAccessInvitation(SharedLogisticContactPM sharedLogisticsContact, ICommonDataContext objectContext)
        {

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(sharedLogisticsContact.Tenant);
            }

            Contact contact = (from c in objectContext.Contacts
                               where c.Id == sharedLogisticsContact.ContactId && c.Tenant == sharedLogisticsContact.Tenant
                               select c).FirstOrDefault();

            CardContact cardContact = (from c in objectContext.CardContacts
                                       where c.ContactId == sharedLogisticsContact.ContactId && c.CardId == sharedLogisticsContact.CardId && c.Tenant == sharedLogisticsContact.Tenant
                                       select c).FirstOrDefault();

            Card card = (from c in objectContext.Cards
                         where c.Id == sharedLogisticsContact.CardId && c.Tenant == sharedLogisticsContact.Tenant
                         select c).FirstOrDefault();


            Contact logedContact = (from c in objectContext.Contacts
                                    where c.Email == HttpContext.Current.User.Identity.Name && c.Tenant == sharedLogisticsContact.Tenant
                                    select c).FirstOrDefault(); //

            if (logedContact == null)
            {
                logedContact = (from c in objectContext.Contacts
                                where c.Email == HttpContext.Current.User.Identity.Name && c.Tenant == 0
                                select c).FirstOrDefault(); //
            }

            string currentUsername = "";
            if (LogitudeSettings.WorkEnvironment == "cloud")
            {
                ContactRepository contactRepository = new ContactRepository(sharedLogisticsContact.Tenant);
                Contact currentUserContact = contactRepository.GetSingleContactByEmailAndTenant(HttpContext.Current.User.Identity.Name, sharedLogisticsContact.Tenant);
                if (currentUserContact != null) currentUsername = currentUserContact.EnglishName;
            }

            MessageArgs messageArgs = new MessageArgs();

            cardContact.InternetAccess = sharedLogisticsContact.InternetAccess;
            string newHashedPassword = null;

            if (sharedLogisticsContact.InternetAccess)
            {
                if (card.SharedLogisticsInvitationStatusCode != 3)
                {
                    card.SharedLogisticsInvitationStatusCode = 2;
                    card.InvitationDate = DateTime.Now;
                }

                string emailMessage = "";

                Tenant tenantCompany = objectContext.Tenants.Where(t => t.Id == sharedLogisticsContact.Tenant).FirstOrDefault();




                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {

                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == contact.Email).FirstOrDefault();

                    GlobalContactRepository globalContactRep = new GlobalContactRepository();
                    GlobalContact globalContact = globalContactRep.GetSingleGlobalContact(contact.Id);

                    if (globalContact != null)
                    {

                        globalContact.InternetAccess = sharedLogisticsContact.InternetAccess;
                        globalContactRep.Update(globalContact);
                    }
                    else
                    {
                        globalContact = new GlobalContact() { Email = contact.Email, Id = contact.Id, GlobalTenantId = contact.Tenant, InternetAccess = sharedLogisticsContact.InternetAccess };
                        globalContactRep.Add(globalContact);

                    }

                    globalContactRep.SubmitChanges();




                    // Area  contactPassword null


                    if (contactPassword == null)
                    {
                        string newPassword = PasswordGenerator.Generate(8);
                        newHashedPassword = PasswordGenerator.GetBCryptHashedPassword(contact.Email, newPassword);
                        contactPassword = new ContactPassword()
                        {
                            Email = contact.Email,
                            Password = newHashedPassword,
                            IsLocked = false,
                            NumberOfRetries = 0,
                            MustChangePassword = true,
                            IsBCrypt = true,

                        };

                        globalContext.ContactPasswords.Add(contactPassword);
                        globalContext.SaveChanges();

                        if ((tenantCompany.IsWebAccessActivated || tenantCompany.IsCargoTrackWebAccessActivated) && !tenantCompany.IsMobileActivated)
                        {
                            string password = newPassword + "  (you will need to change the password on your first login)";

                            emailMessage = GetEmailMessageForShardLogistics(contact, logedContact, tenantCompany, password, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            ActivityDescription = "Web Access Activated";
                        }

                        else if ((!tenantCompany.IsWebAccessActivated || !tenantCompany.IsCargoTrackWebAccessActivated) && tenantCompany.IsMobileActivated)
                        {

                            if (LogitudeSettings.WorkEnvironment == "cloud")
                            {
                                emailMessage = GetEmailMessageForCloud(contact, tenantCompany, newPassword, currentUsername, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }

                            else
                            {
                                string password = newPassword + "  (you will need to change the password on your first login)";
                                emailMessage = GetEmailMessageFroMobile(contact, logedContact, tenantCompany, password, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }
                            ActivityDescription = "Mobile Activated";
                        }

                        else if ((tenantCompany.IsWebAccessActivated || tenantCompany.IsCargoTrackWebAccessActivated) && tenantCompany.IsMobileActivated)
                        {
                            ActivityDescription = "Mobile And Shared Logistics Activated";
                            if (LogitudeSettings.WorkEnvironment == "cloud")
                            {
                                emailMessage = GetEmailMessageForCloud(contact, tenantCompany, newPassword, currentUsername, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }

                            else
                            {

                                string password = newPassword + "  (you will need to change the password on your first login)";
                                emailMessage = GetEmailMessageFroShardLogisticsAndMobile(contact, logedContact, tenantCompany, password, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }
                        }

                    }


                    /////////  Area  contactPassword not  null

                    else
                    {
                        string passwordString = "";

                        if (contactPassword.Password == "123")
                        {
                            string newPassword = PasswordGenerator.Generate(8);
                            newHashedPassword = PasswordGenerator.GetBCryptHashedPassword(contact.Email, newPassword);
                            contactPassword.Password = newHashedPassword;
                            contactPassword.MustChangePassword = true;
                            contactPassword.IsBCrypt = true;
                            globalContext.SaveChanges();

                            if (LogitudeSettings.WorkEnvironment == "cloud") passwordString = newPassword;

                            else passwordString = newPassword + "  (you will need to change the password on your first login)";

                        }
                        else
                        {
                            if (LogitudeSettings.WorkEnvironment == "cloud")
                            {
                                passwordString = passwordString = "הסיסמה הנוכחית שלך";

                            }

                            else passwordString = "your current password (If you lost your password press on forgot password link in the login page and fill your e-mail address to receive new password)";



                        }


                        if ((tenantCompany.IsWebAccessActivated || tenantCompany.IsCargoTrackWebAccessActivated) && !tenantCompany.IsMobileActivated)
                        {

                            emailMessage = emailMessage = GetEmailMessageForShardLogistics(contact, logedContact, tenantCompany, passwordString, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            ActivityDescription = "Web Access Activated";
                        }


                        else if ((!tenantCompany.IsWebAccessActivated || !tenantCompany.IsCargoTrackWebAccessActivated) && tenantCompany.IsMobileActivated)
                        {
                            if (LogitudeSettings.WorkEnvironment == "cloud")
                            {
                                emailMessage = GetEmailMessageForCloud(contact, tenantCompany, passwordString, currentUsername, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }
                            else
                            {

                                emailMessage = GetEmailMessageFroMobile(contact, logedContact, tenantCompany, passwordString, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }


                            ActivityDescription = "Mobile Activated";
                        }

                        else if ((tenantCompany.IsWebAccessActivated || tenantCompany.IsCargoTrackWebAccessActivated) && tenantCompany.IsMobileActivated)
                        {

                            if (LogitudeSettings.WorkEnvironment == "cloud")
                            {
                                emailMessage = GetEmailMessageForCloud(contact, tenantCompany, passwordString, currentUsername, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }
                            else
                            {

                                emailMessage = GetEmailMessageFroShardLogisticsAndMobile(contact, logedContact, tenantCompany, passwordString, ref messageArgs, sharedLogisticsContact.IsCargoTrackingInvitation);
                            }


                            ActivityDescription = "Mobile And Shared Logistics Activated";
                        }
                    }

                    contactPassword.IsSendNotificationForMobile = true;


                    ChangePasswordlogRepository changePasswordlogRepository = new ChangePasswordlogRepository();
                    ChangePasswordLog changePasswordLog = new ChangePasswordLog()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CurrentPassword = contactPassword.Password,
                        EnteredPassword = contactPassword.Password,
                        Email = contact.Email,
                        CreateDate = DateTime.UtcNow,
                        log = "(Invitation ) Email has been sent ",
                    };

                    changePasswordlogRepository.Add(changePasswordLog);
                    changePasswordlogRepository.SubmitChanges();



                    globalContext.SaveChanges();
                    scope.Complete();
                }

                if (tenantCompany.IsWebAccessActivated || tenantCompany.IsCargoTrackWebAccessActivated || tenantCompany.IsMobileActivated)
                {
                    string from = GetEmailFrom(sharedLogisticsContact.Tenant);

                    string subject = GetInvitationSubject(tenantCompany, sharedLogisticsContact);
                    if (!string.IsNullOrEmpty(messageArgs.HtmlTemplate))
                    {

                        subject = !string.IsNullOrEmpty(messageArgs.Subject) ? messageArgs.Subject : subject;
                        from = !string.IsNullOrEmpty(messageArgs.From) ? messageArgs.From : from;
                    }


                    EmailCommunicationParams emailParams = new EmailCommunicationParams()
                    {
                        Subject = subject,
                        From = from,
                        To = contact.Email,
                        CC = messageArgs.CC,
                        BCC = messageArgs.BCC,
                        EmailBody = emailMessage,
                        Tenant = sharedLogisticsContact.Tenant,
                        LoggingUserId = contact.Id,
                        IsBodySecured = true,

                    };
                    Communications.AddEmailCommunicationLogQueue(emailParams, sharedLogisticsContact.Tenant);

                    ActivityLog.SendTotangoContactActivity(contact.Email, "Contact", ActivityDescription, sharedLogisticsContact.Tenant, false, null, "PC");


                }
            }
            else
            {
                List<CardContact> CardContacts = objectContext.CardContacts.Where(t => t.Tenant == sharedLogisticsContact.Tenant).ToList();
                if (!CardContacts.Where(d => d.InternetAccess).Any())
                {
                    card.SharedLogisticsInvitationStatusCode = 1;
                    card.InvitationDate = null;

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        GlobalContactRepository globalContactRep = new GlobalContactRepository();
                        GlobalContact globalContact = globalContactRep.GetSingleGlobalContact(contact.Id);

                        globalContact.InternetAccess = sharedLogisticsContact.InternetAccess;

                        globalContactRep.Update(globalContact);
                        globalContactRep.SubmitChanges();
                        scope.Complete();
                    }

                }
            }

            objectContext.SaveChanges();
        }

        private string GetInvitationSubject(Tenant tenantCompany, SharedLogisticContactPM sharedLogisticsContact)
        {
            if (sharedLogisticsContact.IsCargoTrackingInvitation)
            {
                return tenantCompany.Company + " Cargo Tracking Invitation";
            }

            if (tenantCompany.IsWebAccessActivated && !tenantCompany.IsMobileActivated)
            {
                return tenantCompany.Company + " invites you to “Shared Logistics” with Logitude";
            }

            if (!tenantCompany.IsWebAccessActivated && tenantCompany.IsMobileActivated)
            {
                return LogitudeSettings.WorkEnvironment == "cloud" ? "הזמנה ל-Unifreight Mobile" : tenantCompany.Company + " invites you to “Logitude Mobile";
            }

            if (tenantCompany.IsWebAccessActivated && tenantCompany.IsMobileActivated)
            {
                return LogitudeSettings.WorkEnvironment == "cloud" ? "הזמנה ל-Unifreight Mobile" : tenantCompany.Company + " invites you to “Shared Logistics and Logitude Mobile” with Logitude.";
            }

            return "";
        }

        private string GetEmailFrom(int tenant)
        {
            string systemUrl = GetSystemURL(tenant);
            if (!string.IsNullOrEmpty(systemUrl))
                return "no-reply@" + systemUrl;

            return LogitudeSettings.WorkEnvironment == "cloud" ? "no-reply@amital.co.il" : "no-reply@LogitudeWorld.com";
        }

        private string GetSystemURL(int tenant)
        {
            string systemUrl = "";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetTenantManagementPM(tenant);
                if (tenantManagementPM != null)
                {
                    systemUrl = tenantManagementPM.CustomerURL;
                }
                scope.Complete();
            }
            return GetOnlyDomainNameFromSystemUrl(systemUrl);
        }

        private string GetOnlyDomainNameFromSystemUrl(string systemUrl)
        {
            if (string.IsNullOrEmpty(systemUrl)) return null;
            return systemUrl.Split('/')[0];
        }

        private static string ResolveInvitationvariable(string htmlTemplate, Contact contact, string password)
        {
            if (contact != null)
            {
                htmlTemplate = htmlTemplate.Replace("[InvitationEmail]", contact.Email);
                htmlTemplate = htmlTemplate.Replace("[InviteeName]", contact.EnglishName);
            }
            else
            {
                htmlTemplate = htmlTemplate.Replace("[InvitationEmail]", "");
                htmlTemplate = htmlTemplate.Replace("[InviteeName]", "");
            }

            htmlTemplate = htmlTemplate.Replace("[InvitationPassword]", password);

            return htmlTemplate;
        }


        private static string GetEmailMessageFroShardLogisticsAndMobile(Contact contact, Contact logedContact, Tenant tenantCompany, string password, ref MessageArgs messageArgs, bool isCargoTrackingInvitation)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var documenttype = GetDocumentTypeForInvitation(tenantCompany.Id, isCargoTrackingInvitation);
                if (documenttype != null)
                {
                    messageArgs = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
                    if (!string.IsNullOrEmpty(messageArgs.HtmlTemplate))
                    {
                        messageArgs.HtmlTemplate = ResolveInvitationvariable(messageArgs.HtmlTemplate, contact, password);
                        return messageArgs.HtmlTemplate;
                    }

                }


                StringBuilder HtmlTemplate = new StringBuilder();
                string emailMessage = "";
                string logo = GetLogoInvitation(tenantCompany.Id, "Logitude");
                HtmlTemplate.Append("<!DOCTYPE html>");
                HtmlTemplate.Append("<p style='text-align:left'>");
                HtmlTemplate.Append("Hello " + contact.EnglishName + ",");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append(logedContact.EnglishName + " from " + tenantCompany.Company + " is sending you this invitation to connect to their operation system to check on your shipments.");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Please use the following:");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Address: " + LogitudeSettings.LogitudeURL);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<b>Username: </b>" + contact.Email);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<b>Password: </b>" + password);

                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Logitude is the first true online Freight Forwarding software solution developed specifically for the cloud. Working in the cloud means you can access Logitude anytime, from anywhere , whether you are in your office, at home, or traveling. By using Logitude you can be updated online on your shipments, statuses, documents and more");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("<table style='width:100%;height:50px; background-color:#EEEEEE'><tbody ><tr><td><p style='text-align:center;font-size:20px;font-weight:bold;padding:5px'>  Get the app </p></td></tr><tr> <td align='center'>" + "<a  href='" + LogitudeSettings.IOSAppLink + "'> <img  width='120' height='40' src='cid:AppleStore' /></a>" + "&nbsp" + "<a  href='" + LogitudeSettings.AndroidAppLink + "'> <img  width='120' height='40' src='cid:GooglePlay' /></a></td></tr></tbody></table>");

                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("For more information please visit us at <a href='http://www.logitudeworld.com'>www.logitudeworld.com<a>");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("<img width='290' height='101' src='cid:" + logo + "' />");
                emailMessage = HtmlTemplate.ToString();

                scope.Complete();

                return emailMessage;
            }
        }

        private static string GetEmailMessageFroMobile(Contact contact, Contact logedContact, Tenant tenantCompany, string password, ref MessageArgs messageArgs, bool isCargoTrackingInvitation)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var documenttype = GetDocumentTypeForInvitation(tenantCompany.Id, isCargoTrackingInvitation);
                if (documenttype != null)
                {
                    messageArgs = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
                    if (!string.IsNullOrEmpty(messageArgs.HtmlTemplate))
                    {
                        messageArgs.HtmlTemplate = ResolveInvitationvariable(messageArgs.HtmlTemplate, contact, password);
                        return messageArgs.HtmlTemplate;
                    }


                }

                string logo = GetLogoInvitation(tenantCompany.Id, "Logitude");

                StringBuilder HtmlTemplate = new StringBuilder();
                string emailMessage = "";
                HtmlTemplate.Append("<!DOCTYPE html>");
                HtmlTemplate.Append("<p style='text-align:left'>");
                HtmlTemplate.Append("Hello " + contact.EnglishName + ",");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append(logedContact.EnglishName + " from " + tenantCompany.Company + " is sending you this invitation to use the new Mobile Application to track your shipments.");
                HtmlTemplate.Append("<br />");

                HtmlTemplate.Append("Please use the following:");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Address: " + LogitudeSettings.LogitudeURL);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<b>Username: </b>" + contact.Email);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<b>Password: </b>" + password);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("By Using Logitude Mobile, you can stay up-to-date online with your shipments, statuses and more");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("<table style='width:100%;height:50px; background-color:#EEEEEE'><tbody ><tr><td><p style='text-align:center;font-size:20px;font-weight:bold;padding:5px'>  Get the app </p></td></tr><tr> <td align='center'>" + "<a  href='" + LogitudeSettings.IOSAppLink + "'> <img  width='120' height='40' src='cid:AppleStore' /></a>" + "&nbsp" + "<a  href='" + LogitudeSettings.AndroidAppLink + "'> <img  width='120' height='40' src='cid:GooglePlay' /></a></td></tr></tbody></table>");

                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("For more information please visit us at <a href='http://www.logitudeworld.com'>www.logitudeworld.com<a>");

                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("<img width='290' height='101' src='cid:" + logo + "' />");

                emailMessage = HtmlTemplate.ToString();
                scope.Complete();

                return emailMessage;
            }

        }

        private static string GetEmailMessageForShardLogistics(Contact contact, Contact logedContact, Tenant tenantCompany, string password, ref MessageArgs messageArgs, bool isCargoTrackingInvitation)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var documenttype = GetDocumentTypeForInvitation(tenantCompany.Id, isCargoTrackingInvitation);
                if (documenttype != null)
                {
                    messageArgs = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
                    if (!string.IsNullOrEmpty(messageArgs.HtmlTemplate))
                    {

                        messageArgs.HtmlTemplate = ResolveInvitationvariable(messageArgs.HtmlTemplate, contact, password);
                        return messageArgs.HtmlTemplate;
                    }


                }


                string logo = GetLogoInvitation(tenantCompany.Id, "Logitude");
                string emailMessage = "";
                StringBuilder HtmlTemplate = new StringBuilder();
                HtmlTemplate.Append("<p style='text-align:left'>");
                HtmlTemplate.Append("Hello " + contact.EnglishName + ",");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append(logedContact.EnglishName + " from " + tenantCompany.Company + " is sending you this invitation to connect to their operation system to check on your shipments ");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("To view your shipments online, Please use the following:");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Address: " + LogitudeSettings.LogitudeURL);//System.Configuration.ConfigurationManager.AppSettings.Get("LogitudeURL"));
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<b>Username: </b>" + contact.Email);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<b>Password: </b>" + password);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Logitude is the first true online Freight Forwarding software solution developed specifically for the cloud. Working in the cloud means you can access Logitude anytime, from anywhere , whether you are in your office, at home, or traveling. By using Logitude you can be updated on-line on your shipments, statuses, documents and more");


                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("For more information please visit us at <a href='http://www.logitudeworld.com'>www.logitudeworld.com<a>");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("<img width='290' height='101' src='cid:" + logo + "' />");
                // HtmlTemplate.Append("<img width='258' height='101' src='cid:logo0' />");
                emailMessage = HtmlTemplate.ToString();

                scope.Complete();

                return emailMessage;
            }
        }

        private static string GetEmailMessageForCloud(Contact contact, Tenant tenantCompany, string password, string currentUsername, ref MessageArgs messageArgs, bool isCargoTrackingInvitation)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var documenttype = GetDocumentTypeForInvitation(tenantCompany.Id, isCargoTrackingInvitation);
                if (documenttype != null)
                {
                    messageArgs = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
                    if (!string.IsNullOrEmpty(messageArgs.HtmlTemplate))
                    {
                        messageArgs.HtmlTemplate = ResolveInvitationvariable(messageArgs.HtmlTemplate, contact, password);
                        return messageArgs.HtmlTemplate;
                    }


                }
                string logo = GetLogoInvitation(tenantCompany.Id, "Cloud");
                StringBuilder HtmlTemplate = new StringBuilder();
                string message = "";
                HtmlTemplate.Append("<!DOCTYPE html>");

                HtmlTemplate.Append("<div  dir='rtl'  style='text-align:right;  font-size: 15px ; font-family:Arial'>");


                if (logo == "AppMobileLogo")
                {
                    HtmlTemplate.Append(" <img  align='button' valign='button' width='50' height='50' src='cid:" + logo + "' />");
                }
                else
                {
                    HtmlTemplate.Append(" <img  align='button' valign='button' width='auto' height='auto' src='cid:" + logo + "' />");
                }

                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("לכבוד: " + contact.EnglishName);
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("מאת : " + "<span dir='ltr'>" + currentUsername + " / " + tenantCompany.Company + "</span>");


                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("<div>");
                HtmlTemplate.Append("<b style ='color :#1D66F0;font-size :16px;width:60% ;float:left; dir='ltr' '>Invitation to use Unifreight Mobile</b>");
                HtmlTemplate.Append("<b style ='color :#1D66F0;font-size :16px;width:40%;float:left;'>הזמנה להתחבר ל Unifreight Mobile</b>");

                HtmlTemplate.Append("</div>");



                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("Unifreight Mobile הינה אפליקציית מידע לטלפונים חכמים (אייפון ואנדרואיד), שפותחה במטרה להזרים אליכם מידע שוטף על המשלוחים שלכם ולאפשר לכם לעקוב אחריהם בכל עת ובכל מקום.האפליקציה קלה לשימוש ומאפשרת שליטה בהתרעות (ללא התרעות בכלל , התרעות רק למשלוחים מועדפים)");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("האפליקצייה זמינה להורדה חינם ב Apple Store וב Google Play");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("להלן שם המשתמש והסיסמא שלך לכניסה למערכת");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("שם משתמש: " + contact.Email);
                HtmlTemplate.Append("<br />");
                var Password = password;
                if (password != "הסיסמה הנוכחית שלך")
                {
                    Password = "<span  dir='ltr'>" + password + "</span>";
                }

                HtmlTemplate.Append("סיסמה: " + Password);

                if (password == "הסיסמה הנוכחית שלך")
                {
                    HtmlTemplate.Append("<br /><br />");
                    HtmlTemplate.Append("(אם אינך זוכר/ת את הסיסמא הנוכחית שלך , אנא השתמש ב forgot password בכדי לבצע החלפת סיסמא)");


                }


                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("<b style ='color :#FAA61A;font-size :16px'>הורידו עכשיו והישארו מקוונים ומעודכנים</b>");

                HtmlTemplate.Append("</div>");


                HtmlTemplate.Append("<br /><br />");
                // English

                HtmlTemplate.Append("<div  dir='ltr'  style='text-align:left;  font-size: 15px ; font-family:Arial'>");

                HtmlTemplate.Append(currentUsername + " from / " + tenantCompany.Company + " is sending you this invitation to use the new Mobile Application to track your shipments.");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("Please use the following:");
                HtmlTemplate.Append("<br /><br />");

                HtmlTemplate.Append("<b>Username: </b>" + contact.Email);
                HtmlTemplate.Append("<br />");

                var englishPassword = password;

                if (password == "הסיסמה הנוכחית שלך")
                {
                    englishPassword = "your current password (If you lost your password press on forgot password link in the login page and fill your e-mail address to receive new password)";
                }

                HtmlTemplate.Append("<b>Password: </b>" + englishPassword);

                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("if you lost your password press on forgot password link in the login page and fill your e-mail address to receive new password");

                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<br />");

                HtmlTemplate.Append("<b style ='color :#FAA61A;font-size :16px'>With Unifreight mobile you can access your shipments with real time information and status, as well as receive push notifications on all statuses.</b>");
                HtmlTemplate.Append("</div>");

                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("<table style='width:100%;height:50px; background-color:#EEEEEE'><tbody ><tr><td><p style='text-align:center;font-size:20px;font-weight:bold;padding:5px'>  Get the app </p></td></tr><tr> <td align='center'>" + "<a  href='" + LogitudeSettings.IOSAppLink + "'> <img  width='120' height='40' src='cid:AppleStore' /></a>" + "&nbsp" + "<a  href='" + LogitudeSettings.AndroidAppLink + "'> <img  width='120' height='40' src='cid:GooglePlay' /></a></td></tr></tbody></table>");

                message = HtmlTemplate.ToString();

                if (message.Contains("{"))
                {
                    message.Replace("}", "");
                    message.Replace("{", "");
                }

                scope.Complete();

                return message;
            }
        }

        private static string GetLogoInvitation(int tenant, string workEnvironment)
        {
            string filename = "logo" + tenant.ToString();


            if (workEnvironment == "cloud") filename = "smalllogo" + tenant.ToString();


            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = filename,
                FolderName = "logos",
                Extension = "jpg",
                Tenant = tenant,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] datainByte = storageservice.Read(fileInfo);
            if (datainByte == null)
            {
                filename = "AppMobileLogo";
                if (workEnvironment == "Logitude") filename = "logo0";
            }
            return filename;


        }



        private static DocumentType GetDocumentTypeForInvitation(int tenant, bool isCargoTrackingInvitation)
        {
            DocumentType documentType = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                if (isCargoTrackingInvitation)
                {
                    documentType = documentTypeRepository.GetDocumentTypeByCode("CTIM", tenant);
                }
                if (documentType == null)
                {
                    documentType = documentTypeRepository.GetDocumentTypeByCode("SLCIN", tenant);
                }
                scope.Complete();

            }
            if (documentType != null && !string.IsNullOrEmpty(documentType.DocumentTypeDefaultHTMLTemplateId)) return documentType;
            else return null;

        }






    }
}