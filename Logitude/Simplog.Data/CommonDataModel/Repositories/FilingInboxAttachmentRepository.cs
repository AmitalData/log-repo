using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FilingInboxAttachmentRepository : IRepository<FilingInboxAttachment>
    {
        ICommonDataContext commonDataContext;
        public FilingInboxAttachmentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public FilingInboxAttachmentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FilingInboxAttachment GetSingleFilingInboxAttachment(string id, int tenant)
        {
            return (from a in context.FilingInboxAttachments
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FilingInboxAttachment> GetFilingInboxAttachments(int tenant)
        {
            return (from a in context.FilingInboxAttachments
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(FilingInboxAttachment entity)
        {
            context.FilingInboxAttachments.Add(entity);
        }

        public void Remove(FilingInboxAttachment entity)
        {
            context.FilingInboxAttachments.Attach(entity);
            context.FilingInboxAttachments.Remove(entity);
        }

        public void Update(FilingInboxAttachment entity)
        {
            context.FilingInboxAttachments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FilingInboxAttachment> All()
        {
            return context.FilingInboxAttachments.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FilingInboxAttachment> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FilingInboxAttachment GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
