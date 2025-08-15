using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PackageTypeQuery
    {
        PackageTypeRepository repository;

        public PackageTypeQuery(int tenant)
        {
            repository = new PackageTypeRepository(tenant);
        }
        public PackageTypeQuery(PackageTypeRepository repository)
        {
            this.repository = repository;
        }

        public PackageTypePM GetSinglePM(string id, int tenant)
        {
            PackageTypePM instance = (from a in repository.context.PackageTypes.Include("Measurement")
                                      where a.Tenant == tenant
                                      && a.Id == id
                                      select new PackageTypePM()
                                      {
                                          AddedManually = a.AddedManually,
                                          Code = a.Code,
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          IsAir = a.IsAir,
                                          IsContainer = a.IsContainer,
                                          IsInland = a.IsInland,
                                          IsOcean = a.IsOcean,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          Tenant = a.Tenant,
                                          ContainerSize = a.ContainerSize,
                                          TEU = a.TEU,
                                          Volume = a.Volume,
                                          InActive = a.InActive,
                                          MeasurementId = a.MeasurementId,
                                          MeasurementCode = a.Measurement == null ? "" : a.Measurement.Code,
                                          MeasurementShortName = a.Measurement == null ? "" : a.Measurement.ShortName,
                                          SearchFields = a.SearchFields,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          PrintAs = a.PrintAs,
                                          IsRefrigerated = a.IsRefrigerated,
                                          IsVehicle = a.IsVehicle,
                                      }).FirstOrDefault();

            return instance;
        }
        
        public IQueryable<PackageTypePM> GetPackageTypePMsByTenant(int tenant)
        {
            IQueryable<PackageTypePM> packageTypes = from a in repository.context.PackageTypes.Include("Measurement")
                                                     where a.Tenant == tenant
                                                     select new PackageTypePM()
                                                     {
                                                         AddedManually = a.AddedManually,
                                                         Code = a.Code,
                                                         EnglishName = a.EnglishName,
                                                         Id = a.Id,
                                                         IsAir = a.IsAir,
                                                         IsContainer = a.IsContainer,
                                                         IsInland = a.IsInland,
                                                         IsOcean = a.IsOcean,
                                                         LocalName = a.LocalName,
                                                         Notes = a.Notes,
                                                         Tenant = a.Tenant,
                                                         ContainerSize = a.ContainerSize,
                                                         TEU = a.TEU,
                                                         Volume = a.Volume,
                                                         InActive = a.InActive,
                                                         MeasurementId = a.MeasurementId,
                                                         MeasurementCode = a.Measurement == null ? "" : a.Measurement.Code,
                                                         MeasurementShortName = a.Measurement == null ? "" : a.Measurement.ShortName,
                                                         SearchFields = a.SearchFields,
                                                         ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                         PrintAs = a.PrintAs,
                                                         IsRefrigerated = a.IsRefrigerated,
                                                         IsVehicle = a.IsVehicle,
                                                     };
            return packageTypes;
        }

        public IQueryable<PackageTypeList> GetIQueryableEntityList(IQueryable<PackageType> iQueryable)
        {
            IQueryable<PackageTypeList> result = from entity in iQueryable.Include("Measurement")
                                                 select new PackageTypeList()
                                                 {
                                                     Id = entity.Id,
                                                     Tenant = entity.Tenant,
                                                     Code = entity.Code,
                                                     EnglishName = entity.EnglishName,
                                                     LocalName = entity.LocalName,
                                                     IsAir = entity.IsAir,
                                                     IsInland = entity.IsInland,
                                                     IsOcean = entity.IsOcean,
                                                     Notes = entity.Notes,
                                                     IsContainer = entity.IsContainer,
                                                     TransportModeId = ((entity.IsAir ? "A" : "") + (entity.IsOcean ? "O" : "") + (entity.IsInland ? "I" : "")),
                                                     MeasurementId = entity.MeasurementId,
                                                     MeasurementCode = entity.Measurement == null ? "" : entity.Measurement.Code,
                                                     MeasurementShortName = entity.Measurement == null ? "" : entity.Measurement.ShortName,
                                                     AddedManually = entity.AddedManually,
                                                     ContainerSize = entity.ContainerSize,
                                                     InActive = entity.InActive,
                                                     TEU = entity.TEU,
                                                     Volume = entity.Volume,
                                                     SearchFields = entity.SearchFields,
                                                     PrintAs = entity.PrintAs,
                                                     IsRefrigerated = entity.IsRefrigerated,
                                                     IsVehicle = entity.IsVehicle,
                                                 };
            return result;
        }

        public PackageTypePM GetSinglePMByCode(string Code, int tenant)
        {
            PackageTypePM instance = (from a in repository.context.PackageTypes.Include("Measurement")
                                      where a.Tenant == tenant
                                      && a.Code == Code
                                      select new PackageTypePM()
                                      {
                                          AddedManually = a.AddedManually,
                                          Code = a.Code,
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          IsAir = a.IsAir,
                                          IsContainer = a.IsContainer,
                                          IsInland = a.IsInland,
                                          IsOcean = a.IsOcean,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          Tenant = a.Tenant,
                                          ContainerSize = a.ContainerSize,
                                          TEU = a.TEU,
                                          Volume = a.Volume,
                                          InActive = a.InActive,
                                          MeasurementId = a.MeasurementId,
                                          MeasurementCode = a.Measurement == null ? "" : a.Measurement.Code,
                                          MeasurementShortName = a.Measurement == null ? "" : a.Measurement.ShortName,
                                          SearchFields = a.SearchFields,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          PrintAs = a.PrintAs,
                                          IsRefrigerated = a.IsRefrigerated,
                                          IsVehicle = a.IsVehicle,
                                      }).FirstOrDefault();

            return instance;
        }
        
    }
}