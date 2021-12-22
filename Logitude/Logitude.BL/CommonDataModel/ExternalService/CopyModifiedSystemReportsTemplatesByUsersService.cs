using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.ExternalService
{
    public class CopyModifiedSystemReportsTemplatesByUsersService
    {
        readonly ReportsTemplateRepository reportsTemplateRepository;
        readonly ReportsTemplatesVersionRepository reportsTemplatesVersionRepository;
        readonly DocumentRepository documentRepository;
        readonly ReportRepository reportRepository;
        IBlobService storageservice;
        public CopyModifiedSystemReportsTemplatesByUsersService()
        {
            reportsTemplateRepository = new ReportsTemplateRepository(0);
            reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(0);
            documentRepository = new DocumentRepository(0);
            reportRepository = new ReportRepository(0);
        }

        public void Execute()
        {
            List<ReportsTemplatesVersion> modifiedSystemReportsTemplatesVersions = reportsTemplatesVersionRepository.GetModifiedSystemStimuleReportsTemplatesVersions();
            modifiedSystemReportsTemplatesVersions.ForEach(modifiedSystemReport =>
            {
                CopyReportsTemplate(modifiedSystemReport);
            });

            if(modifiedSystemReportsTemplatesVersions.Count() > 0) SubmitAllChanges();
        }

        private void CopyReportsTemplate(ReportsTemplatesVersion modifiedSystemReportsTemplatesVersion)
        {
            int tenant = modifiedSystemReportsTemplatesVersion.Tenant;
            ReportsTemplate modifiedReportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(modifiedSystemReportsTemplatesVersion.TemplateId, tenant);
            if (modifiedReportsTemplate == null) return;

            ReportsTemplate reportsTemplate = CreateNewReportsTemplate(modifiedReportsTemplate, tenant, modifiedSystemReportsTemplatesVersion.CreatedByUserId);
            string documentId = CreateNewDocument(modifiedSystemReportsTemplatesVersion, tenant);
            CreateNewReportsTemplatesVersion(new ReportsTemplatesVersionArgs
            {
                ModifiedSystemReportsTemplatesVersion = modifiedSystemReportsTemplatesVersion,
                ModifiedReportsTemplate = modifiedReportsTemplate,
                ReportsTemplate = reportsTemplate,
                Tenant = tenant,
                DocumentId = documentId,
            });
            ChangeDefaultTemplateId(modifiedSystemReportsTemplatesVersion, reportsTemplate, tenant);
            UpdateModifiedReportsTemplateAsFixed(modifiedReportsTemplate);
        }

        private ReportsTemplate CreateNewReportsTemplate(ReportsTemplate modifiedReportsTemplate, int tenant, string createdByUserId)
        {
            ReportsTemplate reportsTemplate = new ReportsTemplate()
            {
                Id = IdCounter.GetNumber("ReportsTemplate", tenant).ToString(),
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Description = modifiedReportsTemplate.Description,
                ReportId = modifiedReportsTemplate.ReportId,
                CreatedByUserId = createdByUserId,
                UpdatedByUserId = createdByUserId,
                IsSystem = false,
                InActive = false,
                CurrentVersion = 1,
                TemplateType = modifiedReportsTemplate.TemplateType,
            };
            reportsTemplateRepository.Add(reportsTemplate);

            return reportsTemplate;
        }

        private string CreateNewDocument(ReportsTemplatesVersion modifiedSystemReportsTemplatesVersion, int tenant)
        {
            if (string.IsNullOrEmpty(modifiedSystemReportsTemplatesVersion.ReportDocumentId)) return "";

            Document document = documentRepository.GetSingleDocument(tenant, modifiedSystemReportsTemplatesVersion.ReportDocumentId);
            if (document == null) return "";

            
            return AddDocument(document, tenant);
        }

        public string AddDocument(Document document, int tenant)
        {
            if (document == null) return "";

            string documentId = "";
            Document newDocument = CreateNewDocumentFromExistDocument(document, tenant);
            if (newDocument != null) documentId = newDocument.Id;

            return documentId;
        }

        private Document CreateNewDocumentFromExistDocument(Document document, int tenant)
        {
            byte[] fileData = ReadDocumentFromStorage(document);

            if (fileData == null) return null;

            Document newDocument = new Document
            {
                Id = IdCounter.GetNumber("Document", tenant).ToString(),
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileName = document.FileName,
                FileSize = document.FileSize,
                Extension = document.Extension,
                HasFile = document.HasFile,
                CalculatedFileName = document.CalculatedFileName,
                Folder = document.Folder,

            };
            documentRepository.Add(newDocument);

            SaveNewDocumentIntoStorage(tenant, newDocument, fileData);

            return newDocument;
        }

        private byte[] ReadDocumentFromStorage(Document document)
        {
            storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = "reports",
                Extension = document.Extension,
                Tenant = document.Tenant,

            };
            byte[] fileData = storageservice.Read(fileInfo);
            return fileData;
        }

        private void SaveNewDocumentIntoStorage(int tenant, Document newDocument, byte[] fileData)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = newDocument.Id,
                FolderName = newDocument.Folder,
                Extension = newDocument.Extension,
                Tenant = tenant,
                FileSize = newDocument.FileSize,
                IsEncrypted = true,
            };
            storageservice.Write(fileData, fileInfo);
        }

        private void CreateNewReportsTemplatesVersion(ReportsTemplatesVersionArgs reportsTemplatesVersionArgs)
        {
            ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
            {
                Id = IdCounter.GetNumber("ReportsTemplatesVersion", reportsTemplatesVersionArgs.Tenant).ToString(),
                Tenant = reportsTemplatesVersionArgs.Tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(reportsTemplatesVersionArgs.Tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(reportsTemplatesVersionArgs.Tenant),
                ReportId = reportsTemplatesVersionArgs.ModifiedReportsTemplate.ReportId,
                CreatedByUserId = reportsTemplatesVersionArgs.ModifiedSystemReportsTemplatesVersion.CreatedByUserId,
                UpdatedByUserId = reportsTemplatesVersionArgs.ModifiedSystemReportsTemplatesVersion.CreatedByUserId,
                TemplateId = reportsTemplatesVersionArgs.ReportsTemplate.Id,
                Version = 1,
                ReportDocumentId = !string.IsNullOrEmpty(reportsTemplatesVersionArgs.DocumentId) ? reportsTemplatesVersionArgs.DocumentId : null,
            };

            reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);
        }

        private void ChangeDefaultTemplateId(ReportsTemplatesVersion modifiedSystemReportsTemplatesVersion, ReportsTemplate reportsTemplate, int tenant)
        {
            Report report = reportRepository.GetSingleReport(modifiedSystemReportsTemplatesVersion.ReportId, tenant);
            if (report.DefaultTemplateId != modifiedSystemReportsTemplatesVersion.TemplateId) return;

            report.DefaultTemplateId = reportsTemplate.Id;
            reportRepository.Update(report);

        }

        private void UpdateModifiedReportsTemplateAsFixed(ReportsTemplate modifiedReportsTemplate)
        {
            modifiedReportsTemplate.IsSystemReportFixed = true;
            reportsTemplateRepository.Update(modifiedReportsTemplate);
        }

        private void SubmitAllChanges()
        {
            documentRepository.SubmitChanges();
            reportsTemplateRepository.SubmitChanges();
            reportsTemplatesVersionRepository.SubmitChanges();
            reportRepository.SubmitChanges();
        }
    }

    public class ReportsTemplatesVersionArgs
    {
        public ReportsTemplatesVersion ModifiedSystemReportsTemplatesVersion { get; set; }
        public ReportsTemplate ModifiedReportsTemplate { get; set; }
        public ReportsTemplate ReportsTemplate { get; set; }
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
    }
}
