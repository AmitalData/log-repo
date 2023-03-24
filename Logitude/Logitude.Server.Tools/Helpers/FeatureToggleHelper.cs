using Logitude.Infrastructure.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
  public  class FeatureToggleHelper
    {

        public static bool HasFeatureToggle(string toggleCode, int tenant)
        {
            if(toggleCode?.ToLower() == "CTL".ToLower())
            {
                return false;
            }
            FeatureToggleRepository featureToggleRepository = new FeatureToggleRepository(tenant);
            return featureToggleRepository.HasFeatureToggle(toggleCode,tenant);
        }

        public static bool HasFeatureToggle(string toggleCode, int tenant, int baseTenant)
        {
            if (toggleCode?.ToLower() == "CTL".ToLower())
            {
                return false;
            }
            FeatureToggleRepository featureToggleRepository = new FeatureToggleRepository(baseTenant);
            return featureToggleRepository.HasFeatureToggle(toggleCode, tenant);
        }

    }
}
