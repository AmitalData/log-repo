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
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data;
 
namespace Logitude.ShipmentOrderModule.Data.EntityMapping
{
 
    public class ShipmentOrderMap : EntityTypeConfiguration<ShipmentOrder>
    {
	    string dbms;
        public ShipmentOrderMap()
        { 
				this.ToTable("ShipmentOrders");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").IsRequired().HasMaxLength(1).IsFixedLength();

            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperId).HasColumnName("ShipperId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AgentId).HasColumnName("AgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IncotermId).HasColumnName("IncotermId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AccountManagerId).HasColumnName("AccountManagerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PONumber).HasColumnName("PONumber").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.Master).HasColumnName("Master").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.House).HasColumnName("House").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.CarrierNumber).HasColumnName("CarrierNumber").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.VesselId).HasColumnName("VesselId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ETD).HasColumnName("ETD");

            this.Property(t => t.ETA).HasColumnName("ETA");

            this.Property(t => t.ATD).HasColumnName("ATD");

            this.Property(t => t.ATA).HasColumnName("ATA");

            this.Property(t => t.CustomsAgentId).HasColumnName("CustomsAgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SpecialServicesTypeId).HasColumnName("SpecialServicesTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerReferences).HasColumnName("CustomerReferences").HasMaxLength(300).IsUnicode(false);

            this.Property(t => t.IsReadyForPickup).HasColumnName("IsReadyForPickup");

            this.Property(t => t.PickupEstimatedDateTime).HasColumnName("PickupEstimatedDateTime");

            this.Property(t => t.PickupActualDateTime).HasColumnName("PickupActualDateTime");

            this.Property(t => t.ForwarderId).HasColumnName("ForwarderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BookingConfirmationDate).HasColumnName("BookingConfirmationDate");

            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.SupplyDateTime).HasColumnName("SupplyDateTime");

            this.Property(t => t.OriginPortId).HasColumnName("OriginPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DestinationPortId).HasColumnName("DestinationPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GatewayId).HasColumnName("GatewayId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CasualImporterName).HasColumnName("CasualImporterName").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.CasualSupplierName).HasColumnName("CasualSupplierName").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.PODate).HasColumnName("PODate");

            this.Property(t => t.BookingConfirmationNumber).HasColumnName("BookingConfirmationNumber").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.DirectionId).HasColumnName("DirectionId").IsRequired().HasMaxLength(1).IsFixedLength();

            this.Property(t => t.CarrierId).HasColumnName("CarrierId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 