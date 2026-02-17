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
        internal static void MapInsideShipmentPackage(InsideShipmentPackagePM itemPM, InsideShipmentPackage itemPoco,bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentPackageId = itemPM.ShipmentPackageId;
            }

            itemPoco.Height = itemPM.Height;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.Length = itemPM.Length;
            itemPoco.PackageTypeId = itemPM.PackageTypeId;
            itemPoco.Quantity = itemPM.Quantity;                        
            itemPoco.Weight = itemPM.Weight;
            itemPoco.Width = itemPM.Width;
            itemPoco.OriginalShipmentPackageId = itemPM.OriginalShipmentPackageId;
            itemPoco.OriginalInsideShipmentPackageId = itemPM.OriginalInsideShipmentPackageId;
            itemPoco.Description = itemPM.Description;
            itemPoco.VolumetricWeight = itemPM.VolumetricWeight;
            itemPoco.Reference1 = itemPM.Reference1;
            itemPoco.Reference2 = itemPM.Reference2;
            itemPoco.Reference3 = itemPM.Reference3;
            itemPoco.Reference4 = itemPM.Reference4;
            itemPoco.CommodityNumber = itemPM.CommodityNumber;
            itemPoco.CommodityName = itemPM.CommodityName;
            itemPoco.Make = itemPM.Make;
            itemPoco.Year = itemPM.Year;
            itemPoco.Color = itemPM.Color;
            itemPoco.Model = itemPM.Model;
            itemPoco.ChassisNumber = itemPM.ChassisNumber;
            itemPoco.RegistrationNumber = itemPM.RegistrationNumber;
            itemPoco.CountryId = itemPM.CountryId;
        }
    }
}