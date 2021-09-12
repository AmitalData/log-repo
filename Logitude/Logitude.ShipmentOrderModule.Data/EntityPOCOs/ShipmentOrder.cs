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

namespace Logitude.ShipmentOrderModule.Data.EntityPOCOs
{
   
    public class ShipmentOrder
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("OrderNumber")]
	    public string OrderNumber { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [ForeignKey("ConsigneeCard")]
        [Column("ConsigneeId")]
	    public string ConsigneeId { get; set; }
	      
        public virtual Card ConsigneeCard { get; set; }
        [ForeignKey("ShipperCard")]
        [Column("ShipperId")]
	    public string ShipperId { get; set; }
	      
        public virtual Card ShipperCard { get; set; }
        [ForeignKey("AgentCard")]
        [Column("AgentId")]
	    public string AgentId { get; set; }
	      
        public virtual Card AgentCard { get; set; }
        [ForeignKey("Incoterm")]
        [Column("IncotermId")]
	    public string IncotermId { get; set; }
	      
        public virtual Incoterm Incoterm { get; set; }
        [ForeignKey("AccountManagerUser")]
        [Column("AccountManagerId")]
	    public string AccountManagerId { get; set; }
	      
        public virtual User AccountManagerUser { get; set; }
        [Column("PONumber")]
	    public string PONumber { get; set; }
        [Column("DescriptionOfGoods")]
	    public string DescriptionOfGoods { get; set; }
        [Column("Master")]
	    public string Master { get; set; }
        [Column("House")]
	    public string House { get; set; }
        [Column("CarrierNumber")]
	    public string CarrierNumber { get; set; }
        [ForeignKey("Vessel")]
        [Column("VesselId")]
	    public string VesselId { get; set; }
	      
        public virtual Vessel Vessel { get; set; }
        [Column("ETD")]
	    public DateTime? ETD { get; set; }
        [Column("ETA")]
	    public DateTime? ETA { get; set; }
        [Column("ATD")]
	    public DateTime? ATD { get; set; }
        [Column("ATA")]
	    public DateTime? ATA { get; set; }
        [ForeignKey("CustomsAgentCard")]
        [Column("CustomsAgentId")]
	    public string CustomsAgentId { get; set; }
	      
        public virtual Card CustomsAgentCard { get; set; }
        [ForeignKey("SpecialServicesType")]
        [Column("SpecialServicesTypeId")]
	    public string SpecialServicesTypeId { get; set; }
	      
        public virtual SpecialServicesType SpecialServicesType { get; set; }
        [Column("CustomerReferences")]
	    public string CustomerReferences { get; set; }
        [Column("IsReadyForPickup")]
	    public bool IsReadyForPickup { get; set; }
        [Column("PickupEstimatedDateTime")]
	    public DateTime? PickupEstimatedDateTime { get; set; }
        [Column("PickupActualDateTime")]
	    public DateTime? PickupActualDateTime { get; set; }
        [ForeignKey("FreightForwarderCard")]
        [Column("ForwarderId")]
	    public string ForwarderId { get; set; }
	      
        public virtual Card FreightForwarderCard { get; set; }
        [Column("BookingConfirmationDate")]
	    public DateTime? BookingConfirmationDate { get; set; }
        [Column("ShipmentNumber")]
	    public string ShipmentNumber { get; set; }
        [Column("SupplyDateTime")]
	    public DateTime? SupplyDateTime { get; set; }
        [ForeignKey("OriginPort")]
        [Column("OriginPortId")]
	    public string OriginPortId { get; set; }
	      
        public virtual Port OriginPort { get; set; }
        [ForeignKey("DestinationPort")]
        [Column("DestinationPortId")]
	    public string DestinationPortId { get; set; }
	      
        public virtual Port DestinationPort { get; set; }
        [ForeignKey("Gateway")]
        [Column("GatewayId")]
	    public string GatewayId { get; set; }
	      
        public virtual Port Gateway { get; set; }
        [Column("CasualImporterName")]
	    public string CasualImporterName { get; set; }
        [Column("CasualSupplierName")]
	    public string CasualSupplierName { get; set; }
        [ForeignKey("ShipmentLevel")]
        [Column("ShipmentLevelCode")]
	    public string ShipmentLevelCode { get; set; }
	      
        public virtual ShipmentLevel ShipmentLevel { get; set; }
        [Column("PODate")]
	    public DateTime? PODate { get; set; }
        [Column("BookingConfirmationNumber")]
	    public string BookingConfirmationNumber { get; set; }
        [ForeignKey("Direction")]
        [Column("DirectionId")]
	    public string DirectionId { get; set; }
	      
        public virtual Direction Direction { get; set; }
        [ForeignKey("Carrier")]
        [Column("CarrierId")]
	    public string CarrierId { get; set; }
	      
        public virtual Card Carrier { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("SecurityKey")]
	    public string SecurityKey { get; set; }
        [ForeignKey("Shipment")]
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
	      
        public virtual Shipment Shipment { get; set; }
        [Column("Quantity")]
	    public int? Quantity { get; set; }
        [Column("GrossWeight")]
	    public double? GrossWeight { get; set; }
        [Column("Volume")]
	    public double? Volume { get; set; }
        [ForeignKey("Customer")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card Customer { get; set; }
    }
}
	 