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
        [BeforeFeature("Pre-Prepare-CrateAPPayment")]
        public static void PrePrepareCrateAPPayment()
        {
            new AccountingPaymentMethodPreparation().Prepare();
            new VendorPreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-GetAPPayment")]
        public static void PrePrepareGetAPPayment()
        {
            new AccountingPaymentMethodPreparation().Prepare();
            new VendorPreparation().Prepare();
            new APPaymentPreparation().Prepare();

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
        [BeforeFeature("Pre-Prepare-GetPaymentCheques")]
        public static void PrePrepareGetPaymentCheques()
        {
            new PaymentChequesPreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-GetBankAccount")]
        public static void PrePrepareGetBankAccount()
        {
            new BankAccountPreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-CreateBankDeposit")]
        public static void PrePrepareCreateBankDeposit()
        {
            new BankAccountPreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-GetBankDeposit")]
        public static void PrePrepareGetBankDeposit()
        {
            new BankAccountPreparation().Prepare();
            new BankDepositPreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-GetSingleGLAccountByDisplayNumberAndTenant")]
        public static void PrePrepareGetSingleGLAccountByDisplayNumberAndTenant()
        {
            new AccountPreparation().GetByNumber("19216811");
        }
        [BeforeFeature("Pre-Prepare-GetSingleGLAccountByInternalNumberAndTenant")]
        public static void PrePrepareGetSingleGLAccountByInternalNumberAndTenant()
        {
            new AccountPreparation().PrepareInternalNumber();
        }
        [BeforeFeature("Pre-Prepare-CreateNote")]
        public static void PrePrepareCreateNote()
        {
            new CustomerPreparation().Prepare();
            new AccountingNotes().Prepare();
        }
        [BeforeFeature("Pre-Prepare-ReturnChequeFromBankDeposit")]
        public static void PrePrepareReturnChequeFromBankDeposit()
        {
            new CustomerPreparation().Prepare();
            new ARPaymentPreparation().ChequePrepare();
            new BankDepositPreparation().ChequePrepare();
        }






    }
}
