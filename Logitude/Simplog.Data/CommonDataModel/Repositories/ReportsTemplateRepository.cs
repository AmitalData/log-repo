



using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    
    public class ReportsTemplateRepository : IRepository<ReportsTemplate>
    {
        ICommonDataContext commonDataContext;



        public ReportsTemplateRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ReportsTemplateRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<ReportsTemplate> GetReportsTemplates(int tenant)
        {
            return (from record in context.ReportsTemplates.Include("Report") where record.Tenant == tenant && string.IsNullOrEmpty(record.EntityId) select record);
        }

        public ReportsTemplate GetSingleReportsTemplate(string id, int tenant)
        {
            return (from record in context.ReportsTemplates.Include("Report") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public ReportsTemplate GetSingleReportsTemplateByReprotId(string reportId, int tenant)
        {
            return (from record in context.ReportsTemplates where record.ReportId == reportId && record.Tenant == tenant && string.IsNullOrEmpty(record.EntityId) select record).FirstOrDefault();
        }

        public List<ReportsTemplate> GetReportsTemplatesWithOutInclude(int tenant)
        {
            return (from record in context.ReportsTemplates where record.Tenant == tenant && string.IsNullOrEmpty(record.EntityId) select record).ToList();
        }

        public void Add(ReportsTemplate entity)
        {
            context.ReportsTemplates.Add(entity);
        }

        public void Remove(ReportsTemplate entity)
        {
            try
            {
                context.ReportsTemplates.Attach(entity);
            }
            catch { };
            context.ReportsTemplates.Remove(entity);
        }

        public void Update(ReportsTemplate entity)
        {
            try
            {
                context.ReportsTemplates.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<ReportsTemplate> All()
        {
            return context.ReportsTemplates.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ReportsTemplate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ReportsTemplate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ReportsTemplate> GetMessageTemplatesByMessageTemplateIds(List<string> messageTemplateIds, int tenant)
        {
            return from a in context.ReportsTemplates
                   where a.Tenant == tenant && messageTemplateIds.Contains(a.Id)
                   select a;
        }

    }
}