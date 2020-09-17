using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class WarehouseRepository : IRepository<Warehouse>
    {
        ICommonDataContext commonDataContext;

        public WarehouseRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public WarehouseRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public WarehouseRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public Warehouse GetSingleWarehouse(string id,int tenant = 0)
        {
            return (from a in context.Warehouses.Include("Card")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<Warehouse> GetWarehousesByTenant(int tenant)
        {
            return from a in context.Warehouses.Include("Card")
                   where a.Tenant==tenant
                   select a;
        }

        public IQueryable<Warehouse> GetWarehouses(int tenant)
        {
            return from a in context.Warehouses.Include("Card")
                   where a.Tenant==tenant
                   select a;
        }

        public string GetWarehouseTypeById(string warehouseId, int tenant)
        {
            string warehouseTypeCode = "";
            WarehouseType warehouseType = (from a in context.Warehouses.Include("Card")
                   where a.Tenant == tenant && a.Id == warehouseId
                   select a.WarehouseType).FirstOrDefault();
            if (warehouseTypeCode != null) warehouseTypeCode = warehouseType.Code;

            return warehouseTypeCode;
        }



        public void Add(Warehouse entity)
        {
            context.Warehouses.Add(entity);
        }

        public void Remove(Warehouse entity)
        {
            context.Warehouses.Attach(entity);
            context.SetAsModified(entity);
        }

        public void Update(Warehouse entity)
        {
            context.Warehouses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Warehouse> All()
        {
          return   context.Warehouses.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }


        public List<Warehouse> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Warehouse GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public Warehouse GetFirstSingleByCode(string code, int tenant)
        {
            return (from record in context.Warehouses.Include("Card")
                    where record.Card.Code == code && record.Tenant == tenant
                    select record).FirstOrDefault();
        }
    }
}