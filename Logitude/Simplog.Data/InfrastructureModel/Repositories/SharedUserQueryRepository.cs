using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SharedUserQueryRepository : IRepository<SharedUserQuery>
    {
        IWebFreightContext webFreightContext;

        public SharedUserQueryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        
        public SharedUserQueryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public SharedUserQuery GetSingleSharedUserQuery(string id, int tenant)
        {
            return (from a in context.SharedUserQueries.Include("User").Include("Query")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public List<SharedUserQuery> GetAllByQueryCode(string queryCode)
        {
            return context.SharedUserQueries.Where(d => d.QueryCode == queryCode).ToList();
        }

        public void Add(SharedUserQuery entity)
        {
            context.SharedUserQueries.Add(entity);
        }

        public void Remove(SharedUserQuery entity)
        {
            context.SharedUserQueries.Attach(entity);
            context.SharedUserQueries.Remove(entity);
        }

        public void Update(SharedUserQuery entity)
        {
            context.SharedUserQueries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedUserQuery> All()
        {
            return context.SharedUserQueries.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SharedUserQuery> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SharedUserQuery GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
