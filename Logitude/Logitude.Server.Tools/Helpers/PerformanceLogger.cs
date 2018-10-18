using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Server.Tools.Helpers
{
    public class PerformanceLogger
    {
         
        private static ConcurrentDictionary<string, DateTime> logTimes = new ConcurrentDictionary<string, DateTime>();

        public static string LogCurrentTime()
        {
            string key = Guid.NewGuid().ToString();
            logTimes.TryAdd(key, DateTime.Now);

            return key;
        }


        private static DateTime GetLogTime(string key)
        {
            DateTime date;
            if (logTimes.TryGetValue(key, out date))
            {
                return date;
            }
            else
            {
                return DateTime.Now;
            }
            //return logTimes.Where(s => s.Key.Contains(key)).FirstOrDefault().Value;
        }

        private static void RemoveLogTime(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (logTimes.Keys.Contains(key))
                {
                    DateTime date;
                    logTimes.TryRemove(key, out date);
                }
            }
        }

        public static void AddServerExecutionTimeHeader(string logKey)
        {
            if (!string.IsNullOrEmpty(logKey))
            {
                DateTime callTime = PerformanceLogger.GetLogTime(logKey);
                DateTime completionTime = DateTime.Now;
                int executionTime = (int)((completionTime.Ticks - callTime.Ticks) / TimeSpan.TicksPerMillisecond);
                HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "ServerExecutionTime, X-Custom");
                HttpContext.Current.Response.Headers.Add("ServerExecutionTime", executionTime.ToString());

                PerformanceLogger.RemoveLogTime(logKey);
            }

        }


    }
}
