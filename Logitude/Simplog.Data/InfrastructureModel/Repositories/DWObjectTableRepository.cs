

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWObjectTableRepository : IRepository<DWObjectTable>
    {
        public IWebFreightContext webFreightContext;
        
        public DWObjectTableRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWObjectTableRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWObjectTableRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWObjectTable entity)
        {
            webFreightContext.DWObjectTables.Add(entity);
        }

        public void Remove(DWObjectTable entity)
        {
            webFreightContext.DWObjectTables.Attach(entity);
            webFreightContext.DWObjectTables.Remove(entity);
        }

        public void Update(DWObjectTable entity)
        {
            webFreightContext.DWObjectTables.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWObjectTable> All()
        {
            return webFreightContext.DWObjectTables.ToList();
        }


        public IQueryable<DWObjectTable> GetObjectsByTenant(int tenant)
        {
            IQueryable<DWObjectTable> dWObjectTable = from a in webFreightContext.DWObjectTables
                                                      where a.Tenant == tenant || a.Tenant == 0
                                                   select a;
            return dWObjectTable;
        }



        public DWObjectTable GetSingleDWObjectTable(string code, int Tenant)
        {
            return webFreightContext.DWObjectTables.Where(a => a.Code == code && a.Tenant == Tenant).FirstOrDefault();
        }
    
        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<DWObjectTable> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DWObjectTable GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DWObjectTable> GetDWObjectTables(int tenant)
        {
            return webFreightContext.DWObjectTables;//.Where(a => a.Tenant == tenant);
        }
  
    }
}
