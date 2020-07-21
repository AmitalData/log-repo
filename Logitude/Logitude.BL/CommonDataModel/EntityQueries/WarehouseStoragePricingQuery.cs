using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class WarehouseStoragePricingQuery
    {
        WarehouseStoragePricingRepository repository;

        public WarehouseStoragePricingQuery()
        {
            repository = new WarehouseStoragePricingRepository();
        }

        public WarehouseStoragePricingQuery(int tenant)
        {
            repository = new WarehouseStoragePricingRepository(tenant);
        }

        public WarehouseStoragePricingQuery(WarehouseStoragePricingRepository repository)
        {
            this.repository = repository;
        }

        public WarehouseStoragePricingPM GetSinglePM(string id, int tenant)
        {
            WarehouseStoragePricingPM entity = (from a in repository.context.WarehouseStoragePricings
                                             where a.Tenant == tenant && a.Id == id
                                             select new WarehouseStoragePricingPM()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 WarehouseId = a.WarehouseId,
                                                 StepFrom = a.StepFrom,
                                                 StepTo = a.StepTo,
                                                 Days = a.Days,
                                                 SalePrice = a.SalePrice,
                                                 LineNumber = a.LineNumber,
                                             }).FirstOrDefault();

            return entity;
        }

        public IQueryable<WarehouseStoragePricingPM> GetWarehouseStoragePricingPMsByTenant(int tenant)
        {
            IQueryable<WarehouseStoragePricingPM> WarehouseStoragePricings = from a in repository.context.WarehouseStoragePricings
                                                                       where a.Tenant == tenant
                                                                       select new WarehouseStoragePricingPM()
                                                                       {
                                                                           Id = a.Id,
                                                                           Tenant = a.Tenant,
                                                                           WarehouseId = a.WarehouseId,
                                                                           StepFrom = a.StepFrom,
                                                                           StepTo = a.StepTo,
                                                                           Days = a.Days,
                                                                           SalePrice = a.SalePrice,
                                                                           LineNumber = a.LineNumber,
                                                                       };
            return WarehouseStoragePricings;
        }

        public List<WarehouseStoragePricingPM> GetWarehouseStoragePricingPMsByWarehouseId(string warehouseId, int tenant)
        {
            IQueryable<WarehouseStoragePricingPM> WarehouseStoragePricings = from a in repository.context.WarehouseStoragePricings
                                                                             where a.Tenant == tenant && a.WarehouseId == warehouseId
                                                                             select new WarehouseStoragePricingPM()
                                                                             {
                                                                                 Id = a.Id,
                                                                                 Tenant = a.Tenant,
                                                                                 WarehouseId = a.WarehouseId,
                                                                                 StepFrom = a.StepFrom,
                                                                                 StepTo = a.StepTo,
                                                                                 Days = a.Days,
                                                                                 SalePrice = a.SalePrice,
                                                                                 LineNumber = a.LineNumber,
                                                                             };
            return WarehouseStoragePricings.ToList();
        }
    }
}
