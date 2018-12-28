

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWSubQueryRepository : IRepository<DWSubQuery>
    {
        public IWebFreightContext webFreightContext;

        public DWSubQueryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWSubQueryRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWSubQueryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWSubQuery entity)
        {
            webFreightContext.DWSubQueries.Add(entity);
        }

        public void Remove(DWSubQuery entity)
        {
            webFreightContext.DWSubQueries.Attach(entity);
            webFreightContext.DWSubQueries.Remove(entity);
        }

        public void Update(DWSubQuery entity)
        {
            webFreightContext.DWSubQueries.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWSubQuery> All()
        {
            return webFreightContext.DWSubQueries.ToList();
        }


        public IQueryable<DWSubQuery> GetObjectsByTenant(int tenant)
        {
            IQueryable<DWSubQuery> DWSubQuery = from a in webFreightContext.DWSubQueries
                                                      where a.Tenant == tenant || a.Tenant == 0
                                                      select a;
            return DWSubQuery;
        }



        public DWSubQuery GetSingleDWSubQuery(string Id, int Tenant)
        {
            return webFreightContext.DWSubQueries.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }

        public DWSubQuery GetSingleDWSubQueryByDWQueryId(string DWQueryId, int Tenant)
        {
            return webFreightContext.DWSubQueries.Where(a => a.DWQueryId == DWQueryId && a.Tenant == Tenant).FirstOrDefault();
        }

        public IQueryable<DWSubQuery> GetDWSubQueriesForDWQuery(string DWQueryId, int Tenant)
        {
            return webFreightContext.DWSubQueries.Where(a => a.DWQueryId == DWQueryId && a.Tenant == Tenant);
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<DWSubQuery> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DWSubQuery GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DWSubQuery> GetDWSubQueries(int tenant)
        {
            return webFreightContext.DWSubQueries.Where(a => a.Tenant == tenant);
        }

    }
}
