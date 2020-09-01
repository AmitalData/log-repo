using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CargoTracking.Data.EntityLists
{
   [DataContract]
   public partial class CargoTrackingShipmentMasterList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public string Master  { get; set; }
       [DataMember]
       public DateTime? MainCarriageATD  { get; set; }
       [DataMember]
       public DateTime? MainCarriageETD  { get; set; }
       [DataMember]
       public DateTime? MainCarriageATA  { get; set; }
       [DataMember]
       public DateTime? MainCarriageETA  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
   }

}
	 