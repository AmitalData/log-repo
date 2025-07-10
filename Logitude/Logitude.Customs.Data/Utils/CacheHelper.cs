using Simplog.Server.Infrastructure.Helpers;
using System;

namespace Logitude.Customs.BL.BL
{
    public static class CacheHelper
    {
        static CacheHelper()
        {
            if (CacheManager.CacheWrapper == null)
                CacheManager.CacheWrapper = new MockCacheWrapper();
        }

        public static void ClearCache(string cacheId) => CacheManager.CacheWrapper.Remove(cacheId);

        public static T GetFromCache<T>(string cacheId, Func<T> action)
        {
            object entity = CacheManager.CacheWrapper.Get(cacheId);

            if (entity == null)
            {
                entity = action();

                if (CacheManager.CacheWrapper.Get(cacheId) == null && entity != null)
                    CacheManager.CacheWrapper.Insert(cacheId, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }

            return entity == null ? default : (T)entity;
        }
    }
}
