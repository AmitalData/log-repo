
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Security;
using System.Web;
using Logitude.WarehouseLib.BL.Helpers;
using Logitude.WarehouseLib.BL.DataContracts;
using Simplog.Data.Helpers;

namespace Logitude.WarehouseLib.BL.EntityQueryServices
{

    public partial class WarehouseReleasePackageQueryService
    {


        public List<WarehouseReleasePackagePM> GetWarehouseReleasePackagePMListsByWarehouseReleaseId(string WarehouseReleaseId, int tenant)
        {

            List<WarehouseReleasePackagePM> myResult = (from a in context.WarehouseReleasePackages.Include("WarehouseRelease")
                                                        where a.Tenant == tenant && a.WarehouseRelease.Id == WarehouseReleaseId
                                                        select new WarehouseReleasePackagePM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            Seal = a.Seal,
                                                            Description = a.Description,
                                                            ContainerNumber = a.ContainerNumber,
                                                            Dimensions = a.IsContainer ? "" : a.Length + "-" + a.Width + "-" + a.Height,
                                                            Harmonize = a.Harmonize,
                                                            Height = a.Height,
                                                            Length = a.Length,
                                                            PackageTypeName = a.PackageType.EnglishName,
                                                            WarehouseReleaseId = a.WarehouseReleaseId,
                                                            Width = a.Width,
                                                            PackageTypeId = a.PackageTypeId,
                                                            Volume = a.Volume,
                                                            Quantity = a.Quantity,
                                                            Weight = a.Weight,
                                                            IsContainer = a.IsContainer,
                                                        }).ToList();

            return myResult;
        }



        public List<WarehouseReleasePackageList> GetWarehouseReleasePackageListsByWarehouseReleaseIds(List<string> warehouseReleaseIds, int tenant)
        {

            List<WarehouseReleasePackageList> myResult = (from a in context.WarehouseReleasePackages
                                                        where a.Tenant == tenant && warehouseReleaseIds.Contains(a.WarehouseReleaseId)
                                                        select new WarehouseReleasePackageList()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            Seal = a.Seal,
                                                            Description = a.Description,
                                                            ContainerNumber = a.ContainerNumber,
                                                            Dimensions = a.IsContainer ? "" : a.Length + "-" + a.Width + "-" + a.Height,
                                                            Harmonize = a.Harmonize,
                                                            Height = a.Height,
                                                            Length = a.Length,
                                                            PackageTypeName = a.PackageType.EnglishName,
                                                            WarehouseReleaseId = a.WarehouseReleaseId,
                                                            Width = a.Width,
                                                            PackageTypeId = a.PackageTypeId,
                                                            Volume = a.Volume,
                                                            Quantity = a.Quantity,
                                                            Weight = a.Weight,
                                                            IsContainer = a.IsContainer,
                                                        }).ToList();

            return myResult;
        }

        public List<WarehouseReleasePackageList> GetWarehouseReleasePackageListsByIds(List<string> warehousePackagesReleaseIds, int tenant)
        {
            List<WarehouseReleasePackageList> myResult = (from a in context.WarehouseReleasePackages.Include("WarehouseRelease")
                                                          where a.Tenant == tenant && warehousePackagesReleaseIds.Contains(a.Id)
                                                          select new WarehouseReleasePackageList()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              ActualReleaseDate = a.WarehouseRelease!=null? a.WarehouseRelease.ActualReleaseDate:null,
                                                              ReleaseNumber = a.WarehouseRelease != null ? a.WarehouseRelease.ReleaseNumber : null,
                                                              WarehouseReleaseId = a.WarehouseReleaseId,
                                                          }).ToList();

            return myResult;
        }



        


    }



}



























