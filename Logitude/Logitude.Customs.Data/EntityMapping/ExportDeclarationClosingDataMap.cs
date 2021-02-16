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
 
    public class ExportDeclarationClosingDataMap : EntityTypeConfiguration<ExportDeclarationClosingData>
    {
	    string dbms;
        public ExportDeclarationClosingDataMap()
        { 
			  this.ToTable("ExportDeclarationClosingDatas", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId });
	 
            this.Property(t => t.FinalThirdCargoId).HasColumnName("FinalThirdCargoId").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FinalCargoTypeCode).HasColumnName("FinalCargoTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.FinalManifestNumber).HasColumnName("FinalManifestNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.FinalSecondCargoId).HasColumnName("FinalSecondCargoId").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.LoadingDateTime).HasColumnName("LoadingDateTime");

            this.Property(t => t.FinalShipCode).HasColumnName("FinalShipCode").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.FinalLoadingSite).HasColumnName("FinalLoadingSite").HasMaxLength(10).IsUnicode(false);
        }
    }
}
	 