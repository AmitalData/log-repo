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

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPortalResetUserPasswordService
    {
        IGlobalContext globalContext;
        private string templateName;

        public DigitalPortalResetUserPasswordService()
        {
            globalContext = GlobalContext.GetContext();
        }

        public void ResetUserPassword(ResetPasswordParameters resetPasswordParameters, string brandingTenant)
        {
            templateName = resetPasswordParameters.TemplateName;
            var tenantManagementQuery = new TenantManagementQuery(0);
            if (!string.IsNullOrEmpty(resetPasswordParameters.BrandingTenant))
            {
                TenantManagementPM = tenantManagementQuery.GetSinglePM(int.Parse(resetPasswordParameters.BrandingTenant));
            }

            string reqNumber = GetResetRequestNumber();
            EmailMessageParams emailMessageParams = GetEmailMessageParams();
            TenantManagmentPrivateLabelsPM privatelabel = null;
            string customerURL = TenantManagementPM?.CustomerURL;
            string path = GetFogotPasswordPagePath(resetPasswordParameters, customerURL, reqNumber);

            var emailBodyArgs = new EmailBodyArgs
            {
                PagePath = path,
                ReqestNumber = reqNumber,
                BrandingTenant = brandingTenant,
            };

            EmailBodyResults emailBodyResults = BuildEmailBody(resetPasswordParameters, emailMessageParams, emailBodyArgs);
            
            var emailCommunicationLogBuilderArgs = new EmailCommunicationLogBuilderArgs
            {
                ResetPasswordParameters = resetPasswordParameters,
                Result = emailBodyResults.Result,
                AppMobileEnvironment = resetPasswordParameters.AppEnvironment,
            };

            CreateEmailCommunicationLog(emailBodyResults.HtmlTemplate, emailCommunicationLogBuilderArgs, privatelabel);
        }

        #region private 

        private TenantManagementPM TenantManagementPM { get; set; }

        private string GetResetRequestNumber()
        {
            var random = new Random();
            string randomNumber = random.Next().ToString().Substring(0, 7);
            string reqNumber = Guid.NewGuid().ToString("N") + randomNumber;
            return reqNumber;
        }

        private EmailMessageParams GetEmailMessageParams()
        {
            string tenantName = TenantManagementPM != null ? TenantManagementPM.Name : "";
            string siteUri = TenantManagementPM != null ? TenantManagementPM.CustomerURL : "";
            string environment = "Logitude";
            string senderEmail = "no-reply@" + siteUri;
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

            var result = new MessageArgs();
            var HtmlTemplate = new StringBuilder();

            if (!resetPasswordParameters.IsMobile)
            {
                if (!string.IsNullOrEmpty(resetPasswordParameters.TemplateName) && !string.IsNullOrEmpty(resetPasswordParameters.BrandingTenant))
                {
                    result = new ResetUserPasswordDocumentService(int.Parse(resetPasswordParameters.BrandingTenant)).GetMessageArgsByTemplateName(resetPasswordParameters, emailBodyArgs);
                }
                else if (!string.IsNullOrEmpty(emailBodyArgs.BrandingTenant))
                {
                    var documenttype = GetDocumentTypeForResetPassword(int.Parse(emailBodyArgs.BrandingTenant));
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
                    var emailBodyParams = new EmailBodyParams
                    {
                        EmailMessageParams = emailMessageParams,
                        PagePath = emailBodyArgs.PagePath,
                        Email = resetPasswordParameters.Email,
                    };

                    BuildForgotPasswordEmailBody(HtmlTemplate, emailBodyParams);
                }
            }

            var emailBodyResults = new EmailBodyResults
            {
                HtmlTemplate = HtmlTemplate,
                Result = result,
            };

            return emailBodyResults;
        }

        private void CreateEmailCommunicationLog(StringBuilder HtmlTemplate, EmailCommunicationLogBuilderArgs emailCommunicationLogBuilderArgs, TenantManagmentPrivateLabelsPM privatelabel)
        {
            EmailParameters emailParameters = BuildEmailCommunicationLog(emailCommunicationLogBuilderArgs, privatelabel);
            GlobalContact callContact = globalContext.GlobalContacts
                                                     .Where(m => m.Email == emailCommunicationLogBuilderArgs.ResetPasswordParameters.Email
                                                                 && m.InActive == false
                                                                 && (m.IsUser == true || m.InternetAccess == true))
                                                     .FirstOrDefault();

            var emailParams = new EmailCommunicationParams()
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
            string path = siteUri + @"/resetForgotPassword?email=" + resetPasswordParameters.Email + "&reset_request_number=" + reqNumber;

            if (!string.IsNullOrEmpty(resetPasswordParameters.BrandingTenant))
            {
                path += "&tenant=" + int.Parse(resetPasswordParameters.BrandingTenant);
            }

            return path;
        }

        private string AddBrandingTenantForPagePath(string pagePath, string brandingTenant)
        {
            string path = pagePath;
            path += "&tenant=" + int.Parse(brandingTenant);
            return path;
        }

        private EmailParameters BuildEmailCommunicationLog(EmailCommunicationLogBuilderArgs emailCommunicationLogBuilderArgs, TenantManagmentPrivateLabelsPM privatelabel)
        {
            string tenantName = TenantManagementPM != null ? TenantManagementPM.Name : "";
            string envir = "Digital Portal";
            string fromemail = "no-reply@" + TenantManagementPM.CustomerURL;
            string subject = $"Your {tenantName} {envir} Password";

            if (!string.IsNullOrEmpty(emailCommunicationLogBuilderArgs.Result.HtmlTemplate))
            {
                subject = !string.IsNullOrEmpty(emailCommunicationLogBuilderArgs.Result.Subject) ? emailCommunicationLogBuilderArgs.Result.Subject : subject;
                fromemail = !string.IsNullOrEmpty(emailCommunicationLogBuilderArgs.Result.From) ? emailCommunicationLogBuilderArgs.Result.From : fromemail;
            }

            var emailParameters = new EmailParameters
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

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                documentType = documentTypeRepository.GetDocumentTypeByCode("SLCRP", tenant);
                scope.Complete();
            }

            if (documentType != null && !string.IsNullOrEmpty(documentType.DocumentTypeDefaultHTMLTemplateId))
            {
                return documentType;
            }

            return null;
        }

        #endregion private 
    }
}