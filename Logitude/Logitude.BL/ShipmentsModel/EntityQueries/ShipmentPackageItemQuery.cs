using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPackageItemQuery
    {
        ShipmentPackageItemRepository repository;
        public ShipmentPackageItemQuery(int tenant)
        {
            repository = new ShipmentPackageItemRepository(tenant);
        }
        public ShipmentPackageItemQuery(ShipmentPackageItemRepository myRepository)
        {
            this.repository = myRepository;
        }

        public List<ShipmentPackageItemPM> GetShipmentPackageItems(string packageId, int tenant)
        {
            return (from a in repository.context.ShipmentPackageItems
                    where a.PackageId == packageId && a.Tenant == tenant
                    select new ShipmentPackageItemPM()
                    {
                        PackageId = a.PackageId,
                        LineNumber = a.LineNumber,
                        Tenant = a.Tenant,
                        Description = a.Description,
                        Quantity = a.Quantity,
                        GoodsValue = a.GoodsValue,
                    }).ToList();
        }
    }
}