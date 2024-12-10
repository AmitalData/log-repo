using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class WarehouseStoragePricingRepository : IRepository<WarehouseStoragePricing>
    {
        ICommonDataContext commonDataContext;

        public WarehouseStoragePricingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public WarehouseStoragePricingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public WarehouseStoragePricingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<WarehouseStoragePricing> GetWarehouseStoragePricings(int tenant)
        {
            return (from record in context.WarehouseStoragePricings where record.Tenant == tenant select record);
        }

        public WarehouseStoragePricing GetSingleWarehouseStoragePricing(string id, int tenant)
        {
            return (from record in context.WarehouseStoragePricings where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public void Add(WarehouseStoragePricing entity)
        {
            context.WarehouseStoragePricings.Add(entity);
        }

        public void Remove(WarehouseStoragePricing entity)
        {
            context.WarehouseStoragePricings.Attach(entity);
            context.WarehouseStoragePricings.Remove(entity);
        }

        public void Update(WarehouseStoragePricing entity)
        {
            context.WarehouseStoragePricings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseStoragePricing> All()
        {
            return context.WarehouseStoragePricings.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<WarehouseStoragePricing> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public WarehouseStoragePricing GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
