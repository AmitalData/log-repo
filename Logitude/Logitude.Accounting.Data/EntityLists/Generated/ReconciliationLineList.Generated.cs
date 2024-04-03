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
   public partial class ReconciliationLineList
   {
   
       [Key]
       [DataMember]
       public string ReconciliationId  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public string CurrencyCode  { get; set; }
       [DataMember]
       public string CurrencyName  { get; set; }
       [DataMember]
       public string TransactionId  { get; set; }
       [DataMember]
       public decimal ReconciliationAmount  { get; set; }
       [DataMember]
       public bool IsPartial  { get; set; }
       [DataMember]
       public int GroupNumber  { get; set; }
       [DataMember]
       public bool IsAdjustTransaction  { get; set; }
       [DataMember]
       public bool ColorField  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? DueDate  { get; set; }
       [DataMember]
       public decimal AmountDebit  { get; set; }
       [DataMember]
       public decimal AmountCredit  { get; set; }
       [DataMember]
       public string Reference1  { get; set; }
       [DataMember]
       public string Reference2  { get; set; }
       [DataMember]
       public string Reference3  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string JournalId  { get; set; }
       [DataMember]
       public string JournalNumber  { get; set; }
       [DataMember]
       public string CurrencySign  { get; set; }
       [DataMember]
       public string OpenAmountCurrencySign  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string ReconciledWithTransactionId  { get; set; }
       [DataMember]
       public DateTime? AccountingDate  { get; set; }
       [DataMember]
       public string ReconciliationAmountWithSign  { get; set; }
       [DataMember]
       public bool IsAmountDebitNegative  { get; set; }
       [DataMember]
       public decimal TransactionAmount  { get; set; }
       [DataMember]
       public decimal ExcelTransactionAmount  { get; set; }
       [DataMember]
       public DateTime? RefDate  { get; set; }
   }

}
	 