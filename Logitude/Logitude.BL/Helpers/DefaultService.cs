using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Input;

namespace Logitude.BL.Helpers
{
    public class DefaultService : BaseClasses.BaseInstance<DefaultService>
    {
        private int expirationTime = Environment.GetEnvironmentVariable("CacheDefaultExpirationTime") != null ? Convert.ToInt32(Environment.GetEnvironmentVariable("CacheDefaultExpirationTime")) : 30;

        private const string CachePrefix = "SettingsHelper_{0}";
        public List<DefaultAndConfiguration_Ext> Get(int tenant)
        {
            string cacheKey = GetCacheKey(tenant);
            return GetFromCache(cacheKey, () => FetchByTenant(tenant));
        }

        public List<DefaultAndConfiguration_Ext> Get(int tenant, string setKey)
        {
            string cacheKey = GetCacheKey(tenant, setKey);
            return GetFromCache(cacheKey, () => Fetch(tenant, setKey));
        }

        public DefaultAndConfiguration_Ext Get(int tenant, string setKey, string additionalKey)
        {
            string cacheKey = GetCacheKey(tenant, setKey, additionalKey);
            return GetFromCache(cacheKey, () => Fetch(tenant, setKey, additionalKey));
        }

        private T GetFromCache<T>(string cacheKey, Func<T> func) where T : class
        {
            if (CacheManager.CacheWrapper == null)
            {
                Dictionary<int, string> globalDBs = new Dictionary<int, string>();
                //List<GlobalTenant> globalTenants = new GlobalDomainService().GetActiveTenants();
                //foreach (var item in globalTenants)
                //{
                //    globalDBs.Add(item.Id, item.GlobalDBId);
                //}
                CacheManager.CacheWrapper = new CacheWrapper(HttpRuntime.Cache,globalDBs);
            }
            return CacheManager.GetOrInsertNewObject(
                cacheKey,
                func,
                fromCache: true,
                donotCacheNull: false,
                supressForceInsert: true,
                absoluteExpiration: expirationTime
            );
        }

        private DefaultAndConfiguration_Ext Fetch(int tenant, string setKey, string additionalKey)
        {
            List<DefaultAndConfiguration_Ext> defaultList = Get(tenant, setKey);
            DefaultAndConfiguration_Ext res = defaultList.FirstOrDefault(d => d.AdditionalKey == additionalKey);

            if (res != null)
                return res;

            foreach (var item in additionalKey.Split('.'))
            {
                if (additionalKey.Length == item.Length)
                    break;

                additionalKey = additionalKey.Substring(0, additionalKey.Length - item.Length - 1);

                res = defaultList.FirstOrDefault(d => d.AdditionalKey == additionalKey);

                if (res != null)
                    return res;
            }

            return null;
        }

        private List<DefaultAndConfiguration_Ext> Fetch(int tenant, string setKey) =>
            Get(tenant).Where(s => s.SetKey == setKey).ToList();

        private List<DefaultAndConfiguration_Ext> FetchByTenant(int tenant)
        {
            DefaultAndConfigurationRepository repository = new DefaultAndConfigurationRepository(WebFreightContext.GetContext(tenant));
            List<DefaultAndConfiguration_Ext> result = repository
                .GetDefaultAndConfigurations(tenant).ToList()
                .Select(a => new DefaultAndConfiguration_Ext(a)).ToList();
            return result;
        }

        private string GetCacheKey(int tenant, string setKey = null, string additionalKey = null)
        {
            string key = string.Format(CachePrefix, tenant);

            if (!string.IsNullOrEmpty(setKey))
                key += ";" + setKey;

            if (!string.IsNullOrEmpty(additionalKey))
                key += ";" + additionalKey;

            return key;
        }

        public void ClearCache()
        {
            var allCacheItems = CacheManager.GetAllCacheKeys();
            string cachePrefix = string.Format(CachePrefix, string.Empty);

            foreach (var key in allCacheItems)
                if (key.StartsWith(cachePrefix))
                    CacheManager.ClearCacheItems(k => k == key);
        }

        public void ClearCache(int tenant, string setKey = null, string additionalKey = null)
        {
            string cacheKey = GetCacheKey(tenant);
            CacheManager.ClearCacheItems(key => key == cacheKey);

            if (!string.IsNullOrEmpty(setKey))
            {
                cacheKey = GetCacheKey(tenant, setKey);
                CacheManager.ClearCacheItems(key => key == cacheKey);
            }

            if (!string.IsNullOrEmpty(additionalKey))
            {
                cacheKey = GetCacheKey(tenant, setKey, additionalKey);
                CacheManager.ClearCacheItems(key => key == cacheKey);
            }
        }
    }
}
