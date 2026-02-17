

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWQueryFilterRepository : IRepository<DWQueryFilter>
    {
        public IWebFreightContext webFreightContext;

        public DWQueryFilterRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWQueryFilterRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWQueryFilterRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWQueryFilter entity)
        {
            webFreightContext.DWQueryFilters.Add(entity);
        }

        public void Remove(DWQueryFilter entity)
        {
            webFreightContext.DWQueryFilters.Attach(entity);
            webFreightContext.DWQueryFilters.Remove(entity);
        }

        public void Update(DWQueryFilter entity)
        {
            webFreightContext.DWQueryFilters.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWQueryFilter> All()
        {
            return webFreightContext.DWQueryFilters.ToList();
        }


        public IQueryable<DWQueryFilter> GetObjectsByTenant(int tenant)
        {
            IQueryable<DWQueryFilter> DWQueryFilter = from a in webFreightContext.DWQueryFilters
                                                      where a.Tenant == tenant || a.Tenant == 0
                                                      select a;
            return DWQueryFilter;
        }



        public DWQueryFilter GetSingleDWQueryFilter(string Id, int Tenant)
        {
            return webFreightContext.DWQueryFilters.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<DWQueryFilter> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DWQueryFilter GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DWQueryFilter> GetDWQueryFilters(int tenant)
        {
            return webFreightContext.DWQueryFilters.Where(a => a.Tenant == tenant);
        }

    }
}
