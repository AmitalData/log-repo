using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class CashBookList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public bool? Inactive  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public string CurrencyCode  { get; set; }
       [DataMember]
       public string CurrencyName  { get; set; }
       [DataMember]
       public string CashBookTypeCode  { get; set; }
       [DataMember]
       public string CashBookTypeName  { get; set; }
       [DataMember]
       public decimal? TotalAmount  { get; set; }
       [DataMember]
       public string AccountId  { get; set; }
       [DataMember]
       public string AccountNumber  { get; set; }
       [DataMember]
       public string AccountName  { get; set; }
       [DataMember]
       public string BranchId  { get; set; }
       [DataMember]
       public string CurrencySign  { get; set; }
       [DataMember]
       public string BranchName  { get; set; }
       [DataMember]
       public bool InDepositingProgress  { get; set; }
   }

}
	 