using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL
{
    public static class CacheHelper
    {
        public static T GetFromCache<T>(string cacheId, Func<T> action)
        {
            T entity = (T)CacheManager.CacheWrapper.Get(cacheId);

            if (entity == null)
            {
                entity = action();

                if (CacheManager.CacheWrapper.Get(cacheId) == null && entity != null)
                    CacheManager.CacheWrapper.Insert(cacheId, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }

            return entity;
        }

    }
}
