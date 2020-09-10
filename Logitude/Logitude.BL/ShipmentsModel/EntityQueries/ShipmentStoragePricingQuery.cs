using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentStoragePricingQuery
    {
        ShipmentStoragePricingRepository repository;

        public ShipmentStoragePricingQuery(int tenant)
        {
            repository = new ShipmentStoragePricingRepository(tenant);
        }

        public ShipmentStoragePricingQuery(ShipmentStoragePricingRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentStoragePricingPM GetSinglePM(string id, int tenant)
        {
            ShipmentStoragePricingPM myResult
                = (from a in repository.context.ShipmentStoragePricings
                   where a.Id == id && a.Tenant == tenant
                   select new ShipmentStoragePricingPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       WarehouseId = a.WarehouseId,
                       StepFrom = a.StepFrom,
                       StepTo = a.StepTo,
                       Days = a.Days,
                       SalePrice = a.SalePrice,
                       Amount = a.Amount,
                       LineNumber = a.LineNumber,
                       ChargeableDays = a.ChargeableDays,
                   }).FirstOrDefault();

            return myResult;
        }

        public List<ShipmentStoragePricingPM> GetShipmentStoragePricingsByShipmentId(string shipmentId, int tenant)
        {
            List<ShipmentStoragePricingPM> storagePricings
                = (from a in repository.context.ShipmentStoragePricings
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentStoragePricingPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       WarehouseId = a.WarehouseId,
                       StepFrom = a.StepFrom,
                       StepTo = a.StepTo,
                       Days = a.Days,
                       SalePrice = a.SalePrice,
                       Amount = a.Amount,
                       LineNumber = a.LineNumber,
                       ChargeableDays = a.ChargeableDays,
                   }).ToList();

            return storagePricings;
        }
    }
}
