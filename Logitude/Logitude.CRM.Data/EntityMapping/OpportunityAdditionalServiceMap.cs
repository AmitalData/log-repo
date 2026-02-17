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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data;
 
namespace Logitude.CRM.Data.EntityMapping
{
 
    public class OpportunityAdditionalServiceMap : EntityTypeConfiguration<OpportunityAdditionalService>
    {
	    string dbms;
        public OpportunityAdditionalServiceMap()
        { 
				this.ToTable("OpportunityAdditionalServices");
		
		    this.HasKey(t => new { t.OpportunityId, t.AdditionalServiceId });
	 
            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AdditionalServiceId).HasColumnName("AdditionalServiceId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.NotesRightToLeft).HasColumnName("NotesRightToLeft");
        }
    }
}
	 