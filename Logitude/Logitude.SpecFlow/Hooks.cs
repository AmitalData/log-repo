using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Configuration;
using TechTalk.SpecFlow;

namespace Logitude.SpecFlow
{
    [Binding]
    public class Hooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            CacheManager.CacheWrapper = new MockCacheWrapper();
            string dbms = ConfigurationManager.AppSettings.Get("DBMS");
            LogitudeSettings.DatabaseManagementSystem = dbms;
        }
    }
}