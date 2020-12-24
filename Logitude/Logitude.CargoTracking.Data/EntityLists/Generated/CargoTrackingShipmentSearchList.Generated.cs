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
   public partial class CargoTrackingShipmentSearchList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime ShipmentDate  { get; set; }

       [Key]
       [DataMember]
       public int Id  { get; set; }
       [DataMember]
       public string ShipmentId  { get; set; }
       [DataMember]
       public bool? IsPublic  { get; set; }
       [DataMember]
       public string ReferenceType  { get; set; }
   }

}
	 