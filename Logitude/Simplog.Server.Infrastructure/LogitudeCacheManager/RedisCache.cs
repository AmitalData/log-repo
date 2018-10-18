using Simplog.Server.Infrastructure.Azure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.LogitudeCacheManager
{
    public class RedisCache: IServerCache
    {
        public RedisCache()
        {
            this.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
             {
                 string cacheConnection = AzureRedisCacheDetails.GetConnectionString();
                 return ConnectionMultiplexer.Connect(cacheConnection);
             });

        }
        // Redis Connection string info
        private Lazy<ConnectionMultiplexer> lazyConnection;
        public ConnectionMultiplexer Connection
        {
            get
            {
                return this.lazyConnection.Value;
            }
        }
        public void AddToCache(string key, string value,TimeSpan? expierationTime = null)
        {
            IDatabase cache = this.Connection.GetDatabase();
            cache.StringSet(key, value, expierationTime);
        }

        public string GetFromCache(string key)
        {
            IDatabase cache = this.Connection.GetDatabase();
            return cache.StringGet(key);
        }

        public object RemoveFromCache(string key)
        {
            IDatabase cache = this.Connection.GetDatabase();
            return cache.KeyDelete(key);
        }
    }
}
