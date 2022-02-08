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
 
    public class DeclarationCasualDetailsMap : EntityTypeConfiguration<DeclarationCasualDetails>
    {
	    string dbms;
        public DeclarationCasualDetailsMap()
        { 
			  this.ToTable("DeclarationCasualDetailses", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CasualSupplierName).HasColumnName("CasualSupplierName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.CasualSupplierAddress).HasColumnName("CasualSupplierAddress").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.CasualImporterAddress1).HasColumnName("CasualImporterAddress1").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.CasualImporterAddress2).HasColumnName("CasualImporterAddress2").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.CasualImporterCity).HasColumnName("CasualImporterCity").HasMaxLength(17).IsUnicode(true);

            this.Property(t => t.CasualImporterZipCode).HasColumnName("CasualImporterZipCode").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.CasualImporterFax).HasColumnName("CasualImporterFax").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.CasualImporterEmail).HasColumnName("CasualImporterEmail").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CasualImporterTel).HasColumnName("CasualImporterTel").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.CasualImporterContact).HasColumnName("CasualImporterContact").HasMaxLength(50).IsUnicode(true);
        }
    }
}
	 