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



        public object Get(string key, int tenant = -1)
        {
            return null;
        }

        public List<T> Get<T>(int tenant)
        {
            return null;
        }

        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return cache.GetEnumerator();
        }


        public void Insert(string key, object value, int tenant = -1)
        {
            
        }

        public void Insert(string key, object value, CacheDependency dependencies, int tenant = -1)
        {
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, int tenant = -1)
        {
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback, int tenant = -1)
        {
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback, int tenant = -1)
        {
        }

        public void Insert<T>(int tenant, List<T> value)
        {
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies)
        {
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
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

        public object Invalidate<T>(int tenant)
        {
            return null;
        }

        public object Invalidate(string key, int tenant = -1)
        {
            return null;
        }

        public object Remove(string key)
        {
            return null;
        }

        public object Remove(string key, int tenant = -1)
        {
            return null;
        }
    }
}
