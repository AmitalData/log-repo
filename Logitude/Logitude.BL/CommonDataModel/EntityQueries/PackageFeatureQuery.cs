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
    public class PackageFeatureQuery
    {
        PackageFeatureRepository repository;



        public PackageFeatureQuery(int tenant)
        {
            repository = new PackageFeatureRepository(tenant);
        }

        public PackageFeatureQuery(PackageFeatureRepository repository)
        {
            this.repository = repository;
        }

        public PackageFeaturePM GetPackageFeatureByPackageAndFeature(string packageCode, string featureId, int tenant)
        {
            return (from a in repository.context.PackageFeatures
                    where a.PackageCode == packageCode && a.FeatureId == featureId && a.Tenant == tenant
                    select new PackageFeaturePM()
                    {
                        FeatureId = a.FeatureId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        PackageCode = a.PackageCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).FirstOrDefault();
        }

        public IQueryable<PackageFeaturePM> GetPackageFeaturePMsByTenant(int tenant)
        {
            return (from a in repository.context.PackageFeatures
                    where a.Tenant == tenant
                    select new PackageFeaturePM()
                    {
                        FeatureId = a.FeatureId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        PackageCode = a.PackageCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    });
        }

        public List<PackageFeaturePM> GetPackageFeaturesForPackage(string packageCode, int tenant)
        {
            return (from a in repository.context.PackageFeatures
                    where a.PackageCode == packageCode && a.Tenant == tenant
                    select new PackageFeaturePM()
                    {
                        FeatureId = a.FeatureId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        PackageCode = a.PackageCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).ToList();
        }
    }
}