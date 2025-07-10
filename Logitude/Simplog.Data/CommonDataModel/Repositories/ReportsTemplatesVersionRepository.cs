
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ReportsTemplatesVersionRepository : IRepository<ReportsTemplatesVersion>
    {
        ICommonDataContext commonDataContext;

        public ReportsTemplatesVersionRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ReportsTemplatesVersionRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ReportsTemplatesVersionRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<ReportsTemplatesVersion> GetReportsTemplatesVersions(int tenant)
        {
            return (from record in context.ReportsTemplatesVersions.Include("Document").Include("ReportsTemplate").Include("Report") where record.Tenant == tenant select record);
        }

        public ReportsTemplatesVersion GetSingleReportsTemplatesVersion(string id, int tenant)
        {
            return (from record in context.ReportsTemplatesVersions.Include("Document").Include("ReportsTemplate").Include("Report") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        public ReportsTemplatesVersion GetSingleReportsTemplatesVersionWithOutInclude(string id, int tenant)
        {
            return (from record in context.ReportsTemplatesVersions where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public ReportsTemplatesVersion GetLastReportsTemplatesVersionByReportsTemplateId(string reportsTemplateId, int tenant)
        {
            return (from record in context.ReportsTemplatesVersions where record.TemplateId == reportsTemplateId && record.Tenant == tenant select record).OrderByDescending(d=>d.Version).FirstOrDefault();
        }

        public void Add(ReportsTemplatesVersion entity)
        {
            context.ReportsTemplatesVersions.Add(entity);
        }

        public void Remove(ReportsTemplatesVersion entity)
        {
            try
            {
                context.ReportsTemplatesVersions.Attach(entity);
            }
            catch { };
            context.ReportsTemplatesVersions.Remove(entity);
        }

        public void Update(ReportsTemplatesVersion entity)
        {
            try
            {
                context.ReportsTemplatesVersions.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<ReportsTemplatesVersion> All()
        {
            return context.ReportsTemplatesVersions.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ReportsTemplatesVersion> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ReportsTemplatesVersion GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public string GetReportDocumentIdByReportTemplateId(string templateId, int tenant)
        {
    
            string result = (from a in context.ReportsTemplatesVersions
                        where a.Tenant == tenant && a.TemplateId == templateId
                        select a).OrderByDescending(d => d.Version).Select(d=>d.ReportDocumentId).FirstOrDefault();
       
            return result;
        }


        public string GetReportDocumentIdByReportTemplateIdAndVersion(string templateId, int version, int tenant)
        {
            string result = "";
            ReportsTemplatesVersion templateVersion = (from a in context.ReportsTemplatesVersions
                                                       where a.Tenant == tenant && a.TemplateId == templateId && a.Version == version
                                                       select a).FirstOrDefault();
            if (templateVersion != null)
            {
                result = templateVersion.ReportDocumentId;
            }
            return result;
        }



        public List<ReportsTemplatesVersion> GetReportsTemplatesVersionsByReportsTemplateIds(List<string> reportsTemplateIds, int tenant)
        {
            return (from record in context.ReportsTemplatesVersions
                 where record.Tenant == tenant && reportsTemplateIds.Contains(record.TemplateId)
                 group record by record.TemplateId into grp
                 select grp.OrderByDescending(d => d.Version).FirstOrDefault()).ToList();
        }

        public List<ReportsTemplatesVersion> GetModifiedSystemStimuleReportsTemplatesVersions()
        {
            return (from record in context.ReportsTemplatesVersions.Include("ReportsTemplate").Include("CreatedByUser")
                    where record.Tenant != 0 && record.ReportsTemplate.IsSystem == true && !record.ReportsTemplate.IsSystemReportFixed && record.ReportsTemplate.TemplateType == "R" && record.CreatedByUser.Contact.EnglishName != "System"
                    group record by record.TemplateId into grp
                    select grp.OrderByDescending(d => d.Version).FirstOrDefault()).ToList();
        }
    }
}