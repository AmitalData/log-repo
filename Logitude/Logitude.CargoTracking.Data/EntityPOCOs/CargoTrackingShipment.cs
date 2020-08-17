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

        [Key]
        [Column("Id")]
	    public int Id { get; set; }
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
        [Column("ClearanceDone")]
	    public bool? ClearanceDone { get; set; }
        [Column("PickupDate")]
	    public DateTime? PickupDate { get; set; }
        [Column("ClearanceDate")]
	    public DateTime? ClearanceDate { get; set; }
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
    }
}
	 