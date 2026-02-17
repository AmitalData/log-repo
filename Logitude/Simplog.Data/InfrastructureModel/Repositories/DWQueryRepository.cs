

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWQueryRepository : IRepository<DWQuery>
    {
        public IWebFreightContext webFreightContext;

        public DWQueryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWQueryRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWQueryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWQuery entity)
        {
            webFreightContext.DWQueries.Add(entity);
        }

        public void Remove(DWQuery entity)
        {
            webFreightContext.DWQueries.Attach(entity);
            webFreightContext.DWQueries.Remove(entity);
        }

        public void Update(DWQuery entity)
        {
            webFreightContext.DWQueries.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWQuery> All()
        {
            return webFreightContext.DWQueries.ToList();
        }


        public IQueryable<DWQuery> GetObjectsByTenant(int tenant)
        {
            IQueryable<DWQuery> DWQuery = from a in webFreightContext.DWQueries
                                                      where a.Tenant == tenant || a.Tenant == 0
                                                      select a;
            return DWQuery;
        }



        public DWQuery GetSingleDWQuery(string Id, int Tenant)
        {
            return webFreightContext.DWQueries.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<DWQuery> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DWQuery GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DWQuery> GetDWQueries(int tenant)
        {
            return webFreightContext.DWQueries.Where(a => a.Tenant == tenant);
        }

    }
}
