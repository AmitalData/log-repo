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
   
    public class WarehouseRelease
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
        [Column("ReleaseNumber")]
	    public string ReleaseNumber { get; set; }
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
        [Column("ExpectedReleaseDate")]
	    public DateTime? ExpectedReleaseDate { get; set; }
        [Column("ActualReleaseDate")]
	    public DateTime? ActualReleaseDate { get; set; }
        [Column("ReleaseBy")]
	    public string ReleaseBy { get; set; }
        [Column("SpecialInstruction")]
	    public string SpecialInstruction { get; set; }
        [ForeignKey("WarehouseReleaseStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual WarehouseReleaseStatus WarehouseReleaseStatus { get; set; }
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
        [ForeignKey("ShipmentType")]
        [Column("ShipmentTypeId")]
	    public string ShipmentTypeId { get; set; }
	      
        public virtual ShipmentType ShipmentType { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [ForeignKey("ShipmentLevel")]
        [Column("ShipmentLevelCode")]
	    public string ShipmentLevelCode { get; set; }
	      
        public virtual ShipmentLevel ShipmentLevel { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("Direction")]
        [Column("DirectionId")]
	    public string DirectionId { get; set; }
	      
        public virtual Direction Direction { get; set; }
        [Column("TotalQuantity")]
	    public int TotalQuantity { get; set; }
        [Column("ChargeableWeightUnitCode")]
	    public string ChargeableWeightUnitCode { get; set; }
    }
}
	 