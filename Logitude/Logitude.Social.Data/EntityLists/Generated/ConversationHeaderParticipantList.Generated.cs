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
   public partial class ConversationHeaderParticipantList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ConversationHeaderId  { get; set; }
       [DataMember]
       public string ParticipantUserId  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? LeaveDate  { get; set; }
       [DataMember]
       public bool IsLeft  { get; set; }
       [DataMember]
       public bool Replied  { get; set; }
       [DataMember]
       public bool IsRead  { get; set; }
       [DataMember]
       public DateTime? LastReadDate  { get; set; }
       [DataMember]
       public bool IsDelete  { get; set; }
       [DataMember]
       public DateTime? DeleteDate  { get; set; }
   }

}
	 