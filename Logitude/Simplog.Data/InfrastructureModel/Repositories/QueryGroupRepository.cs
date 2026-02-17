using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QueryGroupRepository:IRepository<QueryGroup>
    {
        IWebFreightContext webFreightContext;
        public QueryGroupRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public QueryGroupRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public QueryGroupRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public QueryGroup GetSingleQueryGroup(string code)
        {
            return (from a in context.QueryGroups
                    where a.Code == code
                    select a).FirstOrDefault();
        }
  
        public IQueryable<QueryGroup> GetQueryGroups()
        {
            return context.QueryGroups;
        }

   
        public void Add(QueryGroup entity)
        {
            context.QueryGroups.Add(entity);
        }

        public void Remove(QueryGroup entity)
        {
            context.QueryGroups.Attach(entity);
            context.QueryGroups.Remove(entity);
        }

        public void Update(QueryGroup entity)
        {
            context.QueryGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QueryGroup> All()
        {
            return context.QueryGroups.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QueryGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QueryGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}