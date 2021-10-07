using System;

namespace Logitude.FullAccounting.Test.Models
{
   
   public class AccountingNotePM 
   {
       public string Id { get; set; }
		public int Tenant { get; set; }
		public DateTime CreateDate { get; set; }
		public string CreatedByUserId { get; set; }
		public DateTime UpdateDate { get; set; }
		public string UpdatedByUserId { get; set; }
		public string CardId { get; set; }
		public string Notes { get; set; }
		public string UpdatedByUserName { get; set; }
		public string CreatedByUserName { get; set; }
	}
   
}
	 