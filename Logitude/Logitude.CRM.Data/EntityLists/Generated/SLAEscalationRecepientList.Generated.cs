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
   public partial class SLAEscalationRecepientList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SLAEscalationId  { get; set; }
       [DataMember]
       public string PreDefinitionId  { get; set; }
       [DataMember]
       public string UserId  { get; set; }
       [DataMember]
       public string PreDefinitionName  { get; set; }
       [DataMember]
       public string UserName  { get; set; }
       [DataMember]
       public string UserEmail  { get; set; }
   }

}
	 