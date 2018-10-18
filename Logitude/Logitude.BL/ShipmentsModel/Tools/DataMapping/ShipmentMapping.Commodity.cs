using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapCommodity(ShipmentCommodityPM itemPM, ShipmentCommodity itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.ChargeRate = itemPM.ChargeRate;
            itemPoco.ChargeAmount = itemPM.ChargeAmount;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.VolumetricWeight = itemPM.VolumetricWeight;
            itemPoco.GrossWeight = itemPM.GrossWeight;
            itemPoco.ChargeableWeight = itemPM.ChargeableWeight;
            itemPoco.NumberOfPackages = itemPM.NumberOfPackages;
            itemPoco.RateClassCode = itemPM.RateClassCode;
            itemPoco.CommodityNumber = itemPM.CommodityNumber;
            itemPoco.DescriptionOfGoods = itemPM.DescriptionOfGoods;
            itemPoco.IsFirstLine = itemPM.IsFirstLine;
        }
    }
}