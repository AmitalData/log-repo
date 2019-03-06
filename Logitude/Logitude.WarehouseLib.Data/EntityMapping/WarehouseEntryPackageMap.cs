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
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data;
 
namespace Logitude.WarehouseLib.Data.EntityMapping
{
 
    public class WarehouseEntryPackageMap : EntityTypeConfiguration<WarehouseEntryPackage>
    {
	    string dbms;
        public WarehouseEntryPackageMap()
        { 
				this.ToTable("WarehouseEntryPackages");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.WarehouseEntryId).HasColumnName("WarehouseEntryId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Quantity).HasColumnName("Quantity").IsRequired();

            this.Property(t => t.Weight).HasColumnName("Weight").HasPrecision(16, 3);

            this.Property(t => t.Volume).HasColumnName("Volume").HasPrecision(16, 3);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(2000).IsUnicode(false);

            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Seal).HasColumnName("Seal").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Harmonize).HasColumnName("Harmonize").HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.Width).HasColumnName("Width");

            this.Property(t => t.Length).HasColumnName("Length");

            this.Property(t => t.Height).HasColumnName("Height");

            this.Property(t => t.IsContainer).HasColumnName("IsContainer").IsRequired();

            this.Property(t => t.Instock).HasColumnName("Instock").IsRequired();

            this.Property(t => t.Location).HasColumnName("Location").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.IsConnectedToShipment).HasColumnName("IsConnectedToShipment");

            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");

            this.Property(t => t.Make).HasColumnName("Make").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.Model).HasColumnName("Model").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.Year).HasColumnName("Year").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.Color).HasColumnName("Color").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.ChassisNumber).HasColumnName("ChassisNumber").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.RegistrationNumber).HasColumnName("RegistrationNumber").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.CountryId).HasColumnName("CountryId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 