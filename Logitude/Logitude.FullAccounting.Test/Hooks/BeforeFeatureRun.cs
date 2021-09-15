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
        public static void PrePrepareAccounting()
        {
            new ActoinPreparation().Prepare();
            new ChartOfAccountPreparation().Prepare();
            new AutomaticReconcilePreparation().Prepare();
            new AccountPreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-NewGLAccount")]
        public static void PrePrepareNewGLAccount()
        {
            new AccountPreparation().PrepareNewAccount();
        }

        [BeforeFeature("Pre-Prepare-ARIvoice")]
        public static void PrePrepareARIvoice()
        {
            new ActoinPreparation().Prepare();
            new ChartOfAccountPreparation().Prepare();
            new AutomaticReconcilePreparation().Prepare();
            new AccountPreparation().Prepare();
            new CustomerPreparation().Prepare();
            new ChargesGroupPreparation().Prepare();
            new ChargeTypePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-APIvoice")]
        public static void PrePrepareAPIvoice()
        {
            new ActoinPreparation().Prepare();
            new ChartOfAccountPreparation().Prepare();
            new AutomaticReconcilePreparation().Prepare();
            new AccountPreparation().Prepare();
            new ChargesGroupPreparation().Prepare();
            new ChargeTypePreparation().Prepare();
            new VendorPreparation().Prepare();
        }


        [BeforeFeature("Pre-Prepare-GetARIvoice")]
        public static void PrePrepareGetARIvoice()
        {
            new ActoinPreparation().Prepare();
            new ChartOfAccountPreparation().Prepare();
            new AutomaticReconcilePreparation().Prepare();
            new AccountPreparation().Prepare();
            new CustomerPreparation().Prepare();
            new ChargesGroupPreparation().Prepare();
            new ChargeTypePreparation().Prepare();
            new ARInvoicePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-GetAPIvoice")]
        public static void PrePrepareGetAPIvoice()
        {
            new ActoinPreparation().Prepare();
            new ChartOfAccountPreparation().Prepare();
            new AutomaticReconcilePreparation().Prepare();
            new AccountPreparation().Prepare();
            new VendorPreparation().Prepare();
            new ChargesGroupPreparation().Prepare();
            new ChargeTypePreparation().Prepare();
            new APInvoicePreparation().Prepare();
        }


    }
}
