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
   public partial class GroupMemberList
   {
   
       [Key]
       [DataMember]
       public string GroupId  { get; set; }

       [Key]
       [DataMember]
       public string UserId  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public DateTime? CancelledDate  { get; set; }
   }

}
	 