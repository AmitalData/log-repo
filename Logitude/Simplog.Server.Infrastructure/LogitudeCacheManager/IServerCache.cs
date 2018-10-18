using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.LogitudeCacheManager
{
    public interface IServerCache
    {
        void AddToCache(string key, string value, TimeSpan? expierationTime = null);
        string GetFromCache(string key);
        object RemoveFromCache(string key);
       
    }
}
