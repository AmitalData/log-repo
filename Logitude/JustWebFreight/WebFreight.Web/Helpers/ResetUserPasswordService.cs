using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class ResetUserPasswordService
    {
        IGlobalContext globalContext;
        public ResetUserPasswordService()
        {
            globalContext = GlobalContext.GetContext();
        }

        private TenantManagementPM tenantManagementPM { get; set; }
        public void ResetUserPassword(ResetPasswordParameters resetPasswordParameters, string brandingTenant)
        {
            //string newPassword = PasswordGenerator.GetBCryptHashedPassword(resetPasswordParameters.Email, PasswordGenerator.Generate(8));
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(0);
            if (!string.IsNullOrEmpty(resetPasswordParameters.BrandingTenant))
                tenantManagementPM = tenantManagementQuery.GetSinglePM(Int32.Parse(resetPasswordParameters.BrandingTenant));

            string reqNumber = GetResetRequestNumber();
            EmailMessageParams emailMessageParams = GetEmailMessageParams();
            TenantManagmentPrivateLabelsPM privatelabel = null;

            string LogitudeURL = LogitudeSettings.LogitudeURL;
            string path = GetFogotPasswordPagePath(resetPasswordParameters, LogitudeURL, reqNumber);           

            if (LogitudeSettings.DeploymentStage != null && 
                (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2")
                && !IsCargoTrackingDomain())
            {
                privatelabel = GetPrivateLabelByLoggedDomain();
                BuildEmailMessageParams(emailMessageParams, privatelabel);
                if (privatelabel != null)
                {
                    path = GetFogotPasswordPagePath(resetPasswordParameters, emailMessageParams.SiteUri, reqNumber);
                }
            }
            EmailBodyArgs emailBodyArgs = new EmailBodyArgs
            {
                PagePath = path,
                ReqestNumber = reqNumber,
                BrandingTenant = brandingTenant,
            };
            EmailBodyResults emailBodyResults = BuildEmailBody(resetPasswordParameters, emailMessageParams, emailBodyArgs);
            EmailCommunicationLogBuilderArgs emailCommunicationLogBuilderArgs = new EmailCommunicationLogBuilderArgs
            {
                ResetPasswordParameters = resetPasswordParameters,
                Result = emailBodyResults.Result,
                AppMobileEnvironment = resetPasswordParameters.AppEnvironment,
            };
            CreateEmailCommunicationLog(emailBodyResults.HtmlTemplate, emailCommunicationLogBuilderArgs, privatelabel);
        }

        private string GetResetRequestNumber()
        {
            Random random = new Random();
            string randomNumber = random.Next().ToString().Substring(0, 7);
            string reqNumber = Guid.NewGuid().ToString("N") + randomNumber;
            return reqNumber;
        }

        private TenantManagmentPrivateLabelsPM GetPrivateLabelByLoggedDomain()
        {
            TenantManagmentPrivateLabelsPM privatelabel = null;
            var url = SecurityUtility.getLoggedDomain();
            if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
            {
                TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
                privatelabel = query.GetSingleActivePMByUrl(url);
            }

            return privatelabel;
        }

        private void BuildEmailMessageParams(EmailMessageParams emailMessageParams, TenantManagmentPrivateLabelsPM privatelabel)
        {
            if (privatelabel != null)
            {
                emailMessageParams.TeamName = privatelabel.PrivateLabelShortName + " Team";
                emailMessageParams.SiteUri = privatelabel.PrivateLabelUrl;
                emailMessageParams.SenderEmail = "no-reply@" + privatelabel.PrivateLabelDomain;
                emailMessageParams.Environment = privatelabel.PrivateLabelShortName;
                emailMessageParams.IsLogBox = true;
            }
            else
            {
                emailMessageParams.TeamName = "LogBox Team";
                emailMessageParams.SiteUri = "system.logbox.co.il";
                emailMessageParams.SenderEmail = "no-reply@logbox.co.il";
                emailMessageParams.Environment = "Logbox";
                emailMessageParams.IsLogBox = true;
            }
        }

        private EmailMessageParams GetEmailMessageParams()
        {
            string teamName = (LogitudeSettings.WorkEnvironment == "cloud" ? "Amital" : LogitudeSettings.ProductName) + " Team";
            string siteUri = LogitudeSettings.WorkEnvironment == "cloud" ? "https://cloud.amital.co.il/" : ("www." + LogitudeSettings.DomainName);
            string environment = LogitudeSettings.WorkEnvironment == "cloud" ? "Amital Cloud" : "Logitude";
            string senderEmail = "no-reply@" + LogitudeSettings.DomainName;

            if (IsCargoTrackingDomain())
            {
                return new EmailMessageParams
                {
                    Environment = "Cargo Tracking",
                    SiteUri = tenantManagementPM != null ? tenantManagementPM.CustomerURL : "",
                    TeamName = "Cargo Tracking Team",
                    SenderEmail = "no-reply@" + SecurityUtility.getLoggedDomain(),
                    TenantName = tenantManagementPM != null ? tenantManagementPM.Name : "",
                    IsCargoTracking = true,
                };

            }
            else return new EmailMessageParams
            {
                Environment = environment,
                SiteUri = siteUri,
                TeamName = teamName,
                SenderEmail = senderEmail,
            };


        }

        private EmailBodyResults BuildEmailBody(ResetPasswordParameters resetPasswordParameters, EmailMessageParams emailMessageParams, EmailBodyArgs emailBodyArgs)
        {
            PasswordResetRequest resetRequest = new PasswordResetRequest
            {
                RequestNumber = emailBodyArgs.ReqestNumber,
                Email = resetPasswordParameters.Email
            };

            if (resetPasswordParameters.IsMobile)
                resetRequest = GetPasswordResetRequestForMobile(resetPasswordParameters.Email, emailBodyArgs.ReqestNumber);

            globalContext.PasswordResetRequests.Add(resetRequest);
            globalContext.SaveChanges();

            MessageArgs result = new MessageArgs();
            StringBuilder HtmlTemplate = new StringBuilder();
            if (!resetPasswordParameters.IsMobile)
            {
                bool IsLoadingTemplate = false;
                if (!string.IsNullOrEmpty(emailBodyArgs.BrandingTenant))
                {
                    var documenttype = GetDocumentTypeForResetPassword(Int32.Parse(emailBodyArgs.BrandingTenant));
                    if (documenttype != null)
                    {
                        result = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
                        if (!string.IsNullOrEmpty(result.HtmlTemplate))
                        {
                            IsLoadingTemplate = true;
                            string url = GetFogotPasswordPagePath(resetPasswordParameters, "", emailBodyArgs.ReqestNumber);
                            url = AddBrandingTenantForPagePath(url, emailBodyArgs.BrandingTenant);
                            result.HtmlTemplate = result.HtmlTemplate.Replace("[ResetPasswordURL]", url);
                            HtmlTemplate.Append(result.HtmlTemplate);
                        }
                    }
                }

                if (!IsLoadingTemplate)
                {
                    EmailBodyParams emailBodyParams = new EmailBodyParams
                    {
                        EmailMessageParams = emailMessageParams,
                        PagePath = emailBodyArgs.PagePath,
                        Email = resetPasswordParameters.Email,
                    };
                    BuildForgotPasswordEmailBody(HtmlTemplate, emailBodyParams);
                }
            }
            else
            {
                BuildTemplateEmailBody(HtmlTemplate, resetRequest);
            }
            EmailBodyResults emailBodyResults = new EmailBodyResults
            {
                HtmlTemplate = HtmlTemplate,
                Result = result,
            };
            return emailBodyResults;
        }

        private void CreateEmailCommunicationLog(StringBuilder HtmlTemplate, EmailCommunicationLogBuilderArgs emailCommunicationLogBuilderArgs, TenantManagmentPrivateLabelsPM privatelabel)
        {
            EmailParameters emailParameters = BuildEmailCommunicationLog(emailCommunicationLogBuilderArgs, privatelabel);
            GlobalContact callContact = globalContext.GlobalContacts.Where(m => m.Email == emailCommunicationLogBuilderArgs.ResetPasswordParameters.Email && m.InActive == false && (m.IsUser == true || m.InternetAccess == true)).FirstOrDefault();
            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                Subject = emailParameters.Subject,
                From = emailParameters.From,
                To = emailCommunicationLogBuilderArgs.ResetPasswordParameters.Email,
                CC = null,
                BCC = null,
                EmailBody = HtmlTemplate.ToString(),
                Tenant = callContact.GlobalTenantId,
                LoggingUserId = callContact.Id,
                IsBodySecured = true,
            };

            Communications.AddEmailCommunicationLogQueue(emailParams, callContact.GlobalTenantId);
        }

        private string GetFogotPasswordPagePath(ResetPasswordParameters resetPasswordParameters, string siteUri, string reqNumber)
        {
            string pageName = string.IsNullOrEmpty(resetPasswordParameters.PageName) ? "PasswordChangePage.aspx" : resetPasswordParameters.PageName;
            string path = (string.IsNullOrEmpty(resetPasswordParameters.Domain) ? siteUri : resetPasswordParameters.Domain) + @"/" + pageName + "?email=" + resetPasswordParameters.Email + "&reset_request_number=" + reqNumber + "&ischamplogin=" + resetPasswordParameters.IsChampLogin;

            if (!string.IsNullOrEmpty(resetPasswordParameters.BrandingTenant))
                path += "&tenant=" + Int32.Parse(resetPasswordParameters.BrandingTenant);

            return path;
        }

        private string AddBrandingTenantForPagePath(string pagePath, string brandingTenant)
        {
            string path = pagePath;
            path += "&tenant=" + Int32.Parse(brandingTenant);

            return path;
        }

        private EmailParameters BuildEmailCommunicationLog(EmailCommunicationLogBuilderArgs emailCommunicationLogBuilderArgs, TenantManagmentPrivateLabelsPM privatelabel)
        {
            string fromemail = LogitudeSettings.WorkEnvironment == "cloud" ? "no-reply@amital.co.il" : "no-reply@LogitudeWorld.com";
            string subject = LogitudeSettings.WorkEnvironment == "cloud" ? "Your Cloud Password!" : "Your Logitude Password! ";

            if (emailCommunicationLogBuilderArgs.ResetPasswordParameters.IsMobile)
            {
                subject = emailCommunicationLogBuilderArgs.AppMobileEnvironment + " Mobile Password";
                fromemail = emailCommunicationLogBuilderArgs.AppMobileEnvironment == "Unifreight" ? "no-reply@amital.co.il" : "no-reply@LogitudeWorld.com";
            }

            if (IsLogboxEnvironment())
            {
                string envir = privatelabel == null ? "Logbox" : privatelabel.PrivateLabelShortName;
                string Email = privatelabel == null ? "no-reply@logbox.co.il" : "no-reply@" + privatelabel.PrivateLabelDomain;
                subject = "Your " + envir + " Password";
                fromemail = Email;
            }

            if (IsCargoTrackingDomain())
            {
                string tenantName = tenantManagementPM != null ? tenantManagementPM.Name : "";
                string envir = "Cargo Tracking";
                fromemail = "no-reply@" + SecurityUtility.getLoggedDomain();
                subject = $"Your {tenantName} {envir} Password";
            }

            if (!string.IsNullOrEmpty(emailCommunicationLogBuilderArgs.Result.HtmlTemplate))
            {
                subject = !string.IsNullOrEmpty(emailCommunicationLogBuilderArgs.Result.Subject) ? emailCommunicationLogBuilderArgs.Result.Subject : subject;
                fromemail = !string.IsNullOrEmpty(emailCommunicationLogBuilderArgs.Result.From) ? emailCommunicationLogBuilderArgs.Result.From : fromemail;
            }
            EmailParameters emailParameters = new EmailParameters
            {
                Subject = subject,
                From = fromemail,
            };

            return emailParameters;
        }

        private static bool IsLogboxEnvironment()
        {
            return LogitudeSettings.DeploymentStage != null && (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2");
        }

        private bool IsCargoTrackingDomain()
        {
            return (tenantManagementPM != null && tenantManagementPM.EnableBranding && !String.IsNullOrEmpty(tenantManagementPM.CustomerURL));
        }
        private PasswordResetRequest GetPasswordResetRequestForMobile(string email, string reqNumber)
        {
            Random generator = new Random();
            string verificationCode = generator.Next(0, 10000000).ToString().Substring(0, 5);

            PasswordResetRequest resetRequest = new PasswordResetRequest
            {
                RequestNumber = reqNumber,
                Email = email,
                CreateDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(15),
                Type = "",
                VerificationCode = verificationCode,
                IsMobileOnly = true
            };

            return resetRequest;
        }

        private void BuildForgotPasswordEmailBody(StringBuilder HtmlTemplate, EmailBodyParams emailBodyParams)
        {
            var tenantName = (emailBodyParams.EmailMessageParams.TenantName != null && IsCargoTrackingDomain()) ? emailBodyParams.EmailMessageParams.TenantName + " " : "";
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("Hi,");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Click on the link below to reset your password.");
            HtmlTemplate.Append("</P>");
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("<a href=" + emailBodyParams.PagePath + ">Reset my Password</a>");
            HtmlTemplate.Append("</P>");
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("You can use the username <b>" + emailBodyParams.Email + "</b>  as the " + tenantName  + " ID to sign in to " + tenantName + emailBodyParams.EmailMessageParams.Environment + " Software.");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Thanks,");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append(tenantName + emailBodyParams.EmailMessageParams.TeamName);
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<a href='http://" + emailBodyParams.EmailMessageParams.SiteUri + "'>" + emailBodyParams.EmailMessageParams.SiteUri + "<a>");
            HtmlTemplate.Append("<br /><span  style='font-size:13px;text-align:left'>Please do not reply directly to this message</span>");
            HtmlTemplate.Append("</P>");
            if (!emailBodyParams.EmailMessageParams.IsLogBox && LogitudeSettings.WorkEnvironment != "cloud")
            {
                HtmlTemplate.Append("<p style='font-size:14px;text-align:left'>" + emailBodyParams.EmailMessageParams.Environment + " is the first true online Freight Forwarding software solution developed specifically for the cloud<br/> <img width='258' height='101' src='cid:logo0' /></p>");
            }
            HtmlTemplate.Append("");
        }

        private void BuildTemplateEmailBody(StringBuilder HtmlTemplate, PasswordResetRequest resetRequest)
        {
            HtmlTemplate.Append("<!DOCTYPE html>");
            HtmlTemplate.Append(" <p >Hi,</p>");
            HtmlTemplate.Append("<p style='margin-top:15px;'>Please enter the following code in the app to set a new password :</p>");
            HtmlTemplate.Append(" <table style='width:60px;height:37px;background-color:#F2F2F2;border:1px solid #808080;text-align:center;margin-left:120px'>   <tr ><td style='text-align:center'>" + resetRequest.VerificationCode + "</td></tr>   </table>");
            HtmlTemplate.Append("<p style='margin-top:15px;'>This code valid for 15 minute from now</p>");
            HtmlTemplate.Append("<p style='margin-top:20px;'>Please do not reply directly to this message</p>");
            //HtmlTemplate.Append("<div style='background-color:#F2F2F2;width:60px;text-align:center;height:37px;vertical-align:middle;line-height:30px;border: 1px solid #808080;'>" + resetRequest.VerificationCode + "</div>");

        }

        private DocumentType GetDocumentTypeForResetPassword(int tenant)
        {
            DocumentType documentType = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                documentType = documentTypeRepository.GetDocumentTypeByCode("SLCRP", tenant);
                scope.Complete();

            }
            if (documentType != null && !string.IsNullOrEmpty(documentType.DocumentTypeDefaultHTMLTemplateId)) return documentType;

            else return null;
        }
    }

    public class EmailMessageParams
    {
        public string Environment { get; set; }
        public string SiteUri { get; set; }
        public string SenderEmail { get; set; }
        public string TeamName { get; set; }
        public bool IsLogBox { get; set; }
        public bool IsCargoTracking { get; set; }

        public string TenantName { get; set; }
    }
    public class EmailBodyParams
    {
        public EmailMessageParams EmailMessageParams { get; set; }
        public string PagePath { get; set; }
        public string Email { get; set; }
    }
    public class EmailCommunicationLogBuilderArgs
    {
        public MessageArgs Result { get; set; }
        public ResetPasswordParameters ResetPasswordParameters { get; set; }
        public string AppMobileEnvironment { get; set; }
    }
    public class EmailParameters
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
    public class EmailBodyResults
    {
        public StringBuilder HtmlTemplate { get; set; }
        public MessageArgs Result { get; set; }
    }
    public class EmailBodyArgs
    {
        public string PagePath { get; set; }
        public string ReqestNumber { get; set; }
        public string BrandingTenant { get; set; }
    }
}