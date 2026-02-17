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
   public partial class FollowerList
   {
   
       [Key]
       [DataMember]
       public string FolloweeUserId  { get; set; }

       [Key]
       [DataMember]
       public string FollowerUserId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public bool? CancelledDate  { get; set; }
   }

}
	 