using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Logitude.OceanTest.Hooks
{
    public class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public void SetUpPrepareDataBeforeFeatureRun()
        {
            CreateShipmentWithContainers();
        }
        public void CreateShipmentWithContainers()
        {

        }
    }
}
