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
        private string automationName;
        public AttachmentDocumentNotifyService(List<DocumentDefultAttachment> documentDefultAttachment , int tenant, string automationName)
        {
            this.documentDefultAttachment = documentDefultAttachment.Where(d => !string.IsNullOrEmpty(d.DocumentTypeName)).ToList();
        
            this.tenant = tenant;
            this.automationName = automationName;
        }

        public void Execute(string notifyBackEmails)
        {
            if (documentDefultAttachment.Count() == 0) return;
            var emailParams = BuildEmailCommunicationParams(notifyBackEmails);
            Communications.AddEmailCommunicationLogQueue(emailParams,tenant);

        }


        private EmailCommunicationParams BuildEmailCommunicationParams(string notifyBackEmails)
        {
            string emailbody = GetEmailTemplate().ToString();
            string fromEmail = GetFromEmail();
            string emailSubject = GetEmailSubject();
            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                From = fromEmail,
                To = notifyBackEmails,
                CC = "",
                BCC = "",
                Subject = emailSubject,
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

        private string GetEmailSubject()
        {
            return "Automation " + this.automationName + " :" + " Not Sent Documents";
        }
        private string GetEmailBody()
     {
            bool isMoreThanOneDocument = documentDefultAttachment.Count > 1;
            string emailString = "Automation " + this.automationName + " failed to send the following ";
            emailString += "document" + (isMoreThanOneDocument ? "s :" : " :");
            emailString += "<div>";
            foreach (DocumentDefultAttachment document in this.documentDefultAttachment)
            {
                emailString += "- " + document.DocumentTypeName + "<br>";
            }

            emailString += "</div>";
            return emailString;
        }
        private StringBuilder GetEmailTemplate()
        {
            StringBuilder HtmlTemplate = new StringBuilder();

            HtmlTemplate.Append("<div style='text-align:left;'>");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append(GetEmailBody());
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("<br /><br />");
            return HtmlTemplate;
        }
  
    }
}