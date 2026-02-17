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
   public partial class ReconcileExternalPageLineList
   {
          [DataMember]
       public string ReconcileExternalPageId  { get; set; }
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public decimal DebitAmount  { get; set; }
       [DataMember]
       public DateTime ReferenceDate  { get; set; }
       [DataMember]
       public string Reference  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public bool IsReconciled  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }

       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public bool SelectCheckBox  { get; set; }
       [DataMember]
       public string ReconcileRemarks  { get; set; }
       [DataMember]
       public int? GroupHash  { get; set; }
       [DataMember]
       public string ReconciliationNumber  { get; set; }
       [DataMember]
       public decimal CreditAmount  { get; set; }
       [DataMember]
       public decimal Amount  { get; set; }
       [DataMember]
       public bool InProgressExternalReconcile  { get; set; }
       [DataMember]
       public bool InReconcileProgress  { get; set; }
   }

}
	 