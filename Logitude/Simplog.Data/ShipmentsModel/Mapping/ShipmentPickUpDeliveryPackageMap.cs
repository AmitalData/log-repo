using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentPickUpDeliveryPackageMap : EntityTypeConfiguration<ShipmentPickUpDeliveryPackage>
    {
        public ShipmentPickUpDeliveryPackageMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.PackageTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPickUpDeliveryId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipperSeal).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Harmonize).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.OriginalShipmentPackageId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Make).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Model).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Year).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Color).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ChassisNumber).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.RegistrationNumber).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerEntityId).HasMaxLength(15).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("ShipmentPickUpDeliveryPackages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.Quantity).HasColumnName("Quantity").IsRequired();
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId");
            this.Property(t => t.ShipmentPickUpDeliveryId).HasColumnName("ShipmentPickUpDeliveryId");
            this.Property(t => t.ShipperSeal).HasColumnName("ShipperSeal");
            this.Property(t => t.Harmonize).HasColumnName("Harmonize");
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.OriginalShipmentPackageId).HasColumnName("OriginalShipmentPackageId");
            this.Property(t => t.IsMultiHarmonize).HasColumnName("IsMultiHarmonize");
            this.Property(t => t.Make).HasColumnName("Make");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Color).HasColumnName("Color");
            this.Property(t => t.ChassisNumber).HasColumnName("ChassisNumber");
            this.Property(t => t.RegistrationNumber).HasColumnName("RegistrationNumber");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.ContainerEntityId).HasColumnName("ContainerEntityId");

            // Relationships
            this.HasOptional(t => t.PackageType).WithMany().HasForeignKey(d => d.PackageTypeId);
            this.HasRequired(t => t.ShipmentPickUpDelivery).WithMany().HasForeignKey(d => d.ShipmentPickUpDeliveryId);
            this.HasOptional(t => t.Country).WithMany().HasForeignKey(d => d.CountryId);

        }
    }
}
