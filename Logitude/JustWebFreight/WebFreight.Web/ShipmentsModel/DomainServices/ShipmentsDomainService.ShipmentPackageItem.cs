using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private ShipmentPackageItemRepository shipmentPackageItemRepository;
        private ShipmentPackageItemQuery shipmentPackageItemQuery;


        public List<ShipmentPackageItemPM> GetShipmentPackageItemsByPackageId(string packageId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shipmentPackageItemQuery = new ShipmentPackageItemQuery(tenant);
            return shipmentPackageItemQuery.GetShipmentPackageItems(packageId, tenant);
        }

        public ShipmentPackageItemList GetSingleShipmentPackageItemList(string packageId, int linNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shipmentPackageItemRepository = new ShipmentPackageItemRepository(tenant);

            ShipmentPackageItem entity = shipmentPackageItemRepository.GetSingleShipmentPackageItem(packageId,linNumber, tenant);
            ShipmentPackageItemList entityList = new ShipmentPackageItemList()
            {
                PackageId = entity.PackageId,
                LineNumber = entity.LineNumber,
                Tenant = entity.Tenant,
                Description = entity.Description,
                Quantity = entity.Quantity,
                GoodsValue = entity.GoodsValue,
            };

            return entityList;
        }

    }
}