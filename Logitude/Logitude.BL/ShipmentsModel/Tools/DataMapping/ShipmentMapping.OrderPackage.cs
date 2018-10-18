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
        internal static void MapOrderPackage(ShipmentOrderPackagePM itemPM, ShipmentOrderPackage itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.PackageTypeId = itemPM.PackageTypeId;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.IsContainer = itemPM.IsContainer;
            itemPoco.Height = itemPM.Height;
            itemPoco.Length = itemPM.Length;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.GrossWeight = itemPM.GrossWeight;
            itemPoco.Width = itemPM.Width;
            itemPoco.VolumetricWeight = itemPM.VolumetricWeight;
        }
    }
}