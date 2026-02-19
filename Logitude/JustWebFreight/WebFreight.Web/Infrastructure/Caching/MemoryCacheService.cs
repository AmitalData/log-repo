using System;
using System.Runtime.Caching;
using System.Linq;

namespace WebFreight.Web.Infrastructure.Caching
{
    /// <summary>
    /// In-memory caching implementation for development/testing
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache;
        private static readonly object _lock = new object();

        public MemoryCacheService()
        {
            _cache = MemoryCache.Default;
        }

        public T Get<T>(string key)
        {
            var value = _cache.Get(key);
            return value != null ? (T)value : default(T);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            if (value == null)
                return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(expiration)
            };

            _cache.Set(key, value, policy);
        }

        public T GetOrAdd<T>(string key, Func<T> factory, TimeSpan expiration)
        {
            var cached = Get<T>(key);
            if (cached != null && !cached.Equals(default(T)))
            {
                return cached;
            }

            lock (_lock)
            {
                // Double-check after acquiring lock
                cached = Get<T>(key);
                if (cached != null && !cached.Equals(default(T)))
                {
                    return cached;
                }

                var value = factory();
                if (value != null && !value.Equals(default(T)))
                {
                    Set(key, value, expiration);
                }

                return value;
            }
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            // Convert wildcard pattern to regex
            var regex = new System.Text.RegularExpressions.Regex(
                "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*") + "$"
            );

            var keysToRemove = _cache
                .Where(kvp => regex.IsMatch(kvp.Key))
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }

        public bool Exists(string key)
        {
            return _cache.Contains(key);
        }

        public void Clear()
        {
            var keys = _cache.Select(kvp => kvp.Key).ToList();
            foreach (var key in keys)
            {
                _cache.Remove(key);
            }
        }
    }
}

