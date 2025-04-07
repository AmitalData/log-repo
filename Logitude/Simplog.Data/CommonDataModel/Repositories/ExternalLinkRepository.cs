using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ExternalLinkRepository : IRepository<ExternalLink>
    {
        ICommonDataContext iContext;

        public ExternalLinkRepository(int tenant)
        {
            iContext = CommonDataContext.GetContext(tenant);
        }

        public ExternalLinkRepository(ICommonDataContext context)
        {
            iContext = context;
        }

        public ExternalLink GetSingleExternalLink(string id, int tenant) => GetSingle(id);
        public ExternalLink GetSingleExternalLink(string id) => GetSingle(id);

        public ExternalLink GetSingleExternalLinkByRef(string referenceId) => 
            Context.ExternalLinks.FirstOrDefault(a => a.Ref == referenceId);

        public ExternalLink GetSingle(string id) =>
            Context.ExternalLinks.FirstOrDefault(a => a.Id == id);

        public IQueryable<ExternalLink> GetExternalLinks(int tenant) => GetExternalLinks();
        public IQueryable<ExternalLink> GetExternalLinks()
        {
            return (from a in Context.ExternalLinks select a);
        }

        public IQueryable<ExternalLink> GetAll()
        {
            return (from a in Context.ExternalLinks select a);
        }

        public void Add(ExternalLink entity)
        {
            Context.ExternalLinks.Add(entity);
        }

        public void Remove(ExternalLink entity)
        {
            Context.ExternalLinks.Attach(entity);
            Context.ExternalLinks.Remove(entity);
        }

        public void Update(ExternalLink entity)
        {
            Context.ExternalLinks.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<ExternalLink> All()
        {
            return Context.ExternalLinks.ToList();
        }

        public ICommonDataContext Context
        {
            get { return iContext; }
        }

        public bool SubmitChanges()
        {
            return Context.SaveChanges() > 0;
        }

        public List<ExternalLink> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ExternalLink GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        void IRepository<ExternalLink>.SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}
