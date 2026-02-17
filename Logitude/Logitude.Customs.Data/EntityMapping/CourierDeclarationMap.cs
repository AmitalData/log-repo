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
 
    public class CourierDeclarationMap : EntityTypeConfiguration<CourierDeclaration>
    {
	    string dbms;
        public CourierDeclarationMap()
        { 
			  this.ToTable("CourierDeclarations", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.CourierMasterId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CourierMasterId).HasColumnName("CourierMasterId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SequenceNumeric).HasColumnName("SequenceNumeric");
        }
    }
}
	 