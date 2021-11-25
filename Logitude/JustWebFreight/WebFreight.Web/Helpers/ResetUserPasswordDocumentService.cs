using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Linq;
using System.Transactions;

namespace WebFreight.Web.Helpers
{
    public class ResetUserPasswordDocumentService
    {
        public MessageArgs GetMessageArgsByTemplateName(ResetPasswordParameters resetPasswordParameters, EmailBodyArgs emailBodyArgs)
        {
            MessageArgs result = new MessageArgs();
            var documenttype = GetDocumentTypeByName(resetPasswordParameters.TemplateName, Int32.Parse(resetPasswordParameters.BrandingTenant));
            if (documenttype == null)
                return result;

            result = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
            if (string.IsNullOrEmpty(result.HtmlTemplate))
                return result;

            string path = "?email=" + resetPasswordParameters.Email + "&reset_request_number=" + emailBodyArgs.ReqestNumber + "&ischamplogin=" + resetPasswordParameters.IsChampLogin;
            result.HtmlTemplate = result.HtmlTemplate.Replace("[ResetPasswordURL]", path);

            return result;
        }

        private DocumentType GetDocumentTypeByName(string templateName, int tenant)
        {
            DocumentType documentType = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                documentType = documentTypeRepository.GetDocumentTypeByCode("SLCRP", tenant);
                scope.Complete();
            }

            if (documentType == null)
                return null;

            var documentTypeTemplateId = GetDocumentTypeTemplateByDocumentTypeIdAndName(documentType, templateName);

            if (documentTypeTemplateId == null)
                return null;

            documentType.DocumentTypeDefaultHTMLTemplateId = documentTypeTemplateId;
            return documentType;
        }

        private string GetDocumentTypeTemplateByDocumentTypeIdAndName(DocumentType documentType, string templateName)
        {
            DocumentTypeTemplate documentTypeTemplate;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(documentType.Tenant);
                documentTypeTemplate = documentTypeTemplateRepository.GetDocumentTypeTemplatesByDocumentTypeId(documentType.Tenant, documentType.Id).Where(x => x.Description == templateName).FirstOrDefault();
                scope.Complete();
            }
            return documentTypeTemplate?.Id;
        }


    }


}