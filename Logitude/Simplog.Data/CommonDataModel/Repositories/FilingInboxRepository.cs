using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FilingInboxRepository : IRepository<FilingInbox>
    {
        ICommonDataContext commonDataContext;
        public FilingInboxRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public FilingInboxRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FilingInbox GetSingleFilingInbox(string id, int tenant)
        {
            return (from a in context.FilingInboxes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FilingInbox> GetFilingInboxes(int tenant)
        {
            return (from a in context.FilingInboxes
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(FilingInbox entity)
        {
            context.FilingInboxes.Add(entity);
        }

        public void Remove(FilingInbox entity)
        {
            context.FilingInboxes.Attach(entity);
            context.FilingInboxes.Remove(entity);
        }

        public void Update(FilingInbox entity)
        {
            context.FilingInboxes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FilingInbox> All()
        {
            return context.FilingInboxes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FilingInbox> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FilingInbox GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
