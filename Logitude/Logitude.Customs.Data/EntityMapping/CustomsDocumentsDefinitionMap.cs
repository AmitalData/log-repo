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
 
    public class CustomsDocumentsDefinitionMap : EntityTypeConfiguration<CustomsDocumentsDefinition>
    {
	    string dbms;
        public CustomsDocumentsDefinitionMap()
        { 
			  this.ToTable("CustomsDocumentsDefinitions", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DocumentTypeCode).HasColumnName("DocumentTypeCode").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.TransportationTypeCode).HasColumnName("TransportationTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ProcessTypeCode).HasColumnName("ProcessTypeCode").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.CargoTypeCode).HasColumnName("CargoTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Mandatory).HasColumnName("Mandatory");

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.DeclarationTypeCode).HasColumnName("DeclarationTypeCode").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 