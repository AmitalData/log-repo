using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPickUpDeliveryPackageQuery
    {
        ShipmentPickUpDeliveryPackageRepository repository;

        public ShipmentPickUpDeliveryPackageQuery(int tenant)
        {
            repository = new ShipmentPickUpDeliveryPackageRepository(tenant);
        }

        public ShipmentPickUpDeliveryPackageQuery(ShipmentPickUpDeliveryPackageRepository repository)
        {
            this.repository = repository;
        }

        public List<ShipmentPickUpDeliveryPackagePM> GetShipmentPickUpDeliveryPackages(string deliveryId, int tenant)
        {
            PickUpDeliveryPackageHarmonizeRepository myRepository = new PickUpDeliveryPackageHarmonizeRepository(repository.context);
            PickUpDeliveryPackageHarmonizeQuery query = new PickUpDeliveryPackageHarmonizeQuery(myRepository);

            List<ShipmentPickUpDeliveryPackagePM> packages = (from a in repository.context.ShipmentPickUpDeliveryPackages
                                                              where a.Tenant == tenant && a.ShipmentPickUpDeliveryId == deliveryId
                                                              select new ShipmentPickUpDeliveryPackagePM()
                                                              {
                                                                  ContainerNumber = a.ContainerNumber,
                                                                  Description = a.Description,
                                                                  Id = a.Id,
                                                                  PackageTypeId = a.PackageTypeId,
                                                                  Quantity = a.Quantity,
                                                                  Tenant = a.Tenant,
                                                                  ShipmentPickUpDeliveryId = a.ShipmentPickUpDeliveryId,
                                                                  Volume = a.Volume,
                                                                  Weight = a.Weight,
                                                                  ShipperSeal = a.ShipperSeal,
                                                                  Harmonize = a.Harmonize,
                                                                  Width = a.Width,
                                                                  Height = a.Height,
                                                                  Length = a.Length,
                                                                  OriginalShipmentPackageId = a.OriginalShipmentPackageId,
                                                                  IsMultiHarmonize = a.IsMultiHarmonize,
                                                                  Make = a.Make,
                                                                  Year = a.Year,
                                                                  Model = a.Model,
                                                                  Color = a.Color,
                                                                  ChassisNumber = a.ChassisNumber,
                                                                  RegistrationNumber = a.RegistrationNumber,
                                                                  CountryId = a.CountryId,
                                                                  ContainerEntityId = a.ContainerEntityId,
                                                              }).ToList();

            foreach (ShipmentPickUpDeliveryPackagePM package in packages)
            {
                package.PickUpDeliveryPackageHarmonizes = query.GetPickUpDeliveryPackageHarmonizes(package.Id, tenant);

                PackageType packageType = PackageTypeRepository.GetSinglePackageType(package.PackageTypeId, package.Tenant, true);
                package.PackageTypeName = packageType != null ? packageType.EnglishName : null;
                package.PackageTypeTEU = packageType != null ? packageType.TEU : 0;
            }
            return packages;
        }

        public ShipmentPickUpDeliveryPackagePM GetSingleShipmentPickUpDeliveryPackagePM(string id, int tenant)
        {
            PickUpDeliveryPackageHarmonizeRepository myRepository = new PickUpDeliveryPackageHarmonizeRepository(repository.context);
            PickUpDeliveryPackageHarmonizeQuery query = new PickUpDeliveryPackageHarmonizeQuery(myRepository);

            ShipmentPickUpDeliveryPackagePM myResult = (from a in repository.context.ShipmentPickUpDeliveryPackages.Include("PackageType")
                                                        where a.Id == id && a.Tenant == tenant
                                                        select new ShipmentPickUpDeliveryPackagePM()
                                                        {
                                                            ContainerNumber = a.ContainerNumber,
                                                            Description = a.Description,
                                                            Id = a.Id,
                                                            PackageTypeId = a.PackageTypeId,
                                                            PackageTypeName = a.PackageType == null ? null : a.PackageType.EnglishName,
                                                            PackageTypeTEU = a.PackageType == null ? 0 : a.PackageType.TEU,
                                                            Quantity = a.Quantity,
                                                            Tenant = a.Tenant,
                                                            ShipmentPickUpDeliveryId = a.ShipmentPickUpDeliveryId,
                                                            Volume = a.Volume,
                                                            Weight = a.Weight,
                                                            ShipperSeal = a.ShipperSeal,
                                                            Harmonize = a.Harmonize,
                                                            Width = a.Width,
                                                            Height = a.Height,
                                                            Length = a.Length,
                                                            OriginalShipmentPackageId = a.OriginalShipmentPackageId,
                                                            IsMultiHarmonize = a.IsMultiHarmonize,
                                                            Make = a.Make,
                                                            Year = a.Year,
                                                            Model = a.Model,
                                                            Color = a.Color,
                                                            ChassisNumber = a.ChassisNumber,
                                                            RegistrationNumber = a.RegistrationNumber,
                                                            CountryId = a.CountryId,
                                                            ContainerEntityId = a.ContainerEntityId,
                                                        }).FirstOrDefault();

            myResult.PickUpDeliveryPackageHarmonizes = query.GetPickUpDeliveryPackageHarmonizes(myResult.Id, tenant);

            return myResult;
        }
        public ShipmentPickUpDeliveryPackagePM GetSinglePM(string id, int tenant)
        {
            return GetSingleShipmentPickUpDeliveryPackagePM( id,  tenant);       
        }            
    }
}