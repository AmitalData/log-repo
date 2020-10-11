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
 
    public class ConsignmentInternalTransitionMap : EntityTypeConfiguration<ConsignmentInternalTransition>
    {
	    string dbms;
        public ConsignmentInternalTransitionMap()
        { 
			  this.ToTable("ConsignmentInternalTransitions", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsignmentNumber).HasColumnName("ConsignmentNumber").HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.SiteCode).HasColumnName("SiteCode").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);
        }
    }
}
	 