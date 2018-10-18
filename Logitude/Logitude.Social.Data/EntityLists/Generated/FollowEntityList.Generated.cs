using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Social.Data.EntityLists
{
   [DataContract]
   public partial class FollowEntityList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public string FollowerUserId  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public DateTime? CancelledDate  { get; set; }
       [DataMember]
       public string FollowerName  { get; set; }
   }

}
	 