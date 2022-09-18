using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class AttachmentDocumentNotifyService
    {
        private List<DocumentDefultAttachment> documentDefultAttachment;
        private int tenant;
        public AttachmentDocumentNotifyService(List<DocumentDefultAttachment> documentDefultAttachment , int tenant)
        {
            this.documentDefultAttachment = documentDefultAttachment;
            this.tenant = tenant;

        }

        public void Execute(string notifyBackEmails)
        {
            var emailParams = BuildNotifyBackEmailCommunications(notifyBackEmails);
            Communications.AddEmailCommunicationLogQueue(emailParams,tenant);

        }


        private EmailCommunicationParams BuildNotifyBackEmailCommunications(string notifyBackEmails)
        {
            string emailBody = GetEmailBody();
            StringBuilder HtmlTemplate = BuildHtmlTemplateWithBody(emailBody);
            string fromEmail = GetFromEmail();

            string emailbody = HtmlTemplate.ToString();
            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                From = fromEmail,
                To = notifyBackEmails,
                CC = "",
                BCC = "",
                Subject = "Test",
                EmailBody = emailbody,
                Tenant = tenant,
            };
            return emailParams;
        }

        private string GetFromEmail()
        {
            string fromEmail = "no-reply@LogitudeWorld.com";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                fromEmail = tenantManagementQuery.GetSystemDomain(tenant);
                scope.Complete();
            }

            return fromEmail;
        }

        private string GetEmailBody()
        {
            return "test";
        }
        private StringBuilder BuildHtmlTemplateWithBody(string emailBody)
        {
            StringBuilder HtmlTemplate = new StringBuilder();

            HtmlTemplate.Append("<div style='text-align:left;'>");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append(emailBody);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("<br /><br />");
            return HtmlTemplate;
        }
  
    }
}