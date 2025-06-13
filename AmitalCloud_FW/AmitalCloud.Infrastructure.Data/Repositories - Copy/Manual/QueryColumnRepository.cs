using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Interfaces;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.BaseClasses;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class QueryColumnRepository: BaseRepository<QueryColumn, string,IAmitalCloudContext> ,IRepository<QueryColumn, string>
    {
        public QueryColumnRepository() : this(0) {}
        public QueryColumnRepository(IAmitalCloudContext context) : base(context)  { }
        public QueryColumnRepository(int tenant) : this(AmitalCloudContext.GetContext(tenant)) { }
        public IQueryable<QueryColumn> GetQueryColumnsByTenant(int tenant)
        {
            IQueryable<QueryColumn> queries = from a in Context.QueryColumns
                                              where a.Tenant == tenant && a.UserId ==null
                                              select a;
            return queries;
        }
      
        public QueryColumn GetSingleQueryColumn(string id,int tenant)
        {
            return (from a in Context.QueryColumns
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public QueryColumn GetSingleQueryColumnByFieldCode(string FieldCode, int tenant)
        {
            return (from a in Context.QueryColumns
                    where a.ObjectFieldCode == FieldCode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
     
        public IQueryable<QueryColumn> GetQueryColumnsByQueryTenantOnly(int tenant, string queryCode)
        {
            IQueryable<QueryColumn> query = from a in Context.QueryColumns
                                            where a.QueryCode == queryCode && a.Tenant == tenant 
                                            select a;
            return query;
        }

        public override void Add(QueryColumn entity)
        {
            Context.QueryColumns.Add(entity);

        }

        public override void Remove(QueryColumn entity)
        {
            Context.QueryColumns.Attach(entity);
            Context.QueryColumns.Remove(entity);
        }

        public override void Update(QueryColumn entity)
        {
            Context.QueryColumns.Attach(entity);
            Context.SetAsModified(entity);
        }

        public override List<QueryColumn> All()
        {
            return Context.QueryColumns.ToList();
        }



        public override List<QueryColumn> GetMulti(IEntityKeyFields<QueryColumn,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public override QueryColumn GetSingle(IEntityKeyFields<QueryColumn,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<QueryColumn> GetQueryColumnsByQueryCodeAndUser(int tenant, string userId, string queryCode)
        {
            IQueryable<QueryColumn> columns = from a in Context.QueryColumns
                                              where a.Tenant == tenant && a.UserId == userId && a.QueryCode == queryCode && a.ObjectField.DisplayInList == true
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


        public List<QueryColumn> GetQueryColumnsByQueryCode(int tenant, string queryCode)
        {
            IQueryable<QueryColumn> columns = from a in Context.QueryColumns
                                              where a.Tenant == tenant  && a.QueryCode == queryCode && a.ObjectField.DisplayInList == true
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

        protected override IQueryable<QueryColumn> Query()
        {
            throw new System.NotImplementedException();
        }
    }
}