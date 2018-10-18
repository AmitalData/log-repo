using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentOrderPackageMap : EntityTypeConfiguration<ShipmentOrderPackage>
    {
        public ShipmentOrderPackageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ShipmentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PackageTypeId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentOrderPackages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.IsContainer).HasColumnName("IsContainer");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId");
            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");

            // Relationships
            this.HasOptional(t => t.PackageType)
                .WithMany()
                .HasForeignKey(d => d.PackageTypeId);

            this.HasRequired(t => t.Shipment)
                .WithMany()
                .HasForeignKey(d => d.ShipmentId);

        }
    }
}
