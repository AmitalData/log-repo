using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    public static class AccountingEntityJournalActions
    {
        public const string APInvoiceApprove = "APInvoiceApprove";
        public const string APInvoiceVoid = "APInvoiceVoid ";
        public const string APPaymentApprove = "APPaymentApprove";
        public const string APPaymentVoid = "APPaymentVoid";
        public const string PaymentCheque = "PaymentCheque";
        public const string BankDepositApprove = "BankDepositApprove";
        public const string BankDepositCancel = "BankDepositCancel";
        public const string BankDepositOutOfDeposit = "BankDepositOutOfDeposit";
        public const string BankDepositChequeRedemption = "BankDepositChequeRedemption";
        public const string YearTransferApprove = "YearTransferApprove";
        public const string YearTransferCancel = "YearTransferCancel";
        public const string ARInvoiceApprove = "ARInvoiceApprove";
        public const string ARInvoiceVoid = "ARInvoiceVoid ";
        public const string ARPaymentApprove = "ARPaymentApprove";
        public const string ARPaymentVoid = "ARPaymentVoid";
        public const string ARPaymentReturnToCustomer = "ARPaymentReturnToCustomer";
        public const string RevaluationApprove = "RevaluationApprove";
        public const string TaxReportClosingJournal = "TaxReportClosingJournal";
    }
}
