using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class WarehouseStoragePricingMap : EntityTypeConfiguration<WarehouseStoragePricing>
    {
        public WarehouseStoragePricingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.WarehouseId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("WarehouseStoragePricings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId");
            this.Property(t => t.StepFrom).HasColumnName("StepFrom");
            this.Property(t => t.StepTo).HasColumnName("StepTo");
            this.Property(t => t.Days).HasColumnName("Days");
            this.Property(t => t.SalePrice).HasColumnName("SalePrice");

            // Relationships
            this.HasRequired(t => t.Warehouse).WithMany().HasForeignKey(d => d.WarehouseId);
        }
    }
}
