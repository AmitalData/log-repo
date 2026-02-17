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
 
    public class OpportunityCompetitorMap : EntityTypeConfiguration<OpportunityCompetitor>
    {
	    string dbms;
        public OpportunityCompetitorMap()
        { 
				this.ToTable("OpportunityCompetitors");
		
		    this.HasKey(t => new { t.OpportunityId, t.CompetitorId });
	 
            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CompetitorId).HasColumnName("CompetitorId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
        }
    }
}
	 