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
   public partial class OccasionList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public DateTime? StartDateTime  { get; set; }
       [DataMember]
       public DateTime? EndDateTime  { get; set; }
       [DataMember]
       public string Goal  { get; set; }
       [DataMember]
       public string Location  { get; set; }
       [DataMember]
       public string OwnerId  { get; set; }
       [DataMember]
       public string IndustryId  { get; set; }
       [DataMember]
       public string OccasionTypeId  { get; set; }
       [DataMember]
       public string OccasionStatusId  { get; set; }
       [DataMember]
       public string CreatedByContactName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string TypeName  { get; set; }
       [DataMember]
       public string OwnerName  { get; set; }
       [DataMember]
       public string OccasionStatusName  { get; set; }
       [DataMember]
       public string IndustryName  { get; set; }
   }

}
	 