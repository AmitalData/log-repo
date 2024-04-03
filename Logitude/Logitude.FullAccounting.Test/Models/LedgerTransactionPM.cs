using System;


namespace Logitude.FullAccounting.Test.Models
{
   
   public class LedgerTransactionPM 
   {
        
       public string Id { get; set; }
       public int Tenant { get; set; }
		public string JournalId { get; set; }
		public int JournalLineNumber { get; set; }
		public DateTime CreateDate { get; set; }
		public string ControlAccountId { get; set; }
		public string AccountId { get; set; }
		public DateTime AccountingDate { get; set; }
		public DateTime DocumentDate { get; set; }
		public DateTime DueDate { get; set; }
		public decimal LocalAmountDebit { get; set; }
		public decimal LocalAmountCredit { get; set; }
		public string CurrencyId { get; set; }
		public decimal ForeignAmountDebit { get; set; }
		public decimal ForeignAmountCredit { get; set; }
		public decimal ExchangeRate { get; set; }
		public string Reference1 { get; set; }
		public string Reference2 { get; set; }
		public string Reference3 { get; set; }
		public decimal OpenAmount { get; set; }
		public string OppositeAccountId { get; set; }
		public string SearchFields { get; set; }
		public string JournalNumber { get; set; }
		public string CurrencyCode { get; set; }
		public string Source { get; set; }
		public string SourceType { get; set; }
		public string OpenAmountCurrencyId { get; set; }
		public string Notes { get; set; }
		public decimal CumulativeLocalAmount { get; set; }
		public decimal CumulativeForeignAmount { get; set; }
		public decimal AmountToReconcile { get; set; }
		public bool Mark { get; set; }
		public string OpenAmountCurrencyCode { get; set; }
		public bool IsReconciled { get; set; }
		public string SourceId { get; set; }
		public string SourceNumber { get; set; }
		public string SourceTypeCode { get; set; }
		public string CurrencySign { get; set; }
		public string OpenAmountCurrencySign { get; set; }
		public int GroupHash { get; set; }
		public bool IsExternalReconcile { get; set; }
		public bool InReconcileProgress { get; set; }
		public decimal ForeignAmount { get; set; }
		public string ReconcileRemarks { get; set; }
		public string OriginalJournalId { get; set; }
		public string OppositeAccountEnglishName { get; set; }
		public string OppositeAccountLocalName { get; set; }
		public string OppositeAccountDisplayNumber { get; set; }
		public string RecoNumber { get; set; }
		public string ReconciliationId { get; set; }
		public decimal? PaymentReconciledAmount { get; set; }
		public bool InProgressExternalReconcile { get; set; }
		public decimal OriginalAmount { get; set; }
		public string ReconcileMethodCode { get; set; }
		public bool IsCumulativeForeignAmountPos { get; set; }
		public bool IsForeignAmountCreditPos { get; set; }
		public bool IsCumulativeLocalAmountPos { get; set; }
		public bool IsLocalAmountCreditPos { get; set; }
		public bool IsForeignAmountPos { get; set; }
		public bool IsOriginalAmountPos { get; set; }
		public string IconCode { get; set; }
		public string ForeignAmountCreditWithSign { get; set; }
		public string CumulativeForeignAmountSign { get; set; }
		public decimal CalculatedLocalAmount { get; set; }
		public decimal CalculatedForeignAmount { get; set; }
		public string AccountDisplayNumber { get; set; }
		public DateTime? PaymentValueDate { get; set; }
		public string PaymentChequeStatus { get; set; }
	}
   
}
	 