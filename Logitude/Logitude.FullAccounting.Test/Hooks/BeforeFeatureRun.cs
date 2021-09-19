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
            new AccountPreparation().Prepare();
            new CustomerPreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-APIvoice")]
        public static void PrePrepareAPIvoice()
        {
            new AccountPreparation().Prepare();
            new VendorPreparation().Prepare();
        }


        [BeforeFeature("Pre-Prepare-GetARIvoice")]
        public static void PrePrepareGetARIvoice()
        {
            new AccountPreparation().Prepare();
            new CustomerPreparation().Prepare();
            new ARInvoicePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-GetAPIvoice")]
        public static void PrePrepareGetAPIvoice()
        {
            new AccountPreparation().Prepare();
            new VendorPreparation().Prepare();
            new APInvoicePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-GetARPayment")]
        public static void PrePrepareGetARPayment()
        {
            new AccountPreparation().Prepare();
            new CustomerPreparation().Prepare();
            new AccountingPaymentMethodPreparation().Prepare();
            new ARPaymentPreparation().Prepare();
        }

        [BeforeFeature("Pre-Prepare-CrateARPayment")]
        public static void PrePrepareCrateARPayment()
        {
            new CustomerPreparation().Prepare();
            new ARInvoicePreparation().Prepare();
            new AccountingPaymentMethodPreparation().Prepare();

        }


    }
}
