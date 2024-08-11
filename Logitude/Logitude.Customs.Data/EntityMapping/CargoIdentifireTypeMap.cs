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
 
    public class CargoIdentifireTypeMap : EntityTypeConfiguration<CargoIdentifireType>
    {
	    string dbms;
        public CargoIdentifireTypeMap()
        { 
			  this.ToTable("CargoIdentifireTypes", "Customs");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(80).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.IsForDeclarationExport).HasColumnName("IsForDeclarationExport");

            this.Property(t => t.IsForDeclarationImport).HasColumnName("IsForDeclarationImport");

            this.Property(t => t.IsForManifest).HasColumnName("IsForManifest");

            this.Property(t => t.IsKey2Mandatory).HasColumnName("IsKey2Mandatory");

            this.Property(t => t.IsKey3Mandatory).HasColumnName("IsKey3Mandatory");

            this.Property(t => t.CargoIdentifierKey1Name).HasColumnName("CargoIdentifierKey1Name").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.CargoIdentifierKey2Name).HasColumnName("CargoIdentifierKey2Name").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.CargoIdentifierKey3Name).HasColumnName("CargoIdentifierKey3Name").HasMaxLength(40).IsUnicode(true);
        }
    }
}
	 