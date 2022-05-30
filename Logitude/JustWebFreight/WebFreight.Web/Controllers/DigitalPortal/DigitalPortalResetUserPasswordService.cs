using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Linq;
using System.Text;
using System.Transactions;
using WebFreight.Web.Helpers;
using WebFreight.Web.Params;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPortalResetUserPasswordService
    {
        IGlobalContext globalContext;
        private string templateName;
        private TenantManagementPM tenantManagementPM { get; set; }

        public DigitalPortalResetUserPasswordService()
        {
            globalContext = GlobalContext.GetContext();
        }

        public void ResetUserPassword(ResetPasswordParameters resetPasswordParameters, string brandingTenant)
        {
            templateName = resetPasswordParameters.TemplateName;
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(0);
            if (!string.IsNullOrEmpty(resetPasswordParameters.BrandingTenant))
                tenantManagementPM = tenantManagementQuery.GetSinglePM(Int32.Parse(resetPasswordParameters.BrandingTenant));

            string reqNumber = GetResetRequestNumber();
            EmailMessageParams emailMessageParams = GetEmailMessageParams();
            TenantManagmentPrivateLabelsPM privatelabel = null;

            string LogitudeURL = LogitudeSettings.LogitudeURL;
            string path = GetFogotPasswordPagePath(resetPasswordParameters, LogitudeURL, reqNumber);

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

        private EmailMessageParams GetEmailMessageParams()
        {
            string tenantName = tenantManagementPM != null ? tenantManagementPM.Name : "";
            string siteUri = tenantManagementPM != null ? tenantManagementPM.CustomerURL : "";
            string environment = "Logitude";
            string senderEmail = "no-reply@" + SecurityUtility.getLoggedDomain();
            string teamName = "Digital Portal Team";

            var emailMessageParams = new EmailMessageParams
            {
                Environment = environment,
                SiteUri = siteUri,
                TeamName = teamName,
                SenderEmail = senderEmail,
                TenantName = tenantName,
            };

            return emailMessageParams;
        }

        private EmailBodyResults BuildEmailBody(ResetPasswordParameters resetPasswordParameters, EmailMessageParams emailMessageParams, EmailBodyArgs emailBodyArgs)
        {
            PasswordResetRequest resetRequest = new PasswordResetRequest
            {
                RequestNumber = emailBodyArgs.ReqestNumber,
                Email = resetPasswordParameters.Email
            };

           
            globalContext.PasswordResetRequests.Add(resetRequest);
            globalContext.SaveChanges();

            MessageArgs result = new MessageArgs();
            StringBuilder HtmlTemplate = new StringBuilder();
            if (!resetPasswordParameters.IsMobile)
            {
                if (!string.IsNullOrEmpty(resetPasswordParameters.TemplateName) && !string.IsNullOrEmpty(resetPasswordParameters.BrandingTenant))
                {
                    result = new ResetUserPasswordDocumentService(int.Parse(resetPasswordParameters.BrandingTenant)).GetMessageArgsByTemplateName(resetPasswordParameters, emailBodyArgs);
                }
                else if (!string.IsNullOrEmpty(emailBodyArgs.BrandingTenant))
                {
                    var documenttype = GetDocumentTypeForResetPassword(Int32.Parse(emailBodyArgs.BrandingTenant));
                    if (documenttype != null)
                    {
                        result = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
                        if (!string.IsNullOrEmpty(result.HtmlTemplate))
                        {
                            string url = GetFogotPasswordPagePath(resetPasswordParameters, "", emailBodyArgs.ReqestNumber);
                            url = AddBrandingTenantForPagePath(url, emailBodyArgs.BrandingTenant);
                            result.HtmlTemplate = result.HtmlTemplate.Replace("[ResetPasswordURL]", url);
                        }
                    }
                }

                HtmlTemplate.Append(result.HtmlTemplate);
                if (string.IsNullOrEmpty(result.HtmlTemplate))
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
            string path = (string.IsNullOrEmpty(resetPasswordParameters.Domain) ? siteUri : resetPasswordParameters.Domain) + @"/" + pageName + "?email=" + resetPasswordParameters.Email + "&reset_request_number=" + reqNumber;

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
            string tenantName = tenantManagementPM != null ? tenantManagementPM.Name : "";
            string envir = "Cargo Tracking";
            string fromemail = "no-reply@" + SecurityUtility.getLoggedDomain();
            string subject = $"Your {tenantName} {envir} Password";

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

        private void BuildForgotPasswordEmailBody(StringBuilder HtmlTemplate, EmailBodyParams emailBodyParams)
        {
            var tenantName = (emailBodyParams.EmailMessageParams.TenantName != null ? emailBodyParams.EmailMessageParams.TenantName + " " : "");
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("Hi,");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Click on the link below to reset your password.");
            HtmlTemplate.Append("</P>");
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("<a href=" + emailBodyParams.PagePath + ">Reset my Password</a>");
            HtmlTemplate.Append("</P>");
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("You can use the username <b>" + emailBodyParams.Email + "</b>  as the " + (String.IsNullOrEmpty(tenantName) ? emailBodyParams.EmailMessageParams.Environment : tenantName) + " ID to sign in to " + tenantName + emailBodyParams.EmailMessageParams.Environment);
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Thanks,");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append(tenantName + emailBodyParams.EmailMessageParams.TeamName);
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<br /><span  style='font-size:13px;text-align:left'>Please do not reply directly to this message</span>");
            HtmlTemplate.Append("</P>");
            if (!emailBodyParams.EmailMessageParams.IsLogBox && LogitudeSettings.WorkEnvironment != "cloud")
            {
                HtmlTemplate.Append("<p style='font-size:14px;text-align:left'>" + emailBodyParams.EmailMessageParams.Environment + " is the first true online Freight Forwarding software solution developed specifically for the cloud<br/> <img width='258' height='101' src='cid:logo0' /></p>");
            }
            HtmlTemplate.Append("");
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
}