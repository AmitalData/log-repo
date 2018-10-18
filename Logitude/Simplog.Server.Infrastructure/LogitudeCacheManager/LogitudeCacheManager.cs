using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.LogitudeCacheManager
{
    public class LogitudeCacheManager
    {
        private static IServerCache serverCache;
        public static IServerCache ServerCache
        {
            get { return serverCache; }
            set
            {
                if (serverCache != null) return; //only set once 
                serverCache = value;
            }
        }
    }
}
