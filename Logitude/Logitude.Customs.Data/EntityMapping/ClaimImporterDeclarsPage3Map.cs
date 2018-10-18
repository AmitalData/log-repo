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
 
    public class ClaimImporterDeclarsPage3Map : EntityTypeConfiguration<ClaimImporterDeclarsPage3>
    {
	    string dbms;
        public ClaimImporterDeclarsPage3Map()
        { 
			  this.ToTable("ClaimImporterDeclarsPage3s", "Customs");
		
		    this.HasKey(t => new { t.ClaimId, t.LineNo });
	 
            this.Property(t => t.ClaimId).HasColumnName("ClaimId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.LineNo).HasColumnName("LineNo").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.ImporterLoiDeclarationTypeCode).HasColumnName("ImporterLoiDeclarationTypeCode").HasMaxLength(2).IsUnicode(false);
        }
    }
}
	 