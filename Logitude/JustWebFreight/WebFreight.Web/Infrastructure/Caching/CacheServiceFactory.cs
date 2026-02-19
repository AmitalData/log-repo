using System;
using System.Configuration;

namespace WebFreight.Web.Infrastructure.Caching
{
    /// <summary>
    /// Factory to create appropriate cache service based on configuration
    /// </summary>
    public static class CacheServiceFactory
    {
        private static ICacheService _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets singleton instance of cache service
        /// </summary>
        public static ICacheService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = CreateCacheService();
                        }
                    }
                }
                return _instance;
            }
        }

        private static ICacheService CreateCacheService()
        {
            string cacheType = ConfigurationManager.AppSettings["CacheType"];

            // Default to Redis if configuration exists, otherwise Memory
            if (string.IsNullOrEmpty(cacheType))
            {
                bool hasRedisConnection = !string.IsNullOrEmpty(
                    ConfigurationManager.ConnectionStrings["Redis"]?.ConnectionString ??
                    ConfigurationManager.AppSettings["RedisConnection"]
                );

                cacheType = hasRedisConnection ? "Redis" : "Memory";
            }

            switch (cacheType.ToLower())
            {
                case "redis":
                    try
                    {
                        return new RedisCacheService();
                    }
                    catch (Exception ex)
                    {
                        // Fallback to memory if Redis fails
                        System.Diagnostics.Trace.WriteLine($"Failed to initialize Redis cache: {ex.Message}. Falling back to MemoryCache.");
                        return new MemoryCacheService();
                    }

                case "memory":
                default:
                    return new MemoryCacheService();
            }
        }

        /// <summary>
        /// Resets the singleton instance (for testing purposes)
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }
}

