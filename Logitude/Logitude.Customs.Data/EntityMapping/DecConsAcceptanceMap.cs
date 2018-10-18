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
 
    public class DecConsAcceptanceMap : EntityTypeConfiguration<DecConsAcceptance>
    {
	    string dbms;
        public DecConsAcceptanceMap()
        { 
			  this.ToTable("DecConsAcceptances", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsignmentNumber).HasColumnName("ConsignmentNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LoadDate).HasColumnName("LoadDate");

            this.Property(t => t.PackageTypeCode).HasColumnName("PackageTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.PackageQuantity).HasColumnName("PackageQuantity");

            this.Property(t => t.GrossMassMeasure).HasColumnName("GrossMassMeasure").HasPrecision(11, 3);
        }
    }
}
	 