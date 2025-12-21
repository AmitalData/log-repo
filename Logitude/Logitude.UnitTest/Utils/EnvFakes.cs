using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.UnitTest.Utils
{
    /// <summary>
    /// Test-only environment fakes to avoid hitting real DB/global context.
    /// </summary>
    public static class EnvFakes
    {
        private static bool _initialized;

        /// <summary>
        /// Call once per test init to ensure GlobalDbHelper and CacheManager are stubbed.
        /// </summary>
        public static void EnsureInitialized()
        {
            if (_initialized) return;

            var stubGlobalDb = new GlobalDB();
            CacheManager.CacheWrapper = new InMemoryCacheWrapper(stubGlobalDb);

            _initialized = true;
        }

        /// <summary>
        /// Minimal in-memory cache wrapper that always returns the stub GlobalDB for any key.
        /// Implements the interface to satisfy callers without hitting real cache/DB.
        /// </summary>
        private class InMemoryCacheWrapper : ICacheWrapper
        {
            private readonly Dictionary<string, object> _store = new Dictionary<string, object>();
            private readonly GlobalDB _stub;

            public InMemoryCacheWrapper(GlobalDB stub)
            {
                _stub = stub ?? new GlobalDB();
            }

            private object GetValue(string key) => _store.ContainsKey(key) ? _store[key] : _stub;

            public object Get(string key, int tenant = -1) => GetValue(key);

            public T Get<T>(string key)
            {
                var v = GetValue(key);
                if (v is T t) return t;
                return default;
            }

            public IDictionaryEnumerator GetEnumerator() => _store.GetEnumerator();

            public void Insert(string key, object value, int tenant = -1)
            {
                _store[key] = value;
            }

            public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback, int tenant = -1)
            {
                _store[key] = value;
                return value;
            }

            public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback, int tenant = -1)
            {
                _store[key] = value;
            }

            public List<T> Get<T>(int tenant)
            {
                var key = $"tenant_{tenant}_{typeof(T).Name}";
                if (_store.TryGetValue(key, out var v) && v is List<T> list) return list;
                return new List<T>();
            }

        public void Insert(string key, object value, CacheDependency dependencies, int tenant = -1)
        {
            _store[key] = value;
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, int tenant = -1)
        {
            _store[key] = value;
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback, int tenant = -1)
        {
            _store[key] = value;
        }

            public void Insert<T>(int tenant, List<T> list)
            {
                _store[$"tenant_{tenant}_{typeof(T).Name}"] = list;
            }

        public void Insert<T>(int tenant, List<T> list, CacheDependency dependencies)
        {
            _store[$"tenant_{tenant}_{typeof(T).Name}"] = list;
        }

            public void Insert<T>(int tenant, List<T> list, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            _store[$"tenant_{tenant}_{typeof(T).Name}"] = list;
        }

        public void Insert<T>(int tenant, List<T> list, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            _store[$"tenant_{tenant}_{typeof(T).Name}"] = list;
        }

            public void Insert<T>(int tenant, List<T> list, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
            {
                _store[$"tenant_{tenant}_{typeof(T).Name}"] = list;
            }

            public object Invalidate<T>(int tenant)
            {
                var key = $"tenant_{tenant}_{typeof(T).Name}";
                _store.Remove(key);
                return null;
            }

            public object Invalidate(string key, int tenant = -1)
            {
                _store.Remove(key);
                return null;
            }

            public object Remove(string key, int tenant = -1)
            {
                _store.Remove(key);
                return null;
            }

            public int Count => _store.Count;
            public long EffectivePercentagePhysicalMemoryLimit => 0;
            public long EffectivePrivateBytesLimit => 0;
        }

    }
}

