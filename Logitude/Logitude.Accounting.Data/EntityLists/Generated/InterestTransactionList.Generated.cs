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
   public partial class InterestTransactionList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDateTime  { get; set; }
       [DataMember]
       public DateTime UpdateDateTime  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string GLAccountId  { get; set; }
       [DataMember]
       public string InterestEntityTypeCode  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public int OriginalEntityLineNumber  { get; set; }
       [DataMember]
       public decimal LocalAmount  { get; set; }
       [DataMember]
       public decimal? ForeignAmount  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public DateTime InterestValueDate  { get; set; }
       [DataMember]
       public string InterestReportId  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public string CurrencyCode  { get; set; }
       [DataMember]
       public string InterestEntityNumber  { get; set; }
       [DataMember]
       public string JournalNumber  { get; set; }
       [DataMember]
       public string InterestEntityType  { get; set; }
       [DataMember]
       public string InterestEntityIconCode  { get; set; }
       [DataMember]
       public string JournalId  { get; set; }
       [DataMember]
       public string AccountEntityCode  { get; set; }
   }

}
	 