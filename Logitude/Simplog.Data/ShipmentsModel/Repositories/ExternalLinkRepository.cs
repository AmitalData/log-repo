using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ExternalLinkRepository : IRepository<ExternalLink>
    {
        IShipmentsContext iContext;

        public ExternalLinkRepository(int tenant)
        {
            iContext = ShipmentsContext.GetContext(tenant);
        }

        public ExternalLinkRepository(IShipmentsContext context)
        {
            iContext = context;
        }

        public ExternalLink GetSingleExternalLink(string Ref)
        {
            return (from a in Context.ExternalLinks where a.Ref == Ref select a).FirstOrDefault();
        }

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

        public IShipmentsContext Context
        {
            get { return iContext; }
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
