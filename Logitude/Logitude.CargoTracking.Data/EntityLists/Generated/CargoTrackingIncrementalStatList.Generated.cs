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
   public partial class CargoTrackingIncrementalStatList
   {
          [DataMember]
       public DateTime StartDate  { get; set; }
       [DataMember]
       public DateTime EndDate  { get; set; }
       [DataMember]
       public int Shipments  { get; set; }
       [DataMember]
       public int Cards  { get; set; }
       [DataMember]
       public int Ports  { get; set; }
       [DataMember]
       public int Countries  { get; set; }
       [DataMember]
       public int TransportModes  { get; set; }
       [DataMember]
       public int ShipmentComputedFields  { get; set; }
       [DataMember]
       public int ShipmentMasterDatas  { get; set; }

       [Key]
       [DataMember]
       public int Id  { get; set; }
       [DataMember]
       public string ErrorLog  { get; set; }
   }

}
	 