using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapPcakge(ShipmentPackagePM itemPM, ShipmentPackage itemPoco, bool isNewEntity, Tenant loggedTenant)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            if (itemPM.ContainerNumber != null)
            {
                itemPM.ContainerNumber = itemPM.ContainerNumber.Trim();
            }

            itemPoco.ClassNumber = itemPM.ClassNumber;
            itemPoco.ContainerNumber = itemPM.ContainerNumber;
            itemPoco.Description = itemPM.Description;
            itemPoco.FlashPoint = itemPM.FlashPoint;
            itemPoco.Harmonize = itemPM.Harmonize;
            itemPoco.Height = itemPM.Height;
            itemPoco.IMDGCode = itemPM.IMDGCode;
            itemPoco.Length = itemPM.Length;
            itemPoco.PackageTypeId = itemPM.PackageTypeId;
            itemPoco.PackagingGroup = itemPM.PackagingGroup;
            itemPoco.MarksAndNumbers = itemPM.MarksAndNumbers;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.ShipperSeal = itemPM.ShipperSeal;
            itemPoco.CarrierSeal = itemPM.CarrierSeal;
            itemPoco.SOC = itemPM.SOC;
            itemPoco.Temperature = itemPM.Temperature;
            itemPoco.Tare = itemPM.Tare;
            itemPoco.UnNumber = itemPM.UnNumber;
            itemPoco.Ventilation = itemPM.Ventilation;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.Weight = itemPM.Weight;
            itemPoco.Width = itemPM.Width;
            itemPoco.MaterialDescription = itemPM.MaterialDescription;
            itemPoco.IsDangerous = itemPM.IsDangerous;
            itemPoco.OriginalShipmentPackageId = itemPM.OriginalShipmentPackageId;
            itemPoco.VolumetricWeight = itemPM.VolumetricWeight;
            itemPoco.CommodityId = itemPM.CommodityId;
            itemPoco.NumberOfInsidePackages = itemPM.NumberOfInsidePackages;
            itemPoco.NumberOfInsidePackagesDetails = itemPM.NumberOfInsidePackagesDetails;
            itemPoco.VGM = itemPM.VGM;
            itemPoco.MethodUsed = itemPM.MethodUsed;
            itemPoco.IsDeliveryFU = itemPM.IsDeliveryFU;
            itemPoco.DeliveryId = itemPM.DeliveryId;
            itemPoco.DeliveryETD = itemPM.DeliveryETD;
            itemPoco.DeliveryATD = itemPM.DeliveryATD;
            itemPoco.DeliveryETA = itemPM.DeliveryETA;
            itemPoco.DeliveryATA = itemPM.DeliveryATA;
            itemPoco.DeliveryTo = itemPM.DeliveryTo;
            itemPoco.DeliveryFrom = itemPM.DeliveryFrom;
            itemPoco.IsEmptyContainerReturnFU = itemPM.IsEmptyContainerReturnFU;
            itemPoco.EmptyContainerReturnId = itemPM.EmptyContainerReturnId;
            itemPoco.EmptyContainerReturnETD = itemPM.EmptyContainerReturnETD;
            itemPoco.EmptyContainerReturnATD = itemPM.EmptyContainerReturnATD;
            itemPoco.EmptyContainerReturnETA = itemPM.EmptyContainerReturnETA;
            itemPoco.EmptyContainerReturnATA = itemPM.EmptyContainerReturnATA;
            itemPoco.EmptyContainerReturnTo = itemPM.EmptyContainerReturnTo;
            itemPoco.EmptyContainerReturnFrom = itemPM.EmptyContainerReturnFrom;
            itemPoco.Reference1 = itemPM.Reference1;
            itemPoco.Reference2 = itemPM.Reference2;
            itemPoco.Reference3 = itemPM.Reference3;
            itemPoco.Reference4 = itemPM.Reference4;
            itemPoco.CommodityNumber = itemPM.CommodityNumber;
            itemPoco.CommodityName = itemPM.CommodityName;
            itemPoco.CeficClass = itemPM.CeficClass;
            itemPoco.KelmerCode = itemPM.KelmerCode;
            itemPoco.EMS = itemPM.EMS;
            itemPoco.ProperShippingName = itemPM.ProperShippingName;
            itemPoco.MarinePollutant = itemPM.MarinePollutant;
            itemPoco.Notes = itemPM.Notes;
            itemPoco.OnCarriageATA = itemPM.OnCarriageATA;
            itemPoco.OnCarriageATD = itemPM.OnCarriageATD;
            itemPoco.OnCarriageETA = itemPM.OnCarriageETA;
            itemPoco.OnCarriageETD = itemPM.OnCarriageETD;
            itemPoco.IsMultiHarmonize = itemPM.IsMultiHarmonize;
            itemPoco.ETD = itemPM.ETD;
            itemPoco.ETA = itemPM.ETA;
            itemPoco.Routing = itemPM.Routing;
            itemPoco.RoutingIds = itemPM.RoutingIds;
            itemPoco.VoyageTripNumber = itemPM.VoyageTripNumber;
            itemPoco.HasContainerException = itemPM.HasContainerException;
            itemPoco.WarehouseReleaseNumber = itemPM.WarehouseReleaseNumber;
            itemPoco.InUse = itemPM.InUse;
            itemPoco.HorseId = itemPM.HorseId;
            itemPoco.LCLContainerTypeId = itemPM.LCLContainerTypeId;
            itemPoco.ContainerEntityId = itemPM.ContainerEntityId;
            if (itemPM.TemperatureUnitCode == null)
            {
                itemPM.TemperatureUnitCode = loggedTenant.TemperatureUnitCode;

                if (itemPM.TemperatureUnitCode == null)
                {
                    itemPM.TemperatureUnitCode = "CEL";
                }
            }

            if (itemPM.FlashPointTemperatureUnitCode == null)
            {
                itemPM.FlashPointTemperatureUnitCode = loggedTenant.TemperatureUnitCode;

                if (itemPM.FlashPointTemperatureUnitCode == null)
                {
                    itemPM.FlashPointTemperatureUnitCode = "CEL";
                }
            }

            itemPoco.TemperatureUnitCode = itemPM.TemperatureUnitCode;
            itemPoco.FlashPointTemperatureUnitCode = itemPM.FlashPointTemperatureUnitCode;
            itemPoco.NonActiveContainer = itemPM.NonActiveContainer;
            itemPoco.LastStatusCode = itemPM.LastStatusCode;
            itemPoco.LastStatusDate = itemPM.LastStatusDate;
            itemPoco.DeliveryTransportModeCode = itemPM.DeliveryTransportModeCode;
            itemPoco.ECRTransportModeCode = itemPM.ECRTransportModeCode;

            itemPoco.Make = itemPM.Make;
            itemPoco.Year = itemPM.Year;
            itemPoco.Color = itemPM.Color;
            itemPoco.Model = itemPM.Model;
            itemPoco.ChassisNumber = itemPM.ChassisNumber;
            itemPoco.RegistrationNumber = itemPM.RegistrationNumber;
            itemPoco.CountryId = itemPM.CountryId;
        }

        public static void MapCommodityPackage(CommodityPackagePM itemPM, ShipmentPackage itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.Description = itemPM.Description;
            itemPoco.Height = itemPM.Height;
            itemPoco.Length = itemPM.Length;
            itemPoco.PackageTypeId = itemPM.PackageTypeId;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.Weight = itemPM.Weight;
            itemPoco.Width = itemPM.Width;
            itemPoco.VolumetricWeight = itemPM.VolumetricWeight;
            itemPoco.CommodityId = itemPM.CommodityId;
        }
    }
}