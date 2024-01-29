using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class CB_TariffDetailsHistoryList
   {
   
       [Key]
       [DataMember]
       public string ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string TariffID  { get; set; }
       [DataMember]
       public string QuotaID  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public string EntityStatusID  { get; set; }
       [DataMember]
       public string WithinQuota_ComputMethDataID  { get; set; }
       [DataMember]
       public string WithoutQuota_ComputMethDataID  { get; set; }
       [DataMember]
       public int ChangeRequestTypePriority  { get; set; }
   }

}
	 