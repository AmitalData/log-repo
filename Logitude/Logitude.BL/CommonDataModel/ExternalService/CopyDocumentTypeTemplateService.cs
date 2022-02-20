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
        private readonly IQueryable<DocumentOut> documentOuts;
        private readonly DocumentTypeTemplateRepository documentTypeTemplateRepository;
        private readonly Dictionary<string, DocumentTypePM> allTenantZeroDocumentTypes;
        private readonly List<DocumentTypeTemplatePM> allSystemTenantZeroDocumentTypeTemplatePMs;
        private readonly List<DocumentTypeTemplatePM> allSystemCurrentTenantDocumentTypeTemplatePMs;
        private readonly string countryCode;
        private bool documentTypeShouldChange = false;
        public CopyDocumentTypeTemplateService(CopyDocumentTypeTemplateArgs copyDocumentTypeTemplateArgs)
        {
            tenant = copyDocumentTypeTemplateArgs.Tenant;
            allTenantZeroDocumentTypes = copyDocumentTypeTemplateArgs.AllTenantZeroDocumentTypes;
            allSystemTenantZeroDocumentTypeTemplatePMs = copyDocumentTypeTemplateArgs.AllSystemTenantZeroDocumentTypeTemplatePMs;
            allSystemCurrentTenantDocumentTypeTemplatePMs = copyDocumentTypeTemplateArgs.AllSystemCurrentTenantDocumentTypeTemplatePMs;
            countryCode = copyDocumentTypeTemplateArgs.CountryCode;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(commonContext);
            documentOuts = documentOutRepository.GetDocumentOuts(tenant);
            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(commonContext);
        }

        public void Execute(DocumentType currentTenantDocumentType, DocumentTypePM tenantZeroDocumentTypePM)
        {
            List<DocumentTypeTemplatePM> tenantZeroDocumentTypeTemplatePMs = allSystemTenantZeroDocumentTypeTemplatePMs.Where(t => t.DocumentTypeId == tenantZeroDocumentTypePM.Id).ToList();
            List<DocumentTypeTemplatePM> currentTenantDocumentTypeTemplatePMs = allSystemCurrentTenantDocumentTypeTemplatePMs.Where(t => t.DocumentTypeId == currentTenantDocumentType.Id).ToList();

            if (!string.IsNullOrEmpty(tenantZeroDocumentTypePM.CountryCode) && currentTenantDocumentType.CountryCode != tenantZeroDocumentTypePM.CountryCode) return;

            foreach (DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM in tenantZeroDocumentTypeTemplatePMs)
            {
                CopyTenantZeroDocumentTypeTemplateToCurrentTenant(currentTenantDocumentType, allSystemCurrentTenantDocumentTypeTemplatePMs, tenantZeroDocumentTypeTemplatePM);
            }

            if (documentTypeShouldChange) SetDefaultDocumentType(currentTenantDocumentType, tenantZeroDocumentTypePM);

            documentTypeTemplateRepository.SubmitChanges();
        }

        private void SetDefaultDocumentType(DocumentType currentTenantDocumentType, DocumentTypePM tenantZeroDocumentTypePM)
        {
            SetDefaultReportDocumentType(currentTenantDocumentType, tenantZeroDocumentTypePM);
            SetDefaultHtmlDocumentType(currentTenantDocumentType, tenantZeroDocumentTypePM);
        }

        private void SetDefaultReportDocumentType(DocumentType currentTenantDocumentType, DocumentTypePM tenantZeroDocumentTypePM)
        {
            DocumentTypeTemplatePM tenantZeroDefaultDocumentTypeReportTemplatePM = allSystemTenantZeroDocumentTypeTemplatePMs.Where(d => d.Id == tenantZeroDocumentTypePM.DocumentTypeDefaultReportTemplateId).FirstOrDefault();
            if (tenantZeroDefaultDocumentTypeReportTemplatePM != null && !documentOuts.Where(d => d.DocumentTemplateId == currentTenantDocumentType.DocumentTypeDefaultReportTemplateId).Any())
            {
                currentTenantDocumentType.DocumentTypeDefaultReportTemplateId = GetDefaultDocumentType(tenantZeroDefaultDocumentTypeReportTemplatePM, currentTenantDocumentType.DocumentTypeDefaultReportTemplateId);
            }
            if (allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.TemplateType == "P").Count() == 1 || (allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.TemplateType == "P").Count()>0 && String.IsNullOrEmpty(currentTenantDocumentType.DocumentTypeDefaultReportTemplateId)))
            {
                currentTenantDocumentType.DocumentTypeDefaultReportTemplateId = allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.TemplateType == "P").FirstOrDefault().Id;
            }
        }

        private void SetDefaultHtmlDocumentType(DocumentType currentTenantDocumentType, DocumentTypePM tenantZeroDocumentTypePM)
        {
            DocumentTypeTemplatePM tenantZeroDefaultDocumentTypeHtmlTemplatePM = allSystemTenantZeroDocumentTypeTemplatePMs.Where(d => d.Id == tenantZeroDocumentTypePM.DocumentTypeDefaultHTMLTemplateId).FirstOrDefault();
            if (tenantZeroDefaultDocumentTypeHtmlTemplatePM != null && !documentOuts.Where(d => d.DocumentTemplateId == currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId).Any())
            {
                currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId = GetDefaultDocumentType(tenantZeroDefaultDocumentTypeHtmlTemplatePM, currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId);
            }
            if (allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.TemplateType != "P").Count() == 1 || (allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.TemplateType != "P").Count() > 0 && String.IsNullOrEmpty(currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId)))
            {
                currentTenantDocumentType.DocumentTypeDefaultHTMLTemplateId = allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.TemplateType != "P").FirstOrDefault().Id;
            }
        }

        private string GetDefaultDocumentType(DocumentTypeTemplatePM tenantZeroDefaultDocumentTypeTemplatePM, string currentTenantDocumentTypeDefaultTemplate)
        {
            DocumentTypeTemplatePM currentDefaultDocumentTypeTemplatePM = allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.OriginalTemplateId == tenantZeroDefaultDocumentTypeTemplatePM.Id && d.CountryCode == countryCode).FirstOrDefault();
            if (currentDefaultDocumentTypeTemplatePM != null) return currentDefaultDocumentTypeTemplatePM.Id;
            
            currentDefaultDocumentTypeTemplatePM = allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.CountryCode == countryCode).FirstOrDefault();
            if (currentDefaultDocumentTypeTemplatePM != null) return currentDefaultDocumentTypeTemplatePM.Id;
            
            currentDefaultDocumentTypeTemplatePM = allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => string.IsNullOrEmpty(d.CountryCode)).FirstOrDefault();
            if (currentDefaultDocumentTypeTemplatePM != null) return currentDefaultDocumentTypeTemplatePM.Id;

            return currentTenantDocumentTypeDefaultTemplate;
        }

        private void CopyTenantZeroDocumentTypeTemplateToCurrentTenant(DocumentType currentTenantDocumentType, List<DocumentTypeTemplatePM> allSystemCurrentTenantDocumentTypeTemplatePMs, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM)
        {
            DocumentTypeTemplatePM currentDocumentTypeTemplatePM = allSystemCurrentTenantDocumentTypeTemplatePMs.Where(d => d.OriginalTemplateId == tenantZeroDocumentTypeTemplatePM.Id).FirstOrDefault();
            DocumentTypeTemplate documentTypeTemplate = null;
            if (currentDocumentTypeTemplatePM == null)
            {
                documentTypeTemplate = CreateNewDocumentTypeTemplateToCurrentTenant(currentTenantDocumentType, tenantZeroDocumentTypeTemplatePM, currentDocumentTypeTemplatePM);
            }
            else if (tenantZeroDocumentTypeTemplatePM.LastUpdateDate != currentDocumentTypeTemplatePM.LastUpdateDate)
            {
                documentTypeTemplate = UpdateCurrentTenantDocumentTypeTemplate(currentTenantDocumentType, tenantZeroDocumentTypeTemplatePM, currentDocumentTypeTemplatePM);
            }
        }

        private DocumentTypeTemplate CreateNewDocumentTypeTemplateToCurrentTenant(DocumentType currentTenantDocumentType, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM, DocumentTypeTemplatePM currentDocumentTypeTemplatePM)
        {
            DocumentTypePM documentTypePM = allTenantZeroDocumentTypes.Values.Where(d => d.Id == tenantZeroDocumentTypeTemplatePM.DocumentTypeId && d.Tenant == 0).FirstOrDefault();

            if (!(tenantZeroDocumentTypeTemplatePM.CountryCode == countryCode) && !string.IsNullOrEmpty(tenantZeroDocumentTypeTemplatePM.CountryCode?.Trim())) return null;

            DocumentTypeTemplate newDocumentTypeTemplate = CreateNewInstanceDocumentTypeTemplate(currentTenantDocumentType, tenantZeroDocumentTypeTemplatePM);
            
            allSystemCurrentTenantDocumentTypeTemplatePMs.Add(CreateNewInstanceDocumentTypeTemplatePM(newDocumentTypeTemplate));
            documentTypeTemplateRepository.Add(newDocumentTypeTemplate);
            documentTypeShouldChange = true;

            return newDocumentTypeTemplate;
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
        private DocumentTypeTemplatePM CreateNewInstanceDocumentTypeTemplatePM(DocumentTypeTemplate newDocumentTypeTemplate)
        {
            return new DocumentTypeTemplatePM()
            {
                Id = newDocumentTypeTemplate.Id,
                Tenant = newDocumentTypeTemplate.Tenant,
                TemplateBody = newDocumentTypeTemplate.TemplateBody,
                TemplateType = newDocumentTypeTemplate.TemplateType,
                HorizontalShift = newDocumentTypeTemplate.HorizontalShift,
                InActive = newDocumentTypeTemplate.InActive,
                Description = newDocumentTypeTemplate.Description,
                DocumentTypeId = newDocumentTypeTemplate.Id,
                EditorTool = newDocumentTypeTemplate.EditorTool,
                CountryCode = newDocumentTypeTemplate.CountryCode,
                Subject = newDocumentTypeTemplate.CountryCode,
                Language = newDocumentTypeTemplate.Language,
                OriginalTemplateId = newDocumentTypeTemplate.Id,
                VerticalShift = newDocumentTypeTemplate.VerticalShift,
                InternalRemarks = newDocumentTypeTemplate.InternalRemarks,
                IsEnabledForCustomers = newDocumentTypeTemplate.IsEnabledForCustomers,
                TemplateBodyHtml = newDocumentTypeTemplate.TemplateBodyHtml,
                TemplateFooterHtml = newDocumentTypeTemplate.TemplateFooterHtml,
                TemplateHeaderHtml = newDocumentTypeTemplate.TemplateHeaderHtml,
                TemplateFooterHeight = newDocumentTypeTemplate.TemplateFooterHeight,
                TemplateHeaderHeight = newDocumentTypeTemplate.TemplateHeaderHeight,
                CC = newDocumentTypeTemplate.CC,
                From = newDocumentTypeTemplate.From,
                ReplyTo = newDocumentTypeTemplate.ReplyTo,
                To = newDocumentTypeTemplate.To,
                LastUpdateDate = newDocumentTypeTemplate.LastUpdateDate,
                IsSystem = newDocumentTypeTemplate.IsSystem,
            };
        }

        private DocumentTypeTemplate UpdateCurrentTenantDocumentTypeTemplate(DocumentType currentTenantDocumentType, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM, DocumentTypeTemplatePM currentDocumentTypeTemplatePM)
        {
            if (!(tenantZeroDocumentTypeTemplatePM.CountryCode == countryCode) && !string.IsNullOrEmpty(tenantZeroDocumentTypeTemplatePM.CountryCode?.Trim())) return null;

            DocumentTypeTemplate currentDocumentTypeTemplate = documentTypeTemplateRepository.GetSingleDocumentTypeTemplate(currentDocumentTypeTemplatePM.Id, currentDocumentTypeTemplatePM.Tenant);
            MapCurrentDocumentTypeTemplate(tenantZeroDocumentTypeTemplatePM, currentDocumentTypeTemplate, currentTenantDocumentType, currentDocumentTypeTemplatePM);

            documentTypeTemplateRepository.Update(currentDocumentTypeTemplate);
            documentTypeShouldChange = true;

            return currentDocumentTypeTemplate;
        }

        private void MapCurrentDocumentTypeTemplate(DocumentTypeTemplatePM tenantZeroDocumentTypeTemplatePM, DocumentTypeTemplate currentDocumentTypeTemplate, DocumentType currentTenantDocumentType, DocumentTypeTemplatePM currentDocumentTypeTemplatePM)
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
        }
    }

    public class CopyDocumentTypeTemplateArgs
    {
        public int Tenant { get; set; }
        public Dictionary<string, DocumentTypePM> AllTenantZeroDocumentTypes { get; set; }
        public List<DocumentTypeTemplatePM> AllSystemTenantZeroDocumentTypeTemplatePMs { get; set; }
        public List<DocumentTypeTemplatePM> AllSystemCurrentTenantDocumentTypeTemplatePMs { get; set; }
        public string CountryCode { get; set; }
    }
}
