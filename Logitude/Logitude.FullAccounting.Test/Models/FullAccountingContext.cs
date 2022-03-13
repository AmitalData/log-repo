using Logitude.Base.Models.PartnersPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Models
{
    public class FullAccountingContext
    {
        public List<JournalLinePM> Journallines { get; internal set; }
        public JournalPM ApprovedJournal { get; internal set; }
        public JournalPM AddedApprovedJournal { get; internal set; }
        public GLAccountPM GLAccount { get; internal set; }
        public GLAccountPM UpdatedGLAccount { get; internal set; }
        public ARInvoicePM ARInvoicePM { get; internal set; }
        public APInvoicePM APInvoicePM { get; internal set; }
        public ARInvoicePM AddedARInvoicePM { get; internal set; }
        public APInvoicePM AddedAPInvoicePM { get; internal set; }
        public List<ARInvoiceLinePM> ARInvoiceLinePM { get; internal set; }
        public List<APInvoiceLinePM> APInvoiceLinePM { get; internal set; }
        public ARPaymentPM ARPaymentPM { get; internal set; }
        public ARPaymentPM AddedARPaymentPM { get; internal set; }
        public APPaymentPM APPaymentPM { get; internal set; }
        public APPaymentPM AddedAPPaymentPM { get; internal set; }
        public CashBookPM CashbookCreated { get; internal set; }
        public CashBookPM Cashbook { get; internal set; }
        public CashBookPM AddedCashbook { get; internal set; }
        public BankAccountPM BankAccount { get; internal set; }
        public BankAccountPM AddedBankAccount { get; internal set; }
        public BankAccountPM BankAccountPM { get; internal set; }
        public PaymentChequePM PaymentChequeCreated { get; internal set; }
        public PaymentChequePM AddedPaymentCheque { get; internal set; }
        public PaymentChequePM PaymentCheque { get; internal set; }
        public BankDepositPM BankDeposit { get; internal set; }
        public BankDepositPM AddedBankDeposit { get; internal set; }
        public Action Action { get; internal set; }
        public Partner CustomerPartner { get; internal set; }
        public CustomerPM Customer { get; internal set; }
    }
}
