using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.Warehouse
{
    public class WarehouseRelaseService
    {

        public void EnableWarehouseRelaseForUse(string releaseNumber,string shipmentId,int tenant)
        {
            WarehouseReleaseRepository warehouseReleaseRepository = new WarehouseReleaseRepository(tenant);
            WarehouseRelease warehouseRelease = warehouseReleaseRepository.GetWarehouseReleasesByReleaseNumberAndShipmentId(releaseNumber , shipmentId, tenant);
            if (warehouseRelease != null && warehouseRelease.IsUsed)
            {
                if(string.IsNullOrEmpty(warehouseRelease.ConnectedTo)) warehouseRelease.ShipmentId = null;
               
                warehouseRelease.IsUsed = false;
                warehouseReleaseRepository.Update(warehouseRelease);
                warehouseReleaseRepository.SubmitChanges();
            }
        }
    }
}