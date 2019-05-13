using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.TariffModule.Data.EntityLists
{
   [DataContract]
   public partial class TariffVersionList
   {
   
       [Key]
       [DataMember]
       public string TariffId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? ExpirationDate  { get; set; }

       [Key]
       [DataMember]
       public int Version  { get; set; }
       [DataMember]
       public bool IsDraft  { get; set; }
       [DataMember]
       public DateTime? ApproveDate  { get; set; }
       [DataMember]
       public int ParentVersionNumber  { get; set; }
   }

}
	 