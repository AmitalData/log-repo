
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace Simplog.Server.Infrastructure.Helpers
{
   public class NoCache4uWrapper: ICacheWrapper
    {
        Dictionary<string, object> cacheDictionary;
        public NoCache4uWrapper()
        {
            cacheDictionary = new Dictionary<string, object>();
        }
        public int Count
        {
            get { return cacheDictionary.Count; }
        }

        public long EffectivePercentagePhysicalMemoryLimit
        {
            get { return 1; }
        }

        public long EffectivePrivateBytesLimit
        {
            get { return 1; }
        }

        public object Add(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            TestObject(value);
            return value;

        }

        private static void TestObject(object value)
        {
            if (value != null)
            {
                var FullName = value.GetType().FullName.ToLower();
                if ("simplog.global.data.globalmodel.entitypocos.globaldb" == FullName) return;
                if (FullName.Contains("Logitude.Customs.Data.EntityPOCOs".ToLower()))
                {
                    AmitalDebuggerUtil.Break();
                }
                if (FullName.Contains("EntityPOCOs".ToLower()))
                {

                    AmitalDebuggerUtil.Break();
                }
            }
        }

        public object Get(string key)
        {
            if (cacheDictionary.Keys.Contains(key))
            {
                return cacheDictionary[key];
            }
            return null;
        }

        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return cacheDictionary.GetEnumerator(); 
            //throw new NotImplementedException();
        }

        public void Insert(string key, object value)
        {
            //cacheDictionary.Add(key, value);
            TestObject(value);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies)
        {
            TestObject(value);
            //cacheDictionary.Add(key, value);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            TestObject(value);
            //cacheDictionary.Add(key, value);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemUpdateCallback onUpdateCallback)
        {
            TestObject(value);
            //cacheDictionary.Add(key, value);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            TestObject(value);
            //cacheDictionary.Add(key, value);
        }

        public object Invalidate(string key)
        {
            return null;
            return new NullCache();
           return  cacheDictionary.Remove(key);
        }


        public object Remove(string key)
        {
            return null;
            return new NullCache();
            return cacheDictionary.Remove(key);
        }

        public object Get(string key, int tenant = -1)
        {
            throw new NotImplementedException();
        }

        public void Insert(string key, object value, int tenant = -1)
        {
            throw new NotImplementedException();
        }

        public void Insert(string key, object value, CacheDependency dependencies, int tenant = -1)
        {
            throw new NotImplementedException();
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, int tenant = -1)
        {
            throw new NotImplementedException();
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback, int tenant = -1)
        {
            throw new NotImplementedException();
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback, int tenant = -1)
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(int tenant, List<T> value)
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies)
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public List<T> Get<T>(int tenant)
        {
            throw new NotImplementedException();
        }

        public object Invalidate<T>(int tenant)
        {
            throw new NotImplementedException();
        }

        public object Invalidate(string key, int tenant = -1)
        {
            throw new NotImplementedException();
        }

        public object Remove(string key, int tenant = -1)
        {
            throw new NotImplementedException();
        }
    }
}
