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

        public void EnableWarehouseRelaseForUse(string releaseNumber, int tenant)
        {
            WarehouseReleaseRepository warehouseReleaseRepository = new WarehouseReleaseRepository(tenant);
            WarehouseRelease warehouseRelease = warehouseReleaseRepository.GetWarehouseReleasesByReleaseNumber(releaseNumber, tenant);
            if (warehouseRelease != null && warehouseRelease.IsUsed)
            {
                warehouseRelease.IsUsed = false;
                warehouseReleaseRepository.Update(warehouseRelease);
                warehouseReleaseRepository.SubmitChanges();
            }
        }
    }
}