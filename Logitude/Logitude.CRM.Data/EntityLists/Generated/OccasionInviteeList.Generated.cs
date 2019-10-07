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
   public partial class OccasionInviteeList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime AddedDate  { get; set; }
       [DataMember]
       public string AddedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string OccasionId  { get; set; }
       [DataMember]
       public string ContactId  { get; set; }
       [DataMember]
       public string AddedByUserName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string OccasionName  { get; set; }
       [DataMember]
       public string ContactName  { get; set; }
       [DataMember]
       public bool Invited  { get; set; }
       [DataMember]
       public bool Participated  { get; set; }
       [DataMember]
       public string ContactPhone  { get; set; }
       [DataMember]
       public string ContactEmail  { get; set; }
       [DataMember]
       public string ContactTel  { get; set; }
       [DataMember]
       public string ContactPosition  { get; set; }
       [DataMember]
       public string ContactMobile  { get; set; }
   }

}
	 