using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.Helpers
{
    public class SettingsHelper : BaseClasses.BaseInstance<SettingsHelper>
    {
        private const string CachePrefix = "SettingsHelper_{0}";
        public List<DefaultAndConfiguration_Ext> GetByTenant(int tenant)
        {
            string cacheKey = string.Format(CachePrefix, tenant);

            if (CacheManager.CacheWrapper == null)
                CacheManager.CacheWrapper = new CacheWrapper(HttpRuntime.Cache);

            return CacheManager.GetOrInsertNewObject(
                cacheKey,
                () => FetchSettingsByTenant(tenant),
                fromCache: true,
                donotCacheNull: false,
                supressForceInsert: true,
                absoluteExpiration: 30
            );

        }

        public DefaultAndConfiguration_Ext GetByTenant(int tenant, string key)
        {
            var settings = GetByTenant(tenant);
            return settings.FirstOrDefault(s => s.SetKey == key);
        }

        public void ClearCacheByTenant(string tenant)
        {
            string cacheKey = string.Format(CachePrefix, tenant);
            CacheManager.ClearCacheItems(key => key == cacheKey);
        }


        public void ClearAllCache()
        {
            var allCacheItems = CacheManager.GetAllCacheKeys();
            string cachePrefix = string.Format(CachePrefix, string.Empty);

            foreach (var key in allCacheItems)
            {
                if (key.StartsWith(cachePrefix))
                {
                    CacheManager.ClearCacheItems(k => k == key);
                }
            }
        }

        private List<DefaultAndConfiguration_Ext> FetchSettingsByTenant(int tenant)
        {
            try
            {
                IWebFreightContext customContext = WebFreightContext.GetContext(tenant);

                DefaultAndConfigurationRepository repository = new DefaultAndConfigurationRepository(customContext);
                List<DefaultAndConfiguration_Ext> result =
                (from a in repository.context.DefaultAndConfigurations
                 where a.Tenant == tenant
                 select new DefaultAndConfiguration_Ext
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     CreateDate = a.CreateDate,
                     SearchFields = a.SearchFields,
                     Is_Active = a.Is_Active,
                     StoreInCache = a.StoreInCache,
                     SetKey = a.SetKey,
                     AdditionalKey = a.AdditionalKey,
                     SortOrder = a.SortOrder,
                     SetValueType1 = a.SetValueType1,
                     Value1 = a.Value1,
                     SetValueType2 = a.SetValueType2,
                     Value2 = a.Value2,
                     AllowInheritance = a.AllowInheritance
                 }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
