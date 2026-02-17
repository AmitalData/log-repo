using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CustomTableRepository:IRepository<CustomTable>
    {

        IWebFreightContext webFreightContext;
        public CustomTableRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public CustomTableRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public CustomTableRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<CustomTable> GetCustomTablesByTenant(int tenant)
        {
            IQueryable<CustomTable> customTables = from a in context.CustomTables
                                                   where a.Tenant == tenant
                                                   select a;
            return customTables;
        }

        public IQueryable<CustomTable> GetCustomTablesByTenantAndObjectTable(string objectTableId, int tenant)
        {
            IQueryable<CustomTable> customTables = from a in context.CustomTables
                                                   where a.ObjectTableId == objectTableId && a.Tenant == tenant
                                                   select a;

            return customTables;
        }

        public void Add(CustomTable entity)
        {
            context.CustomTables.Add(entity);
        }

        public void Remove(CustomTable entity)
        {
            context.CustomTables.Attach(entity);
            context.CustomTables.Remove(entity);
        }

        public void Update(CustomTable entity)
        {
            context.CustomTables.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomTable> All()
        {
            return context.CustomTables.ToList();
        }

        public IWebFreightContext context
        {
            get {return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CustomTable> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomTable GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}