using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Model.EntityClasses;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class DefaultKeyService
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);
        public static List<DefaultAndConfigurationKey> GetByTenant(int tenant)
        {
            if (_cache.Contains(tenant.ToString()))
            {
                return _cache.Get(tenant.ToString()) as List<DefaultAndConfigurationKey>;
            }
            else
            {
                List<DefaultAndConfigurationKey> settings = FetchSettingsByTenant(tenant);
                _cache.Add(tenant.ToString(), settings, DateTimeOffset.Now.Add(_cacheExpiration));
                return settings;
            }
        }

        public static DefaultAndConfigurationKey GetByTenant(int tenant, string key)
        {
            var settings = GetByTenant(tenant);
			return settings.SingleOrDefault(s => string.Equals(s.SetKey, key, StringComparison.OrdinalIgnoreCase));
        }

        public static void ClearCacheByTenant(string tenant)
        {
            if (_cache.Contains(tenant))
            {
                _cache.Remove(tenant);
            }
        }

        public static void ClearAllCache()
        {
            foreach (var item in _cache)
            {
                _cache.Remove(item.Key);
            }
        }

        private static List<DefaultAndConfigurationKey> FetchSettingsByTenant(int tenant)
        {
            try
            {
                Repository<DefaultAndConfigurationKey> repository = new Repository<DefaultAndConfigurationKey>(AmitalCloudContext.GetContext(tenant));
				List<DefaultAndConfigurationKey> result = repository.GetMulti(a => a.Tenant == tenant);
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
