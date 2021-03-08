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
   public partial class CargoTrackingShipmentList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string EntityId  { get; set; }
       [DataMember]
       public string ForwardingShipmentHeaderId  { get; set; }
       [DataMember]
       public string CustomsShipmentHeaderId  { get; set; }
       [DataMember]
       public string EntityType  { get; set; }
       [DataMember]
       public string CurrentMilestoneCode  { get; set; }
       [DataMember]
       public DateTime? CurrentMilestoneDate  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string TransportModeId  { get; set; }
       [DataMember]
       public string Master  { get; set; }
       [DataMember]
       public string House  { get; set; }
       [DataMember]
       public string ShipmentNumber  { get; set; }
       [DataMember]
       public string FromPortId  { get; set; }
       [DataMember]
       public string ToPortId  { get; set; }
       [DataMember]
       public string ShipperId  { get; set; }
       [DataMember]
       public string ConsigneeId  { get; set; }
       [DataMember]
       public double? GrossWeight  { get; set; }
       [DataMember]
       public double? Volume  { get; set; }
       [DataMember]
       public bool? PickupDone  { get; set; }
       [DataMember]
       public DateTime? PickupDate  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string SecurityKey  { get; set; }
       [DataMember]
       public string ConsigneeName  { get; set; }
       [DataMember]
       public string ShipperName  { get; set; }
       [DataMember]
       public string CustomerReference  { get; set; }
       [DataMember]
       public DateTime? PickupEstimationDate  { get; set; }
       [DataMember]
       public DateTime? FromWarehouseDate  { get; set; }
       [DataMember]
       public DateTime? FromWarehouseEstimationDate  { get; set; }
       [DataMember]
       public string FromWarehouseNotes  { get; set; }
       [DataMember]
       public bool? DepartureDone  { get; set; }
       [DataMember]
       public DateTime? DepartureDate  { get; set; }
       [DataMember]
       public DateTime? DepartureEstimationDate  { get; set; }
       [DataMember]
       public bool? ArrivalDone  { get; set; }
       [DataMember]
       public DateTime? ArrivalDate  { get; set; }
       [DataMember]
       public DateTime? ArrivalEstimationDate  { get; set; }
       [DataMember]
       public bool? ToWarehouseDone  { get; set; }
       [DataMember]
       public DateTime? ToWarehouseDate  { get; set; }
       [DataMember]
       public DateTime? ToWarehouseEstimationDate  { get; set; }
       [DataMember]
       public string ToWarehouseNotes  { get; set; }
       [DataMember]
       public bool? CustomsPaymentDone  { get; set; }
       [DataMember]
       public DateTime? CustomsPaymentDate  { get; set; }
       [DataMember]
       public bool? ClearanceDone  { get; set; }
       [DataMember]
       public DateTime? ClearanceDate  { get; set; }
       [DataMember]
       public bool? DeliveredDone  { get; set; }
       [DataMember]
       public DateTime? DeliveredDate  { get; set; }
       [DataMember]
       public DateTime? DeliveredEstimationDate  { get; set; }
       [DataMember]
       public bool? FromWarehouseDone  { get; set; }
       [DataMember]
       public DateTime? FirstPickupETD  { get; set; }
       [DataMember]
       public DateTime? WarehouseLegActualEntryDate  { get; set; }
       [DataMember]
       public DateTime? WarehouseLegExpectedEntryDate  { get; set; }
       [DataMember]
       public string WarehouseLegRemarks  { get; set; }
       [DataMember]
       public DateTime? DeclarationDate  { get; set; }
       [DataMember]
       public DateTime? CustomsClearanceDate  { get; set; }

       [Key]
       [DataMember]
       public int Id  { get; set; }
       [DataMember]
       public string SearchReferences  { get; set; }
       [DataMember]
       public string FromPortName  { get; set; }
       [DataMember]
       public string ToPortName  { get; set; }
       [DataMember]
       public string CurrentMilestoneName  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public string FromPortCountryCode  { get; set; }
       [DataMember]
       public string ToPortCountryCode  { get; set; }
       [DataMember]
       public bool IsFavorite  { get; set; }
       [DataMember]
       public string FutureMilstoneCode  { get; set; }
       [DataMember]
       public DateTime? FutureMilstoneDate  { get; set; }
       [DataMember]
       public string FutureMilstoneName  { get; set; }
       [DataMember]
       public string ContainersNumbers  { get; set; }
       [DataMember]
       public int? PackagesQuantity  { get; set; }
       [DataMember]
       public string DirectionId  { get; set; }
       [DataMember]
       public bool AssignedTruckerDone  { get; set; }
       [DataMember]
       public DateTime? AssignedTruckerDate  { get; set; }
       [DataMember]
       public DateTime? AssignedTruckerEstimationDate  { get; set; }
       [DataMember]
       public string AssignedTruckerNotes  { get; set; }
       [DataMember]
       public bool? AssignedCustomsAgentDone  { get; set; }
       [DataMember]
       public DateTime? AssignedCustomsAgentDate  { get; set; }
       [DataMember]
       public DateTime? AssignedCustomsAgentEstDate  { get; set; }
       [DataMember]
       public string AssignedCustomsAgentNotes  { get; set; }
       [DataMember]
       public string AssignedCustomsAgentExcReason  { get; set; }
       [DataMember]
       public string GrossWeightUnitCode  { get; set; }
   }

}
	 