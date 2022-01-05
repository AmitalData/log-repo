using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.ExternalService
{
    public class CopyDocumentTypeTemplateService
    {
        private readonly int tenant;
        private readonly DocumentOutRepository documentOutRepository;
        private readonly DocumentTypeTemplateQuery documentTypeTemplateQuery;
        private readonly DocumentTypeTemplateRepository documentTypeTemplateRepository;
        private readonly Dictionary<string, DocumentTypePM> tenantZeroDocumentTypes;
        private readonly string countryCode;
        public CopyDocumentTypeTemplateService(int tenant, Dictionary<string, DocumentTypePM> tenantZeroDocumentTypes)
        {
            this.tenant = tenant;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            documentOutRepository = new DocumentOutRepository(commonContext);
            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(commonContext);
            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(documentTypeTemplateRepository);
            this.tenantZeroDocumentTypes = tenantZeroDocumentTypes;
            countryCode = GetCurrentTenantCountryCode(tenant);
        }

        private string GetCurrentTenantCountryCode(int tenant)
        {
            string countryCode = "";
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            if (currentTenant == null) return countryCode;
            if (currentTenant.AddressId == null) return countryCode;

            ICommonDataContext objectContext = CommonDataContext.GetContext(currentTenant.Id);
            AddressRepository addressRepository = new AddressRepository(objectContext);
            AddressQuery addressQuery = new AddressQuery(addressRepository);
            AddressPM address = addressQuery.GetSinglePM(currentTenant.AddressId, currentTenant.Id);
            if (address != null)
            {
                countryCode = address.CountryCode;
            }

            return countryCode;
        }

        public void Execute(DocumentType currentTenantDocumentType, DocumentTypePM tenantZeroDocumentTypePM)
        {
            List<DocumentTypeTemplatePM> tenantZeroDocumentTypeTemplatePMs = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeId(tenantZeroDocumentTypePM.Id, 0);
            List<DocumentTypeTemplatePM> currentTenantDocumentTypeTemplatePMs = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeId(currentTenantDocumentType.Id, tenant);
            if(tenantZeroDocumentTypeTemplatePMs.Count() > 0)
            {

            }
            foreach (DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM in tenantZeroDocumentTypeTemplatePMs)
            {
                CopyTenantZeroDocumentTypeTemplateToCurrentTenant(currentTenantDocumentType, currentTenantDocumentTypeTemplatePMs, tenantZeroDocumentTypeTemplatePM);
            }

            documentTypeTemplateRepository.SubmitChanges();
        }

        private void CopyTenantZeroDocumentTypeTemplateToCurrentTenant(DocumentType currentTenantDocumentType, List<DocumentTypeTemplatePM> currentTenantDocumentTypeTemplatePMs, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM)
        {
            DocumentTypeTemplatePM currentDocumentTypeTemplatePM = currentTenantDocumentTypeTemplatePMs.Where(d => d.OriginalTemplateId == tenantZeroDocumentTypeTemplatePM.Id).FirstOrDefault();
            if (currentDocumentTypeTemplatePM == null)
            {
                CreateNewDocumentTypeTemplateToCurrentTenant(currentTenantDocumentType, tenantZeroDocumentTypeTemplatePM);
            }
            else if (tenantZeroDocumentTypeTemplatePM.IsSystem && tenantZeroDocumentTypeTemplatePM.LastUpdateDate != currentDocumentTypeTemplatePM.LastUpdateDate)
            {
                UpdateCurrentTenantDocumentTypeTemplate(currentTenantDocumentType, tenantZeroDocumentTypeTemplatePM, currentDocumentTypeTemplatePM);
            }
        }

        private void CreateNewDocumentTypeTemplateToCurrentTenant(DocumentType currentTenantDocumentType, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM)
        {
            bool sameCountry = currentTenantDocumentType.CountryCode == countryCode;
            DocumentTypePM documentTypePM = tenantZeroDocumentTypes.Values.Where(d => d.Id == tenantZeroDocumentTypeTemplatePM.DocumentTypeId && d.Tenant == 0).FirstOrDefault();

            if (!tenantZeroDocumentTypeTemplatePM.IsSystem) return;
            if (!sameCountry && !(tenantZeroDocumentTypeTemplatePM.CountryCode == countryCode) && !string.IsNullOrEmpty(tenantZeroDocumentTypeTemplatePM.CountryCode?.Trim())) return;

            DocumentTypeTemplate newtemplate = CreateNewInstanceDocumentTypeTemplate(currentTenantDocumentType, tenantZeroDocumentTypeTemplatePM);

            bool hasDefaultTemplate = !string.IsNullOrEmpty(currentTenantDocumentType.DocumentTypeDefaultReportTemplateId);
            if (!hasDefaultTemplate && newtemplate.TemplateType == "P") currentTenantDocumentType.DocumentTypeDefaultReportTemplateId = newtemplate.Id;
            else if (!hasDefaultTemplate) currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId = newtemplate.Id;

            documentTypeTemplateRepository.Add(newtemplate);
        }

        private DocumentTypeTemplate CreateNewInstanceDocumentTypeTemplate(DocumentType currentTenantDocumentType, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM)
        {
            return new DocumentTypeTemplate()
            {
                Id = IdCounter.GetNumber("DocumentTypeTemplate", tenant).ToString(),
                Tenant = tenant,
                TemplateBody = tenantZeroDocumentTypeTemplatePM.TemplateBody,
                TemplateType = tenantZeroDocumentTypeTemplatePM.TemplateType,
                HorizontalShift = tenantZeroDocumentTypeTemplatePM.HorizontalShift,
                InActive = tenantZeroDocumentTypeTemplatePM.InActive,
                Description = tenantZeroDocumentTypeTemplatePM.Description,
                DocumentTypeId = currentTenantDocumentType.Id,
                EditorTool = tenantZeroDocumentTypeTemplatePM.EditorTool,
                CountryCode = tenantZeroDocumentTypeTemplatePM.CountryCode,
                Subject = tenantZeroDocumentTypeTemplatePM.CountryCode,
                Language = tenantZeroDocumentTypeTemplatePM.Language,
                OriginalTemplateId = tenantZeroDocumentTypeTemplatePM.Id,
                VerticalShift = tenantZeroDocumentTypeTemplatePM.VerticalShift,
                InternalRemarks = tenantZeroDocumentTypeTemplatePM.InternalRemarks,
                IsEnabledForCustomers = true,
                TemplateBodyHtml = tenantZeroDocumentTypeTemplatePM.TemplateBodyHtml,
                TemplateFooterHtml = tenantZeroDocumentTypeTemplatePM.TemplateFooterHtml,
                TemplateHeaderHtml = tenantZeroDocumentTypeTemplatePM.TemplateHeaderHtml,
                TemplateFooterHeight = tenantZeroDocumentTypeTemplatePM.TemplateFooterHeight,
                TemplateHeaderHeight = tenantZeroDocumentTypeTemplatePM.TemplateHeaderHeight,
                CC = tenantZeroDocumentTypeTemplatePM.CC,
                From = tenantZeroDocumentTypeTemplatePM.From,
                ReplyTo = tenantZeroDocumentTypeTemplatePM.ReplyTo,
                To = tenantZeroDocumentTypeTemplatePM.To,
                LastUpdateDate = tenantZeroDocumentTypeTemplatePM.LastUpdateDate,
                IsSystem = true,
            };
        }

        private void UpdateCurrentTenantDocumentTypeTemplate(DocumentType currentTenantDocumentType, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM, DocumentTypeTemplatePM currentDocumentTypeTemplatePM)
        {
            bool sameCountry = currentTenantDocumentType.CountryCode == countryCode;
            if (!tenantZeroDocumentTypeTemplatePM.IsSystem) return;
            if (!sameCountry && !(tenantZeroDocumentTypeTemplatePM.CountryCode == countryCode) && !string.IsNullOrEmpty(tenantZeroDocumentTypeTemplatePM.CountryCode?.Trim())) return;

            DocumentTypeTemplate currentDocumentTypeTemplate = documentTypeTemplateRepository.GetSingleDocumentTypeTemplate(currentDocumentTypeTemplatePM.Id, currentDocumentTypeTemplatePM.Tenant);
            MapCurrentDocumentTypeTemplate(tenantZeroDocumentTypeTemplatePM, currentDocumentTypeTemplate, currentTenantDocumentType);

            documentTypeTemplateRepository.Update(currentDocumentTypeTemplate);
        }

        private void MapCurrentDocumentTypeTemplate(DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM, DocumentTypeTemplate currentDocumentTypeTemplate, DocumentType currentTenantDocumentType)
        {
            currentDocumentTypeTemplate.TemplateBody = tenantZeroDocumentTypeTemplatePM.TemplateBody;
            currentDocumentTypeTemplate.TemplateType = tenantZeroDocumentTypeTemplatePM.TemplateType;
            currentDocumentTypeTemplate.HorizontalShift = tenantZeroDocumentTypeTemplatePM.HorizontalShift;
            currentDocumentTypeTemplate.InActive = tenantZeroDocumentTypeTemplatePM.InActive;
            currentDocumentTypeTemplate.Description = tenantZeroDocumentTypeTemplatePM.Description;
            currentDocumentTypeTemplate.EditorTool = tenantZeroDocumentTypeTemplatePM.EditorTool;
            currentDocumentTypeTemplate.CountryCode = tenantZeroDocumentTypeTemplatePM.CountryCode;
            currentDocumentTypeTemplate.Subject = tenantZeroDocumentTypeTemplatePM.CountryCode;
            currentDocumentTypeTemplate.Language = tenantZeroDocumentTypeTemplatePM.Language;
            currentDocumentTypeTemplate.VerticalShift = tenantZeroDocumentTypeTemplatePM.VerticalShift;
            currentDocumentTypeTemplate.InternalRemarks = tenantZeroDocumentTypeTemplatePM.InternalRemarks;
            currentDocumentTypeTemplate.TemplateBodyHtml = tenantZeroDocumentTypeTemplatePM.TemplateBodyHtml;
            currentDocumentTypeTemplate.TemplateFooterHtml = tenantZeroDocumentTypeTemplatePM.TemplateFooterHtml;
            currentDocumentTypeTemplate.TemplateHeaderHtml = tenantZeroDocumentTypeTemplatePM.TemplateHeaderHtml;
            currentDocumentTypeTemplate.TemplateFooterHeight = tenantZeroDocumentTypeTemplatePM.TemplateFooterHeight;
            currentDocumentTypeTemplate.TemplateHeaderHeight = tenantZeroDocumentTypeTemplatePM.TemplateHeaderHeight;
            currentDocumentTypeTemplate.To = tenantZeroDocumentTypeTemplatePM.To;
            currentDocumentTypeTemplate.CC = tenantZeroDocumentTypeTemplatePM.CC;
            currentDocumentTypeTemplate.From = tenantZeroDocumentTypeTemplatePM.From;
            currentDocumentTypeTemplate.ReplyTo = tenantZeroDocumentTypeTemplatePM.ReplyTo;
            currentDocumentTypeTemplate.LastUpdateDate = tenantZeroDocumentTypeTemplatePM.LastUpdateDate;
            currentDocumentTypeTemplate.IsSystem = true;

            bool isCurrentDefaultTemplate = GetIsCurrentDefaultTemplate(currentDocumentTypeTemplate, currentTenantDocumentType);
            if (isCurrentDefaultTemplate)
                SetDefaultDocumentTypeTemplate(currentTenantDocumentType, currentDocumentTypeTemplate);
        }

        private static bool GetIsCurrentDefaultTemplate(DocumentTypeTemplate currentDocumentTypeTemplate, DocumentType currentTenantDocumentType)
        {
            return currentTenantDocumentType.DocumentTypeDefaultReportTemplateId == currentDocumentTypeTemplate.Id
                || currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId == currentDocumentTypeTemplate.Id;
        }

        private void SetDefaultDocumentTypeTemplate(DocumentType currentTenantDocumentType, DocumentTypeTemplate currentDocumentTypeTemplatePM)
        {
            IQueryable<DocumentOut> documentOuts = documentOutRepository.GetDocumentOuts(currentDocumentTypeTemplatePM.Tenant);
            if (currentDocumentTypeTemplatePM.TemplateType == "P" && !documentOuts.Where(d => d.DocumentTemplateId == currentDocumentTypeTemplatePM.Id).Any())
            {
                currentTenantDocumentType.DocumentTypeDefaultReportTemplateId = currentDocumentTypeTemplatePM.Id;
            }
            else if (!documentOuts.Where(d => d.EmailTemplateId == currentDocumentTypeTemplatePM.Id).Any())
            {
                currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId = currentDocumentTypeTemplatePM.Id;
            }
        }
    }
}
