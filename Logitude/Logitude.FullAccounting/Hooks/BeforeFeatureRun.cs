using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Hooks
{
    [Binding]
    public class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {

        }
    }
}
