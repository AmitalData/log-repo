using System;

namespace AmitalCloud.Infrastructure.Model.Persistence.Helpers
{
    public static class DbContextBaseUtil
    {
        readonly static bool _UnifreightDataIncludedInMain_FeatureOn;
        public static bool UnifreightDataIncludedInMain_FeatureOn { get { return _UnifreightDataIncludedInMain_FeatureOn; } }
        static DbContextBaseUtil()
        {
            _UnifreightDataIncludedInMain_FeatureOn = true;
            return;
        }
        public static string GetConnectionStringWithAmitalNetRole(string currentConnectionString)
        {
            return currentConnectionString;
        }
        public static bool? ToLog { get; set; }
        public static DateTime? MaxPoolSizeWasReachedWhileSave { get; set; }
    }

}
