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
        public static void MapPickUpDeliveryPackage(ShipmentPickUpDeliveryPackagePM itemPM, ShipmentPickUpDeliveryPackage itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
            }

            itemPoco.ContainerNumber = itemPM.ContainerNumber;
            itemPoco.Description = itemPM.Description;
            itemPoco.PackageTypeId = itemPM.PackageTypeId;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.ShipmentPickUpDeliveryId = itemPM.ShipmentPickUpDeliveryId;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.Weight = itemPM.Weight;
            itemPoco.ShipperSeal = itemPM.ShipperSeal;
            itemPoco.Harmonize = itemPM.Harmonize;
            itemPoco.Width = itemPM.Width;
            itemPoco.Height = itemPM.Height;
            itemPoco.Length = itemPM.Length;
            itemPoco.OriginalShipmentPackageId = itemPM.OriginalShipmentPackageId;
            itemPoco.IsMultiHarmonize = itemPM.IsMultiHarmonize;
            itemPoco.Make = itemPM.Make;
            itemPoco.Year = itemPM.Year;
            itemPoco.Color = itemPM.Color;
            itemPoco.Model = itemPM.Model;
            itemPoco.ChassisNumber = itemPM.ChassisNumber;
            itemPoco.RegistrationNumber = itemPM.RegistrationNumber;
            itemPoco.CountryId = itemPM.CountryId;
            itemPoco.ContainerEntityId = itemPM.ContainerEntityId;
        }
    }
}