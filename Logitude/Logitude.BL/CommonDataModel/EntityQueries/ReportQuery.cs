using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ReportQuery
    {
        ReportRepository repository;



        public ReportQuery(int tenant)
        {
            repository = new ReportRepository(tenant);
        }

        public ReportQuery(ReportRepository reportRepository)
        {
            repository = reportRepository;
        }

        public ReportPM GetSinglePM(string id, int tenant)
        {
            Report report = repository.GetSingleReport(id);
            ReportPM entity = new ReportPM()
                               {
                                   Id = report.Id,
                                   Name = report.Name,
                                   LocalName = report.LocalName,
                                   FilterControlName = report.FilterControlName,
                                   Description = report.Description,
                                   SearchFields = report.SearchFields,
                                   Code = report.Code,
                                   ReportGroupId = report.ReportGroupId,
                                   FeatureId = report.FeatureId,
                                   FeatureCode = report.Feature != null ? report.Feature.Code : null,
                                   ReportDocumentId = report.ReportDocumentId,
                                   InActive = report.InActive,
                                   Tenant = report.Tenant,
                                   FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                                   DefaultTemplateId = report.DefaultTemplateId,
                                   DefaultMessageTemplateId = report.DefaultMessageTemplateId,
                                   FeatureUniqeCode = report.FeatureUniqeCode,
                                   AvailableForScheduling = report.AvailableForScheduling,
                                   DisablePreview = report.DisablePreview,
                                   DefaultExcelTemplateId = report.DefaultExcelTemplateId,
                                   IsExcelReportAllowed = report.IsExcelReportAllowed,
            };

            ReportModificationRepository modificationRep = new ReportModificationRepository(tenant);
            ReportModification modification = modificationRep.GetSingleReportModification(entity.Id, tenant);
            if (modification != null)
            {
                entity.ReportDocumentId = modification.ReportDocumentId;
            }
            if (!string.IsNullOrEmpty(entity.ReportDocumentId))
            {
                entity.hasTemplate = true;
            }
            ReportPM securedPm = new ReportPM();
            SecuredMapping.GetMappedPM(entity, securedPm, "Report", tenant);

            return securedPm;
        }

        public IQueryable<ReportPM> GetReportPMsByTenant(int tenant)
        {
            IQueryable<ReportPM> reports = from a in repository.context.Reports.Include("Feature")
                                           where a.Tenant == tenant
                                           select new ReportPM()
                                            {
                                                Id = a.Id,
                                                Name = a.Name,
                                                LocalName = a.LocalName,
                                               FilterControlName = a.FilterControlName,
                                                Description = a.Description,
                                                SearchFields = a.SearchFields,
                                                Code = a.Code,
                                                ReportGroupId = a.ReportGroupId,
                                                FeatureId = a.FeatureId,
                                                FeatureCode = a.Feature != null ? a.Feature.Code : null,
                                                InActive = a.InActive,
                                                Tenant = a.Tenant,
                                               FilterHtmlComponentUrl = a.FilterHtmlComponentUrl,
                                               DefaultTemplateId =a.DefaultTemplateId,
                                               DefaultMessageTemplateId = a.DefaultMessageTemplateId,
                                               FeatureUniqeCode = a.FeatureUniqeCode,
                                               AvailableForScheduling = a.AvailableForScheduling,
                                               DisablePreview = a.DisablePreview,
                                               DefaultExcelTemplateId = a.DefaultExcelTemplateId,
                                               IsExcelReportAllowed = a.IsExcelReportAllowed,
                                           };
            return reports;
        }

        public ReportPM GetReportByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Reports.Include("Feature")
                         where a.Tenant == tenant && a.Name == name
                         select new ReportPM()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             LocalName = a.LocalName,
                             FilterControlName = a.FilterControlName,
                             Description = a.Description,
                             SearchFields = a.SearchFields,
                             Code = a.Code,
                             ReportGroupId = a.ReportGroupId,
                             FeatureId = a.FeatureId,
                             FeatureCode = a.Feature != null ? a.Feature.Code : null,
                             InActive = a.InActive,
                             Tenant = a.Tenant,
                             FilterHtmlComponentUrl = a.FilterHtmlComponentUrl,
                             DefaultTemplateId = a.DefaultTemplateId,
                             DefaultMessageTemplateId = a.DefaultMessageTemplateId,
                             FeatureUniqeCode = a.FeatureUniqeCode,
                             AvailableForScheduling = a.AvailableForScheduling,
                             DisablePreview = a.DisablePreview,
                             DefaultExcelTemplateId = a.DefaultExcelTemplateId,
                             IsExcelReportAllowed = a.IsExcelReportAllowed,
                         }).FirstOrDefault();

            return query;
        }

        public IQueryable<ReportList> GetIQueryableEntityList(IQueryable<Report> iQueryable)
        {
            IQueryable<ReportList> result = from report in iQueryable.Include("Feature")
                                            select new ReportList()
                                            {
                                                Id = report.Id,
                                                Name = report.Name,
                                                LocalName = report.LocalName,
                                                FilterControlName = report.FilterControlName,
                                                Description = report.Description,
                                                SearchFields = report.SearchFields,
                                                Code = report.Code,
                                                ReportGroupId = report.ReportGroupId,
                                                FeatureId = report.FeatureId,
                                                FeatureCode = report.Feature != null ? report.Feature.Code : null,
                                                InActive = report.InActive,
                                                Tenant = report.Tenant,
                                                FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                                                DefaultTemplateId = report.DefaultTemplateId,
                                                DefaultMessageTemplateId = report.DefaultMessageTemplateId,
                                                FeatureUniqeCode = report.FeatureUniqeCode,
                                                AvailableForScheduling = report.AvailableForScheduling,
                                                DisablePreview = report.DisablePreview,
                                                DefaultExcelTemplateId = report.DefaultExcelTemplateId,
                                                IsExcelReportAllowed = report.IsExcelReportAllowed,
                                            };
            return result;
        }

        public List<ReportList> GetReportsWithModifications(int tenant)
        {
            ReportModificationRepository reportModificationRep = new ReportModificationRepository(tenant);
            IQueryable<Report> reports = repository.GetReports(tenant);
            List<ReportModification> modifications = reportModificationRep.GetReportModifications(tenant).ToList();

            List<ReportList> result = (from report in reports.Include("Feature")
                                       select new ReportList()
                                       {
                                           Id = report.Id,
                                           Name = report.Name,
                                           LocalName = report.LocalName,
                                           FilterControlName = report.FilterControlName,
                                           Description = report.Description,
                                           SearchFields = report.SearchFields,
                                           Code = report.Code,
                                           ReportGroupId = report.ReportGroupId,
                                           FeatureId = report.FeatureId,
                                           FeatureCode = report.Feature != null ? report.Feature.Code : null,
                                           ReportDocumentId = report.ReportDocumentId,
                                           InActive = report.InActive,
                                           Tenant = report.Tenant,
                                           FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                                           DefaultTemplateId = report.DefaultTemplateId,
                                           DefaultMessageTemplateId = report.DefaultMessageTemplateId,
                                           FeatureUniqeCode = report.FeatureUniqeCode,
                                           AvailableForScheduling = report.AvailableForScheduling,
                                           DisablePreview = report.DisablePreview,
                                           DefaultExcelTemplateId = report.DefaultExcelTemplateId,
                                           IsExcelReportAllowed = report.IsExcelReportAllowed,

                                       }).ToList();

            foreach (ReportList report in result)
            {
                ReportModification modification = modifications.Where(d => d.ReportId == report.Id).FirstOrDefault();
                if (modification != null)
                {
                    report.ReportDocumentId = modification.ReportDocumentId;
                }
            }
            return result;
        }

        public Report GetFirstReportForTenant(int tenant)
        {
            return (from a in repository.context.Reports
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public List<ReportList> GetReportListsByGroupId(string groupId, int tenant)
        {
            ReportModificationRepository reportModificationRep = new ReportModificationRepository(tenant);
            IQueryable<Report> reports = repository.GetReports(tenant);
            List<ReportModification> modifications = reportModificationRep.GetReportModifications(tenant).ToList();

            List<ReportList> result = (from report in reports.Include("Feature")
                                       where report.ReportGroupId == groupId
                                       select new ReportList()
                                       {
                                           Id = report.Id,
                                           Name = report.Name,
                                           LocalName = report.LocalName,
                                           FilterControlName = report.FilterControlName,
                                           Description = report.Description,
                                           SearchFields = report.SearchFields,
                                           Code = report.Code,
                                           ReportGroupId = report.ReportGroupId,
                                           FeatureId = report.FeatureId,
                                           FeatureCode = report.Feature != null ? report.Feature.Code : null,
                                           ReportDocumentId = report.ReportDocumentId,
                                           InActive = report.InActive,
                                           Tenant = report.Tenant,
                                           FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                                           DefaultTemplateId = report.DefaultTemplateId,
                                           DefaultMessageTemplateId = report.DefaultMessageTemplateId,
                                           FeatureUniqeCode = report.FeatureUniqeCode,
                                           AvailableForScheduling = report.AvailableForScheduling,
                                           DisablePreview = report.DisablePreview,
                                           DefaultExcelTemplateId = report.DefaultExcelTemplateId,
                                           IsExcelReportAllowed = report.IsExcelReportAllowed,
                                       }).ToList();

            foreach (ReportList report in result)
            {
                ReportModification modification = modifications.Where(d => d.ReportId == report.Id).FirstOrDefault();
                if (modification != null)
                {
                    report.ReportDocumentId = modification.ReportDocumentId;
                }
            }
            return result;
        }

        public ReportList GetReportByCode(string code, int tenant)
        {
            ReportModificationRepository reportModificationRep = new ReportModificationRepository(tenant);
            IQueryable<Report> reports = repository.GetReports(tenant);
            List<ReportModification> modifications = reportModificationRep.GetReportModifications(tenant).ToList();

            ReportList result = (from report in reports.Include("Feature")
                                       where report.Code == code
                                       select new ReportList()
                                       {
                                           Id = report.Id,
                                           Name = report.Name,
                                           LocalName = report.LocalName,
                                           FilterControlName = report.FilterControlName,
                                           Description = report.Description,
                                           SearchFields = report.SearchFields,
                                           Code = report.Code,
                                           ReportGroupId = report.ReportGroupId,
                                           FeatureId = report.FeatureId,
                                           FeatureCode = report.Feature != null ? report.Feature.Code : null,
                                           ReportDocumentId = report.ReportDocumentId,
                                           InActive = report.InActive,
                                           Tenant = report.Tenant,
                                           FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                                           DefaultTemplateId = report.DefaultTemplateId,
                                           DefaultMessageTemplateId = report.DefaultMessageTemplateId,
                                           FeatureUniqeCode = report.FeatureUniqeCode,
                                           AvailableForScheduling = report.AvailableForScheduling,
                                           DisablePreview = report.DisablePreview,
                                           DefaultExcelTemplateId = report.DefaultExcelTemplateId,
                                           IsExcelReportAllowed = report.IsExcelReportAllowed,
                                       }).FirstOrDefault();

            ReportModification modification = modifications.Where(d => d.ReportId == result.Id).FirstOrDefault();
            if (modification != null)
            {
                result.ReportDocumentId = modification.ReportDocumentId;
            }
            return result;
        }

        public Report GetReportOnlyByCode(string code, int tenant)
        {
            return (from a in repository.context.Reports
                    where a.Tenant == tenant && a.Code == code && !a.InActive
                    select a).FirstOrDefault();
        }

        public List<ReportList> GetReportListsByGroupIdAndTenant(string groupId, int tenant)
        {
 
            IQueryable<Report> reports = repository.GetReports(tenant);
      

            List<ReportList> result = (from report in reports.Include("Feature")
                                       where report.ReportGroupId == groupId
                                       select new ReportList()
                                       {
                                           Id = report.Id,
                                           Name = report.Name,
                                           LocalName = report.LocalName,
                                           FilterControlName = report.FilterControlName,
                                           Description = report.Description,
                                           SearchFields = report.SearchFields,
                                           Code = report.Code,
                                           ReportGroupId = report.ReportGroupId,
                                           FeatureId = report.FeatureId,
                                           FeatureCode = report.Feature != null ? report.Feature.Code : null,
                                           ReportDocumentId = report.ReportDocumentId,
                                           InActive = report.InActive,
                                           Tenant = report.Tenant,
                                           FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
                                           DefaultTemplateId = report.DefaultTemplateId,
                                           DefaultMessageTemplateId = report.DefaultMessageTemplateId,
                                           FeatureUniqeCode = report.FeatureUniqeCode,
                                           AvailableForScheduling = report.AvailableForScheduling,
                                           DisablePreview = report.DisablePreview,
                                           DefaultExcelTemplateId = report.DefaultExcelTemplateId,
                                           IsExcelReportAllowed = report.IsExcelReportAllowed,
                                       }).ToList();

          
            return result;
        }
    
        public string GetReportCodeById(string id, int tenant)
        {
            return repository.GetReportCodeById(id, tenant);
        }

    }
}
