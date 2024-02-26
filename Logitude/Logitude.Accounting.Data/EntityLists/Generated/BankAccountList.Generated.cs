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
   public partial class BankAccountList
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
       public string LocalName  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public string BranchNumber  { get; set; }
       [DataMember]
       public string AccountNumber  { get; set; }
       [DataMember]
       public string GLAccountId  { get; set; }
       [DataMember]
       public string DeferredGLAccountId  { get; set; }
       [DataMember]
       public string IBAN  { get; set; }
       [DataMember]
       public string SwiftCode  { get; set; }
       [DataMember]
       public string BranchAddress  { get; set; }
       [DataMember]
       public bool? Inactive  { get; set; }
       [DataMember]
       public int? ChequeCounter  { get; set; }
       [DataMember]
       public string GLAccountNumber  { get; set; }
       [DataMember]
       public string DeferedGLAccountNumber  { get; set; }
       [DataMember]
       public string BankCode  { get; set; }
       [DataMember]
       public string GLAccountCurrencyId  { get; set; }
       [DataMember]
       public string LastPageNumber  { get; set; }
       [DataMember]
       public DateTime? LastPageEndDate  { get; set; }
       [DataMember]
       public decimal? LastPageCloseBalance  { get; set; }
       [DataMember]
       public string TransferGLAcccountId  { get; set; }
       [DataMember]
       public string DeferedGLAccountLocalName  { get; set; }
       [DataMember]
       public string TransferGLAcccountNumber  { get; set; }
       [DataMember]
       public string TransferGLAcccountLocalName  { get; set; }
       [DataMember]
       public string DeferedGLAccountEnglishName  { get; set; }
       [DataMember]
       public string TransferGLAcccountEnglishName  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public string CurrencyName  { get; set; }
       [DataMember]
       public string CurrencyCode  { get; set; }
       [DataMember]
       public string CurrencySign  { get; set; }
       [DataMember]
       public string PrintingBranchNumber  { get; set; }
       [DataMember]
       public string PrintingAccountNumber  { get; set; }
       [DataMember]
       public string TotalOpenExternalTransactions  { get; set; }
       [DataMember]
       public string TotalOpenPagesLines  { get; set; }
       [DataMember]
       public string ChequeCounterSeriesID  { get; set; }
   }

}
	 