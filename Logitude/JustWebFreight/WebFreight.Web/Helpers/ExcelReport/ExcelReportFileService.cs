using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;

namespace WebFreight.Web.Helpers.ExcelReport
{
    public class ExcelReportFileService
    {
        private readonly int tenant;

        public ExcelReportFileService(int tenant)
        {
            this.tenant = tenant;
        }

        public byte[] GetReport(bool isNew, string reportTemplateId)
        {
            byte[] fileData = null;
            Document document = null;
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            if (isNew)
            {
                document = documentRepository.GetSingleDocument(tenant, reportTemplateId);
            }
            else
            {
                ReportsTemplatesVersionQuery reportsTemplatesVersionQuery = new ReportsTemplatesVersionQuery(tenant);
                ReportsTemplatesVersionPM reportsTemplatesVersionPM = reportsTemplatesVersionQuery.GetLastReportsTemplatesVersionPMByReportsTemplateId(reportTemplateId, tenant);
                if (reportsTemplatesVersionPM != null)
                {
                    document = documentRepository.GetSingleDocument(reportsTemplatesVersionPM.Tenant, reportsTemplatesVersionPM.ReportDocumentId);
                }
            }
            if (document != null)
            {
                fileData = ReadFieldDataFromDocument(document);
            }

            return fileData;
        }

        private byte[] ReadFieldDataFromDocument(Document document)
        {

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,

            };
            byte[] fileData = storageservice.Read(fileInfo);
            return fileData;

        }
        public void SaveDocumentOnDifferentFile(string reportsTemplateId, byte[] fileData, string userEmail)
        {
            UserRepository userRep = new UserRepository(tenant);
            User loggedUser = userRep.GetSingleUserByEmail(userEmail, tenant);

            ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
            ReportsTemplate reportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(reportsTemplateId, tenant);

            ReportHelper reportHelper = new ReportHelper();
            DocumentFile documentFile = new DocumentFile() { FileName = reportsTemplate.Description, FileData = fileData, Extension = "xml", Folder = "reports", Tenant = tenant };
            Document newDocument = reportHelper.CreateDocumentAndWriteOnStorage(documentFile);

            #region Create Reports Templates Version 
            ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
            ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
            {
                Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                ReportId = reportsTemplate.ReportId,
                CreatedByUserId = loggedUser.Id,
                UpdatedByUserId = loggedUser.Id,
                TemplateId = reportsTemplate.Id,
                Version = (reportsTemplate.CurrentVersion + 1),
                ReportDocumentId = newDocument.Id,
            };
            reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);
            reportsTemplatesVersionRepository.SubmitChanges();
            #endregion

            #region Update Reports Template
            reportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
            reportsTemplate.UpdatedByUserId = loggedUser.Id;
            reportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            reportsTemplateRepository.Update(reportsTemplate);
            reportsTemplateRepository.SubmitChanges();
            #endregion
        }

        public void SaveDocumentOnSameFile(string reportsTemplateId, byte[] fileData)
        {
            ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
            ReportsTemplatesVersion reportsTemplatesVersion = reportsTemplatesVersionRepository.GetLastReportsTemplatesVersionByReportsTemplateId(reportsTemplateId, tenant);
            if (reportsTemplatesVersion != null)
            {
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = reportsTemplatesVersion.ReportDocumentId,
                    FolderName = "reports",
                    Extension = "xml",
                    Tenant = tenant,
                    FileSize = fileData.Length,

                };
                storageservice.Write(fileData, fileInfo);
            }
        }
    }
}