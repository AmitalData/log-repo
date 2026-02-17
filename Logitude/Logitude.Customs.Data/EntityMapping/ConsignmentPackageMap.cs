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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class ConsignmentPackageMap : EntityTypeConfiguration<ConsignmentPackage>
    {
	    string dbms;
        public ConsignmentPackageMap()
        { 
			  this.ToTable("ConsignmentPackages", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ConsignmentNumber).HasColumnName("ConsignmentNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.PackageMeasureQualifierCode).HasColumnName("PackageMeasureQualifierCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.PackageQuantity).HasColumnName("PackageQuantity");

            this.Property(t => t.GrossMassMeasure).HasColumnName("GrossMassMeasure");

            this.Property(t => t.PackageTypeCode).HasColumnName("PackageTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.MarksNumbers).HasColumnName("MarksNumbers").HasMaxLength(512).IsUnicode(false);

            this.Property(t => t.SequenceNumeric).HasColumnName("SequenceNumeric");

            this.Property(t => t.PackageQuantityTypeCode).HasColumnName("PackageQuantityTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.GrossMassMeasureTypeCode).HasColumnName("GrossMassMeasureTypeCode").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 