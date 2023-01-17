using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class InsideShipmentPackageQuery
    {
        InsideShipmentPackageRepository repository;        
        public InsideShipmentPackageQuery(int tenant)
        {
            repository = new InsideShipmentPackageRepository(tenant);
        }
        public InsideShipmentPackageQuery(InsideShipmentPackageRepository myRepository)
        {
            repository = myRepository;
        }

        public List<InsideShipmentPackagePM> GetInsideShipmentPackages(string shipmentPackageId, int tenant)
        {
            ShipmentPackageHarmonizeRepository shipmentPackageHarmonizeRepository = new ShipmentPackageHarmonizeRepository(repository.context);
            ShipmentPackageHarmonizeQuery shipmentPackageHarmonizeQuery = new ShipmentPackageHarmonizeQuery(shipmentPackageHarmonizeRepository);

            List<InsideShipmentPackagePM> myResult = (from a in repository.context.InsideShipmentPackages.Include("PackageType").Include("Country")
                                                      where a.Tenant == tenant && a.ShipmentPackageId == shipmentPackageId
                                                      select new InsideShipmentPackagePM()
                                                      {
                                                          Height = a.Height,
                                                          Id = a.Id,
                                                          Length = a.Length,
                                                          Quantity = a.Quantity,
                                                          ShipmentPackageId = a.ShipmentPackageId,
                                                          Tenant = a.Tenant,
                                                          Weight = a.Weight,
                                                          Width = a.Width,
                                                          Volume = a.Volume,
                                                          Description = a.Description,
                                                          OriginalShipmentPackageId = a.OriginalShipmentPackageId,
                                                          OriginalInsideShipmentPackageId = a.OriginalInsideShipmentPackageId,
                                                          VolumetricWeight = a.VolumetricWeight,
                                                          PackageTypeId = a.PackageTypeId,
                                                          PackageTypeCode = a.PackageType == null ? null : a.PackageType.Code,
                                                          PackageTypeName = a.PackageType == null ? null : a.PackageType.EnglishName,
                                                          PackageTypeLocalName = a.PackageType == null ? null : a.PackageType.LocalName,
                                                          PackageTypeNote = a.PackageType == null ? null : a.PackageType.Notes,
                                                          PrintAs = a.PackageType == null ? null : a.PackageType.PrintAs,
                                                          IsPackageAddedManually = a.PackageType == null ? false : a.PackageType.AddedManually,
                                                          IsContainer = a.PackageType == null ? false : a.PackageType.IsContainer,
                                                          PackageTypeIsInland = a.PackageType == null ? false : a.PackageType.IsInland,
                                                          PackageTypeIsOcean = a.PackageType == null ? false : a.PackageType.IsOcean,
                                                          PackageTypeIsAir = a.PackageType == null ? false : a.PackageType.IsAir,
                                                          TEU = a.PackageType == null ? 0 : a.PackageType.TEU,
                                                          ContainerSize = a.PackageType == null ? 0 : a.PackageType.ContainerSize,
                                                          PackageTypeVolume = a.PackageType == null ? 0 : a.PackageType.Volume,
                                                          Reference1 = a.Reference1,
                                                          Reference2 = a.Reference2,
                                                          Reference3 = a.Reference3,
                                                          Reference4 = a.Reference4,
                                                          CommodityNumber = a.CommodityNumber,
                                                          CommodityName = a.CommodityName,
                                                          Make = a.Make,
                                                          Year = a.Year,
                                                          Model = a.Model,
                                                          Color = a.Color,
                                                          ChassisNumber = a.ChassisNumber,
                                                          RegistrationNumber = a.RegistrationNumber,
                                                          CountryId = a.CountryId,
                                                          CountryName = a.Country != null ? a.Country.EnglishName : "",
                                                          CountryCode = a.Country != null ? a.Country.Code : "",
                                                          Harmonize = a.Harmonize,
                                                          IsMultiHarmonize = a.IsMultiHarmonize,
                                                          HorseName = a.Horse == null ? null : a.Horse.Name,
                                                          HorseId = a.HorseId,
                                                      }).OrderBy(a => a.Id).ToList();

            foreach (InsideShipmentPackagePM package in myResult)
            {
                package.InsidePackageHarmonizes = shipmentPackageHarmonizeQuery.GetInsideShipmentPackageHarmonizes(package.Id, package.Tenant);
                package.VolumeInCBM = ShipmentMapping.GetVolumeInCBM("CBM", package.Volume);
                package.GrossWeightInKG = ShipmentMapping.GetWeightInKG("KG", package.Weight);
                package.VolumeInCBF = ShipmentMapping.GetVolumeInCBF(package.VolumeInCBM);
                package.GrossWeightInLB = ShipmentMapping.GetWeightInLB(package.GrossWeightInKG);
            }

            return myResult;
        }

        public InsideShipmentPackagePM GetSinglePM(string shipmentPackageId, int tenant)
        {
            ShipmentPackageHarmonizeRepository shipmentPackageHarmonizeRepository = new ShipmentPackageHarmonizeRepository(repository.context);
            ShipmentPackageHarmonizeQuery shipmentPackageHarmonizeQuery = new ShipmentPackageHarmonizeQuery(shipmentPackageHarmonizeRepository);

            InsideShipmentPackagePM myResult = (from a in repository.context.InsideShipmentPackages.Include("PackageType").Include("Country")
                                                where a.Tenant == tenant && a.Id == shipmentPackageId
                                                select new InsideShipmentPackagePM()
                                                {
                                                    Height = a.Height,
                                                    Id = a.Id,
                                                    Length = a.Length,
                                                    Quantity = a.Quantity,
                                                    ShipmentPackageId = a.ShipmentPackageId,
                                                    Tenant = a.Tenant,
                                                    Weight = a.Weight,
                                                    Width = a.Width,
                                                    Volume = a.Volume,
                                                    Description = a.Description,
                                                    OriginalShipmentPackageId = a.OriginalShipmentPackageId,
                                                    OriginalInsideShipmentPackageId = a.OriginalInsideShipmentPackageId,
                                                    VolumetricWeight = a.VolumetricWeight,
                                                    PackageTypeId = a.PackageTypeId,
                                                    PackageTypeCode = a.PackageType == null ? null : a.PackageType.Code,
                                                    PackageTypeName = a.PackageType == null ? null : a.PackageType.EnglishName,
                                                    PackageTypeLocalName = a.PackageType == null ? null : a.PackageType.LocalName,
                                                    PackageTypeNote = a.PackageType == null ? null : a.PackageType.Notes,
                                                    PrintAs = a.PackageType == null ? null : a.PackageType.PrintAs,
                                                    IsPackageAddedManually = a.PackageType == null ? false : a.PackageType.AddedManually,
                                                    IsContainer = a.PackageType == null ? false : a.PackageType.IsContainer,
                                                    PackageTypeIsInland = a.PackageType == null ? false : a.PackageType.IsInland,
                                                    PackageTypeIsOcean = a.PackageType == null ? false : a.PackageType.IsOcean,
                                                    PackageTypeIsAir = a.PackageType == null ? false : a.PackageType.IsAir,
                                                    TEU = a.PackageType == null ? 0 : a.PackageType.TEU,
                                                    ContainerSize = a.PackageType == null ? 0 : a.PackageType.ContainerSize,
                                                    PackageTypeVolume = a.PackageType == null ? 0 : a.PackageType.Volume,
                                                    Reference1 = a.Reference1,
                                                    Reference2 = a.Reference2,
                                                    Reference3 = a.Reference3,
                                                    Reference4 = a.Reference4,
                                                    CommodityNumber = a.CommodityNumber,
                                                    CommodityName = a.CommodityName,
                                                    Make = a.Make,
                                                    Year = a.Year,
                                                    Model = a.Model,
                                                    Color = a.Color,
                                                    ChassisNumber = a.ChassisNumber,
                                                    RegistrationNumber = a.RegistrationNumber,
                                                    CountryId = a.CountryId,
                                                    CountryName = a.Country != null ? a.Country.EnglishName : "",
                                                    CountryCode = a.Country != null ? a.Country.Code : "",
                                                    Harmonize = a.Harmonize,
                                                    IsMultiHarmonize = a.IsMultiHarmonize,
                                                    HorseName = a.Horse == null ? null : a.Horse.Name,
                                                    HorseId = a.HorseId,
                                                }).FirstOrDefault();

            myResult.InsidePackageHarmonizes = shipmentPackageHarmonizeQuery.GetInsideShipmentPackageHarmonizes(myResult.Id, tenant);

            return myResult;
        }
    }
}