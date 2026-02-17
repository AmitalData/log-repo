using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
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
            return settings.FirstOrDefault(s => s.SetKey == key);
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
                DefaultAndConfigurationKeyRepository repository = new DefaultAndConfigurationKeyRepository();
                List<DefaultAndConfigurationKey> result =
                (from a in repository.context.DefaultAndConfigurationKey
                 where a.Tenant == tenant
                 select a).ToList();

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
