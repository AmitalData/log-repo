using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using System;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPackageQuery
    {
        ShipmentPackageRepository repository;         
        public ShipmentPackageQuery(int tenant)
        {
            repository = new ShipmentPackageRepository(tenant);
        }
        public ShipmentPackageQuery(ShipmentPackageRepository repository)
        {
            this.repository = repository;
        }

        public List<HouseContainerPackage> GetFCLPackagesData(string shipmentId, int tenant)
        {
            List<HouseContainerPackage> result = new List<HouseContainerPackage>();
            result = (from a in repository.context.ShipmentPackages
                      where a.Tenant == tenant && a.ShipmentId == shipmentId
                      group a by new { a.PackageTypeId } into g
                      select new HouseContainerPackage()
                      {
                          Id = g.Key.PackageTypeId,
                          Quantity = g.Sum(s => s.Quantity),
                          ConsoleId = shipmentId
                      }).ToList();

            return result;
        }
        public List<CommodityPackagePM> GetPackagesByCommodityId(string commodityId, int tenant)
        {
            List<CommodityPackagePM> myResult = (from a in repository.context.ShipmentPackages.Include("PackageType")
                                                where a.Tenant == tenant && a.CommodityId == commodityId
                                                 select new CommodityPackagePM()
                                                {
                                                    Description = a.Description,
                                                    Height = a.Height,
                                                    Id = a.Id,
                                                    Length = a.Length,
                                                    PackageTypeId = a.PackageTypeId,
                                                    Quantity = a.Quantity,
                                                    ShipmentId = a.ShipmentId,
                                                    Tenant = a.Tenant,
                                                    Volume = a.Volume,
                                                    Weight = a.Weight,
                                                    Width = a.Width,
                                                    VolumetricWeight = a.VolumetricWeight,
                                                    CommodityId = a.CommodityId,
                                                    NumberOfInsidePackages = a.NumberOfInsidePackages,
                                                    NumberOfInsidePackagesDetails = a.NumberOfInsidePackagesDetails,
                                                    

                                                 }).ToList();

            return myResult;
        }

        public ShipmentPackagePM GetSinglePM(string shipmentId, int tenant)
        {
            InsideShipmentPackageRepository insideShipmentPackagesRepository = new InsideShipmentPackageRepository(repository.context);
            InsideShipmentPackageQuery insideShipmentPackageQuery = new InsideShipmentPackageQuery(insideShipmentPackagesRepository);

            ShipmentPackageItemRepository shipmentPackageItemRepository = new ShipmentPackageItemRepository(repository.context);
            ShipmentPackageItemQuery shipmentPackageItemQuery = new ShipmentPackageItemQuery(shipmentPackageItemRepository);

            ShipmentPackageHarmonizeRepository shipmentPackageHarmonizeRepository = new ShipmentPackageHarmonizeRepository(repository.context);
            ShipmentPackageHarmonizeQuery shipmentPackageHarmonizeQuery = new ShipmentPackageHarmonizeQuery(shipmentPackageHarmonizeRepository);

            ShipmentPackagePM myResult
                = (from a in repository.context.ShipmentPackages.Include("PackageType").Include("LastStatus").Include("Horse")
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentPackagePM()
                   {
                       ClassNumber = a.ClassNumber,
                       ContainerNumber = a.ContainerNumber,
                       Description = a.Description,
                       FlashPoint = a.FlashPoint,
                       Harmonize = a.Harmonize,
                       OriginalShipmentPackageId = a.OriginalShipmentPackageId,
                       Height = a.Height,
                       Id = a.Id,
                       IMDGCode = a.IMDGCode,
                       Length = a.Length,
                       MarksAndNumbers = a.MarksAndNumbers,
                       Quantity = a.Quantity,
                       InUse = a.InUse,
                       ShipperSeal = a.ShipperSeal,
                       CarrierSeal = a.CarrierSeal,
                       ShipmentPMId = a.ShipmentId,
                       ShipmentId = a.ShipmentId,
                       SOC = a.SOC,
                       Temperature = a.Temperature,
                       Tenant = a.Tenant,
                       Tare = a.Tare,
                       UnNumber = a.UnNumber,
                       Ventilation = a.Ventilation,
                       Volume = a.Volume,
                       Weight = a.Weight,
                       Width = a.Width,
                       PackagingGroup = a.PackagingGroup,
                       MaterialDescription = a.MaterialDescription,
                       IsDangerous = a.IsDangerous,
                       VolumetricWeight = a.VolumetricWeight,
                       CommodityId = a.CommodityId,
                       NumberOfInsidePackages = a.NumberOfInsidePackages,
                       NumberOfInsidePackagesDetails = a.NumberOfInsidePackagesDetails,
                       VGM = a.VGM,
                       MethodUsed = a.MethodUsed,
                       PackageTypeId = a.PackageTypeId,
                       PackageTypeCode = a.PackageType == null ? null : a.PackageType.Code,
                       PackageTypeName = a.PackageType == null ? null : a.PackageType.EnglishName,
                       PackageTypeLocalName = a.PackageType == null ? null : a.PackageType.LocalName,
                       PackageTypeNote = a.PackageType == null ? null : a.PackageType.Notes,
                       PrintAs = a.PackageType == null ? null : a.PackageType.PrintAs,
                       IsPackageAddedManually = a.PackageType == null ? false : a.PackageType.AddedManually,
                       IsContainer = a.PackageType == null ? false : a.PackageType.IsContainer,
                       IsContainerRefrigerated = a.PackageType == null ? false : a.PackageType.IsRefrigerated,
                       PackageTypeIsInland = a.PackageType == null ? false : a.PackageType.IsInland,
                       PackageTypeIsOcean = a.PackageType == null ? false : a.PackageType.IsOcean,
                       PackageTypeIsAir = a.PackageType == null ? false : a.PackageType.IsAir,
                       TEU = a.PackageType == null ? 0 : a.PackageType.TEU,
                       ContainerSize = a.PackageType == null ? 0 : a.PackageType.ContainerSize,
                       PackageTypeVolume = a.PackageType == null ? 0 : a.PackageType.Volume,
                       IsDeliveryFU = a.IsDeliveryFU,
                       DeliveryId = a.DeliveryId,
                       DeliveryETD = a.DeliveryETD,
                       DeliveryATD = a.DeliveryATD,
                       DeliveryETA = a.DeliveryETA,
                       DeliveryATA = a.DeliveryATA,
                       DeliveryTo = a.DeliveryTo,
                       DeliveryFrom = a.DeliveryFrom,
                       IsEmptyContainerReturnFU = a.IsEmptyContainerReturnFU,
                       EmptyContainerReturnId = a.EmptyContainerReturnId,
                       EmptyContainerReturnETD = a.EmptyContainerReturnETD,
                       EmptyContainerReturnATD = a.EmptyContainerReturnATD,
                       EmptyContainerReturnETA = a.EmptyContainerReturnETA,
                       EmptyContainerReturnATA = a.EmptyContainerReturnATA,
                       EmptyContainerReturnTo = a.EmptyContainerReturnTo,
                       EmptyContainerReturnFrom = a.EmptyContainerReturnFrom,
                       Reference1 = a.Reference1,
                       Reference2 = a.Reference2,
                       Reference3 = a.Reference3,
                       Reference4 = a.Reference4,
                       CommodityNumber = a.CommodityNumber,
                       CommodityName = a.CommodityName,
                       CeficClass = a.CeficClass,
                       KelmerCode = a.KelmerCode,
                       EMS = a.EMS,
                       ProperShippingName = a.ProperShippingName,
                       MarinePollutant = a.MarinePollutant,
                       Notes = a.Notes,
                       TemperatureUnitCode = a.TemperatureUnitCode,
                       FlashPointTemperatureUnitCode = a.FlashPointTemperatureUnitCode,
                       NonActiveContainer = a.NonActiveContainer,
                       OnCarriageATA = a.OnCarriageATA,
                       OnCarriageATD = a.OnCarriageATD,
                       OnCarriageETA = a.OnCarriageETA,
                       OnCarriageETD = a.OnCarriageETD,
                       LastStatusCode = a.LastStatusCode,
                       LastStatusDate = a.LastStatusDate,
                       LastStatusName = a.LastStatus == null ? null : a.LastStatus.Name,
                       DeliveryTransportModeCode = a.DeliveryTransportModeCode,
                       ECRTransportModeCode = a.ECRTransportModeCode,
                       IsMultiHarmonize = a.IsMultiHarmonize,
                       ETD = a.ETD,
                       ETA = a.ETA,
                       Routing = a.Routing,
                       RoutingIds = a.RoutingIds,
                       VoyageTripNumber = a.VoyageTripNumber,
                       HasContainerException = a.HasContainerException,
                       Make = a.Make,
                       Year = a.Year,
                       Model = a.Model,
                       Color = a.Color,
                       ChassisNumber = a.ChassisNumber,
                       RegistrationNumber = a.RegistrationNumber,
                       CountryId = a.CountryId,
                       WarehouseReleaseNumber = a.WarehouseReleaseNumber,
                       HorseId = a.HorseId,
                       HorseName = a.Horse == null ? null : a.Horse.Name,
                       LCLContainerTypeId = a.LCLContainerTypeId,
                       ContainerEntityId = a.ContainerEntityId,
                   }).FirstOrDefault();

            myResult.InsideShipmentPackages = insideShipmentPackageQuery.GetInsideShipmentPackages(myResult.Id, tenant);
            myResult.ShipmentPackageItems = shipmentPackageItemQuery.GetShipmentPackageItems(myResult.Id, tenant);
            myResult.ShipmentPackageHarmonizes = shipmentPackageHarmonizeQuery.GetShipmentPackageHarmonizes(myResult.Id, tenant);

            return myResult;
        }

        public List<ShipmentPackagePM> GetShipmentPackages(string shipmentId, string myShipmentNumber, int tenant)
        {
            InsideShipmentPackageRepository insideShipmentPackagesRepository = new InsideShipmentPackageRepository(repository.context);
            InsideShipmentPackageQuery insideShipmentPackageQuery = new InsideShipmentPackageQuery(insideShipmentPackagesRepository);

            ShipmentPackageItemRepository shipmentPackageItemRepository = new ShipmentPackageItemRepository(repository.context);
            ShipmentPackageItemQuery shipmentPackageItemQuery = new ShipmentPackageItemQuery(shipmentPackageItemRepository);

            ShipmentPackageHarmonizeRepository shipmentPackageHarmonizeRepository = new ShipmentPackageHarmonizeRepository(repository.context);
            ShipmentPackageHarmonizeQuery shipmentPackageHarmonizeQuery = new ShipmentPackageHarmonizeQuery(shipmentPackageHarmonizeRepository);

            List<ShipmentPackagePM> shipmentPackages
                = (from a in repository.context.ShipmentPackages.Include("Horse")
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentPackagePM()
                   {
                       ClassNumber = a.ClassNumber,
                       ContainerNumber = a.ContainerNumber,
                       Description = a.Description,
                       FlashPoint = a.FlashPoint,
                       Harmonize = a.Harmonize,
                       OriginalShipmentPackageId = a.OriginalShipmentPackageId,
                       Height = a.Height,
                       Id = a.Id,
                       IMDGCode = a.IMDGCode,
                       Length = a.Length,
                       MarksAndNumbers = a.MarksAndNumbers,
                       Quantity = a.Quantity,
                       InUse = a.InUse,
                       ShipperSeal = a.ShipperSeal,
                       CarrierSeal = a.CarrierSeal,
                       ShipmentPMId = a.ShipmentId,
                       ShipmentId = a.ShipmentId,
                       SOC = a.SOC,
                       Temperature = a.Temperature,
                       Tenant = a.Tenant,
                       Tare = a.Tare,
                       UnNumber = a.UnNumber,
                       Ventilation = a.Ventilation,
                       Volume = a.Volume,
                       Weight = a.Weight,
                       Width = a.Width,
                       PackagingGroup = a.PackagingGroup,
                       MaterialDescription = a.MaterialDescription,
                       IsDangerous = a.IsDangerous,
                       VolumetricWeight = a.VolumetricWeight,
                       CommodityId = a.CommodityId,
                       NumberOfInsidePackages = a.NumberOfInsidePackages,
                       NumberOfInsidePackagesDetails = a.NumberOfInsidePackagesDetails,
                       VGM = a.VGM,
                       MethodUsed = a.MethodUsed,
                       ShipmentNumber = myShipmentNumber,
                       PackageTypeId = a.PackageTypeId,
                       IsDeliveryFU = a.IsDeliveryFU,
                       DeliveryId = a.DeliveryId,
                       DeliveryETD = a.DeliveryETD,
                       DeliveryATD = a.DeliveryATD,
                       DeliveryETA = a.DeliveryETA,
                       DeliveryATA = a.DeliveryATA,
                       DeliveryTo = a.DeliveryTo,
                       DeliveryFrom = a.DeliveryFrom,
                       IsEmptyContainerReturnFU = a.IsEmptyContainerReturnFU,
                       EmptyContainerReturnId = a.EmptyContainerReturnId,
                       EmptyContainerReturnETD = a.EmptyContainerReturnETD,
                       EmptyContainerReturnATD = a.EmptyContainerReturnATD,
                       EmptyContainerReturnETA = a.EmptyContainerReturnETA,
                       EmptyContainerReturnATA = a.EmptyContainerReturnATA,
                       EmptyContainerReturnTo = a.EmptyContainerReturnTo,
                       EmptyContainerReturnFrom = a.EmptyContainerReturnFrom,
                       Reference1 = a.Reference1,
                       Reference2 = a.Reference2,
                       Reference3 = a.Reference3,
                       Reference4 = a.Reference4,
                       CommodityNumber = a.CommodityNumber,
                       CommodityName = a.CommodityName,
                       CeficClass = a.CeficClass,
                       KelmerCode = a.KelmerCode,
                       EMS = a.EMS,
                       ProperShippingName = a.ProperShippingName,
                       MarinePollutant = a.MarinePollutant,
                       Notes = a.Notes,
                       TemperatureUnitCode = a.TemperatureUnitCode,
                       FlashPointTemperatureUnitCode = a.FlashPointTemperatureUnitCode,
                       NonActiveContainer = a.NonActiveContainer,
                       OnCarriageATA = a.OnCarriageATA,
                       OnCarriageATD = a.OnCarriageATD,
                       OnCarriageETA = a.OnCarriageETA,
                       OnCarriageETD = a.OnCarriageETD,
                       LastStatusCode = a.LastStatusCode,
                       LastStatusDate = a.LastStatusDate,
                       //LastStatusName = a.LastStatus == null ? null : a.LastStatus.Name,
                       DeliveryTransportModeCode = a.DeliveryTransportModeCode,
                       ECRTransportModeCode = a.ECRTransportModeCode,
                       IsMultiHarmonize = a.IsMultiHarmonize,
                       ETD = a.ETD,
                       ETA = a.ETA,
                       Routing = a.Routing,
                       RoutingIds = a.RoutingIds,
                       VoyageTripNumber = a.VoyageTripNumber,
                       HasContainerException = a.HasContainerException,
                       Make = a.Make,
                       Year = a.Year,
                       Model = a.Model,
                       Color = a.Color,
                       ChassisNumber = a.ChassisNumber,
                       RegistrationNumber = a.RegistrationNumber,
                       CountryId = a.CountryId,
                       //CountryName = a.Country != null ? a.Country.EnglishName : "",
                       WarehouseReleaseNumber = a.WarehouseReleaseNumber,
                       HorseId = a.HorseId,
                       HorseName = a.Horse == null ? null : a.Horse.Name,
                       LCLContainerTypeId = a.LCLContainerTypeId,
                       ContainerEntityId = a.ContainerEntityId,
                   }).ToList();

            var commonContext = CommonDataContext.GetContext(tenant);
            PackageTypeRepository PTypeRepo = new PackageTypeRepository(commonContext);
            //CountryRepository CountryRepo = new CountryRepository(commonContext);
            //PackageTypeRepository PTypeRepo = new PackageTypeRepository(commonContext);

            foreach (ShipmentPackagePM package in shipmentPackages)
            {
                package.InsideShipmentPackages = insideShipmentPackageQuery.GetInsideShipmentPackages(package.Id, package.Tenant);
                package.ShipmentPackageItems = shipmentPackageItemQuery.GetShipmentPackageItems(package.Id, package.Tenant);
                package.ShipmentPackageHarmonizes = shipmentPackageHarmonizeQuery.GetShipmentPackageHarmonizes(package.Id, package.Tenant);
                if (!string.IsNullOrEmpty(package.PackageTypeId))
                {
                    PackageType CurrentPackageType = PTypeRepo.GetSinglePackageType(package.PackageTypeId, tenant);
                    package.PackageTypeCode = CurrentPackageType.Code;
                    package.PackageTypeName = CurrentPackageType.EnglishName;
                    package.PackageTypeLocalName = CurrentPackageType.LocalName;
                    package.PackageTypeNote = CurrentPackageType.Notes;
                    package.PrintAs = CurrentPackageType.PrintAs;
                    package.IsPackageAddedManually = CurrentPackageType.AddedManually;
                    package.IsContainer = CurrentPackageType.IsContainer;
                    package.IsContainerRefrigerated = CurrentPackageType.IsRefrigerated;
                    package.PackageTypeIsInland = CurrentPackageType.IsInland;
                    package.PackageTypeIsOcean = CurrentPackageType.IsOcean;
                    package.PackageTypeIsAir = CurrentPackageType.IsAir;
                    package.TEU = CurrentPackageType.TEU;
                    package.ContainerSize = CurrentPackageType.ContainerSize;
                    package.PackageTypeVolume = CurrentPackageType.Volume;
                    package.IsVehicle = CurrentPackageType.IsVehicle;

                }
                if (!string.IsNullOrEmpty(package.CountryId))
                {
                    package.CountryName = (from d in commonContext.Countries

                                           where d.Id == package.CountryId

                                           select d.EnglishName).FirstOrDefault();

                }
                

                this.MapTheLastStatusCode(package);

            }

            return shipmentPackages;
        }

        private void MapTheLastStatusCode(ShipmentPackagePM package)
        {
            if (!string.IsNullOrEmpty(package.LastStatusCode))
            {
                package.LastStatusName = (from d in repository.context.INTTRAStatuses
                                          where d.Code == package.LastStatusCode
                                          select d.Name).FirstOrDefault();
            }
        }

        public List<ShipmentPackagePM> GetShipmentPackages(List<string> shipmentIds, int tenant)
        {
            List<ShipmentPackagePM> myResult = new List<ShipmentPackagePM>();

            if (shipmentIds.Count > 0)
            {
                myResult = (from a in repository.context.ShipmentPackages.Include("PackageType").Include("LastStatus")
                            where a.Tenant == tenant && shipmentIds.Contains(a.ShipmentId)
                            select new ShipmentPackagePM()
                            {
                                ClassNumber = a.ClassNumber,
                                OriginalShipmentPackageId = a.OriginalShipmentPackageId,
                                ContainerNumber = a.ContainerNumber,
                                Description = a.Description,
                                FlashPoint = a.FlashPoint,
                                Harmonize = a.Harmonize,
                                Height = a.Height,
                                Id = a.Id,
                                IMDGCode = a.IMDGCode,
                                Length = a.Length,
                                MarksAndNumbers = a.MarksAndNumbers,
                                Quantity = a.Quantity,
                                InUse = a.InUse,
                                ShipperSeal = a.ShipperSeal,
                                CarrierSeal = a.CarrierSeal,
                                ShipmentPMId = a.ShipmentId,
                                ShipmentId = a.ShipmentId,
                                SOC = a.SOC,
                                Temperature = a.Temperature,
                                Tenant = a.Tenant,
                                Tare = a.Tare,
                                UnNumber = a.UnNumber,
                                Ventilation = a.Ventilation,
                                Volume = a.Volume,
                                Weight = a.Weight,
                                Width = a.Width,
                                PackagingGroup = a.PackagingGroup,
                                MaterialDescription = a.MaterialDescription,
                                IsDangerous = a.IsDangerous,
                                VolumetricWeight = a.VolumetricWeight,
                                CommodityId = a.CommodityId,
                                NumberOfInsidePackages = a.NumberOfInsidePackages,
                                NumberOfInsidePackagesDetails = a.NumberOfInsidePackagesDetails,
                                VGM = a.VGM,
                                MethodUsed = a.MethodUsed,
                                PackageTypeId = a.PackageTypeId,
                                PackageTypeCode = a.PackageType == null ? null : a.PackageType.Code,
                                PackageTypeName = a.PackageType == null ? null : a.PackageType.EnglishName,
                                PackageTypeLocalName = a.PackageType == null ? null : a.PackageType.LocalName,
                                PackageTypeNote = a.PackageType == null ? null : a.PackageType.Notes,
                                PrintAs = a.PackageType == null ? null : a.PackageType.PrintAs,
                                IsPackageAddedManually = a.PackageType == null ? false : a.PackageType.AddedManually,
                                IsContainer = a.PackageType == null ? false : a.PackageType.IsContainer,
                                IsContainerRefrigerated = a.PackageType == null ? false : a.PackageType.IsRefrigerated,
                                PackageTypeIsInland = a.PackageType == null ? false : a.PackageType.IsInland,
                                PackageTypeIsOcean = a.PackageType == null ? false : a.PackageType.IsOcean,
                                PackageTypeIsAir = a.PackageType == null ? false : a.PackageType.IsAir,
                                TEU = a.PackageType == null ? 0 : a.PackageType.TEU,
                                ContainerSize = a.PackageType == null ? 0 : a.PackageType.ContainerSize,
                                PackageTypeVolume = a.PackageType == null ? 0 : a.PackageType.Volume,
                                IsDeliveryFU = a.IsDeliveryFU,
                                DeliveryId = a.DeliveryId,
                                DeliveryETD = a.DeliveryETD,
                                DeliveryATD = a.DeliveryATD,
                                DeliveryETA = a.DeliveryETA,
                                DeliveryATA = a.DeliveryATA,
                                DeliveryTo = a.DeliveryTo,
                                DeliveryFrom = a.DeliveryFrom,
                                IsEmptyContainerReturnFU = a.IsEmptyContainerReturnFU,
                                EmptyContainerReturnId = a.EmptyContainerReturnId,
                                EmptyContainerReturnETD = a.EmptyContainerReturnETD,
                                EmptyContainerReturnATD = a.EmptyContainerReturnATD,
                                EmptyContainerReturnETA = a.EmptyContainerReturnETA,
                                EmptyContainerReturnATA = a.EmptyContainerReturnATA,
                                EmptyContainerReturnTo = a.EmptyContainerReturnTo,
                                EmptyContainerReturnFrom = a.EmptyContainerReturnFrom,
                                Reference1 = a.Reference1,
                                Reference2 = a.Reference2,
                                Reference3 = a.Reference3,
                                Reference4 = a.Reference4,
                                CommodityNumber = a.CommodityNumber,
                                CommodityName = a.CommodityName,
                                CeficClass = a.CeficClass,
                                KelmerCode = a.KelmerCode,
                                EMS = a.EMS,
                                ProperShippingName = a.ProperShippingName,
                                MarinePollutant = a.MarinePollutant,
                                Notes = a.Notes,
                                TemperatureUnitCode = a.TemperatureUnitCode,
                                FlashPointTemperatureUnitCode = a.FlashPointTemperatureUnitCode,
                                NonActiveContainer = a.NonActiveContainer,
                                OnCarriageATA = a.OnCarriageATA,
                                OnCarriageATD = a.OnCarriageATD,
                                OnCarriageETA = a.OnCarriageETA,
                                OnCarriageETD = a.OnCarriageETD,
                                LastStatusCode = a.LastStatusCode,
                                LastStatusDate = a.LastStatusDate,
                                LastStatusName = a.LastStatus == null ? null : a.LastStatus.Name,
                                DeliveryTransportModeCode = a.DeliveryTransportModeCode,
                                ECRTransportModeCode = a.ECRTransportModeCode,
                                IsMultiHarmonize = a.IsMultiHarmonize,
                                ETD = a.ETD,
                                ETA = a.ETA,
                                Routing = a.Routing,
                                RoutingIds = a.RoutingIds,
                                VoyageTripNumber = a.VoyageTripNumber,
                                HasContainerException = a.HasContainerException,
                                Make = a.Make,
                                Year = a.Year,
                                Model = a.Model,
                                Color = a.Color,
                                ChassisNumber = a.ChassisNumber,
                                RegistrationNumber = a.RegistrationNumber,
                                CountryId = a.CountryId,
                                WarehouseReleaseNumber = a.WarehouseReleaseNumber,
                                LCLContainerTypeId = a.LCLContainerTypeId,
                                ContainerEntityId = a.ContainerEntityId,
                            }).ToList();
            }

            return myResult;
        }
    }
}