

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWQueryColumnRepository : IRepository<DWQueryColumn>
    {
        public IWebFreightContext webFreightContext;

        public DWQueryColumnRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWQueryColumnRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWQueryColumnRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWQueryColumn entity)
        {
            webFreightContext.DWQueryColumns.Add(entity);
        }

        public void Remove(DWQueryColumn entity)
        {
            webFreightContext.DWQueryColumns.Attach(entity);
            webFreightContext.DWQueryColumns.Remove(entity);
        }

        public void Update(DWQueryColumn entity)
        {
            webFreightContext.DWQueryColumns.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWQueryColumn> All()
        {
            return webFreightContext.DWQueryColumns.ToList();
        }


        public IQueryable<DWQueryColumn> GetObjectsByTenant(int tenant)
        {
            IQueryable<DWQueryColumn> DWQueryColumn = from a in webFreightContext.DWQueryColumns
                                                      where a.Tenant == tenant || a.Tenant == 0
                                                      select a;
            return DWQueryColumn;
        }



        public DWQueryColumn GetSingleDWQueryColumn(string Id, int Tenant)
        {
            return webFreightContext.DWQueryColumns.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<DWQueryColumn> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public DWQueryColumn GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DWQueryColumn> GetDWQueryColumns(int tenant)
        {
            return webFreightContext.DWQueryColumns.Where(a => a.Tenant == tenant);
        }

    }
}
