using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
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
        private string entityReference;
        private string objectTableName;

        public AttachmentDocumentNotifyService(List<DocumentDefultAttachment> documentDefultAttachment, int tenant, string automationName)
        {
            this.documentDefultAttachment = documentDefultAttachment.Where(d => !string.IsNullOrEmpty(d.DocumentTypeName)).ToList();

            this.tenant = tenant;
            this.automationName = automationName;
        }

        public void Execute(string notifyBackEmails, AutomationSendEmailArgs automationSendEmailArgs)
        {
            if (documentDefultAttachment.Count() == 0) return;
            entityReference = automationSendEmailArgs.EntityReference;
            objectTableName = automationSendEmailArgs.ObjectTableName;
            var emailParams = BuildEmailCommunicationParams(notifyBackEmails);
            Communications.AddEmailCommunicationLogQueue(emailParams, tenant);

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
            var currentTenant = new TenantManagementRepository().GetSingleTenantManagement(tenant);
            const string boldFontWeight = "bold";
            const string redColor = "red";
            const string blueColor = "#6082B6";
            const string blackColor = "black";

            bool isMoreThanOneDocument = documentDefultAttachment.Count > 1;

            string emailString = "<span " + GetTextColorAndWeightStyle(blueColor, boldFontWeight) + ">" + this.automationName + "</span> Automation in ";
            emailString += "<span " + GetTextColorAndWeightStyle(blueColor, boldFontWeight) + ">" + currentTenant?.Name + "</span>";
            emailString += " company failed to send the following ";
            emailString += "document" + (isMoreThanOneDocument ? "s for " : " for ");
            emailString += "<span " + GetTextColorAndWeightStyle(blackColor, boldFontWeight) + ">" + objectTableName + " number </span>";
            emailString += "<span " + GetTextColorAndWeightStyle(blueColor, boldFontWeight) + ">" + entityReference + ":</span>";


            emailString += "<div>";
            foreach (DocumentDefultAttachment document in this.documentDefultAttachment)
            {
                emailString += "- " + document.DocumentTypeName + ": <span "+ GetTextColorAndWeightStyle(redColor, boldFontWeight) + ">" + GetFailureReason(document.Type) + "</span> <br />";
            }
            emailString += "</div>";
           
            return emailString;
        }
        private string GetFailureReason(string documentType)
        {
            if (documentType == "DocOut") return "The Document is not printed";
            if (documentType == "DocIn") return "The Document is not uploaded";
            return "The Document is not uploaded or The Document is not printed";
        }
        private string GetTextColorAndWeightStyle(string color, string fontWeight)
        {
            return "style=\"color:" + color + "; font-weight: " + fontWeight + ";\"";
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