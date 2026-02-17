using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PackageQuery
    {
        PackageRepository repository;

        public PackageQuery()
        {
            repository = new PackageRepository(); 
        }

        public PackageQuery(int tenant)
        {
            repository = new PackageRepository(tenant);
        }

        public PackageQuery(PackageRepository repository)
        {
            this.repository = repository;
        }

        public List<PackagePM> GetPackagePMs()
        {
            List<PackagePM> myList = (from a in repository.context.Packages.Include("FeaturePackageType")
                                      select new PackagePM()
                                      {
                                          Code = a.Code,
                                          Name = a.Name,
                                          InActive = a.InActive,
                                          FeaturePackageTypeCode = a.FeaturePackageTypeCode,
                                          FeaturePackageTypeName = a.FeaturePackageType == null ? null : a.FeaturePackageType.Name,
                                          SearchFields = a.SearchFields,
                                      }).ToList();

            PackageConnectedPackageQuery connectedPackageQuery = new PackageConnectedPackageQuery(0);
            foreach (PackagePM item in myList.Where(d => d.FeaturePackageTypeCode == "PK" || d.FeaturePackageTypeCode == "AD"))
            {
                item.ConnectedPackages = connectedPackageQuery.GetConnectedPackageByPackageCode(item.Code).ToList();
            }

            return myList;
        }

        public PackagePM GetSinglePM(string code)
        {
            PackagePM entityPM = (from a in repository.context.Packages.Include("FeaturePackageType")
                           where a.Code == code
                           select new PackagePM()
                           {
                               Code = a.Code,
                               Name = a.Name,
                               InActive = a.InActive,
                               FeaturePackageTypeCode = a.FeaturePackageTypeCode,
                               FeaturePackageTypeName = a.FeaturePackageType == null ? null : a.FeaturePackageType.Name,
                               SearchFields = a.SearchFields,
                           }).FirstOrDefault();

            if (entityPM.FeaturePackageTypeCode == "PK" || entityPM.FeaturePackageTypeCode == "AD")
            {
                PackageConnectedPackageQuery connectedPackageQuery = new PackageConnectedPackageQuery(0);
                entityPM.ConnectedPackages = connectedPackageQuery.GetConnectedPackageByPackageCode(entityPM.Code).ToList();
            }

            return entityPM;
        }

        public IQueryable<PackageList> GetIQueryableEntityList(IQueryable<Package> iQueryable)
        {
            var result = from a in iQueryable.Include("FeaturePackageType")
                         select new PackageList()
                         {
                             Code = a.Code,
                             Name = a.Name,
                             InActive = a.InActive,
                             FeaturePackageTypeCode = a.FeaturePackageTypeCode,
                             FeaturePackageTypeName = a.FeaturePackageType == null ? null : a.FeaturePackageType.Name,
                             SearchFields = a.SearchFields,
                         };

            return result;
        }
    }
}
