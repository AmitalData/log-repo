using System;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace WebFreight.Web.Infrastructure.Caching
{
    /// <summary>
    /// Redis-based caching implementation for production use
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redis;
        private readonly ConnectionMultiplexer _connection;
        private readonly TelemetryClient _telemetry;
        private static readonly object _lock = new object();
        private static ConnectionMultiplexer _staticConnection;

        public RedisCacheService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Redis"]?.ConnectionString
                                      ?? ConfigurationManager.AppSettings["RedisConnection"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Redis connection string not configured");
            }

            // Use singleton pattern for ConnectionMultiplexer (best practice)
            if (_staticConnection == null || !_staticConnection.IsConnected)
            {
                lock (_lock)
                {
                    if (_staticConnection == null || !_staticConnection.IsConnected)
                    {
                        _staticConnection?.Dispose();
                        _staticConnection = ConnectionMultiplexer.Connect(connectionString);
                    }
                }
            }

            _connection = _staticConnection;
            _redis = _connection.GetDatabase();
            _telemetry = new TelemetryClient();
        }

        public T Get<T>(string key)
        {
            try
            {
                var value = _redis.StringGet(key);

                if (value.HasValue)
                {
                    TrackCacheMetric("CacheHit", key);
                    return JsonConvert.DeserializeObject<T>(value);
                }

                TrackCacheMetric("CacheMiss", key);
                return default(T);
            }
            catch (Exception ex)
            {
                TrackCacheException("Get", key, ex);
                return default(T);
            }
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            try
            {
                if (value == null)
                    return;

                var json = JsonConvert.SerializeObject(value);
                _redis.StringSet(key, json, expiration);

                TrackCacheMetric("CacheSet", key);
            }
            catch (Exception ex)
            {
                TrackCacheException("Set", key, ex);
            }
        }

        public T GetOrAdd<T>(string key, Func<T> factory, TimeSpan expiration)
        {
            try
            {
                // Try to get from cache first
                var cached = Get<T>(key);
                if (cached != null && !cached.Equals(default(T)))
                {
                    return cached;
                }

                // Cache miss - generate value
                var value = factory();
                if (value != null && !value.Equals(default(T)))
                {
                    Set(key, value, expiration);
                }

                return value;
            }
            catch (Exception ex)
            {
                TrackCacheException("GetOrAdd", key, ex);
                // Fallback to factory if cache fails
                return factory();
            }
        }

        public void Remove(string key)
        {
            try
            {
                _redis.KeyDelete(key);
                TrackCacheMetric("CacheRemove", key);
            }
            catch (Exception ex)
            {
                TrackCacheException("Remove", key, ex);
            }
        }

        public void RemoveByPattern(string pattern)
        {
            try
            {
                var server = _connection.GetServer(_connection.GetEndPoints().First());
                var keys = server.Keys(pattern: pattern).ToArray();

                if (keys.Length > 0)
                {
                    _redis.KeyDelete(keys);
                    TrackCacheMetric("CacheRemoveByPattern", pattern, keys.Length);
                }
            }
            catch (Exception ex)
            {
                TrackCacheException("RemoveByPattern", pattern, ex);
            }
        }

        public bool Exists(string key)
        {
            try
            {
                return _redis.KeyExists(key);
            }
            catch (Exception ex)
            {
                TrackCacheException("Exists", key, ex);
                return false;
            }
        }

        public void Clear()
        {
            try
            {
                var server = _connection.GetServer(_connection.GetEndPoints().First());
                server.FlushDatabase();
                TrackCacheMetric("CacheClear", "ALL");
            }
            catch (Exception ex)
            {
                TrackCacheException("Clear", "ALL", ex);
            }
        }

        #region Monitoring & Telemetry

        private void TrackCacheMetric(string metricName, string key, int count = 1)
        {
            try
            {
                var properties = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "CacheKey", key },
                    { "CacheType", "Redis" }
                };

                _telemetry.TrackMetric(metricName, count, properties);
            }
            catch
            {
                // Suppress telemetry errors
            }
        }

        private void TrackCacheException(string operation, string key, Exception ex)
        {
            try
            {
                var properties = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "Operation", operation },
                    { "CacheKey", key },
                    { "CacheType", "Redis" }
                };

                _telemetry.TrackException(ex, properties);
            }
            catch
            {
                // Suppress telemetry errors
            }
        }

        #endregion
    }
}

