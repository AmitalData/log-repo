using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{

   public class BankDepositPM 
   {
       public string Id { get; set; }
       public int Tenant { get; set; }
		public DateTime CreateDate { get; set; }
		public string CreatedByUserId { get; set; }
		public DateTime UpdateDate { get; set; }
		public string UpdatedByUserId { get; set; }
		public string SearchFields { get; set; }
		public int DepositNumber { get; set; }
		public DateTime DepositDate { get; set; }
		public string DepositCurrencyId { get; set; }
		public decimal LocalDepositAmount { get; set; }
		public decimal ForeignAmount { get; set; }
		public string DepositBankAccountId { get; set; }
		public string CashBookId { get; set; }
		public DateTime AccountingDate { get; set; }
		
       public string CashBookGLAccountId { get; set; }
		public bool IsCashDeposit { get; set; }
		public string DeferredGLAccountId { get; set; }
		public string CashGLAccountId { get; set; }
		public bool IsCanceled { get; set; }
		public string DepositCurrencyCode { get; set; }
		public string JournalNumber { get; set; }
		public string JournalId { get; set; }
		public string CashBookName { get; set; }
		public DateTime? LastActivityDate { get; set; }
		public string LastActivityTypeName { get; set; }
		public string LastActivityByUserName { get; set; }
		public string CreatedByUserName { get; set; }
		public string BankAccountNumber { get; set; }
		public string JournalQueueId { get; set; }
	}
   
}
	 