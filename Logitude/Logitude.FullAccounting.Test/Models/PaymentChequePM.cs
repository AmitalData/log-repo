using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{
    public partial class PaymentChequePM
    {

        public string Id { get; set; }

        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public string InternalNumber { get; set; }
        public string ChequeNumber { get; set; }
        public string PayToGLAccountId { get; set; }
        public string PayToName { get; set; }
        public string BankAccountId { get; set; }
        public string BankAccountGLAccountId { get; set; }
        public decimal? LocalAmount { get; set; }
        public string CurrencyId { get; set; }
        public decimal? ForeignAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public DateTime? ValueDate { get; set; }
        public DateTime? PrintDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public bool IsCancelled { get; set; }
        public string CancelledByUserId { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancellationRemarks { get; set; }
        public string PaymentChequeStatusCode { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string GLAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string PaymentChequeStatusName { get; set; }
        public string BankAccountCode { get; set; }
        public string Notes { get; set; }


        public int PaymentChequeLineLastLine { get; set; }
        public string GLAccountCurrencyId { get; set; }
        public string BankGLAccountCurrencyId { get; set; }
        public bool IsGLAccountMultiCurrency { get; set; }
        public bool IsBankGlAccountMultiCur { get; set; }
        public string UniqueField { get; set; }
        public string GLAccountName { get; set; }
        public string CurrencyCode { get; set; }
        public string JournalNumber { get; set; }
        public string JournalId { get; set; }
        public string StatusEnglishName { get; set; }
        public string BankLocalName { get; set; }
        public string BankEnglishName { get; set; }
        public string APPaymentId { get; set; }
        public bool CancelledByAPPayment { get; set; }
        public string APPaymentNo { get; set; }
    }

}
