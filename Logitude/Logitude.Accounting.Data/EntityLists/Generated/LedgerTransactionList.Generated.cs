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
   public partial class LedgerTransactionList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string JournalId  { get; set; }
       [DataMember]
       public int JournalLineNumber  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string ControlAccountId  { get; set; }
       [DataMember]
       public string AccountId  { get; set; }
       [DataMember]
       public DateTime AccountingDate  { get; set; }
       [DataMember]
       public DateTime DocumentDate  { get; set; }
       [DataMember]
       public DateTime DueDate  { get; set; }
       [DataMember]
       public decimal LocalAmountDebit  { get; set; }
       [DataMember]
       public decimal LocalAmountCredit  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public decimal ForeignAmountDebit  { get; set; }
       [DataMember]
       public decimal ForeignAmountCredit  { get; set; }
       [DataMember]
       public decimal ExchangeRate  { get; set; }
       [DataMember]
       public string Reference1  { get; set; }
       [DataMember]
       public string Reference2  { get; set; }
       [DataMember]
       public string Reference3  { get; set; }
       [DataMember]
       public decimal OpenAmount  { get; set; }
       [DataMember]
       public string OppositeAccountId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string JournalNumber  { get; set; }
       [DataMember]
       public string CurrencyCode  { get; set; }
       [DataMember]
       public string Source  { get; set; }
       [DataMember]
       public string SourceType  { get; set; }
       [DataMember]
       public string OpenAmountCurrencyId  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public decimal CumulativeLocalAmount  { get; set; }
       [DataMember]
       public decimal CumulativeForeignAmount  { get; set; }
       [DataMember]
       public decimal AmountToReconcile  { get; set; }
       [DataMember]
       public bool Mark  { get; set; }
       [DataMember]
       public string OpenAmountCurrencyCode  { get; set; }
       [DataMember]
       public bool IsReconciled  { get; set; }
       [DataMember]
       public string SourceId  { get; set; }
       [DataMember]
       public string SourceNumber  { get; set; }
       [DataMember]
       public string SourceTypeCode  { get; set; }
       [DataMember]
       public bool SelectCheckBox  { get; set; }
       [DataMember]
       public string CurrencySign  { get; set; }
       [DataMember]
       public string OpenAmountCurrencySign  { get; set; }
       [DataMember]
       public int GroupHash  { get; set; }
       [DataMember]
       public bool IsExternalReconcile  { get; set; }
       [DataMember]
       public bool InReconcileProgress  { get; set; }
       [DataMember]
       public decimal ForeignAmount  { get; set; }
       [DataMember]
       public string ReconcileRemarks  { get; set; }
       [DataMember]
       public string OppositeAccountEnglishName  { get; set; }
       [DataMember]
       public string OppositeAccountLocalName  { get; set; }
       [DataMember]
       public string OppositeAccountDisplayNumber  { get; set; }
       [DataMember]
       public string RecoNumber  { get; set; }
       [DataMember]
       public string ReconciliationId  { get; set; }
       [DataMember]
       public decimal? PaymentReconciledAmount  { get; set; }
       [DataMember]
       public bool InProgressExternalReconcile  { get; set; }
       [DataMember]
       public decimal OriginalAmount  { get; set; }
   }

}
	 