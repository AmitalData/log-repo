using Logitude.OceanTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Logitude.OceanTest.Hooks
{
    [Binding]
    public static class BeforeFeatureRun
    {

        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            new ShipmentDataPreparation().Prepar();
        }
        
    }
}
