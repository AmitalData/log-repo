using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.CargoTracking.Data.EntityPOCOs
{
   
    public class CargoTrackingShipment
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("EntityId")]
	    public string EntityId { get; set; }
        [Column("ForwardingShipmentHeaderId")]
	    public string ForwardingShipmentHeaderId { get; set; }
        [Column("CustomsShipmentHeaderId")]
	    public string CustomsShipmentHeaderId { get; set; }
        [ForeignKey("CargoTrackingHeaderEntityType")]
        [Column("EntityType")]
	    public string EntityType { get; set; }
	      
        public virtual CargoTrackingHeaderEntityType CargoTrackingHeaderEntityType { get; set; }
        [ForeignKey("CargoTrackingMilestone")]
        [Column("CurrentMilestoneCode")]
	    public string CurrentMilestoneCode { get; set; }
	      
        public virtual CargoTrackingMilestone CargoTrackingMilestone { get; set; }
        [Column("CurrentMilestoneDate")]
	    public DateTime? CurrentMilestoneDate { get; set; }
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
        [Column("Master")]
	    public string Master { get; set; }
        [Column("House")]
	    public string House { get; set; }
        [Column("ShipmentNumber")]
	    public string ShipmentNumber { get; set; }
        [Column("FromPortId")]
	    public string FromPortId { get; set; }
        [Column("ToPortId")]
	    public string ToPortId { get; set; }
        [Column("ShipperId")]
	    public string ShipperId { get; set; }
        [Column("ConsigneeId")]
	    public string ConsigneeId { get; set; }
        [Column("GrossWeight")]
	    public double? GrossWeight { get; set; }
        [Column("Volume")]
	    public double? Volume { get; set; }
        [Column("PickupDone")]
	    public bool? PickupDone { get; set; }
        [Column("PickupDate")]
	    public DateTime? PickupDate { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("SecurityKey")]
	    public string SecurityKey { get; set; }
        [Column("ConsigneeName")]
	    public string ConsigneeName { get; set; }
        [Column("ShipperName")]
	    public string ShipperName { get; set; }
        [Column("CustomerReference")]
	    public string CustomerReference { get; set; }
        [Column("IsMainRecord")]
	    public bool IsMainRecord { get; set; }
        [Column("PickupEstimationDate")]
	    public DateTime? PickupEstimationDate { get; set; }
        [Column("FromWarehouseDate")]
	    public DateTime? FromWarehouseDate { get; set; }
        [Column("FromWarehouseEstimationDate")]
	    public DateTime? FromWarehouseEstimationDate { get; set; }
        [Column("FromWarehouseNotes")]
	    public string FromWarehouseNotes { get; set; }
        [Column("DepartureDone")]
	    public bool? DepartureDone { get; set; }
        [Column("DepartureDate")]
	    public DateTime? DepartureDate { get; set; }
        [Column("DepartureEstimationDate")]
	    public DateTime? DepartureEstimationDate { get; set; }
        [Column("ArrivalDone")]
	    public bool? ArrivalDone { get; set; }
        [Column("ArrivalDate")]
	    public DateTime? ArrivalDate { get; set; }
        [Column("ArrivalEstimationDate")]
	    public DateTime? ArrivalEstimationDate { get; set; }
        [Column("ToWarehouseDone")]
	    public bool? ToWarehouseDone { get; set; }
        [Column("ToWarehouseDate")]
	    public DateTime? ToWarehouseDate { get; set; }
        [Column("ToWarehouseEstimationDate")]
	    public DateTime? ToWarehouseEstimationDate { get; set; }
        [Column("ToWarehouseNotes")]
	    public string ToWarehouseNotes { get; set; }
        [Column("CustomsPaymentDone")]
	    public bool? CustomsPaymentDone { get; set; }
        [Column("CustomsPaymentDate")]
	    public DateTime? CustomsPaymentDate { get; set; }
        [Column("ClearanceDone")]
	    public bool? ClearanceDone { get; set; }
        [Column("ClearanceDate")]
	    public DateTime? ClearanceDate { get; set; }
        [Column("DeliveredDone")]
	    public bool? DeliveredDone { get; set; }
        [Column("DeliveredDate")]
	    public DateTime? DeliveredDate { get; set; }
        [Column("DeliveredEstimationDate")]
	    public DateTime? DeliveredEstimationDate { get; set; }
        [Column("FromWarehouseDone")]
	    public bool? FromWarehouseDone { get; set; }
        [Column("FirstPickupETD")]
	    public DateTime? FirstPickupETD { get; set; }
        [Column("WarehouseLegActualEntryDate")]
	    public DateTime? WarehouseLegActualEntryDate { get; set; }
        [Column("WarehouseLegExpectedEntryDate")]
	    public DateTime? WarehouseLegExpectedEntryDate { get; set; }
        [Column("WarehouseLegRemarks")]
	    public string WarehouseLegRemarks { get; set; }
        [Column("DeclarationDate")]
	    public DateTime? DeclarationDate { get; set; }
        [Column("CustomsClearanceDate")]
	    public DateTime? CustomsClearanceDate { get; set; }
     [Key]
        [Column("Id")]
	    public int Id { get; set; }
        [Column("ContainersNumbers")]
	    public string ContainersNumbers { get; set; }
        [Column("PackagesQuantity")]
	    public int? PackagesQuantity { get; set; }
        [Column("DirectionId")]
	    public string DirectionId { get; set; }
        [Column("ShipmentLevelCode")]
	    public string ShipmentLevelCode { get; set; }
        [Column("AssignedTruckerDone")]
	    public bool AssignedTruckerDone { get; set; }
        [Column("AssignedTruckerDate")]
	    public DateTime? AssignedTruckerDate { get; set; }
        [Column("AssignedTruckerEstimationDate")]
	    public DateTime? AssignedTruckerEstimationDate { get; set; }
        [Column("AssignedTruckerNotes")]
	    public string AssignedTruckerNotes { get; set; }
        [Column("AssignedCustomsAgentDone")]
	    public bool? AssignedCustomsAgentDone { get; set; }
        [Column("AssignedCustomsAgentDate")]
	    public DateTime? AssignedCustomsAgentDate { get; set; }
        [Column("AssignedCustomsAgentEstDate")]
	    public DateTime? AssignedCustomsAgentEstDate { get; set; }
        [Column("AssignedCustomsAgentNotes")]
	    public string AssignedCustomsAgentNotes { get; set; }
        [Column("AssignedCustomsAgentExcReason")]
	    public string AssignedCustomsAgentExcReason { get; set; }
        [Column("DeliveryDone")]
	    public bool DeliveryDone { get; set; }
        [Column("DeliveryDate")]
	    public DateTime? DeliveryDate { get; set; }
        [Column("DeliveryEstimationDate")]
	    public DateTime? DeliveryEstimationDate { get; set; }
        [Column("DeliveryNotes")]
	    public string DeliveryNotes { get; set; }
        [Column("DeliveryExceptionReason")]
	    public string DeliveryExceptionReason { get; set; }
        [Column("GrossWeightUnitCode")]
	    public string GrossWeightUnitCode { get; set; }
        [Column("ForwardingShipmentNumber")]
	    public string ForwardingShipmentNumber { get; set; }
        [Column("ShipmentTypeCode")]
	    public string ShipmentTypeCode { get; set; }
    }
}
	 