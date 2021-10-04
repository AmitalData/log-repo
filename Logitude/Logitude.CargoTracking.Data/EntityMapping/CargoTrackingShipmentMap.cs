using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data;
 
namespace Logitude.CargoTracking.Data.EntityMapping
{
 
    public class CargoTrackingShipmentMap : EntityTypeConfiguration<CargoTrackingShipment>
    {
	    string dbms;
        public CargoTrackingShipmentMap()
        { 
				this.ToTable("CargoTrackingShipments");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.EntityId).HasColumnName("EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ForwardingShipmentHeaderId).HasColumnName("ForwardingShipmentHeaderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsShipmentHeaderId).HasColumnName("CustomsShipmentHeaderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityType).HasColumnName("EntityType").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CurrentMilestoneCode).HasColumnName("CurrentMilestoneCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CurrentMilestoneDate).HasColumnName("CurrentMilestoneDate");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Master).HasColumnName("Master").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.House).HasColumnName("House").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperId).HasColumnName("ShipperId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");

            this.Property(t => t.Volume).HasColumnName("Volume");

            this.Property(t => t.PickupDone).HasColumnName("PickupDone");

            this.Property(t => t.PickupDate).HasColumnName("PickupDate");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.SecurityKey).HasColumnName("SecurityKey").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ConsigneeName).HasColumnName("ConsigneeName").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.ShipperName).HasColumnName("ShipperName").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.CustomerReference).HasColumnName("CustomerReference").HasMaxLength(101).IsUnicode(false);

            this.Property(t => t.IsMainRecord).HasColumnName("IsMainRecord").IsRequired();

            this.Property(t => t.PickupEstimationDate).HasColumnName("PickupEstimationDate");

            this.Property(t => t.FromWarehouseDate).HasColumnName("FromWarehouseDate");

            this.Property(t => t.FromWarehouseEstimationDate).HasColumnName("FromWarehouseEstimationDate");

            this.Property(t => t.FromWarehouseNotes).HasColumnName("FromWarehouseNotes").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.DepartureDone).HasColumnName("DepartureDone");

            this.Property(t => t.DepartureDate).HasColumnName("DepartureDate");

            this.Property(t => t.DepartureEstimationDate).HasColumnName("DepartureEstimationDate");

            this.Property(t => t.ArrivalDone).HasColumnName("ArrivalDone");

            this.Property(t => t.ArrivalDate).HasColumnName("ArrivalDate");

            this.Property(t => t.ArrivalEstimationDate).HasColumnName("ArrivalEstimationDate");

            this.Property(t => t.ToWarehouseDone).HasColumnName("ToWarehouseDone");

            this.Property(t => t.ToWarehouseDate).HasColumnName("ToWarehouseDate");

            this.Property(t => t.ToWarehouseEstimationDate).HasColumnName("ToWarehouseEstimationDate");

            this.Property(t => t.ToWarehouseNotes).HasColumnName("ToWarehouseNotes").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.CustomsPaymentDone).HasColumnName("CustomsPaymentDone");

            this.Property(t => t.CustomsPaymentDate).HasColumnName("CustomsPaymentDate");

            this.Property(t => t.ClearanceDone).HasColumnName("ClearanceDone");

            this.Property(t => t.ClearanceDate).HasColumnName("ClearanceDate");

            this.Property(t => t.DeliveredDone).HasColumnName("DeliveredDone");

            this.Property(t => t.DeliveredDate).HasColumnName("DeliveredDate");

            this.Property(t => t.DeliveredEstimationDate).HasColumnName("DeliveredEstimationDate");

            this.Property(t => t.FromWarehouseDone).HasColumnName("FromWarehouseDone");

            this.Property(t => t.FirstPickupETD).HasColumnName("FirstPickupETD");

            this.Property(t => t.WarehouseLegActualEntryDate).HasColumnName("WarehouseLegActualEntryDate");

            this.Property(t => t.WarehouseLegExpectedEntryDate).HasColumnName("WarehouseLegExpectedEntryDate");

            this.Property(t => t.WarehouseLegRemarks).HasColumnName("WarehouseLegRemarks").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.DeclarationDate).HasColumnName("DeclarationDate");

            this.Property(t => t.CustomsClearanceDate).HasColumnName("CustomsClearanceDate");

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.ContainersNumbers).HasColumnName("ContainersNumbers").IsMaxLength().IsUnicode(true);

            this.Property(t => t.PackagesQuantity).HasColumnName("PackagesQuantity");

            this.Property(t => t.DirectionId).HasColumnName("DirectionId").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.AssignedTruckerDone).HasColumnName("AssignedTruckerDone");

            this.Property(t => t.AssignedTruckerDate).HasColumnName("AssignedTruckerDate");

            this.Property(t => t.AssignedTruckerEstimationDate).HasColumnName("AssignedTruckerEstimationDate");

            this.Property(t => t.AssignedTruckerNotes).HasColumnName("AssignedTruckerNotes").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.AssignedCustomsAgentDone).HasColumnName("AssignedCustomsAgentDone");

            this.Property(t => t.AssignedCustomsAgentDate).HasColumnName("AssignedCustomsAgentDate");

            this.Property(t => t.AssignedCustomsAgentEstDate).HasColumnName("AssignedCustomsAgentEstDate");

            this.Property(t => t.AssignedCustomsAgentNotes).HasColumnName("AssignedCustomsAgentNotes").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.AssignedCustomsAgentExcReason).HasColumnName("AssignedCustomsAgentExcReason").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.DeliveryDone).HasColumnName("DeliveryDone");

            this.Property(t => t.DeliveryDate).HasColumnName("DeliveryDate");

            this.Property(t => t.DeliveryEstimationDate).HasColumnName("DeliveryEstimationDate");

            this.Property(t => t.DeliveryNotes).HasColumnName("DeliveryNotes").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.DeliveryExceptionReason).HasColumnName("DeliveryExceptionReason").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ForwardingShipmentNumber).HasColumnName("ForwardingShipmentNumber").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.ShipmentTypeCode).HasColumnName("ShipmentTypeCode").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.CurrentMilestoneExceptions).HasColumnName("CurrentMilestoneExceptions").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.ForwardingHouse).HasColumnName("ForwardingHouse").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.ForwardingMaster).HasColumnName("ForwardingMaster").HasMaxLength(222).IsUnicode(true);

            this.Property(t => t.ForwardingShipmentLevelCode).HasColumnName("ForwardingShipmentLevelCode").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.GoodsClassificationDate).HasColumnName("GoodsClassificationDate");

            this.Property(t => t.GoodsClassificationEstDate).HasColumnName("GoodsClassificationEstDate");

            this.Property(t => t.GoodsClassificationNotes).HasColumnName("GoodsClassificationNotes").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.DocumentInspectionDate).HasColumnName("DocumentInspectionDate");

            this.Property(t => t.DocumentInspectionEstDate).HasColumnName("DocumentInspectionEstDate");

            this.Property(t => t.DocumentInspectionNotes).HasColumnName("DocumentInspectionNotes").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.DocumentInspectionDone).HasColumnName("DocumentInspectionDone");

            this.Property(t => t.GoodsClassificationDone).HasColumnName("GoodsClassificationDone");

            this.Property(t => t.GatepassArrivedDate).HasColumnName("GatepassArrivedDate");

            this.Property(t => t.GatepassArrivedEstDate).HasColumnName("GatepassArrivedEstDate");

            this.Property(t => t.GatepassArrivedNotes).HasColumnName("GatepassArrivedNotes").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.GatepassArrivedDone).HasColumnName("GatepassArrivedDone");

            this.Property(t => t.ImportManifest).HasColumnName("ImportManifest").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CreatedDone).HasColumnName("CreatedDone");

            this.Property(t => t.PrevForwardingShipmentId).HasColumnName("PrevForwardingShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BookingDone).HasColumnName("BookingDone");

            this.Property(t => t.BookingDate).HasColumnName("BookingDate");

            this.Property(t => t.BookingEstimationDate).HasColumnName("BookingEstimationDate");

            this.Property(t => t.BookingNotes).HasColumnName("BookingNotes").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.BookingExceptionReason).HasColumnName("BookingExceptionReason").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.PaymentRequiredDone).HasColumnName("PaymentRequiredDone");

            this.Property(t => t.PaymentRequiredEstimationDate).HasColumnName("PaymentRequiredEstimationDate");

            this.Property(t => t.PaymentRequiredDate).HasColumnName("PaymentRequiredDate");

            this.Property(t => t.PaymentRequiredNotes).HasColumnName("PaymentRequiredNotes").HasMaxLength(32).IsUnicode(true);

            this.Property(t => t.PaymentReceivedDone).HasColumnName("PaymentReceivedDone");

            this.Property(t => t.PaymentReceivedEstomationDate).HasColumnName("PaymentReceivedEstomationDate");

            this.Property(t => t.PaymentReceivedDate).HasColumnName("PaymentReceivedDate");

            this.Property(t => t.PaymentReceivedNotes).HasColumnName("PaymentReceivedNotes").HasMaxLength(32).IsUnicode(true);
        }
    }
}
	 