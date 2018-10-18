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
   public partial class CorrespondenceList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CreatedByContactId  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public bool IsInternal  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public string ActivityTypeCode  { get; set; }
       [DataMember]
       public string ActivityId  { get; set; }
       [DataMember]
       public string ActivitySubject  { get; set; }
       [DataMember]
       public string ContactName  { get; set; }
       [DataMember]
       public string CCs  { get; set; }
       [DataMember]
       public string Bcc  { get; set; }
       [DataMember]
       public bool NotifyMe  { get; set; }
       [DataMember]
       public bool NotifyOwner  { get; set; }
       [DataMember]
       public string InternalUsers  { get; set; }
       [DataMember]
       public string ContactEmail  { get; set; }
       [DataMember]
       public string Direction  { get; set; }
       [DataMember]
       public string HTMLFullBody  { get; set; }
       [DataMember]
       public bool RightToLeft  { get; set; }
   }

}
	 