using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ReportExecutionLogRepository : IRepository<ReportExecutionLog>
    {
        ICommonDataContext commonDataContext;

        public ReportExecutionLogRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ReportExecutionLogRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ReportExecutionLogRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<ReportExecutionLog> GetReportExecutionLogs(int tenant)
        {
            if (tenant != 0)
            {
                return (from record in context.ReportExecutionLogs where record.Tenant == tenant select record);
            }
            else
            {
                return (from record in context.ReportExecutionLogs select record);
            }
        }
        // used by generated controller
        public ReportExecutionLog GetSingleReportExecutionLog(string id, int tenant)
        {
            var result = (from record in context.ReportExecutionLogs.Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("Report") where record.Id == id select record).FirstOrDefault();
            return result;
        }

        public ReportExecutionLog GetReportExecutionLog(string id, int tenant)
        {
            return (from record in context.ReportExecutionLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        public ReportExecutionLog GetReportExecutionLog(string id)
        {
            return (from record in context.ReportExecutionLogs where record.Id == id select record).FirstOrDefault();
        }


        public void Add(ReportExecutionLog entity)
        {
            entity.SearchFields = entity.ReportId;
            context.ReportExecutionLogs.Add(entity);
        }

        public void Remove(ReportExecutionLog entity)
        {
            try
            {
                context.ReportExecutionLogs.Attach(entity);
            }
            catch { };
            context.ReportExecutionLogs.Remove(entity);
        }

        public void Update(ReportExecutionLog entity)
        {
            try
            {
                context.ReportExecutionLogs.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<ReportExecutionLog> All()
        {
            return context.ReportExecutionLogs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ReportExecutionLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ReportExecutionLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}