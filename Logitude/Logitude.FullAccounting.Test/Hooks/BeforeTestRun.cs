using Logitude.FullAccounting.Test.Services.Preparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Logitude.Logitude.FullAccounting.Test.Hooks
{
    [Binding]
    public sealed class BeforeTestRun
    {
        [BeforeTestRun]
        public static void SetupTimeManagementPreparation()
        {
            new BranchPreparation().Prepare();
            new ActoinPreparation().Prepare();
            new ChartOfAccountPreparation().Prepare();
            new AutomaticReconcilePreparation().Prepare();
            new ChargesGroupPreparation().Prepare();
            new ChargeTypePreparation().Prepare();
            new CashBookPreparation().Prepare();
        }
        
    }
}
