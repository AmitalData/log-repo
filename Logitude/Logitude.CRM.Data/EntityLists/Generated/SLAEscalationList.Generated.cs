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
   public partial class SLAEscalationList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SLAHeaderId  { get; set; }
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string EscalationFor  { get; set; }
       [DataMember]
       public string EscalationActionTimeIndicator  { get; set; }
       [DataMember]
       public int? EscalationTime  { get; set; }
       [DataMember]
       public string EscalationTimeUnit  { get; set; }
       [DataMember]
       public int? EscalaitonTimeInMinutes  { get; set; }
       [DataMember]
       public string TimeIndicator  { get; set; }
       [DataMember]
       public string TimeUnitName  { get; set; }
   }

}
	 