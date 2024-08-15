using Logitude.SystemLogs;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Global.Data;
using Simplog.Data;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Configuration;

namespace Logitude.Server.Tools.Helpers
{
    public class EmailAlersManager
    {
        public Dictionary<string, string> TableRows { get; set; }
        public StringBuilder HtmlTemplate { get; set; }
        public bool UseThanksText { get; set; }

        public EmailAlersManager()
        {
            this.TableRows = new Dictionary<string, string>();
            this.HtmlTemplate = new StringBuilder();
        }

        public void SendEmailAlert(int tenant, string toEmails, string subject, string loginMessage = null)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(commonContext);

            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetLoggedUserEmail(tenant), tenant);
            if (loggedContact == null)
            {
                loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
            }
            if (toEmails != null)
            {
                string[] recipientEmails = toEmails.Split(';');
                foreach (string toEmail in recipientEmails)
                {
                    if (!string.IsNullOrEmpty(toEmail) && !string.IsNullOrWhiteSpace(toEmail))
                    {
                        Contact contact = contactRepository.GetSingleContactByEmail(toEmail, tenant);
                        string toContactName = contact != null ? " " + contact.EnglishName : "";
                        string From = "no-reply@amital.co.il";

                        if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && LogitudeSettings.DeploymentStage.ToLower() == "simplog")
                        {
                            From = "no-reply@logitudeworld.com";
                        }

                        TenantManagmentPrivateLabels privatelabel = null;
                        string PLURL = null;
                        if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2"))
                        {
                            
                            HttpContext context = HttpContext.Current;
                            string url = context.Request.Url.ToString().Split('/')[2];//("http://", "");

                            var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
                            bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true";

                            if (isAppServiceENV || isAppService)
                            {
                                if (!string.IsNullOrEmpty(context.Request.Headers["X-ORIGINAL-HOST"]))
                                    url = context.Request.Headers["X-ORIGINAL-HOST"];
                            }

                            if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                            {
                                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                                {
                                    TenantManagmentPrivateLabelsRepository query = new TenantManagmentPrivateLabelsRepository();
                                    privatelabel = query.GetSingleTenantManagmentPrivateLabelByURL_Cache(url);
                                    scope.Complete();

                                }
                            }
                            if (privatelabel != null)
                            {
                                From = privatelabel.ContactUsEmail;
                                PLURL = privatelabel.PrivateLabelUrl;
                            }
                            else
                            {
                                From = "no-reply@logbox.co.il";
                            } 
                        }
                        string notificationMail = BuildAlertEmailEnvelope(toContactName, tenant, loginMessage, PLURL);

                        EmailCommunicationParams emailParams = new EmailCommunicationParams()
                        {
                            Subject = subject,
                            From = From,
                            To = toEmail,
                            CC = null,
                            BCC = null,
                            EmailBody = notificationMail,
                            Tenant = tenant,
                            LoggingUserId = loggedContact.Id,
                             
                        };

                        Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
                    }
                }
            }
        }

        public void SendEmailAlertForConfirmedBooking(int tenant, List<string> toEmails, string subject, string loginMessage = null)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(commonContext);

            foreach (string email in toEmails)
            {
                Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);
                string toContactName = contact != null ? " " + contact.EnglishName : "";

                string notificationMail = BuildAlertEmailEnvelope(toContactName, tenant, loginMessage);
                //emailService.InsertCommunicationLog(tenant, contact.Id, contact.Email, TenantServerConfigration.GetCurrentDateTime(tenant), "no-reply@logitudeworld.com", email, subject, notificationMail, true);

                EmailCommunicationParams emailParams = new EmailCommunicationParams()
                {
                    Subject = subject,
                    From = "no-reply@logitudeworld.com",
                    To = email,
                    CC = null,
                    BCC = null,
                    EmailBody = HtmlTemplate.ToString(),
                    Tenant = tenant,
                    LoggingUserId = contact.Id,
                     
                };

                Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
            }
        }

        private string BuildAlertEmailEnvelope(string recipientName, int tenant, string loginMessage,string URL = null)
        {
            StringBuilder EnvelopeHtmlTemplate = new StringBuilder();

            EnvelopeHtmlTemplate.Append(//font-size:11px;
                "<p style='border-style:solid;border-radius:7px;border-color:#385D8A;background-color:#4F81BD;font-family:Century;text-align:center;color:white;vertical-align: middle;padding:5px'>"
                + "Automatic e<span style='font-family:Arial'>-</span>mail Notification"
                + "</p>"
               );

            EnvelopeHtmlTemplate.Append("<div style='text-align:left;font-family:Century;'>");//font-size:11px;
            EnvelopeHtmlTemplate.Append("<p style='text-align:left'>");

            EnvelopeHtmlTemplate.Append("Hello" + recipientName + ",");

            if (UseThanksText)
            {
                EnvelopeHtmlTemplate.Append("<p style='font-size:16px; font-weight:bold;'>Thank you for using our service</p>");
            }

            EnvelopeHtmlTemplate.Append("<br /><br />");

            EnvelopeHtmlTemplate.Append(this.HtmlTemplate.ToString());
            EnvelopeHtmlTemplate.Append(BuildTableRows());
            EnvelopeHtmlTemplate.Append("<br /><br />");

            if (string.IsNullOrEmpty(loginMessage))
            {
                EnvelopeHtmlTemplate.Append("For more details please <a href='" + (!string.IsNullOrEmpty(URL) ? URL : LogitudeSettings.LogitudeURL) + "'>login</a>");
            }
            else
            {
                EnvelopeHtmlTemplate.Append(loginMessage.Replace("login", "<a href='" + LogitudeSettings.LogitudeURL + "'>login</a>"));
            }

            EnvelopeHtmlTemplate.Append("</p>");
            EnvelopeHtmlTemplate.Append("</div>");
            EnvelopeHtmlTemplate.Append("<br/>");
            EnvelopeHtmlTemplate.Append("<div style='vertical-align:top;display:table;text-align:center'>");
            if (!string.IsNullOrEmpty(LogitudeSettings.EmailAlertSignature))
            {
                TenantManagmentPrivateLabels privatelabel = null;
                string PLSign = LogitudeSettings.EmailAlertSignature;
                if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2"))
                {

                    HttpContext context = HttpContext.Current;
                    string url = context.Request.Url.ToString().Split('/')[2];//("http://", "");
                                                                              //var url = SecurityUtility.getLoggedDomain();

                    var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
                    bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true";

                    if (isAppServiceENV || isAppService)
                    {
                        if (!string.IsNullOrEmpty(context.Request.Headers["X-ORIGINAL-HOST"]))
                            url = context.Request.Headers["X-ORIGINAL-HOST"];
                    }

                    if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                        {
                            TenantManagmentPrivateLabelsRepository query = new TenantManagmentPrivateLabelsRepository();
                            privatelabel = query.GetSingleTenantManagmentPrivateLabelByURL_Cache(url);
                            scope.Complete();
                        }
                    }
                    if (privatelabel != null)
                    {
                        PLSign = LogitudeSettings.EmailAlertSignature.Replace("LogBox", privatelabel.PrivateLabelShortName);
                       
                    }
                }
                EnvelopeHtmlTemplate.Append(PLSign);
            }
            else
            {
                if (LogitudeSettings.WorkEnvironment == "cloud")
                {
                    EnvelopeHtmlTemplate.Append("Created By <b>Unifreight Cloud Services</b>");
                }
                else
                {
                    EnvelopeHtmlTemplate.Append("Created By <b>Logitude World</b>");
                }

                //EnvelopeHtmlTemplate.Append("<img  src='cid:logo" + tenant + "' />");//width='258' height='101'
                EnvelopeHtmlTemplate.Append("</div>");
            }

            return EnvelopeHtmlTemplate.ToString();
        }

        private string BuildTableRows()
        {
            StringBuilder tableBuilder = new StringBuilder();
            if (TableRows.Keys.Count > 0)
            {
                tableBuilder.Append("<table style='border:none'>");
                foreach (string label in TableRows.Keys)
                {
                    tableBuilder.Append("<tr>");
                    tableBuilder.Append("<td style='width:150px'>");
                    tableBuilder.Append(label);
                    tableBuilder.Append("</td>");

                    tableBuilder.Append("<td>");
                    tableBuilder.Append(": " + TableRows[label]);
                    tableBuilder.Append("</td>");

                    tableBuilder.Append("</tr>");

                }

                tableBuilder.Append("</table>");
            }

            return tableBuilder.ToString();

        }
    }
}
