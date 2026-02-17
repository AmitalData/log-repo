using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QueryColumnRepository:IRepository<QueryColumn>
    {


        IWebFreightContext webFreightContext;
        public QueryColumnRepository()
        {
            webFreightContext = new WebFreightContext();

        }

        public QueryColumnRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public QueryColumnRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<QueryColumn> GetQueryColumnsByTenant(int tenant)
        {
            IQueryable<QueryColumn> queries = from a in context.QueryColumns
                                              where a.Tenant == tenant && a.UserId ==null
                                              select a;
            return queries;
        }
      
        public QueryColumn GetSingleQueryColumn(string id,int tenant)
        {
            return (from a in context.QueryColumns
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public QueryColumn GetSingleQueryColumnByFieldId(string FieldId, int tenant)
        {
            return (from a in context.QueryColumns
                    where a.ObjectFieldId == FieldId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
     
        public IQueryable<QueryColumn> GetQueryColumnsByQueryTenantOnly(int tenant, string queryId)
        {
            IQueryable<QueryColumn> query = from a in context.QueryColumns
                                            where a.QueryId == queryId && a.Tenant == tenant 
                                            select a;
            return query;
        }

        public void Add(QueryColumn entity)
        {
            context.QueryColumns.Add(entity);

        }

        public void Remove(QueryColumn entity)
        {
            context.QueryColumns.Attach(entity);
            context.QueryColumns.Remove(entity);
        }

        public void Update(QueryColumn entity)
        {
            context.QueryColumns.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QueryColumn> All()
        {
            return context.QueryColumns.ToList();
        }

        public IWebFreightContext context
        {
            get {return webFreightContext; }
        }

        public void SubmitChanges()
        {
           context.SaveChanges();
        }

        public List<QueryColumn> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QueryColumn GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<QueryColumn> GetQueryColumnsByQueryIdAndUser(int tenant, string userId, string queryId)
        {
            IQueryable<QueryColumn> columns = from a in webFreightContext.QueryColumns
                                              where a.Tenant == tenant && a.UserId == userId && a.QueryId == queryId && a.ObjectField.DisplayInList == true
                                              select a;            

            List<QueryColumn> Cols = new List<QueryColumn>();
            foreach (var item in columns)
            {
                if (!Cols.Contains(item))
                {
                    Cols.Add(item);
                }
            }

            return Cols;
        }
    }
}