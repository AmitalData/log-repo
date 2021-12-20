using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentOrderPackageQuery
    {
        ShipmentOrderPackageRepository repository;
         
        public ShipmentOrderPackageQuery(int tenant)
        {
            repository = new ShipmentOrderPackageRepository(tenant);
        }

        public ShipmentOrderPackageQuery(ShipmentOrderPackageRepository repository)
        {
            this.repository = repository;
        }

        public List<ShipmentOrderPackagePM> GetShipmentOrderPackagesByShipment(string shipmentId, int tenant)
        {
            List<ShipmentOrderPackagePM> shipmentOrderPackages = (from a in repository.context.ShipmentOrderPackages
                                                                  where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                                  select new ShipmentOrderPackagePM()
                                                                  {
                                                                      Id = a.Id,
                                                                      PackageTypeId = a.PackageTypeId,
                                                                      Quantity = a.Quantity,
                                                                      Tenant = a.Tenant,
                                                                      IsContainer = a.IsContainer,
                                                                      ShipmentId = a.ShipmentId,
                                                                      GrossWeight = a.GrossWeight,
                                                                      Volume = a.Volume,
                                                                      Length = a.Length,
                                                                      Height = a.Height,
                                                                      Width = a.Width,
                                                                      VolumetricWeight = a.VolumetricWeight,
                                                                      ContainerTypeId = a.PackageTypeId,
                                                                      ContainerNumber = a.ContainerNumber,
                                                                  }).ToList();

            foreach (ShipmentOrderPackagePM package in shipmentOrderPackages)
            {
                PackageType packageType = PackageTypeRepository.GetSinglePackageType(package.PackageTypeId, package.Tenant, true);
                if (packageType != null)
                {
                    package.PackageTypeName = packageType.EnglishName;
                }
                package.Dimensions = packageType != null && packageType.IsContainer ? "" : package.Length + "-" + package.Width + "-" + package.Height;
            }
            return shipmentOrderPackages;
        }

        public ShipmentOrderPackagePM GetSingleShipmentOrderPackagePM(string id)
        {
            return (from a in repository.context.ShipmentOrderPackages.Include("PackageType") 
                    where a.Id == id
                    select new ShipmentOrderPackagePM()
                    {
                        Id = a.Id,
                        PackageTypeId = a.PackageTypeId,
                        PackageTypeName = a.PackageType == null ? null : a.PackageType.EnglishName,
                        Quantity = a.Quantity,
                        Tenant = a.Tenant,
                        IsContainer = a.IsContainer,
                        ShipmentId = a.ShipmentId,
                        GrossWeight = a.GrossWeight,
                        Volume = a.Volume,
                        Length = a.Length,
                        Height = a.Height,
                        Width = a.Width,
                        VolumetricWeight = a.VolumetricWeight,
                        ContainerNumber = a.ContainerNumber,
                    }).FirstOrDefault();
        }
    }
}