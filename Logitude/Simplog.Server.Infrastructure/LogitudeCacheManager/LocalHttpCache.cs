using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Simplog.Server.Infrastructure.LogitudeCacheManager
{
    public class LocalHttpCache : IServerCache
    {
        Cache cache;
        public LocalHttpCache()
        {
            this.cache = HttpContext.Current.Cache;
        }
      
        public void AddToCache(string key, string value, TimeSpan? expierationTime = null)
        {
            if (expierationTime != null)
            {
                this.cache.Insert(key, value, null, System.DateTime.UtcNow.AddTicks(expierationTime.Value.Ticks), TimeSpan.Zero);
            }
            else
            {
                this.cache.Insert(key, value);
            }
        }

        public string GetFromCache(string key)
        {
           return this.cache.Get(key).ToString();
        }

        public object RemoveFromCache(string key)
        {
            return this.cache.Remove(key);
        }
    }
}