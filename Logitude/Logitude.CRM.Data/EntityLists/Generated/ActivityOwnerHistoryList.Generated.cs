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
   public partial class ActivityOwnerHistoryList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ActivityId  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public DateTime? ModifiedDate  { get; set; }
       [DataMember]
       public bool NeedSynchronization  { get; set; }
   }

}
	 