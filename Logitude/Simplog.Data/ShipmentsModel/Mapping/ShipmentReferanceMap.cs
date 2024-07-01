using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentReferanceMap : EntityTypeConfiguration<ShipmentReferance>
    {
        public ShipmentReferanceMap()
        {
            this.HasKey(t => t.ShipmentId);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReferenceType).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.PartnerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReferenceValue).HasMaxLength(30).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentReferances");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");         
            this.Property(t => t.LineNumber).HasColumnName("LineNumber");
            this.Property(t => t.ReferenceType).HasColumnName("ReferenceType");
            this.Property(t => t.PartnerId).HasColumnName("PartnerId");
            this.Property(t => t.ReferenceValue).HasColumnName("ReferenceValue");

            // Relationships
            this.HasOptional(t => t.ReferenceType).WithMany().HasForeignKey(d => d.ReferenceTypeCode);
            this.HasOptional(t => t.PartnerId).WithMany().HasForeignKey(d => d.Card);
            this.HasRequired(t => t.ShipmentId).WithMany().HasForeignKey(d => d.Shipment);
        }
    }
}
