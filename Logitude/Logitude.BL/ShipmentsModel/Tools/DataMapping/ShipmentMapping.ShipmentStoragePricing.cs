using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapShipmentStoragePricing(ShipmentStoragePricingPM itemPM, ShipmentStoragePricing itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
                itemPoco.WarehouseId = itemPM.WarehouseId;
            }

            itemPoco.StepFrom = itemPM.StepFrom;
            itemPoco.StepTo = itemPM.StepTo;
            itemPoco.Days = itemPM.Days;
            itemPoco.SalePrice = itemPM.SalePrice;
            itemPoco.Amount = itemPM.Amount;
            itemPoco.LineNumber = itemPM.LineNumber;
            itemPoco.ChargeableDays = itemPM.ChargeableDays;
        }
    }
}
