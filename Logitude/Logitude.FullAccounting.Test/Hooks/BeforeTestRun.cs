using Logitude.FullAccounting.Test.Services.Preparation;
using Logitude.FullAccountingTests.Services;
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
            new FullAccountingSettingService().UpdateSetting();
            new ChargeTypePreparation().Prepare();
            new CashBookPreparation().Prepare();
            new BankCodePreparation().Prepare();
            new AccountingPaymentMethodPreparation().Prepare();
        }

    }
}
