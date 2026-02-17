using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Caching;

namespace Simplog.Server.Infrastructure.Helpers
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
            return cache.Get(key);
        }

        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return cache.GetEnumerator();
        }

        public void Insert(string key, object value)
        {
            cache.Insert(key, value);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies)
        {
            cache.Insert(key, value, dependencies);
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
