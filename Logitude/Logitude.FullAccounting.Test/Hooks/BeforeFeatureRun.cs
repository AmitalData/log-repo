using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services.Preparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Logitude.Logitude.FullAccounting.Test.Hooks
{
    [Binding]
    public class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare-Accounting")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            new ActoinPreparation().Prepare();
            new ChartOfAccountPreparation().Prepare();
            new AutomaticReconcilePreparation().Prepare();
            new AccountPreparation().Prepare();
            

        }
    }
}
