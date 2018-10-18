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
   public partial class OpportunityStageList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string OpportunityId  { get; set; }
       [DataMember]
       public string FromStageId  { get; set; }
       [DataMember]
       public string ToStageId  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public DateTime? LastStageDate  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public string OpportunityTypeId  { get; set; }
   }

}
	 