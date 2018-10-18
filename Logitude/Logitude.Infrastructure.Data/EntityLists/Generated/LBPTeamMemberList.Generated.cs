using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Infrastructure.Data.EntityLists
{
   [DataContract]
   public partial class LBPTeamMemberList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string MemberUserId  { get; set; }
       [DataMember]
       public string TeamId  { get; set; }
       [DataMember]
       public DateTime AddDate  { get; set; }
       [DataMember]
       public string MemberTeamId  { get; set; }
   }

}
	 