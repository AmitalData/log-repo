using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Windows;

namespace MetaDataGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CacheManager.CacheWrapper = CacheManager.CacheWrapper ?? new CacheWrapper(Cache);
        }

        public static HttpRuntime _httpRuntime { get; set; }
        public static Cache Cache
        {
            get
            {
                try
                {
                    EnsureHttpRuntime();
                    return HttpRuntime.Cache;
                }
                catch (Exception e)
                {
                    //Logger.LogMe(e.ToString(), true);
                }
                return null;

            }



        }
        private static void EnsureHttpRuntime()
        {
            try
            {
                if (null == _httpRuntime)
                {
                    try
                    {
                        //Monitor.Enter(typeof(State));
                        if (null == _httpRuntime)
                        {
                            // Create an Http Content to give us access to the cache.
                            _httpRuntime = new HttpRuntime();

                        }
                    }
                    finally
                    {
                        //Monitor.Exit(typeof(State));
                    }

                }
            }
            catch (Exception e)
            {
                //Logger.LogMe(e.ToString(), true);
            }
        }
           
    }
}
