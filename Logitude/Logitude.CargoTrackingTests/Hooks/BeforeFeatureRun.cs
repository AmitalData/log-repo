using Logitude.CargoTrackingTests.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Logitude.CargoTrackingTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {
        [BeforeFeature("GetConnectionStrings")]
        public static void GetConnectionStrings()
        {
            ConfigurationsGetter configurationsGetter = ConfigurationsGetter.GetInstance();
        }
    }
}
