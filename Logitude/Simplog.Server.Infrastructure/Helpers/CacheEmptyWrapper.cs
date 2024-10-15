using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Caching;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class CacheEmptyWrapper : ICacheWrapper
    {
       Cache cache;
       public CacheEmptyWrapper(Cache cache)
        {
            this.cache = cache; 
        }
        public int Count
        {
            get { return 0; }
        }

        public long EffectivePercentagePhysicalMemoryLimit
        {
            get { return cache.EffectivePercentagePhysicalMemoryLimit; }
        }

        public long EffectivePrivateBytesLimit
        {
            get { return cache.EffectivePrivateBytesLimit; }
        }

        public object Add(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            return null ;
        }

        public object Get(string key)
        {
            return null ;
        }

        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return cache.GetEnumerator();
        }

        public void Insert(string key, object value)
        {
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies)
        {
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemUpdateCallback onUpdateCallback)
        {
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
        }

        public object Invalidate(string key)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public object Remove(string key)
        {
            return cache.Remove(key);
        }
    }
}
