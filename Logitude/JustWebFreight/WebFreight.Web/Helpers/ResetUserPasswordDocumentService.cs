using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Linq;
using System.Transactions;
using WebFreight.Web.Params;

namespace WebFreight.Web.Helpers
{
    public class ResetUserPasswordDocumentService
    {
        private readonly ICommonDataContext commonDataContext;
        private readonly int tenant;

        public ResetUserPasswordDocumentService(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
            this.tenant = tenant;
        }


        public MessageArgs GetMessageArgsByTemplateName(ResetPasswordParameters resetPasswordParameters, EmailBodyArgs emailBodyArgs)
        {
            MessageArgs result = new MessageArgs();

            var documenttype =!string.IsNullOrEmpty(resetPasswordParameters.DocumentTypeCode) ? GetDocumentTypeByCode(resetPasswordParameters.DocumentTypeCode, tenant) :  GetDocumentTypeByName(resetPasswordParameters.TemplateName, tenant);
            if (documenttype == null)
                return result;

            result = HtmlEditorHelper.GetHtmlFromTemplate(documenttype.DocumentTypeDefaultHTMLTemplateId, documenttype.ObjectTableId, documenttype.Tenant);
            if (string.IsNullOrEmpty(result.HtmlTemplate))
                return result;

            Contact contact = GetContact(resetPasswordParameters);


            string path = "?email=" + resetPasswordParameters.Email + "&reset_request_number=" + emailBodyArgs.ReqestNumber + "&ischamplogin=" + resetPasswordParameters.IsChampLogin;
            if (resetPasswordParameters.IsCargoTracking)
            {
                path = @"/" + "cargo-tracking" + @"/" + "changepassword?email=" + resetPasswordParameters.Email + "&reset_request_number=" + emailBodyArgs.ReqestNumber + "&ischamplogin=" + resetPasswordParameters.IsChampLogin + "&tenant=" + Int32.Parse(resetPasswordParameters.BrandingTenant);
            }

            result.HtmlTemplate = result.HtmlTemplate.Replace("[ResetPasswordURL]", path);
            result.HtmlTemplate = result.HtmlTemplate.Replace("[InvitationEmail]", resetPasswordParameters.Email);
            result.HtmlTemplate = result.HtmlTemplate.Replace("[InviteeName]", contact?.EnglishName ?? "");
            return result;
        }




        private Contact GetContact(ResetPasswordParameters resetPasswordParameters)
        {
            return (from c in commonDataContext.Contacts
                    where c.Email == resetPasswordParameters.Email && c.Tenant == tenant
                    select c).FirstOrDefault();
        }


        private DocumentType GetDocumentTypeByCode(string documentTypeCode, int tenant)
        {
            DocumentType documentType = null;


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                documentType = documentTypeRepository.GetDocumentTypeByCode((!string.IsNullOrEmpty(documentTypeCode) ? documentTypeCode : "SLCRP"), tenant);
                scope.Complete();
            }

            return documentType;
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