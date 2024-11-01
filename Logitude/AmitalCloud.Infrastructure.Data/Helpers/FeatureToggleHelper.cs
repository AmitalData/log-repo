using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public  class FeatureToggleHelper
    {

        public static bool HasFeatureToggle(string toggleCode, int tenant) => HasFeatureToggle(toggleCode, tenant, 0);  

        public static bool HasFeatureToggle(string toggleCode, int tenant, int baseTenant)
        {
            return new Repository<FeatureToggle>(AmitalCloudContext.GetContext(baseTenant)).GetMulti(a => a.ToggleCode == toggleCode && a.Tenant == tenant).Any(); 
        }

    }
}
