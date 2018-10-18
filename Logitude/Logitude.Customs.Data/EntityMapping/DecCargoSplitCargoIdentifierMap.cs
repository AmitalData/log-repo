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
 
    public class DecCargoSplitCargoIdentifierMap : EntityTypeConfiguration<DecCargoSplitCargoIdentifier>
    {
	    string dbms;
        public DecCargoSplitCargoIdentifierMap()
        { 
			  this.ToTable("DecCargoSplitCargoIdentifiers", "Customs");
		
		    this.HasKey(t => new { t.DeclarationCargoSplitId, t.LineNumber });
	 
            this.Property(t => t.DeclarationCargoSplitId).HasColumnName("DeclarationCargoSplitId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").HasDatabaseGeneratedOption(null);

            this.Property(t => t.CargoIdentifierKey1).HasColumnName("CargoIdentifierKey1").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey2).HasColumnName("CargoIdentifierKey2").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoIdentifierKey3).HasColumnName("CargoIdentifierKey3").HasMaxLength(35).IsUnicode(false);
        }
    }
}
	 