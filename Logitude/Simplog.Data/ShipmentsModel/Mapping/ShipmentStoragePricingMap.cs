using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentStoragePricingMap : EntityTypeConfiguration<ShipmentStoragePricing>
    {
        public ShipmentStoragePricingMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.WarehouseId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentStoragePricings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId");
            this.Property(t => t.StepFrom).HasColumnName("StepFrom");
            this.Property(t => t.StepTo).HasColumnName("StepTo");
            this.Property(t => t.Days).HasColumnName("Days");
            this.Property(t => t.SalePrice).HasColumnName("SalePrice");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.LineNumber).HasColumnName("LineNumber");

            // Relationships            
            this.HasRequired(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);
            this.HasOptional(t => t.Warehouse).WithMany().HasForeignKey(d => d.WarehouseId);
        }
    }
}

