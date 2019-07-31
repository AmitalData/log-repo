using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class InsideShipmentPackageMap : EntityTypeConfiguration<InsideShipmentPackage>
    {
        public InsideShipmentPackageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPackageId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.OriginalShipmentPackageId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OriginalInsideShipmentPackageId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Reference1).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.Reference2).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.Reference3).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.Reference4).HasMaxLength(2000).IsUnicode(false);
            this.Property(t => t.CommodityNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.CommodityName).HasMaxLength(250).IsUnicode(false);

            this.Property(t => t.Make).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Model).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Year).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Color).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ChassisNumber).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.RegistrationNumber).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CountryId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("InsideShipmentPackages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ShipmentPackageId).HasColumnName("ShipmentPackageId");
            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.OriginalShipmentPackageId).HasColumnName("OriginalShipmentPackageId");
            
            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");
            this.Property(t => t.Reference1).HasColumnName("Reference1");
            this.Property(t => t.Reference2).HasColumnName("Reference2");
            this.Property(t => t.Reference3).HasColumnName("Reference3");
            this.Property(t => t.Reference4).HasColumnName("Reference4");
            this.Property(t => t.CommodityNumber).HasColumnName("CommodityNumber");
            this.Property(t => t.CommodityName).HasColumnName("CommodityName");

            this.Property(t => t.Make).HasColumnName("Make");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Color).HasColumnName("Color");
            this.Property(t => t.ChassisNumber).HasColumnName("ChassisNumber");
            this.Property(t => t.RegistrationNumber).HasColumnName("RegistrationNumber");
            this.Property(t => t.CountryId).HasColumnName("CountryId");


            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
            this.Property(t => t.OriginalInsideShipmentPackageId).HasColumnName("OriginInsideShipmentPackageId");
            }
            //#else
            else
            {
            this.Property(t => t.OriginalInsideShipmentPackageId).HasColumnName("OriginalInsideShipmentPackageId");
            }
            
//#endif

            // Relationships
            this.HasOptional(t => t.PackageType)
                .WithMany()
                .HasForeignKey(d => d.PackageTypeId);
            this.HasRequired(t => t.ShipmentPackage)
                .WithMany()
                .HasForeignKey(d => d.ShipmentPackageId);
            this.HasOptional(t => t.Country).WithMany().HasForeignKey(d => d.CountryId);

        }
    }
}
