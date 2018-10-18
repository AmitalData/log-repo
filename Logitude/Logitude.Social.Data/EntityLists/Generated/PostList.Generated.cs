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
   public partial class PostList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CreatedById  { get; set; }
       [DataMember]
       public string GroupId  { get; set; }
       [DataMember]
       public string BodyText  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public string ParentPostId  { get; set; }
       [DataMember]
       public int NumberOfLikes  { get; set; }
       [DataMember]
       public bool IsPrivate  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public bool IsAutomatic  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string EntityDescription  { get; set; }
       [DataMember]
       public int NumberOfComments  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string UserImageDetailId  { get; set; }
       [DataMember]
       public int? IndexColor  { get; set; }
       [DataMember]
       public string DefaultColor  { get; set; }
   }

}
	 