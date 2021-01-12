using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Text;

namespace WebFreight.Web.Helpers
{
    public class AutomationDocumentHelper
    {
        private int tenant;
        private AutomationSendEmailArgs automationDocumentHelperArgs;
        private Automation automation;
        public AutomationDocumentHelper(AutomationSendEmailArgs automationDocumentHelperArgs)
        {
            this.tenant = automationDocumentHelperArgs.Tenant;
            this.automation = automationDocumentHelperArgs.Automation;
            this.automationDocumentHelperArgs = automationDocumentHelperArgs;
        }

        public AutomationDocumentResult GetAutomationDocumentResult()
        {
            HtmlEditorResolveResult htmlEditorResolveResult = GetHtmlEditorResolveResult();
            byte[] htmlData = GetEmailBodyData(htmlEditorResolveResult.HtmlString);
            Document document = CreateNewDocument(htmlData);
            WriteDocumentOnStorage(document, htmlData);

            AutomationDocumentResult automationDocumentResult = new AutomationDocumentResult
            {
                HtmlEditorResolveResult = htmlEditorResolveResult,
                HtmlData = htmlData,
                DocumentId = document.Id,
                ObjectTableId = automationDocumentHelperArgs.ObjectTableId,
            };

            return automationDocumentResult;
        }

        private HtmlEditorResolveResult GetHtmlEditorResolveResult()
        {
            HtmlEditorResolveResult htmlEditorResolveResult = new HtmlEditorResolveResult();
            if (!string.IsNullOrEmpty(automation.TemplateId))
            {
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);
                DocumentTypeTemplate template = documentTypeTemplateRepository.GetSingleDocumentTypeTemplateWithOutInClude(automation.TemplateId, tenant);
                htmlEditorResolveResult = GetHtmlEditorResolveResultByDocumentTypeTemplate(template);
            }

            return htmlEditorResolveResult;
        }

        private HtmlEditorResolveResult GetHtmlEditorResolveResultByDocumentTypeTemplate(DocumentTypeTemplate template)
        {
            HtmlEditorResolveResult htmlEditorResolveResult = new HtmlEditorResolveResult();
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            if (template != null)
            {
                try
                {
                    string userId = GetHtmlEditorResolveArgsUserId(template.LastUpdatedByUserId);
                    HtmlEditorResolveArgs htmlEditorResolveArgs = GetNewHtmlEditorResolveArgs(template, userId);
                    htmlEditorResolveResult = htmlEditorHelper.GetEditorHtmlData(htmlEditorResolveArgs);
                    if (!string.IsNullOrEmpty(htmlEditorResolveResult.HtmlString))
                        htmlEditorResolveResult.HtmlString = htmlEditorHelper.GetLogoHtmlString(htmlEditorResolveResult.HtmlString);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "EntityChangeWorkerRole", "", null);
                }
            }

            return htmlEditorResolveResult;
        }

        private HtmlEditorResolveArgs GetNewHtmlEditorResolveArgs(DocumentTypeTemplate template, string userId)
        {
            HtmlEditorResolveArgs htmlEditorResolveArgs = new HtmlEditorResolveArgs()
            {
                Subject = template.Subject,
                From = template.From,
                ReplyTo = template.ReplyTo,
                Cc = template.CC,
                Bcc = template.BCC,
                UserId = userId,
                EntityId = automationDocumentHelperArgs.EntityId,
                ObjectTableId = automationDocumentHelperArgs.ObjectTableId,
                DocumentTemplateId = template.Id,
                Tenant = tenant,
                IsSendMail = true,
                DocumentTypeTemplate = template,
            };

            return htmlEditorResolveArgs;
        }

        private string GetHtmlEditorResolveArgsUserId(string templateLastUpdatedByUserId)
        {
            string userId = templateLastUpdatedByUserId;
            if (!string.IsNullOrEmpty(automationDocumentHelperArgs.CreateByUserId))
                userId = automationDocumentHelperArgs.CreateByUserId;
            return userId;
        }

        private void WriteDocumentOnStorage(Document document, byte[] htmlData)
        {
            string filename = document.Id + ".html";
            string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = htmlData.Length,
            };

            storageservice.Write(htmlData, fileInfo);
        }

        private Document CreateNewDocument(byte[] htmlData)
        {
            Document document = new Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "html",
                FileSize = Convert.ToInt32(htmlData.Length),
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant).ToString(),
                Folder = "others",
            };

            DocumentRepository documentRep = new DocumentRepository(tenant);
            documentRep.Add(document);
            documentRep.SubmitChanges();
            return document;
        }

        private byte[] GetEmailBodyData(string htmlTemplateBody)
        {
            string htmlString = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body>";
            htmlString += htmlTemplateBody;
            htmlString += "</body></html>";
            return Encoding.UTF8.GetBytes(htmlString);
        }
    }

    public class AutomationDocumentResult
    {
        public HtmlEditorResolveResult HtmlEditorResolveResult { get; set; }
        public byte[] HtmlData { get; set; }
        public string DocumentId { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }
        public string ToEmail { get; set; }
    }
}