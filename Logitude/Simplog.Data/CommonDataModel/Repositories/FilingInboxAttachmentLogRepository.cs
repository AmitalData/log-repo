using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FilingInboxAttachmentLogRepository : IRepository<FilingInboxAttachmentLog>
    {
        ICommonDataContext commonDataContext;
        public FilingInboxAttachmentLogRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public FilingInboxAttachmentLogRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FilingInboxAttachmentLog GetSingleFilingInboxAttachmentLog(string id, int tenant)
        {
            return (from a in context.FilingInboxAttachmentLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FilingInboxAttachmentLog> GetFilingInboxAttachmentLogs(int tenant)
        {
            return (from a in context.FilingInboxAttachmentLogs
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(FilingInboxAttachmentLog entity)
        {
            context.FilingInboxAttachmentLogs.Add(entity);
        }

        public void Remove(FilingInboxAttachmentLog entity)
        {
            context.FilingInboxAttachmentLogs.Attach(entity);
            context.FilingInboxAttachmentLogs.Remove(entity);
        }

        public void Update(FilingInboxAttachmentLog entity)
        {
            context.FilingInboxAttachmentLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FilingInboxAttachmentLog> All()
        {
            return context.FilingInboxAttachmentLogs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FilingInboxAttachmentLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FilingInboxAttachmentLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
