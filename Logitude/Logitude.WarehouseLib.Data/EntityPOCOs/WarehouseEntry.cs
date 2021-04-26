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

namespace Logitude.WarehouseLib.Data.EntityPOCOs
{
   
    public class WarehouseEntry
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
        [Column("EntryNumber")]
	    public string EntryNumber { get; set; }
        [ForeignKey("Customer")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card Customer { get; set; }
        [ForeignKey("Shipment")]
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
	      
        public virtual Shipment Shipment { get; set; }
        [Column("ShipmentNumber")]
	    public string ShipmentNumber { get; set; }
        [ForeignKey("Warehouse")]
        [Column("WarehouseId")]
	    public string WarehouseId { get; set; }
	      
        public virtual Warehouse Warehouse { get; set; }
        [Column("ExpectedEntryDate")]
	    public DateTime? ExpectedEntryDate { get; set; }
        [Column("ActualEntryDate")]
	    public DateTime? ActualEntryDate { get; set; }
        [Column("ReceivedBy")]
	    public string ReceivedBy { get; set; }
        [Column("SpecialInstruction")]
	    public string SpecialInstruction { get; set; }
        [ForeignKey("WarehouseEntryStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual WarehouseEntryStatus WarehouseEntryStatus { get; set; }
        [Column("TotalPieces")]
	    public int TotalPieces { get; set; }
        [Column("TotalGrossWeight")]
	    public decimal TotalGrossWeight { get; set; }
        [Column("GrossWeightUnitCode")]
	    public string GrossWeightUnitCode { get; set; }
        [Column("TotalVolume")]
	    public decimal TotalVolume { get; set; }
        [Column("VolumeUnitCode")]
	    public string VolumeUnitCode { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("CustomerRef1")]
	    public string CustomerRef1 { get; set; }
        [Column("CustomerRef2")]
	    public string CustomerRef2 { get; set; }
        [Column("HouseNumber")]
	    public string HouseNumber { get; set; }
        [Column("MasterNumber")]
	    public string MasterNumber { get; set; }
        [Column("DimensionsUnitCode")]
	    public string DimensionsUnitCode { get; set; }
        [ForeignKey("ShipmentLevel")]
        [Column("ShipmentLevelCode")]
	    public string ShipmentLevelCode { get; set; }
	      
        public virtual ShipmentLevel ShipmentLevel { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [ForeignKey("FromPort")]
        [Column("FromPortId")]
	    public string FromPortId { get; set; }
	      
        public virtual Port FromPort { get; set; }
        [ForeignKey("ToPort")]
        [Column("ToPortId")]
	    public string ToPortId { get; set; }
	      
        public virtual Port ToPort { get; set; }
        [Column("TruckerId")]
	    public string TruckerId { get; set; }
        [Column("TruckerReference")]
	    public string TruckerReference { get; set; }
        [ForeignKey("Shipper")]
        [Column("ShipperId")]
	    public string ShipperId { get; set; }
	      
        public virtual Card Shipper { get; set; }
        [ForeignKey("Direction")]
        [Column("DirectionId")]
	    public string DirectionId { get; set; }
	      
        public virtual Direction Direction { get; set; }
        [ForeignKey("ShipmentType")]
        [Column("ShipmentTypeId")]
	    public string ShipmentTypeId { get; set; }
	      
        public virtual ShipmentType ShipmentType { get; set; }
        [Column("EntryReference")]
	    public string EntryReference { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("ConnectedToShipment")]
	    public bool ConnectedToShipment { get; set; }
        [Column("FromAddressId")]
	    public string FromAddressId { get; set; }
        [Column("ToAddressId")]
	    public string ToAddressId { get; set; }
        [ForeignKey("ConsigneeCard")]
        [Column("ConsigneeId")]
	    public string ConsigneeId { get; set; }
	      
        public virtual Card ConsigneeCard { get; set; }
        [Column("ShipperReference1")]
	    public string ShipperReference1 { get; set; }
        [Column("ConsigneeReference1")]
	    public string ConsigneeReference1 { get; set; }
        [Column("ConsigneeReference2")]
	    public string ConsigneeReference2 { get; set; }
        [Column("ShipperReference2")]
	    public string ShipperReference2 { get; set; }
        [Column("ShipperName")]
	    public string ShipperName { get; set; }
        [Column("ConsigneeName")]
	    public string ConsigneeName { get; set; }
        [Column("Manufacturer")]
	    public string Manufacturer { get; set; }
        [Column("FromPartnerId")]
	    public string FromPartnerId { get; set; }
        [Column("ToPartnerId")]
	    public string ToPartnerId { get; set; }
        [Column("ChargeableWeightUnitCode")]
	    public string ChargeableWeightUnitCode { get; set; }
        [Column("TotalVolumetricWeight")]
	    public decimal TotalVolumetricWeight { get; set; }
        [Column("LastStatusUpdateDate")]
	    public DateTime? LastStatusUpdateDate { get; set; }
        [Column("ConnectedTo")]
	    public string ConnectedTo { get; set; }
        [Column("Ratio")]
	    public double? Ratio { get; set; }
        [Column("ToTypeCode")]
	    public string ToTypeCode { get; set; }
        [Column("FromTypeCode")]
	    public string FromTypeCode { get; set; }
        [ForeignKey("FromCountry")]
        [Column("FromCountryId")]
	    public string FromCountryId { get; set; }
	      
        public virtual Country FromCountry { get; set; }
        [ForeignKey("ToCountry")]
        [Column("ToCountryId")]
	    public string ToCountryId { get; set; }
	      
        public virtual Country ToCountry { get; set; }
        [Column("MasterShipmentNumber")]
	    public string MasterShipmentNumber { get; set; }
    }
}
	 