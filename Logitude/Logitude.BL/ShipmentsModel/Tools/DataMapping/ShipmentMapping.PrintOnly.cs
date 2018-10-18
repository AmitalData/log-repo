using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        internal static void MapAWBPrintOnly(ShipmentAWBPrintOnlyPM itemPM, ShipmentAWBPrintOnly itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.Amount = itemPM.Amount;
            itemPoco.CurrencyId = itemPM.CurrencyId;
            itemPoco.DueTypeCode = itemPM.DueTypeCode;
            itemPoco.ExchangeRate = itemPM.ExchangeRate;
            itemPoco.PrepaidCollectId = itemPM.PrepaidCollectId;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.UnitPrice = itemPM.UnitPrice;
            itemPoco.MeasurementId = itemPM.MeasurementId;
            itemPoco.IATACodeId = itemPM.IATACodeId;
        }
    }
}