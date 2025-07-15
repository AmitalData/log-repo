using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ExternalLinkRepository : IRepository<ExternalLink>
    {
        public readonly ICommonDataContext Context;

        public ExternalLinkRepository(int tenant)
        {
            Context = CommonDataContext.GetContext(tenant);
        }

        public ExternalLinkRepository(ICommonDataContext context)
        {
            Context = context;
        }

        public ExternalLink GetSingleExternalLink(string id, int tenant) =>
            Context.ExternalLinks.FirstOrDefault(x => x.Id == id && x.Tenant == tenant);

        public ExternalLink GetSingleExternalLinkByRef(string reference, int tenant) =>
            Context.ExternalLinks.FirstOrDefault(x => x.Ref == reference && x.Tenant == tenant);

        public IQueryable<ExternalLink> GetExternalLinks(int tenant) =>
            Context.ExternalLinks.Where(x => x.Tenant == tenant || x.Tenant == 0);

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

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public List<ExternalLink> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ExternalLink GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
