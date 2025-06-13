using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Model.EntityClasses ;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Caching;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class CacheWrapper : ICacheWrapper
    {
		Cache cache;
        public CacheWrapper(Cache cache)
		{
            this.cache = cache;
            List<GlobalTenant> globalTenants = new Repository<GlobalTenant>(GlobalContext.GetContext()).GetAll(0);
            cache.Insert(GetCacheKey<GlobalTenant>(null), globalTenants);
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

        //public object Add(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        //{
        //    return cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        //}

        public object Get(string key, int tenant = -1)
        {
            string cacheKey = GetCacheKey(key, tenant);
            if (CacheLogger.IsCacheLoggerEnabled)
                CacheLogger.LogKey(cacheKey);
            return cache.Get(cacheKey);
        }

        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return cache.GetEnumerator();
        }

        public void Insert(string key, object value, int tenant = -1)
        {
            cache.Insert(GetCacheKey(key, tenant), value, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, int tenant = -1)
        {
            cache.Insert(GetCacheKey(key, tenant), value, dependencies, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, int tenant = -1)
        {
            cache.Insert(GetCacheKey(key, tenant), value, dependencies, absoluteExpiration, slidingExpiration);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemUpdateCallback onUpdateCallback, int tenant = -1)
        {
            cache.Insert(GetCacheKey(key, tenant), value, dependencies, absoluteExpiration, slidingExpiration, onUpdateCallback);
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback, int tenant = -1)
        {
            cache.Insert(GetCacheKey(key, tenant), value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
        public void Insert<T>(int tenant, List<T> value)
        {
            Insert<T>(tenant, value, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        }
        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            cache.Insert(GetCacheKey<T>(tenant), value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
        public void Insert<T>(int tenant, List<T> value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            cache.Insert(GetCacheKey<T>(tenant), value, dependencies, absoluteExpiration, slidingExpiration, onUpdateCallback);
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
            return (List<T>)cache.Get(GetCacheKey<T>(tenant));
        }
        public object Invalidate<T>(int tenant)
        {
            return Invalidate(GetCacheKey<T>(tenant), tenant);
        }

        public object Invalidate(string key, int tenant = -1)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
                {
                    Remove(key, tenant);
                    CacheMessageSender.SendMessageToTopic(GetCacheKey(key, tenant));
                    scope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public object Remove(string key, int tenant = -1)
        {
            return cache.Remove(GetCacheKey(key, tenant));
        }
        private string GetCacheKey<T>(int? tenant) => $"Table_({typeof(T).Name}_{tenant})";
        private string GetCacheKey(string key, int tenant) => $"DB_({GetDB(tenant)})_OriginalKey_({key})";
        private string GetDB(int tenant)
        {
            if (tenant == -1)
            {
                if (HttpContext.Current.Items.Contains("Tenant"))
                {
                    tenant = Convert.ToInt32(HttpContext.Current.Items["Tenant"]);
                }
                else if (HttpContext.Current.Items.Contains("authToken"))
                {
                    tenant = (HttpContext.Current.Items["authToken"] as AuthenticationToken).Tenant;
                }
                else
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        string cacheKey = $"Token_({token})";
                        var authenticationToken = (AuthenticationToken)cache.Get(cacheKey);
                        if (authenticationToken?.Tenant != null)
                        {
                            tenant = authenticationToken.Tenant;
                        }
                    }
                    if (tenant == -1)
                    {
                        tenant = 0;
                    }
                }
            }
            List<GlobalTenant> globalTenants = (List<GlobalTenant>)cache.Get(GetCacheKey<GlobalTenant>(null));
            return globalTenants.Where(a => a.Id == tenant).FirstOrDefault().GlobalDBId;
        }
    }
}
