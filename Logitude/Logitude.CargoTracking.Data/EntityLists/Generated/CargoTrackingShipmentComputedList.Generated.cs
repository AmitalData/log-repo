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
   public partial class CargoTrackingShipmentComputedList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public DateTime? FirstPickupATD  { get; set; }
       [DataMember]
       public DateTime? FinalDeliveryATA  { get; set; }
       [DataMember]
       public DateTime? FinalDeliveryETA  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
   }

}
	 