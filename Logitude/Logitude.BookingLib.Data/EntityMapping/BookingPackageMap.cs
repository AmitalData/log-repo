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
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data;
 
namespace Logitude.BookingLib.Data.EntityMapping
{
 
    public class BookingPackageMap : EntityTypeConfiguration<BookingPackage>
    {
	    string dbms;
        public BookingPackageMap()
        { 
				this.ToTable("BookingPackages");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.BookingId).HasColumnName("BookingId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Seal).HasColumnName("Seal").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Quantity).HasColumnName("Quantity").IsRequired();

            this.Property(t => t.Weight).HasColumnName("Weight").HasPrecision(18, 3);

            this.Property(t => t.Volume).HasColumnName("Volume").HasPrecision(18, 3);

            this.Property(t => t.Tare).HasColumnName("Tare");

            this.Property(t => t.Height).HasColumnName("Height");

            this.Property(t => t.Width).HasColumnName("Width");

            this.Property(t => t.Length).HasColumnName("Length");

            this.Property(t => t.UnNumber).HasColumnName("UnNumber").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ClassNumber).HasColumnName("ClassNumber").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.Temperature).HasColumnName("Temperature");

            this.Property(t => t.Ventilation).HasColumnName("Ventilation");

            this.Property(t => t.Seal2).HasColumnName("Seal2").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SOC).HasColumnName("SOC");

            this.Property(t => t.MarksAndNumbers).HasColumnName("MarksAndNumbers").HasMaxLength(250).IsUnicode(false);

            this.Property(t => t.PackagingGroup).HasColumnName("PackagingGroup").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.IMDGCode).HasColumnName("IMDGCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.FlashPoint).HasColumnName("FlashPoint").HasMaxLength(8).IsUnicode(false);

            this.Property(t => t.Harmonize).HasColumnName("Harmonize").HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.MaterialDescription).HasColumnName("MaterialDescription").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.IsDangerous).HasColumnName("IsDangerous").IsRequired();

            this.Property(t => t.OriginalBookingPackageId).HasColumnName("OriginalBookingPackageId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight").HasPrecision(18, 3);

            this.Property(t => t.CommodityId).HasColumnName("CommodityId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 