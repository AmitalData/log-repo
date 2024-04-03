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
   public partial class JournalList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string JournalNumber  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime AccountingDate  { get; set; }
       [DataMember]
       public string TypeCode  { get; set; }
       [DataMember]
       public string AccountingEntityCode  { get; set; }
       [DataMember]
       public string ExternalNo  { get; set; }
       [DataMember]
       public string TypeName  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string AccountingEntityName  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public DateTime? ApproveDate  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string ApprovedByUserName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string AccountingEntityReference  { get; set; }
       [DataMember]
       public DateTime? VoidDate  { get; set; }
       [DataMember]
       public string OriginalJournalName  { get; set; }
       [DataMember]
       public string VoidedByUserName  { get; set; }
       [DataMember]
       public bool? IsVoided  { get; set; }
       [DataMember]
       public string ExternalSystem  { get; set; }
       [DataMember]
       public string LastActivityTypeName  { get; set; }
       [DataMember]
       public string LastActivityByUserName  { get; set; }
       [DataMember]
       public DateTime? LastActivityDate  { get; set; }
       [DataMember]
       public string StatusLocalName  { get; set; }
       [DataMember]
       public string TypeLocalName  { get; set; }
       [DataMember]
       public bool IsLedgerCreated  { get; set; }
       [DataMember]
       public DateTime? DocumentDate  { get; set; }
       [DataMember]
       public DateTime? DueDate  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public int? SecurityLevel  { get; set; }
   }

}
	 