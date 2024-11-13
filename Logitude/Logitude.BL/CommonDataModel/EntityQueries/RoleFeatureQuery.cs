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
    public class RoleFeatureQuery
    {
        RoleFeatureRepository repository;

        public RoleFeatureQuery()
        {
            repository = new RoleFeatureRepository(); 
        }

        public RoleFeatureQuery(int tenant)
        {
            repository = new RoleFeatureRepository(tenant);
        }

        public RoleFeatureQuery(RoleFeatureRepository repository)
        {
            this.repository = repository;
        }

        public RoleFeaturePM GetRoleFeatureByRoleAndFeature(string roleId, string featureId, int tenant)
        {
            return (from a in repository.context.RoleFeatures
                    where a.RoleId == roleId && a.FeatureId == featureId && a.Tenant == tenant
                    select new RoleFeaturePM()
                    {
                        FeatureId = a.FeatureId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        RoleId = a.RoleId,
                        FeatureAccessLevelCode = a.FeatureAccessLevelCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).FirstOrDefault();
        }

        public IQueryable<RoleFeaturePM> GetRoleFeaturePMsByTenant(int tenant)
        {
            return (from a in repository.context.RoleFeatures
                    where a.Tenant == tenant
                    select new RoleFeaturePM()
                    {
                        FeatureId = a.FeatureId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        RoleId = a.RoleId,
                        FeatureAccessLevelCode = a.FeatureAccessLevelCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    });
        }

        public List<RoleFeaturePM> GetRoleFeaturesForRole(string roleId, int tenant)
        {
            return (from a in repository.context.RoleFeatures
                    where a.RoleId == roleId && a.Tenant == tenant
                    select new RoleFeaturePM()
                    {
                        FeatureId = a.FeatureId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        RoleId = a.RoleId,
                        FeatureAccessLevelCode = a.FeatureAccessLevelCode,
                        FeatureUniqeCode = a.FeatureUniqeCode
                    }).ToList();
        }
		public List<RoleFeature> GetRoleFeaturesForRoleFromCache(string roleId, int tenant)
		{
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(message: $"After GetRoleFeaturesForRoleFromCache roleId: {roleId},tenant: {tenant}");

            string cacheKey = $"RoleFeature_{roleId}_{tenant}";
            List<RoleFeature> entity= (List<RoleFeature>)CacheManager.CacheWrapper.Get(cacheKey);
            if (entity == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(message: $"Before GetAllowedFeaturesForRole repository.context.RoleFeatures.GetConnection().Database: {repository.context.GetConnection()?.Database}");

                entity = (from a in repository.context.RoleFeatures
                          where (a.Tenant == tenant || a.Tenant == 0) && a.RoleId == roleId
                          select a).ToList();
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(message: $"After GetAllowedFeaturesForRole repository.context.RoleFeatures.GetConnection().Database: {repository.context.GetConnection()?.Database}");

                if (entity != null)
                {
                    CacheManager.CacheWrapper.Insert(cacheKey, entity);
                }
            }
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(message: $"Before GetAllowedFeaturesForRole entity: {string.Join(",",entity?.Select(a=>a.Id))}");

            return entity;
		}
	}
}