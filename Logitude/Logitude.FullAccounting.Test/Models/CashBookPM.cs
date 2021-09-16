using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{

   public class CashBookPM 
   {

        public string Id { get; set; }
		public int Tenant { get; set; }
		public DateTime CreateDate { get; set; }
		public string CreatedByUserId { get; set; }
		public DateTime UpdateDate { get; set; }
		public string UpdatedByUserId { get; set; }
		public string SearchFields { get; set; }
		public string CreatedByUserName { get; set; }
		public string UpdatedByUserName { get; set; }
		public string EnglishName { get; set; }
		public string LocalName { get; set; }
		public bool? Inactive { get; set; }
		public string CurrencyId { get; set; }
		public string CurrencyCode { get; set; }
		public string CurrencyName { get; set; }
		public string CashBookTypeCode { get; set; }
		public string CashBookTypeName { get; set; }
		public decimal? TotalAmount { get; set; }
		public string AccountId { get; set; }
		public string AccountNumber { get; set; }
		public string AccountName { get; set; }
		
       public string BranchId { get; set; }
		public string CurrencySign { get; set; }
		public string BranchName { get; set; }
	}
   
}
	 