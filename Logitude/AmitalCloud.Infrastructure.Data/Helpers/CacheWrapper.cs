using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Caching;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class CacheWrapper:ICacheWrapper
    {

        Cache cache;
        public CacheWrapper(Cache cache)
        {
            this.cache = cache;
        }
        public int Count
        {
            get { return cache.Count; }
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
            return cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public object Get(string key)
        { 
            if(CacheLogger.IsCacheLoggerEnabled)
                CacheLogger.LogKey(key);
            return cache.Get(key);
        }

        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return cache.GetEnumerator();
        }

        public void Insert(string key, object value)
        {
            cache.Insert(key, value,null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies)
        {
            cache.Insert(key, value, dependencies,System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemUpdateCallback onUpdateCallback)
        {
            cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration,onUpdateCallback);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
        public void Insert<T>(int tenant, List<T> value)
        {
            Insert<T>(tenant, value, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        }
        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            string cacheKey = $"Table_({typeof(T).Name}_{tenant})";
            cache.Insert(cacheKey, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            string cacheKey = $"Table_({typeof(T).Name}_{tenant})";
            cache.Insert(cacheKey, value, dependencies, absoluteExpiration, slidingExpiration, onUpdateCallback);
        }
        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies)
        {
            Insert<T>(tenant, value, dependencies, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        }
        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Insert<T>(tenant, value, dependencies, absoluteExpiration, slidingExpiration, CacheItemPriority.Default, null);
        }
        public List<T> Get<T>(int tenant)
        {
            string cacheKey = $"Table_({typeof(T).Name}_{tenant})";
            return (List<T>)Get(cacheKey);
        }
        public object Invalidate<T>(int tenant)
        {
            string cacheKey = $"Table_({typeof(T).Name}_{tenant})";
            return Invalidate(cacheKey);
        }

        public object Invalidate(string key)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
                {
                    Remove(key);
                    CacheMessageSender.SendMessageToTopic(key);
                    scope.Complete();
                }
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
