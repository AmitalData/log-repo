using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.ShipmentOrderModule.Data.EntityLists
{
   [DataContract]
   public partial class ShipmentOrderList
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
       public string OrderNumber  { get; set; }
       [DataMember]
       public string TransportModeId  { get; set; }
       [DataMember]
       public string ConsigneeId  { get; set; }
       [DataMember]
       public string ShipperId  { get; set; }
       [DataMember]
       public string AgentId  { get; set; }
       [DataMember]
       public string IncotermId  { get; set; }
       [DataMember]
       public string AccountManagerId  { get; set; }
       [DataMember]
       public string PONumber  { get; set; }
       [DataMember]
       public string DescriptionOfGoods  { get; set; }
       [DataMember]
       public string ShipmentTypeId  { get; set; }
       [DataMember]
       public string Master  { get; set; }
       [DataMember]
       public string House  { get; set; }
       [DataMember]
       public string CarrierNumber  { get; set; }
       [DataMember]
       public DateTime? ETD  { get; set; }
       [DataMember]
       public DateTime? ETA  { get; set; }
       [DataMember]
       public DateTime? ATD  { get; set; }
       [DataMember]
       public DateTime? ATA  { get; set; }
       [DataMember]
       public string CustomsAgentId  { get; set; }
       [DataMember]
       public string SpecialServicesTypeId  { get; set; }
       [DataMember]
       public string CustomerReferences  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public string ShipmentTypeName  { get; set; }
       [DataMember]
       public bool IsReadyForPickup  { get; set; }
       [DataMember]
       public DateTime? PickupEstimatedDateTime  { get; set; }
       [DataMember]
       public DateTime? PickupActualDateTime  { get; set; }
       [DataMember]
       public string ForwarderId  { get; set; }
       [DataMember]
       public DateTime? BookingConfirmationDate  { get; set; }
       [DataMember]
       public string ConsigneeName  { get; set; }
       [DataMember]
       public string ShipperName  { get; set; }
       [DataMember]
       public string AgentName  { get; set; }
       [DataMember]
       public string IncotermCode  { get; set; }
       [DataMember]
       public string AccountManagerName  { get; set; }
       [DataMember]
       public string VesselName  { get; set; }
       [DataMember]
       public string CustomsAgentName  { get; set; }
       [DataMember]
       public string SpecialServicesTypeName  { get; set; }
       [DataMember]
       public string ForwarderName  { get; set; }
   }

}
	 