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
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(null);

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

            this.Property(t => t.ClearanceDone).HasColumnName("ClearanceDone");

            this.Property(t => t.PickupDate).HasColumnName("PickupDate");

            this.Property(t => t.ClearanceDate).HasColumnName("ClearanceDate");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.SecurityKey).HasColumnName("SecurityKey").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ConsigneeName).HasColumnName("ConsigneeName").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.ShipperName).HasColumnName("ShipperName").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.CustomerReference).HasColumnName("CustomerReference").HasMaxLength(101).IsUnicode(false);

            this.Property(t => t.IsMainRecord).HasColumnName("IsMainRecord").IsRequired();
        }
    }
}
	 