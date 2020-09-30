using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CRM.Data.EntityLists
{
   [DataContract]
   public partial class TicketEscalationList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public string TicketId  { get; set; }
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string EscalationFor  { get; set; }
       [DataMember]
       public string Recepients  { get; set; }
       [DataMember]
       public bool IsClose  { get; set; }
       [DataMember]
       public bool IsSLAViolated  { get; set; }
       [DataMember]
       public DateTime? DueDate  { get; set; }
       [DataMember]
       public DateTime? CloseDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string EscalationForName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
   }

}
	 