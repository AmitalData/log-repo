using System;


namespace Logitude.FullAccounting.Test.Models
{
   public class BankAccountPM 
   {
       public string Id { get; set; }
       public int Tenant { get; set; }
		public DateTime CreateDate { get; set; }
		public string CreatedByUserId { get; set; }
		public DateTime UpdateDate { get; set; }
		public string UpdatedByUserId { get; set; }
		public string SearchFields { get; set; }
		public string LocalName { get; set; }
		public string EnglishName { get; set; }
		public string BankId { get; set; }
		public string BranchNumber { get; set; }
		public string AccountNumber { get; set; }
		public string GLAccountId { get; set; }
		public string DeferredGLAccountId { get; set; }
		public string IBAN { get; set; }
		public string SwiftCode { get; set; }
		public string BranchAddress { get; set; }
		public bool? Inactive { get; set; }
		public int? ChequeCounter { get; set; }
		public string GLAccountNumber { get; set; }
		public string DeferedGLAccountNumber { get; set; }
		public string BankCode { get; set; }
		public string GLAccountCurrencyId { get; set; }
		public string LastPageNumber { get; set; }
		public DateTime? LastPageEndDate { get; set; }
		public decimal? LastPageCloseBalance { get; set; }
		public string TransferGLAcccountId { get; set; }
		public string DeferedGLAccountLocalName { get; set; }
		public string TransferGLAcccountNumber { get; set; }
		public string TransferGLAcccountLocalName { get; set; }
		public string DeferedGLAccountEnglishName { get; set; }
		public string TransferGLAcccountEnglishName { get; set; }
		public bool IsBankPageEvent { get; set; }
		public string CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public string CurrencyCode { get; set; }
		public string CurrencySign { get; set; }
		public string PrintingBranchNumber { get; set; }
		public string PrintingAccountNumber { get; set; }
		public string BankCodeEnglishName { get; set; }
		public string BankCodeLocalName { get; set; }
	}
   
}
	 