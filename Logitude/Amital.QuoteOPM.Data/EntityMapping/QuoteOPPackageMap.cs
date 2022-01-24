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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPPackageMap : EntityTypeConfiguration<QuoteOPPackage>
    {
	    string dbms;
        public QuoteOPPackageMap()
        { 
				this.ToTable("QuoteOPPackages");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.QuoteOPId).HasColumnName("QuoteOPId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PackageTypeId).HasColumnName("PackageTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Quantity).HasColumnName("Quantity");

            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");

            this.Property(t => t.Volume).HasColumnName("Volume");

            this.Property(t => t.Height).HasColumnName("Height");

            this.Property(t => t.Width).HasColumnName("Width");

            this.Property(t => t.Length).HasColumnName("Length");

            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");
        }
    }
}
	 