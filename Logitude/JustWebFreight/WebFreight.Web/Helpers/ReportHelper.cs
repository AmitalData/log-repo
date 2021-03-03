
using HtmlAgilityPack;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers.DataProviderHelpers;
using WebFreight.Web.ReportsWebServices;
using WebFreight.Web.ReportsWebServices.LogitudeReports;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Bluesnap;
using WebFreight.Web.ReportsWebServices.LogitudeReports.CRM;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Operational;
using WebFreight.Web.ReportsWebServices.LogitudeReports.TimeManagement;
using WebFreight.Web.ShipmentPackageModel;
using WebFreight.Web.TaxesApprovalModel;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class ReportHelper
    {
        public void CopyReports(int? tenantNumber = null)
        {

            ReportRepository reportRepository = new ReportRepository(0);
            ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(0);
            ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(0);
            DocumentRepository documentRepository = new DocumentRepository(0);
            ReportModificationRepository reportModificationRep = new ReportModificationRepository(0);


            ContactRepository contactRepository = new ContactRepository(0);
            List<Report> reportList = reportRepository.GetReports(0).ToList();
            TenantQuery tenantQuery = new TenantQuery(0);
            List<TenantList> tenantList = new List<TenantList>();
            string proessType = "all Tenant";

            if (tenantNumber != null)
            {
                proessType = "Sign up";
                tenantList = tenantQuery.GetAllTenantLists().Where(d => d.Id == (int)tenantNumber).ToList();
            }
            else tenantList = tenantQuery.GetAllTenantLists().ToList();

            foreach (TenantList tenant in tenantList)
            {
                try
                {
                    List<Report> myReports = new List<Report>();
                    #region Report
                    if (tenant.Id != 0)
                    {
                        myReports = reportRepository.GetReports(tenant.Id).ToList();

                        foreach (Report report in reportList)
                        {
                            Report repo = myReports.Where(d => d.Code == report.Code && d.Tenant == tenant.Id).FirstOrDefault();
                            if (repo == null)
                            {
                                Report newReport = new Report()
                                {
                                    Id = IdCounter.GetNumber("Report", tenant.Id).ToString(),
                                    Tenant = tenant.Id,
                                    Name = report.Name,
                                    LocalName = report.LocalName,
                                    SearchFields = report.SearchFields,
                                    Code = report.Code,
                                    Description = report.Description,
                                    FilterControlName = report.FilterControlName,
                                    FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                                    InActive = report.InActive,
                                    ReportGroupId = report.ReportGroupId,
                                    FeatureId = report.FeatureId,
                                    FeatureUniqeCode = report.FeatureUniqeCode,
                                    AvailableForScheduling = report.AvailableForScheduling,
                                    DisablePreview = report.DisablePreview,
                                    
                                };
                                reportRepository.Add(newReport);
                                myReports.Add(newReport);

                            }

                        }

                        reportRepository.SubmitChanges();

                    }
                    else
                    {
                        myReports = reportList;

                    }
                    #endregion

                    #region Template and Version 

                    if (reportsTemplateRepository.GetReportsTemplates(tenant.Id).FirstOrDefault() == null)
                    {
                        string userId = contactRepository.GetConactIdByemail("system@tenant" + tenant.Id.ToString() + ".com", tenant.Id);

                        List<ReportModification> modifications = reportModificationRep.GetReportModifications(tenant.Id).ToList();
                        List<Document> documentLists = new List<Document>();

                        #region Full Documents Lists
                        List<string> documentIds = new List<string>();
                        foreach (Report myReport in myReports)
                        {
                            Report report = reportList.Where(D => D.Code == myReport.Code).FirstOrDefault();
                            if (report == null) report = myReport;

                            if (!string.IsNullOrEmpty(report.ReportDocumentId) && !documentIds.Contains(report.ReportDocumentId))
                            {
                                documentIds.Add(report.ReportDocumentId);
                            }

                            ReportModification modification = modifications.Where(d => d.ReportId == report.Id).FirstOrDefault();
                            if (modification != null)
                            {
                                if (!string.IsNullOrEmpty(modification.ReportDocumentId) && !documentIds.Contains(modification.ReportDocumentId))
                                {
                                    documentIds.Add(modification.ReportDocumentId);
                                }
                            }
                        }
                        if (documentIds.Count > 0)
                        {
                            documentLists = documentRepository.GetDocumentsByIds(documentIds);
                        }


                        #endregion

                        if (documentLists.Count > 0)
                        {
                            List<ReportsTemplate> reportsTemplates = new List<ReportsTemplate>();
                            foreach (Report myReport in myReports)
                            {
                                Report report = reportList.Where(D => D.Code == myReport.Code).FirstOrDefault();
                                if (report == null) report = myReport;

                                reportsTemplates = new List<ReportsTemplate>();
                                #region Document

                                string defaultTemplateId = "";

                                //Tenant 0
                                bool IsHaveReportTemplateInTenantZero = false;
                                if (!string.IsNullOrEmpty(report.ReportDocumentId))
                                {
                                    string documentId = AddDocument(documentRepository, report.ReportDocumentId, report.Tenant, tenant.Id, documentLists);
                                    if (!string.IsNullOrEmpty(documentId))
                                    {
                                        IsHaveReportTemplateInTenantZero = true;

                                        defaultTemplateId = AddReportTemplate(myReport.Id, myReport.Name, userId, documentId, tenant.Id, reportsTemplateRepository, reportsTemplatesVersionRepository, reportsTemplates, true, "R");
                                    }
                                }

                                //My Tenant
                                ReportModification modification = modifications.Where(d => d.ReportId == report.Id).FirstOrDefault();
                                if (modification != null)
                                {
                                    string documentId = AddDocument(documentRepository, modification.ReportDocumentId, tenant.Id, tenant.Id, documentLists);
                                    if (!string.IsNullOrEmpty(documentId))
                                    {
                                        string description = myReport.Name;

                                        if (IsHaveReportTemplateInTenantZero)
                                        {
                                            ReportsTemplate reportsTemplate = reportsTemplates.Where(d => d.ReportId == myReport.Id && d.Tenant == myReport.Tenant).FirstOrDefault();
                                            if (reportsTemplate != null) reportsTemplate.Description += " (1)";
                                            description += " (2)";
                                        }

                                        defaultTemplateId = AddReportTemplate(myReport.Id, description, userId, documentId, tenant.Id, reportsTemplateRepository, reportsTemplatesVersionRepository, reportsTemplates, false, "R");
                                    }
                                }

                                if (!string.IsNullOrEmpty(defaultTemplateId))
                                {
                                    myReport.DefaultTemplateId = defaultTemplateId;
                                    reportRepository.Update(myReport);
                                }

                                #endregion


                            }

                            documentRepository.SubmitChanges();
                            reportsTemplateRepository.SubmitChanges();
                            reportsTemplatesVersionRepository.SubmitChanges();
                            reportRepository.SubmitChanges();
                        }

                    }
                    #endregion
                }
                catch (Exception ex)
                {

                    string data = "@ Tenant : " + tenant.Id + "@ Exception : " + ex.Message;
                    AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "Copy Report in " + proessType, null);
                }
            }

        }

   
       //private bool IsUsingFileStreamAndTiffImage(int tenant)
       // {
       //     return (tenant == 1526 || tenant == 1) ? true : false;
       // }

        public string GetSpecificPageFromStimulReportAsBase64(ReportFliter reportFliter)
        {
            string url = "";
            string extension = "tiff"; //IsUsingFileStreamAndTiffImage(reportFliter.tenant) ? "tiff" : "mdc";
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = GetNewBlobFileInfo((reportFliter.ReportKey + "@" + reportFliter.ReportName + extension), extension, reportFliter.tenant);
            byte[] result = storageservice.Read(fileInfo);
            if (result != null)
            {
                url = GetSpecificPageFromTiffImageAsBase64(reportFliter, result);

            }
            return url;
        }






        public string AddReportTemplate(string reportId, string description, string userId, string documentId, int tenant, ReportsTemplateRepository reportsTemplateRepository, ReportsTemplatesVersionRepository reportsTemplatesVersionRepository, List<ReportsTemplate> reportsTemplates, bool isSystem, string templateType)
        {

            #region ReportsTemplate

            ReportsTemplate reportsTemplate = new ReportsTemplate()
            {
                Id = IdCounter.GetNumber("ReportsTemplate", tenant).ToString(),
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Description = description,
                ReportId = reportId,
                CreatedByUserId = userId,
                UpdatedByUserId = userId,
                IsSystem = isSystem,
                InActive = false,
                CurrentVersion = 1,
                TemplateType = templateType,

            };
            reportsTemplateRepository.Add(reportsTemplate);

            if (reportsTemplates != null)
            {
                reportsTemplates.Add(reportsTemplate);
            }
            #endregion

            #region ReportsTemplatesVersion

            ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
            {
                Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                ReportId = reportId,
                CreatedByUserId = userId,
                UpdatedByUserId = userId,
                TemplateId = reportsTemplate.Id,
                Version = 1,
                ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
            };

            reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);

            #endregion

            return reportsTemplate.Id;
        }

        public string AddDocument(DocumentRepository documentRepository, string documentId, int tenant, int mytenant, List<Document> documentLists)
        {
            Document newDocument = null;
            string result = "";
            Document document = documentLists.Where(d => d.Id == documentId && d.Tenant == tenant).FirstOrDefault();
            if (document != null)
            {
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = "reports",
                    Extension = document.Extension,
                    Tenant = document.Tenant,

                };
                byte[] fileData = storageservice.Read(fileInfo);

                if (fileData != null)
                {

                    newDocument = new Document
                    {
                        Id = IdCounter.GetNumber("Document", mytenant).ToString(),
                        Tenant = mytenant,
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(mytenant),
                        FileName = document.FileName,
                        FileSize = document.FileSize,
                        Extension = document.Extension,
                        HasFile = document.HasFile,
                        CalculatedFileName = document.CalculatedFileName,
                        Folder = document.Folder,

                    };
                    documentRepository.Add(newDocument);



                    fileInfo = new BlobFileInfo()
                    {
                        FileName = newDocument.Id,
                        FolderName = newDocument.Folder,
                        Extension = newDocument.Extension,
                        Tenant = mytenant,
                        FileSize = newDocument.FileSize,
                        IsEncrypted = true,
                    };


                    storageservice.Write(fileData, fileInfo);
                }
            }

            if (newDocument != null)
            {
                result = newDocument.Id;
            }


            return result;

        }

        public ReportsTemplate CopyReportsTemplate(string reportsTemplateId, string userId, int tenant)
        {
            ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
            ReportsTemplate originalReportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(reportsTemplateId, tenant);
            ReportsTemplate reportsTemplate = null;
            if (originalReportsTemplate != null)
            {
                reportsTemplate = new ReportsTemplate()
                {
                    Id = IdCounter.GetNumber("ReportsTemplate", tenant).ToString(),
                    Tenant = tenant,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    Description = originalReportsTemplate.Description,
                    ReportId = originalReportsTemplate.ReportId,
                    CreatedByUserId = userId,
                    UpdatedByUserId = userId,
                    IsSystem = false,
                    InActive = false,
                    CurrentVersion = 1,
                    TemplateType = originalReportsTemplate.TemplateType,
                };
                reportsTemplateRepository.Add(reportsTemplate);

                ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
                ReportsTemplatesVersion originalReportsTemplatesVersion = reportsTemplatesVersionRepository.GetLastReportsTemplatesVersionByReportsTemplateId(originalReportsTemplate.Id, tenant);

                #region CreateDocument
                string documentId = "";
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                if (originalReportsTemplatesVersion != null && !string.IsNullOrEmpty(originalReportsTemplatesVersion.ReportDocumentId))
                {

                    Document document = documentRepository.GetSingleDocument(originalReportsTemplatesVersion.Tenant, originalReportsTemplatesVersion.ReportDocumentId);
                    if (document != null)
                    {
                        List<Document> documentLists = new List<Document>();
                        documentLists.Add(document);
                        documentId = AddDocument(documentRepository, document.Id, tenant, tenant, documentLists);

                    }
                }
                #endregion

                ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
                {
                    Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
                    Tenant = tenant,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    ReportId = originalReportsTemplate.ReportId,
                    CreatedByUserId = userId,
                    UpdatedByUserId = userId,
                    TemplateId = reportsTemplate.Id,
                    Version = 1,
                    ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
                };
                reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);


                documentRepository.SubmitChanges();
                reportsTemplateRepository.SubmitChanges();
                reportsTemplatesVersionRepository.SubmitChanges();

            }


            return reportsTemplate;
        }

        public ReportsTemplatesVersion RestoreReportsTemplatesVersion(string reportsTemplatesVersionId, string userId, int tenant)
        {
            ReportsTemplatesVersion reportsTemplatesVersion = null;
            ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
            ReportsTemplatesVersion currentReportsTemplatesVersion = reportsTemplatesVersionRepository.GetSingleReportsTemplatesVersionWithOutInclude(reportsTemplatesVersionId, tenant);
            if (currentReportsTemplatesVersion != null)
            {
                ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
                ReportsTemplate reportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(currentReportsTemplatesVersion.TemplateId, tenant);

                #region CreateDocument
                string documentId = "";
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                if (currentReportsTemplatesVersion != null && !string.IsNullOrEmpty(currentReportsTemplatesVersion.ReportDocumentId))
                {

                    Document document = documentRepository.GetSingleDocument(currentReportsTemplatesVersion.Tenant, currentReportsTemplatesVersion.ReportDocumentId);
                    if (document != null)
                    {
                        List<Document> documentLists = new List<Document>();
                        documentLists.Add(document);
                        documentId = AddDocument(documentRepository, document.Id, tenant, tenant, documentLists);
                        documentRepository.SubmitChanges();

                    }
                }
                #endregion

                #region reportsTemplatesVersion
                reportsTemplatesVersion = new ReportsTemplatesVersion()
                {
                    Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
                    Tenant = tenant,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    ReportId = currentReportsTemplatesVersion.ReportId,
                    CreatedByUserId = userId,
                    UpdatedByUserId = userId,
                    TemplateId = reportsTemplate.Id,
                    Version = (reportsTemplate.CurrentVersion + 1),
                    ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
                    IsRestored = true,
                };
                reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);

                reportsTemplatesVersionRepository.SubmitChanges();
                #endregion


                #region reportsTemplates
                reportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
                reportsTemplate.UpdatedByUserId = userId;
                reportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                reportsTemplateRepository.Update(reportsTemplate);
                reportsTemplateRepository.SubmitChanges();
                #endregion

            }
            return reportsTemplatesVersion;
        }

        public Document CreateDocumentAndWriteOnStorage(string fileName, byte[] fileData, string extension, string folder, int tenant)
        {
            #region Create Document and Write on Storage
            DocumentRepository documentRepository = new DocumentRepository(tenant);

            Document newDocument = new Document
            {
                Id = IdCounter.GetNumber("Document", tenant).ToString(),
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileName = fileName,
                FileSize = fileData.Length,
                Extension = extension,
                HasFile = true,
                CalculatedFileName = fileName,
                Folder = "reports",

            };
            documentRepository.Add(newDocument);
            documentRepository.SubmitChanges();
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = newDocument.Id,
                FolderName = folder,
                Extension = extension,
                Tenant = tenant,
                FileSize = newDocument.FileSize,

            };

            storageservice.Write(fileData, fileInfo);

            #endregion

            return newDocument;
        }

        public void StimulReportSaved(string processType, string reportTemplateId, byte[] fileData, string userId, int tenant)
        {
            if (fileData != null)
            {
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
                ReportsTemplate reportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(reportTemplateId, tenant);

                if (processType == "SaveOnSameDocument")
                {
                    ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
                    ReportsTemplatesVersion reportsTemplatesVersion = reportsTemplatesVersionRepository.GetLastReportsTemplatesVersionByReportsTemplateId(reportTemplateId, tenant);
                    if (reportsTemplatesVersion != null)
                    {
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = reportsTemplatesVersion.ReportDocumentId,
                            FolderName = "reports",
                            Extension = "mrt",
                            Tenant = tenant,
                            FileSize = fileData.Length,

                        };
                        storageservice.Write(fileData, fileInfo);
                    }
                }
                else
                {

                    ReportHelper reportHelper = new ReportHelper();
                    Document newDocument = reportHelper.CreateDocumentAndWriteOnStorage(reportsTemplate.Description, fileData, "mrt", "reports", tenant);

                    #region Create Reports Templates Version 
                    ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
                    ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
                    {
                        Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
                        Tenant = tenant,
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        ReportId = reportsTemplate.ReportId,
                        CreatedByUserId = userId,
                        UpdatedByUserId = userId,
                        TemplateId = reportsTemplate.Id,
                        Version = (reportsTemplate.CurrentVersion + 1),
                        ReportDocumentId = newDocument.Id,
                    };
                    reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);
                    reportsTemplatesVersionRepository.SubmitChanges();
                    #endregion

                    #region Update Reports Template
                    reportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
                    reportsTemplate.UpdatedByUserId = userId;
                    reportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    reportsTemplateRepository.Update(reportsTemplate);
                    reportsTemplateRepository.SubmitChanges();
                    #endregion
                }

            }


        }
        public byte[] LoadDataToStimulReport(string processType, string reportTemplateId, int tenant)
        {
            byte[] fileData = null;
            Document document = null;
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            if (processType == "ReportPreview")
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

        public void UpdateReportLocalNames()
        {
            ReportRepository reportRepository = new ReportRepository(0);
            TenantQuery tenantQuery = new TenantQuery(0);
            List<TenantList> tenantList = new List<TenantList>();
            ContactRepository contactRepository = new ContactRepository(0);

            try
            {

                /// method  1
                List<Report> tenant0ReportListWithLocalNames = reportRepository.GetReportsWithLocalNamesTenant0().ToList();
                List<Report> nonTenant0ReportList = reportRepository.GetReportsExceptTenant0().ToList();

                foreach (Report report in tenant0ReportListWithLocalNames)
                {
                    if (report != null)
                    {
                        List<Report> relatedReports = nonTenant0ReportList.Where(d => d.Code == report.Code).ToList();
                        if (relatedReports != null)
                        {
                            relatedReports.ForEach((elem) =>
                            {
                                elem.LocalName = report.LocalName;
                                reportRepository.Update(elem);
                            });
                            reportRepository.SubmitChanges();
                        }
                    }
                }


                /// method 2
                //List<Report> tenant0ReportList = reportRepository.GetReports(0).ToList();
                //List<Report> nonTenant0ReportList = reportRepository.GetReportsExceptTenant0().ToList();


                //foreach (Report report in nonTenant0ReportList)
                //{
                //    if (report != null)
                //    {
                //        Report t0Report = tenant0ReportList.Find(d => d.Code == report.Code);
                //        if (t0Report != null)
                //        {
                //            report.LocalName = t0Report.LocalName;
                //            reportRepository.Update(report);
                //            reportRepository.SubmitChanges();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new ApplicationException("RPTERR001:" + ex.Message);
            }
        }

        #region StimualReport


        public string GetReportDateTimeFormat(ReportFliter reportFliter)
        {
            ICommonDataContext context = CommonDataContext.GetContext(reportFliter.tenant);
            CommonDataDomainService commonService = new CommonDataDomainService();
            Tenant currentTenant = context.Tenants.Where(t => t.Id == reportFliter.tenant).FirstOrDefault();
            string datetimeformat = @"dd\/MM\/yyyy";
            if (!string.IsNullOrEmpty(currentTenant.DateTimeFormat)) datetimeformat = currentTenant.DateTimeFormat;
            return datetimeformat;


        }

        public  void ReportAuthentication(ReportFliter reportFliter , int tenant)
        {
            if (tenant != reportFliter.tenant)
            {
                throw new Exception("Sorry you’re not authenticated to view this report");
            }

            if (!string.IsNullOrEmpty(reportFliter.UserId))
            {
                UserQuery userQuery = new UserQuery(tenant);
                bool isExist = userQuery.CheckIfUserExistInTenant(reportFliter.UserId, tenant);
                if (!isExist)
                {
                    throw new Exception("Sorry you’re not authenticated to view this report");
                }
            }
        }
        public CustomerPotentialActualDataProvider BuildCustomerPotentialActualDataProvider(ReportFliter reportFliter)
        {
            CustomerPotentialActualDataProvider customerPotentialActualDataProvider = null;
            if (reportFliter != null)
            {
                byte[] filters = GetReportFilters(reportFliter.QueryFilterItemLists);
                byte[] reportDataProvider = BuildReportDataProvider(reportFliter, filters);
                if (reportDataProvider != null)
                {
                    MemoryStream memorystream = new MemoryStream(reportDataProvider);
                    XmlSerializer serializer = new XmlSerializer(typeof(CustomerPotentialActualDataProvider));
                    customerPotentialActualDataProvider = (CustomerPotentialActualDataProvider)serializer.Deserialize(memorystream);
                }
            }
            return customerPotentialActualDataProvider;
        }



        public string BuildStimulReport(ReportFliter reportFliter)
        {
            AdvancedDateResolver advancedDateResolver = new AdvancedDateResolver();
            List<QueryFilterItem> reportFilterItems = advancedDateResolver.ResolveDateValues(reportFliter.QueryFilterItemLists);
            reportFliter.QueryFilterItemLists = reportFilterItems;

            string result = string.Empty;
            StiReport stiReport = GetStimulReportByReportFilter(reportFliter);
            if (stiReport != null)
            {
                result = WriteReportToStorage(reportFliter, stiReport);
            }
            return result;
        }

        public StiReport GetStimulReportByReportFilter(ReportFliter reportFliter)
        {
            StiReport report = null;
            if (reportFliter != null)
            {
                byte[] filters = GetReportFilters(reportFliter.QueryFilterItemLists);
                byte[] reportDataProvider = BuildReportDataProvider(reportFliter, filters);
                if (reportDataProvider == null && reportFliter.IsSchedulerReport) return report;
                ReportsTemplatesWebService reportsTemplatesWebService = new ReportsTemplatesWebService();
                ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(reportFliter.tenant);
                string reportDocumentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateId(reportFliter.DefaultTemplateId, reportFliter.tenant);
                byte[] template = reportsTemplatesWebService.GetReportTemplate(reportDocumentId, reportFliter.tenant, false);
                if (template == null) throw new Exception("Report Template is missing");
                else
                {
                    ReportStimulDataProviderDetails reportStimulDataProviderDetails = GetReportStimulDataProviderDetails(reportDataProvider, reportFliter);
                    report = GetStimulReportByTemplateAndProviderDetails(reportStimulDataProviderDetails, template);
                }
            }
            return report;
        }

        public byte[] GetReportFilters(List<QueryFilterItem> queryFilterItemLists)
        {
            QueryOperations queryOperations = new QueryOperations();
            queryOperations.QueryFilterItems = new System.Collections.Generic.List<QueryFilterItem>();
            if (queryFilterItemLists != null)
            {
                foreach (QueryFilterItem filterItem in queryFilterItemLists)
                {
                    if (filterItem.FieldDataType == "Date")
                    {
                        if (filterItem.FieldValue != null)
                            filterItem.FieldValue = DateTime.Parse(filterItem.FieldValue.ToString());
                    }
                    queryOperations.QueryFilterItems.Add(filterItem);
                }
            }
            FilterSerializer filterSeriazlizer = new FilterSerializer();
            byte[] filters = filterSeriazlizer.SerializeFilterItems(queryOperations);

            return filters;
        }

        public byte[] BuildReportDataProvider(ReportFliter reportFliter, byte[] filters)
        {
            LogitudeReportsWebService logitudeReportsWebService = new LogitudeReportsWebService();
            byte[] dataProvider = null;
            switch (reportFliter.ReportCode)
            {
                #region
                case "ATRE":
                    {
                        dataProvider = logitudeReportsWebService.LoadAutomationTestReportDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "SHEL":
                    {
                        dataProvider=  LoadShipmentsEventsListDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "SHST":
                    {
                        dataProvider = logitudeReportsWebService.LoadShipmentsStocksData(filters, reportFliter.tenant);
                        break;
                    }
                case "SHRR":
                    {
                        ShipperReturnsManager myDataManager = new ShipperReturnsManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }
                case "ERLR":
                    {
                        ExternalReconciliationLinesReportManager ExternalReconciliationManager = new ExternalReconciliationLinesReportManager(filters, reportFliter.tenant);
                        dataProvider = ExternalReconciliationManager.GetData();
                        break;
                    }
                case "URDR":
                    {
                        UserDefinedReportManager UserDefinedReportManager = new UserDefinedReportManager(filters, reportFliter.tenant);
                        dataProvider = UserDefinedReportManager.GetData();
                        break;
                    }
                case "UPTR":
                    {
                        dataProvider = logitudeReportsWebService.LoadUsersByTenantData(filters, reportFliter.tenant);
                        break;
                    }

                case "INVN":
                    {
                        dataProvider = logitudeReportsWebService.LoadInventoryData(filters, reportFliter.tenant);
                        break;
                    }

                case "RACL":
                    {
                        AccountingLedgerManager myDataManager = new AccountingLedgerManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "ASDB":
                    {
                        DashBoardWebService dashBoardWebService = new DashBoardWebService();
                        dataProvider = dashBoardWebService.LoadActivityStatusDetailsData(filters, reportFliter.tenant);
                        break;
                    }

                case "RAAR":
                    {
                        dataProvider = logitudeReportsWebService.LoadAgedAccountsReceivableData(filters, reportFliter.tenant);
                        break;
                    }

                case "EBRP":
                    {
                        dataProvider = logitudeReportsWebService.LoadBookingsData(filters, reportFliter.tenant);
                        break;
                    }

                case "RALS":
                    {
                        dataProvider = logitudeReportsWebService.LoadAirlineStatisticsData(filters, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
                        break;
                    }

                case "RSLS":
                    {
                        dataProvider = logitudeReportsWebService.LoadShippingLineStatisticsData(filters, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
                        break;
                    }

                case "CODT":
                    {
                        dataProvider = logitudeReportsWebService.LoadContainerDetailsVoyageData(filters, reportFliter.tenant);
                        break;
                    }

                case "COTR":
                    {

                        dataProvider = logitudeReportsWebService.LoadContainerTruckingData(filters, reportFliter.tenant);
                        break;
                    }

                case "CUAD":
                    {
                        dataProvider = logitudeReportsWebService.LoadCustomerAdditionalServicesData(filters, reportFliter.tenant);
                        break;
                    }

                case "CUPA":
                    {
                        dataProvider = logitudeReportsWebService.LoadCustomerPotentialActualData(filters, reportFliter.tenant);
                        break;
                    }

                case "EWRP":
                    {
                        dataProvider = logitudeReportsWebService.LoadEAWBsData(filters, reportFliter.tenant);
                        break;
                    }

                case "EXIN":
                    {
                        dataProvider = logitudeReportsWebService.LoadExpectedIncomeData(filters, reportFliter.tenant);
                        break;
                    }

                case "FBRP":
                    {
                        dataProvider = logitudeReportsWebService.LoadFlightBookingData(filters, reportFliter.tenant);
                        break;
                    }

                case "RITS":
                    {
                        dataProvider = logitudeReportsWebService.LoadIATAStatisticsData(filters, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
                        break;
                    }

                case "RIBP":
                    {
                        dataProvider = logitudeReportsWebService.LoadInvoiceByPartnerData(filters, reportFliter.CurrentCurrencyCodeType, reportFliter.DateType, reportFliter.tenant);
                        break;
                    }

                case "RINV":
                    {
                        dataProvider = logitudeReportsWebService.LoadInvoicesData(filters, reportFliter.tenant);
                        break;
                    }

                case "RAPI":
                    {
                        dataProvider = logitudeReportsWebService.LoadAPInvoicesData(filters, reportFliter.tenant);
                        break;
                    }

                case "MCOR":
                    {
                        MonthlyConversionManager myDataManager = new MonthlyConversionManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "OCRP":
                    {
                        RegisterShipmentPackageWebService registerShipmentPackageWebService = new RegisterShipmentPackageWebService();
                        dataProvider = registerShipmentPackageWebService.RegisterShipments(filters, reportFliter.tenant, reportFliter.CustomerId);
                        break;
                    }

                case "PUAC":
                    {
                        dataProvider = logitudeReportsWebService.LoadParticipantsUsersActivitiesData(filters, reportFliter.tenant);
                        break;
                    }

                case "RPRS":
                    {
                        dataProvider = logitudeReportsWebService.LoadProfitByShipmentData(filters, reportFliter.CurrentCurrencyCodeType, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
                        break;
                    }

                case "RQUO":
                    {
                        dataProvider = logitudeReportsWebService.LoadQuotesData(filters, reportFliter.QuoteCustomerTypeCode, reportFliter.tenant);
                        break;
                    }

                case "SCHT":
                case "SCHA":
                    {
                        dataProvider = logitudeReportsWebService.LoadShipmentChargesAnalysisData(filters, reportFliter.tenant);
                        break;
                    }

                case "OPSC":
                    {
                        dataProvider = logitudeReportsWebService.LoadOpportunityStageChangingData(filters, reportFliter.tenant);
                        break;
                    }

                case "RSID":
                    {
                        dataProvider = logitudeReportsWebService.LoadStatementByInvoiceDateData(filters, reportFliter.tenant);
                        break;
                    }

                case "RSTA":
                    {
                        StatementReportManager manager = new StatementReportManager(filters, reportFliter.tenant);
                        dataProvider = manager.GetData();
                        break;
                    }

                case "RSAS":
                    {
                        dataProvider = logitudeReportsWebService.LoadStatementAgingData(filters, reportFliter.tenant);
                        break;
                    }

                case "SBAG":
                    {
                        dataProvider = logitudeReportsWebService.LoadStatisticsByAgentData(filters, reportFliter.tenant);
                        break;
                    }

                case "RCLS":
                    {
                        StatisticsByCustomerManager manager = new StatisticsByCustomerManager(filters, reportFliter.tenant);
                        dataProvider = manager.GetData();
                        break;
                    }

                case "ARID":
                    {
                        AccountingDepositReportManager accountingDepositReportManager = new AccountingDepositReportManager(filters, reportFliter.tenant);
                        dataProvider = accountingDepositReportManager.GetData();
                        break;
                    }

                case "CASS":
                    {
                        dataProvider = logitudeReportsWebService.LoadCASSData(filters, reportFliter.tenant);
                        break;
                    }

                case "SPQS":
                    {
                        ShipmentProfitVSQuoteEstimateManager myDataManager = new ShipmentProfitVSQuoteEstimateManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "AREX":
                    {
                        ArchivoExportadoManager myDataManager = new ArchivoExportadoManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "DSCA":
                    {
                        DetailedShipmentChargesManager myDataManager = new DetailedShipmentChargesManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "INVR":
                    {
                        dataProvider = logitudeReportsWebService.LoadInvoicesVatAndRoutingData(filters, reportFliter.tenant);
                        break;
                    }

                case "EMTS":
                    {
                        //dataProvider = logitudeReportsWebService.LoadEmployeeTimeSheetData(filters, reportFliter.tenant);

                        EmployeeTimeSheetManager myDataManager = new EmployeeTimeSheetManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();

                        break;
                    }

                case "WDTS":
                    {
                        WorkPerDaysProjectManager myDataManager = new WorkPerDaysProjectManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();

                        //var isUsingNewCode = false;
                        //if (isUsingNewCode)
                        //{
                        //    WorkPerDaysProjectManager myDataManager = new WorkPerDaysProjectManager(filters, reportFliter.tenant);
                        //    dataProvider = myDataManager.GetData();
                        //}

                        //else
                        //{
                        //    dataProvider = logitudeReportsWebService.LoadWorkPerDaysProjectData(filters, reportFliter.tenant);
                        //}

                        break;
                    }
                case "WGTS":
                    {
                        WorkPerDaysCategoryManager myDataManager = new WorkPerDaysCategoryManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }
                case "TPTS":
                    {
                        dataProvider = logitudeReportsWebService.LoadTasksWithoutProjectsData(filters, reportFliter.tenant);
                        break;
                    }

                case "AGER":
                    {
                        dataProvider = logitudeReportsWebService.LoadAccountingAgingDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "OSBC":
                    {
                        dataProvider = logitudeReportsWebService.LoadOpenShipmentsByCustomerDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "PTVC":
                    {
                        dataProvider = logitudeReportsWebService.LoadParentVsChildTenantsDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "REXR":
                    {
                        dataProvider = logitudeReportsWebService.LoadRevenueExpenseDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "TRBR":
                    {
                        dataProvider = logitudeReportsWebService.LoadTrailBalanceDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "LICM":
                    {
                        dataProvider = logitudeReportsWebService.LoadLicenseManagementDataProvider(filters, reportFliter.tenant);
                        break;
                    }

                case "SHID":
                    {
                        dataProvider = logitudeReportsWebService.LoadShipmentDetailsDataProvider(filters, reportFliter.tenant);
                        dataProvider = IsDataProviderHaveListWithValues(dataProvider, reportFliter.IsSchedulerReport) ? dataProvider : null;
                        break;
                    }

                case "VDK":
                    {
                        dataProvider = logitudeReportsWebService.LoadVDKDataProvider(filters, reportFliter.tenant);
                        break;
                    }
                case "VEHI":
                    {

                        VehiclesManager myDataManager = new VehiclesManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }
                case "VDCA":
                    {
                        VendorChargesAnalysisManager myDataManager = new VendorChargesAnalysisManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "LTRP":
                    {
                        dataProvider = logitudeReportsWebService.LoadLedgerTransactionDataProvider(filters, reportFliter.tenant);
                        dataProvider = IsDataProviderHaveListWithValues(dataProvider, reportFliter.IsSchedulerReport) ? dataProvider : null;
                        break;
                    }

                case "UNER":
                    {
                        UnicargoExportManager myDataManager = new UnicargoExportManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "FLBM":
                    {
                        FlightBookingsManifestManager myDataManager = new FlightBookingsManifestManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "RCRF":
                    {
                        RacingQuoteManager myDataManager = new RacingQuoteManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }

                case "BSPR":
                    {
                        BluesnapPaymentsReportManager myDataManager = new BluesnapPaymentsReportManager(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }
                    case "CSSR":
                    {
                        dataProvider = logitudeReportsWebService.LoadCustomerStatusDataProvider(filters, reportFliter.tenant);
                        break;
                    }
                    #endregion
            }
            return dataProvider;
        }

        private bool IsDataProviderHaveListWithValues(byte[] dataProvider, bool isScheduler)
        {
            if (isScheduler)
            {
                List<PropertyInfo> properties = dataProvider?.GetType()?.GetProperties()?
                .Where(d => d.GetValue(dataProvider) is IList).ToList();
                foreach (PropertyInfo propInfo in properties)
                {
                    object value = propInfo.GetValue(dataProvider, null);
                    List<object> genericList = (value as IEnumerable<object>)?.Cast<object>()?.ToList();
                    if (genericList != null && genericList.Count() > 0) return true;
                }
                return false;
            }
            return true;
        }

        public ReportStimulDataProviderDetails GetReportStimulDataProviderDetails(byte[] dataProvider , ReportFliter reportFliter)
        {
            ReportStimulDataProviderDetails stimulReportDataProviderDetails  = new ReportStimulDataProviderDetails();
            MemoryStream memorystream = new MemoryStream(dataProvider);
            stimulReportDataProviderDetails.Tenant = reportFliter.tenant;

            switch (reportFliter.ReportCode)
            {
                case "RALS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AirlineStatisticsDataProvider));
                        AirlineStatisticsDataProvider reportDataProvider = (AirlineStatisticsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Airline Statistics", Name = "AirlineStatisticsDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        break;
                    }

                case "RSLS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ShippingLineStatisticsDataProvider));
                        ShippingLineStatisticsDataProvider reportDataProvider = (ShippingLineStatisticsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Shippingline Statistics", Name = "ShippingLineStatisticsDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RPRS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ProfitByShipmentDataProvider));
                        ProfitByShipmentDataProvider reportDataProvider = (ProfitByShipmentDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Profit By Shipment", Name = "ProfitByShipmentDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RCLS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(StatisticsByClientDataProvider));
                        StatisticsByClientDataProvider reportDataProvider = (StatisticsByClientDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Statistics By Customer", Name = "StatisticsByClientDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RSTA":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(StatementDataProvider));
                        StatementDataProvider reportDataProvider = (StatementDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Statement", Name = "StatementDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RIBP":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(InvoicesByPartnerDataProvider));
                        InvoicesByPartnerDataProvider reportDataProvider = (InvoicesByPartnerDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "InvoicesByPartner", Name = "InvoicesByPartnerDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RQUO":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(QuotesDataProvider));
                        QuotesDataProvider reportDataProvider = (QuotesDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Quotes", Name = "QuotesDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RINV":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(InvoiceDataProvider));
                        InvoiceDataProvider reportDataProvider = (InvoiceDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Invoices", Name = "InvoiceDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RAPI":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(InvoiceDataProvider));
                        InvoiceDataProvider reportDataProvider = (InvoiceDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Invoices", Name = "InvoiceDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RAAR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AgedAccountsReceivableDataProvider));
                        AgedAccountsReceivableDataProvider reportDataProvider = (AgedAccountsReceivableDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "AgedAccountsReceivable", Name = "AgedAccountsReceivableDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "ROPF":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(TaxesApproval));
                        TaxesApproval reportDataProvider = (TaxesApproval)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "TaxesApproval", Name = "TaxesApproval", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RITS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(IATAStatisticsDataProvider));
                        IATAStatisticsDataProvider reportDataProvider = (IATAStatisticsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "IATA Statistics", Name = "IATAStatisticsDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RACL":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AccountingLedgerDataProvider));
                        AccountingLedgerDataProvider reportDataProvider = (AccountingLedgerDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Accounting Ledger", Name = "AccountingLedgerDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "OCRP":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(RegisterShipmentPackageDataProvider));
                        RegisterShipmentPackageDataProvider reportDataProvider = (RegisterShipmentPackageDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Shipment Packages", Name = "RegisterShipmentPackageDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "RSID":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(StatementByInvoiceDateDataProvider));
                        StatementByInvoiceDateDataProvider reportDataProvider = (StatementByInvoiceDateDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "StatementByInvoiceDate", Name = "StatementByInvoiceDateDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        

                        break;
                    }

                case "OPSC":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(OpportunityStageChangingDataProvider));
                        OpportunityStageChangingDataProvider reportDataProvider = (OpportunityStageChangingDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "OpportunityStageChanging", Name = "OpportunityStageChangingDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "MCOR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(OpportunityMonthlyConversionDataProvider));
                        OpportunityMonthlyConversionDataProvider reportDataProvider = (OpportunityMonthlyConversionDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "OpportunityMonthlyConversion", Name = "OpportunityMonthlyConversionDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "EXIN":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ExpectedIncomeDataProvider));
                        ExpectedIncomeDataProvider reportDataProvider = (ExpectedIncomeDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ExpectedIncome", Name = "ExpectedIncomeDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "COTR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ContainerTruckingDataProvider));
                        ContainerTruckingDataProvider reportDataProvider = (ContainerTruckingDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ContainerTrucking", Name = "ContainerTruckingDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "CODT":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ContainerDetailsVoyageDataProvider));
                        ContainerDetailsVoyageDataProvider reportDataProvider = (ContainerDetailsVoyageDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ContainerDetailsVoyage", Name = "ContainerDetailsVoyageDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "APOP":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ApprovedOpportunitiesDataProvider));
                        ApprovedOpportunitiesDataProvider reportDataProvider = (ApprovedOpportunitiesDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ApprovedOpportunities", Name = "ApprovedOpportunitiesDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "CUAD":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CustomerAdditionalServicesDataProvider));
                        CustomerAdditionalServicesDataProvider reportDataProvider = (CustomerAdditionalServicesDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "CustomerAdditionalServices", Name = "CustomerAdditionalServicesDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "CUPA":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CustomerPotentialActualDataProvider));
                        CustomerPotentialActualDataProvider reportDataProvider = (CustomerPotentialActualDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "CustomerPotentialActual", Name = "CustomerPotentialActualDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "OPAS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(OpportunitiesAdditionalServicesDataProvider));
                        OpportunitiesAdditionalServicesDataProvider reportDataProvider = (OpportunitiesAdditionalServicesDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "OpportunitiesAdditionalServices", Name = "OpportunitiesAdditionalServicesDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "SBAG":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(StatisticsByAgentDataProvider));
                        StatisticsByAgentDataProvider reportDataProvider = (StatisticsByAgentDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Statistics By Agent", Name = "StatisticsByAgentDataProvider", BusinessObjectValue = reportDataProvider };
                        stimulReportDataProviderDetails.Logo = reportDataProvider.Logo;
                        
                        break;
                    }

                case "ASDB":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(DashBoardDataClass));
                        DashBoardDataClass reportDataProvider = (DashBoardDataClass)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "DashBoardDataClass", Name = "DashBoardDataClass", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "EBRP":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(BookingsDataProvider));
                        BookingsDataProvider reportDataProvider = (BookingsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "e-Booking", Name = "BookingsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "EWRP":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(EAWBsDataProvider));
                        EAWBsDataProvider reportDataProvider = (EAWBsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "e-AWBs", Name = "EAWBsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "FBRP":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(FlightBookingDataProvider));
                        FlightBookingDataProvider reportDataProvider = (FlightBookingDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "FlightBooking", Name = "FlightBookingDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "SCHT":
                case "SCHA":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentChargesAnalysisDataProvider));
                        ShipmentChargesAnalysisDataProvider reportDataProvider = (ShipmentChargesAnalysisDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ShipmentAnalysis", Name = "ShipmentChargesAnalysisDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "PUAC":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ParticipantsUsersActivitiesDataProvider));
                        ParticipantsUsersActivitiesDataProvider reportDataProvider = (ParticipantsUsersActivitiesDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ParticipantsUsersActivities", Name = "ParticipantsUsersActivitiesDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "ARID":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ARInvoiceDepositDataProvider));
                        ARInvoiceDepositDataProvider reportDataProvider = (ARInvoiceDepositDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Bank Deposit", Name = "ARInvoiceDepositDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "CASS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CASSDataProvider));
                        CASSDataProvider reportDataProvider = (CASSDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "CASS", Name = "CASSDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "SPQS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentProfitVSQuoteEstimateDataProvider));
                        ShipmentProfitVSQuoteEstimateDataProvider reportDataProvider = (ShipmentProfitVSQuoteEstimateDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Shipment Profit vs. Quote Estimate", Name = "ShipmentProfitVSQuoteEstimateDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "DSCA":
                case "AREX":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ArchivoExportadoDataProvider));
                        ArchivoExportadoDataProvider reportDataProvider = (ArchivoExportadoDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Archivo Exportado", Name = "ArchivoExportadoDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;

                    }

                case "INVN":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(InventoryDataProvider));
                        InventoryDataProvider reportDataProvider = (InventoryDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Inventory", Name = "InventoryDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "INVR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ARInvoiceIncludeVATRoutingsDataProvider));
                        ARInvoiceIncludeVATRoutingsDataProvider reportDataProvider = (ARInvoiceIncludeVATRoutingsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ARInvoiceIncludeVATRoutings", Name = "ARInvoiceIncludeVATRoutingsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }
                case "EMTS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(EmployeeTimeSheetDataProvider));
                        EmployeeTimeSheetDataProvider reportDataProvider = (EmployeeTimeSheetDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "EmployeeTimeSheet", Name = "EmployeeTimeSheetDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }
                case "WDTS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(WorkDaysPerProjectDataProvider));
                        WorkDaysPerProjectDataProvider reportDataProvider = (WorkDaysPerProjectDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "WorkHoursPerProject", Name = "WorkHoursPerProjectDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "WGTS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(WorkDaysPerCategoryDataProvider));
                        WorkDaysPerCategoryDataProvider reportDataProvider = (WorkDaysPerCategoryDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "WorkPerDaysCategoryManager", Name = "WorkDaysPerCategoryDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "TPTS":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(TasksWithoutProjectsDataProvider));
                        TasksWithoutProjectsDataProvider reportDataProvider = (TasksWithoutProjectsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "TasksWithoutProjects", Name = "TasksWithoutProjectsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }
                case "AGER":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AccountingAgingDataProvider));
                        AccountingAgingDataProvider reportDataProvider = (AccountingAgingDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "AGER", Name = "AccountingAgingDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "LTRP":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LedgerTransactionsDataProvider));
                        LedgerTransactionsDataProvider reportDataProvider = (LedgerTransactionsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "LTRP", Name = "LedgerTransactionsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }
                case "CSSR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CustomerStatusDataProvider));
                        CustomerStatusDataProvider reportDataProvider = (CustomerStatusDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "CSSR", Name = "CustomerStatusDataProvider", BusinessObjectValue = reportDataProvider };

                        break;
                    }
                case "OSBC":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(OpenShipmentsByCustomerDataProvider));
                        OpenShipmentsByCustomerDataProvider reportDataProvider = (OpenShipmentsByCustomerDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "OpenShipmentsByCustomer", Name = "OpenShipmentsByCustomerDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "PTVC":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ParentVsChildTenantsDataProvider));
                        ParentVsChildTenantsDataProvider reportDataProvider = (ParentVsChildTenantsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ParentVsChildTenants", Name = "ParentVsChildTenantsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }
                case "REXR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(RevenueExpenseDataProvider));
                        RevenueExpenseDataProvider reportDataProvider = (RevenueExpenseDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Accounting", Name = "RevenueExpenseDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "TRBR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(RevenueExpenseDataProvider));
                        RevenueExpenseDataProvider reportDataProvider = (RevenueExpenseDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Accounting", Name = "RevenueExpenseDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "UPTR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(UsersByTenantDataProvider));
                        UsersByTenantDataProvider reportDataProvider = (UsersByTenantDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "UsersByTenant", Name = "UsersByTenantDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "LICM":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LicenseManagementDataProvider));
                        LicenseManagementDataProvider reportDataProvider = (LicenseManagementDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "LicenseManagement", Name = "LicenseManagementDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }


                case "SHST":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentsStocksDataProvider));
                        ShipmentsStocksDataProvider reportDataProvider = (ShipmentsStocksDataProvider)serializer.Deserialize(memorystream);
                        //  reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Shipments Stocks", Name = "ShipmentsStocksDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "SHID":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentDetailsDataProvider));
                        ShipmentDetailsDataProvider reportDataProvider = (ShipmentDetailsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ShipmentDetails", Name = "ShipmentDetailsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }


                case "VDK":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(VDKDataProvider));
                        VDKDataProvider reportDataProvider = (VDKDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "VDK", Name = "VDKDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }


                case "VEHI":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(VehiclesDataProvider));
                        VehiclesDataProvider reportDataProvider = (VehiclesDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "Vehicles", Name = "VehiclesDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "VDCA":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(VendorChargesAnalysisDataProvider));
                        VendorChargesAnalysisDataProvider reportDataProvider = (VendorChargesAnalysisDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "VendorChargesAnalysis", Name = "VendorChargesAnalysisDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "UNER":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(UnicargoExportDataProvider));
                        UnicargoExportDataProvider reportDataProvider = (UnicargoExportDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "UnicargoExport", Name = "UnicargoExportDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }


                case "SHEL":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentsEventsListDataProvider));
                        ShipmentsEventsListDataProvider reportDataProvider = (ShipmentsEventsListDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ShipmentsEventsList", Name = "ShipmentsEventsListDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }
                case "SHRR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipperReturnsDataProvider));
                        ShipperReturnsDataProvider reportDataProvider = (ShipperReturnsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ShipperReturns", Name = "ShipperReturnsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "ERLR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ExternalReconciliationLinesReportDataProvider));
                        ExternalReconciliationLinesReportDataProvider reportDataProvider = (ExternalReconciliationLinesReportDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "ERLR", Name = "ExternalReconciliationLinesReportDataProvider", BusinessObjectValue = reportDataProvider };
                        break;
                    }
                case "URDR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(UserDefinedReportDataProvider));
                        UserDefinedReportDataProvider reportDataProvider = (UserDefinedReportDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "URDR", Name = "UserDefinedReportDataProvider", BusinessObjectValue = reportDataProvider };
                        break;
                    }

                case "ATRE":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AutomationTestReportDataProvider));
                        AutomationTestReportDataProvider reportDataProvider = (AutomationTestReportDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "AutomationTestReport", Name = "AutomationTestReportDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "FLBM":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(FlightBookingsManifestDataProvider));
                        FlightBookingsManifestDataProvider reportDataProvider = (FlightBookingsManifestDataProvider)serializer.Deserialize(memorystream);                        
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "FlightBookingsManifest", Name = "FlightBookingsManifestDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }

                case "BSPR":
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(BluesnapPaymentsDataProvider));
                        BluesnapPaymentsDataProvider reportDataProvider = (BluesnapPaymentsDataProvider)serializer.Deserialize(memorystream);
                        reportDataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(stimulReportDataProviderDetails.Tenant);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "BluesnapPayments", Name = "BluesnapPaymentsDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }


                case "RCRF":
                    { 
                        XmlSerializer serializer = new XmlSerializer(typeof(RacingQuoteDataProvider));
                        RacingQuoteDataProvider reportDataProvider = (RacingQuoteDataProvider)serializer.Deserialize(memorystream);
                        stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject() { Category = "RacingQuote", Name = "RacingQuoteDataProvider", BusinessObjectValue = reportDataProvider };
                        
                        break;
                    }
            }
            return stimulReportDataProviderDetails;
        }

        private StiReport GetStimulReportByTemplateAndProviderDetails(ReportStimulDataProviderDetails reportStimulDataProviderDetails, byte[] reportTemplate)
        {
            StiReport report = new StiReport();
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();

            string dllName = exportDocumentHelper.GetDllName(reportTemplate, report);
            byte[] dllData = exportDocumentHelper.GetDllFromStorage(dllName, reportStimulDataProviderDetails.Tenant);
            if (false)
            {
                if (dllData != null && dllData.Count() != 0)
                {
                    report = StiReport.GetReportFromAssembly(dllData);
                    if (reportStimulDataProviderDetails.CurrentBusinessObject != null) exportDocumentHelper.RegBusinessObject(report, reportStimulDataProviderDetails.CurrentBusinessObject);
                    report.NeedsCompiling = false;
                    exportDocumentHelper.AddLogo(report, reportStimulDataProviderDetails.Logo);
                }
                else
                {
                    if (reportStimulDataProviderDetails.CurrentBusinessObject != null) exportDocumentHelper.RegBusinessObject(report, reportStimulDataProviderDetails.CurrentBusinessObject);
                    report.Load(reportTemplate);

                    exportDocumentHelper.AddLogo(report, reportStimulDataProviderDetails.Logo);

                    exportDocumentHelper.SaveDllFileInStorage(reportTemplate, report, reportStimulDataProviderDetails.Tenant);
                }
            }
            else
            {
                if (reportStimulDataProviderDetails.CurrentBusinessObject != null) exportDocumentHelper.RegBusinessObject(report, reportStimulDataProviderDetails.CurrentBusinessObject);
                report.Load(reportTemplate);
                exportDocumentHelper.AddLogo(report, reportStimulDataProviderDetails.Logo);
            }

            report.AutoLocalizeReportOnRun = true;

            if (LogitudeSettings.LogitudeURL != "http://localhost:9996"
                && LogitudeSettings.LogitudeURL != "http://127.0.0.1:81")
            {
                report.ReportCacheMode = StiReportCacheMode.On;
                report.RenderedPages.CacheMode = true;
                report.RenderedPages.CanUseCacheMode = true;
            }
            //report.Culture = "he-IL"; // we can use report globalization to translate lables, google "Glabalization manager stimulsoft" for more
            report.Render(false);
            return report;
        }

        private string WriteReportToStorage(ReportFliter reportFliter, StiReport report)
        {
            SaveStimulReport(reportFliter, report);
            string url = "";
            if (!reportFliter.ReportsRunUsingWR)
            {
                url = ExportStimulaImage(report, reportFliter);
            }
            return url;
        }

        private void SaveStimulReport(ReportFliter reportFliter, StiReport report)
        {
            SaveStimulReportUsingFileStreamByFileType(reportFliter, report, "mdc");

            if (!reportFliter.DisablePreview)
            {
                SaveStimulReportUsingFileStreamByFileType(reportFliter, report,"tiff");
            }
        }


        private void SaveStimulReportUsingFileStreamByFileType(ReportFliter reportFliter, StiReport report , string fileType)
        {
            try
            {
                string tempFilePath = Path.Combine(Path.GetTempPath(), reportFliter.ReportKey + "." + fileType);
                var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
                if (fileType == "mdc") report.SaveDocument(fileStream);
                else if (fileType == "tiff") report.ExportDocument(StiExportFormat.ImageTiff, fileStream, new StiTiffExportSettings() { PageRange = StiPagesRange.All , ImageResolution = 200 });
                else if (fileType == "xlsx") new StiExcel2007ExportService().ExportExcel(report, fileStream, new StiExcel2007ExportSettings() { UseOnePageHeaderAndFooter = true });
                fileStream.Close();
                ReadFileFromStreamFileAndSaveOnStorgeByChunks(tempFilePath, reportFliter, fileType);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SaveStimulReportUsingFileStream to "+ fileType + "file", null, null);
                SaveStimulReportUsingMemoryStreamByFileType(reportFliter, report, fileType);
            }
        }

        private void SaveStimulReportUsingMemoryStreamByFileType(ReportFliter reportFliter, StiReport report, string fileType)
        {
            var stream = new MemoryStream();
            if (fileType == "mdc") report.SaveDocument(stream);
            else if (fileType == "tiff") report.ExportDocument(StiExportFormat.ImageTiff, stream, new StiTiffExportSettings() { PageRange = StiPagesRange.All , ImageResolution = 200 });
            else if (fileType == "xlsx") new StiExcel2007ExportService().ExportExcel(report, stream, new StiExcel2007ExportSettings() { UseOnePageHeaderAndFooter = true });
            if (stream != null)
            {
                byte[] reportData = stream.ToArray();
                if (reportData != null)
                {
                    if (string.IsNullOrEmpty(reportFliter.ReportKey) || !reportFliter.ReportsRunUsingWR) reportFliter.ReportKey = Guid.NewGuid().ToString();
                    BlobFileInfo fileInfo = GetNewBlobFileInfo((reportFliter.ReportKey + "@" + reportFliter.ReportName+ fileType), fileType, reportFliter.tenant);
                    fileInfo.FileSize = reportData.Length;

                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    storageservice.Write(reportData, fileInfo);
                }
            }
        }

        private void ReadFileFromStreamFileAndSaveOnStorgeByChunks(string tempFilePath, ReportFliter reportFliter, string extension)
        {
            BlobFileInfo fileInfo = GetNewBlobFileInfo((reportFliter.ReportKey + "@" + reportFliter.ReportName + extension), extension, reportFliter.tenant);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            List<string> blockIdsList = new List<string>();
            int bufferNumber = 0; long sendSize = 0;
            using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                fileInfo.FileSize = fileStream.Length;
                var buffer = new byte[GetChunkSize(fileStream.Length, sendSize)];
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {  sendSize += buffer.Length;
                    var blockId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    blockIdsList.Add(blockId);
                    storageservice.WriteBlock(buffer, sendSize, blockIdsList.ToArray(), bufferNumber, fileInfo);
                    bufferNumber += 1;
                    buffer = new byte[GetChunkSize(fileStream.Length, sendSize)];
                }
                fileStream.Close();
            }
            File.Delete(tempFilePath);
        }

        private long GetChunkSize(long fileSize ,long sendSize)
        {
            long chunkSize = 1000000;
            if ((fileSize - sendSize) < chunkSize) chunkSize = fileSize - sendSize;

            return chunkSize;
        }

        private BlobFileInfo GetNewBlobFileInfo(string fileName,  string extension ,int tenant)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,
                FolderName = "others",
                Tenant = tenant,
                Extension = extension,
            };
            return fileInfo;
        }

        public string ExportStimulaImage(StiReport stiReport, ReportFliter reportFliter)
        {
            string url = "";
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                stiReport.ExportDocument(StiExportFormat.ImagePng, memoryStream, new StiPngExportSettings() { PageRange = new StiPagesRange(StiRangeType.Pages, reportFliter.NumberOfPage.ToString(), reportFliter.NumberOfPage), ImageResolution = 300, ImageFormat = StiImageFormat.Color });
            }
            catch (Exception ex)
            {
                stiReport.ExportDocument(StiExportFormat.ImagePng, memoryStream, new StiPngExportSettings() { PageRange = new StiPagesRange(StiRangeType.Pages, reportFliter.NumberOfPage.ToString(), reportFliter.NumberOfPage), ImageResolution = 200, ImageFormat = StiImageFormat.Color });
            }

            string base64String = System.Convert.ToBase64String(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);
            reportFliter.PageCount = stiReport.RenderedPages.Count;
            url = "data:image/jpg;base64," + base64String;
            return url;
        }

        private byte[] LoadShipmentsEventsListDataProvider(byte[] filters, int tenant)
        {
           
            LogitudeReportsWebService logitudeReportsWebService = new LogitudeReportsWebService();
            return logitudeReportsWebService.LoadShipmentsEventsListDataProvider(filters, tenant);
        }        

        private bool IsHaveReport(string reportCode)
        {
            if (string.IsNullOrEmpty(reportCode))
            {
                switch (reportCode.ToLower())
                {
                    case "INVN":
                    case "RACL":
                    case "ASDB":
                    case "RAAR":
                    case "EBRP":
                    case "RALS":
                    case "RSLS":
                    case "CODT":
                    case "COTR":
                    case "CUAD":
                    case "CUPA":
                    case "EWRP":
                    case "EXIN":
                    case "FBRP":
                    case "RITS":
                    case "RIBP":
                    case "RINV":
                    case "RAPI":
                    case "MCOR":
                    case "OCRP":
                    case "PUAC":
                    case "RPRS":
                    case "RQUO":
                    case "SCHT":
                    case "SCHA":
                    case "OPSC":
                    case "RSID":
                    case "RSTA":
                    case "RSAS":
                    case "SBAG":
                    case "RCLS":
                    case "ARID":
                    case "CASS":
                    case "SPQS":
                    case "AREX":
                    case "INVR":
                    case "EMTS":
                    case "WDTS":
                    case "TPTS":
                    case "AGER":
                    case "OSBC":
                    case "PTVC":
                    case "LICM":
                    case "LTRP":
                    case "CSSR":

                        return true;

                    default:
                        return false;
                }
            }
            else return false;
        }


        public string GetSpecificPageFromTiffImageAsBase64(ReportFliter reportFliter, byte[] data)
        {
            string url = string.Empty;
            if (data != null)
            {
                System.Drawing.Bitmap bitmapReport = (Bitmap)Image.FromStream(new MemoryStream(data));
                reportFliter.PageCount= bitmapReport.GetFrameCount(FrameDimension.Page);
                bitmapReport.SelectActiveFrame(FrameDimension.Page, reportFliter.NumberOfPage - 1);
                MemoryStream byteStream = new MemoryStream();
                bitmapReport.Save(byteStream, ImageFormat.Jpeg);
                byte[] imagebyte = byteStream.ToArray();
                url = "data:image/jpg;base64," + System.Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
            }
            return url;
        }
        #endregion

        #region UpdateReport

        public void UpdateReports(int tenant)
        {

            ReportRepository reportRepository = new ReportRepository(0);
            ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(0);
            ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(0);
            DocumentRepository documentRepository = new DocumentRepository(0);
            ContactRepository contactRepository = new ContactRepository(0);


            List<Report> reportList = reportRepository.GetReports(0).ToList();
            string userId = contactRepository.GetConactIdByemail("system@tenant" + tenant.ToString() + ".com", tenant);
            List<ReportsTemplate> tenantZeroReportsTemplate = reportsTemplateRepository.GetReportsTemplatesWithOutInclude(0);
            List<ReportsTemplate> myTenantReportsTemplate = reportsTemplateRepository.GetReportsTemplatesWithOutInclude(tenant);

            List<ReportsTemplatesVersion> tenantZeroReportsTemplatesVersionLists = reportsTemplatesVersionRepository.GetReportsTemplatesVersionsByReportsTemplateIds(tenantZeroReportsTemplate.Select(d => d.Id).ToList(), 0);
            List<ReportsTemplatesVersion> myTenantReportsTemplatesVersion = reportsTemplatesVersionRepository.GetReportsTemplatesVersionsByReportsTemplateIds(myTenantReportsTemplate.Select(d => d.Id).ToList(), tenant);
            List<Document> documentLists = documentRepository.GetDocumentsByIds(tenantZeroReportsTemplatesVersionLists.Select(d=>d.ReportDocumentId).ToList());
            List<Report> myReports = new List<Report>();

            if (tenant != 0)
            {
                myReports = reportRepository.GetReports(tenant).ToList();
                bool isChangeReport = false;
                foreach (Report report in reportList)
                {
                    if (myReports.Where(d => d.Code == report.Code && d.Tenant == tenant).FirstOrDefault() == null)
                    {
                        Report newReport = new Report()
                        {
                            Id = IdCounter.GetNumber("Report", tenant).ToString(),
                            Tenant = tenant,
                            Name = report.Name,
                            SearchFields = report.SearchFields,
                            Code = report.Code,
                            Description = report.Description,
                            FilterControlName = report.FilterControlName,
                            FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                            InActive = report.InActive,
                            ReportGroupId = report.ReportGroupId,
                            FeatureId = report.FeatureId,
                            LocalName = report.LocalName,
                            FeatureUniqeCode = report.FeatureUniqeCode,
                            AvailableForScheduling = report.AvailableForScheduling,
                            DisablePreview = report.DisablePreview,
                            
                        };
                        reportRepository.Add(newReport);
                        myReports.Add(newReport);
                        isChangeReport = true;
                    }
                }

                if (isChangeReport) reportRepository.SubmitChanges();


                isChangeReport = false;
                foreach (Report report in reportList)
                {
                    ReportsTemplate systemReportTemplate = tenantZeroReportsTemplate.Where(d => d.ReportId == report.Id &&  d.Id == report.DefaultTemplateId).FirstOrDefault();
                    if (systemReportTemplate != null)
                    {
                        Report myReport = myReports.Where(d => d.Code == report.Code && d.Tenant == tenant).FirstOrDefault();
                        if (myReport != null)
                        {
                            ReportsTemplate myReportsTemplate = myTenantReportsTemplate.Where(d => d.ReportId == myReport.Id && d.IsSystem ).FirstOrDefault();
                            if(myReportsTemplate== null)
                            {
                                myReportsTemplate = myTenantReportsTemplate.Where(d => d.ReportId == myReport.Id && d.Id == myReport.DefaultTemplateId).FirstOrDefault();
                            }

                            if (myReportsTemplate == null)
                            {
                                ReportsTemplatesVersion tenantZeroReportsTemplatesVersion = tenantZeroReportsTemplatesVersionLists.Where(d => d.ReportId == report.Id && d.TemplateId == systemReportTemplate.Id).FirstOrDefault();
                                if(tenantZeroReportsTemplatesVersion != null && !string.IsNullOrEmpty(tenantZeroReportsTemplatesVersion.ReportDocumentId))
                                {
                                    string documentId = AddDocument(documentRepository, tenantZeroReportsTemplatesVersion.ReportDocumentId, report.Tenant, tenant, documentLists);
                                    if (!string.IsNullOrEmpty(documentId))
                                    {
                                        myReport.DefaultTemplateId = AddReportTemplate(myReport.Id, systemReportTemplate.Description, userId, documentId, tenant, reportsTemplateRepository, reportsTemplatesVersionRepository, null, true, "R");
                                        isChangeReport = true;
                                    }
                                }
                            }
                            else
                            {
                                ReportsTemplatesVersion tenantZeroReportsTemplatesVersion = tenantZeroReportsTemplatesVersionLists.Where(d => d.ReportId == report.Id && d.TemplateId == systemReportTemplate.Id).FirstOrDefault();
                                ReportsTemplatesVersion myReportsTemplatesVersion = myTenantReportsTemplatesVersion.Where(d => d.ReportId == myReport.Id && d.TemplateId == myReportsTemplate.Id && d.Version == myReportsTemplate.CurrentVersion).FirstOrDefault();

                                if(tenantZeroReportsTemplatesVersion != null && myReportsTemplatesVersion != null)
                                {
                                    if(tenantZeroReportsTemplatesVersion.UpdateDate > myReportsTemplatesVersion.UpdateDate)
                                    {
                                        string documentId = AddDocument(documentRepository, tenantZeroReportsTemplatesVersion.ReportDocumentId, tenantZeroReportsTemplatesVersion.Tenant, tenant, documentLists);
                                        ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
                                        {
                                            Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
                                            Tenant = tenant,
                                            CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                            UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                            ReportId = myReportsTemplate.ReportId,
                                            CreatedByUserId = userId,
                                            UpdatedByUserId = userId,
                                            TemplateId = myReportsTemplate.Id,
                                            Version = myReportsTemplate.CurrentVersion+1,
                                            ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
                                        };

                                       reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);
                                       myReportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
                                       myReportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                       myReportsTemplate.UpdatedByUserId = userId;
                                        isChangeReport = true;
                                    }
                                }


                            }
                        }
                    }

                }


                if (isChangeReport)
                {
                    documentRepository.SubmitChanges();
                    reportsTemplateRepository.SubmitChanges();
                    reportsTemplatesVersionRepository.SubmitChanges();
                    reportRepository.SubmitChanges();
                }


            }

        }

        #endregion

        public ReportFliter BuildReportDataViewWorkerRole(ReportFliter reportFliter)
        {

            ReportExecutionLogRepository reportExecutionLogRepository = new ReportExecutionLogRepository(reportFliter.tenant);
            reportFliter.ReportKey = Guid.NewGuid().ToString();

            ReportExecutionLog reportExecutionLog = new ReportExecutionLog()
            {
                Id = reportFliter.ReportKey,
                CreateDate = DateTime.Now,
                CreatedByUserId = reportFliter.UserId,
                ReportFilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(reportFliter),
                Tenant = reportFliter.tenant,
                StatusCode = "W",
                ReportId = reportFliter.ReportId,
                ReportTemplateId = reportFliter.DefaultTemplateId,
                DisablePreview = reportFliter.DisablePreview,

            };

            reportExecutionLogRepository.Add(reportExecutionLog);
            reportExecutionLogRepository.SubmitChanges();

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ReportExecutionLogQueue", reportExecutionLog.Tenant);
            queueservice.Send(new Dictionary<string, string>() { { "ReportExecutionLogId", reportExecutionLog.Id }, { "Tenant", reportExecutionLog.Tenant.ToString() } }, reportExecutionLog.Tenant, null, null, null, null);
            return reportFliter;

        }


    }


    public class BuildReportDataResult
    {
       public CustomerPotentialActualDataProvider CustomerPotentialActualDataProvider { get; set; }
       public string UrlImage { get; set; }
    }

    public class ReportStimulDataProviderDetails
    {
        public StiBusinessObject CurrentBusinessObject { get; set; }
        public byte[] Logo { get; set; }
        public int Tenant { get; set; }
    }
}