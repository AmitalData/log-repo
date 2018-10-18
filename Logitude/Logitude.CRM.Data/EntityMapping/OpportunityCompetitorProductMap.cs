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
 
    public class OpportunityCompetitorProductMap : EntityTypeConfiguration<OpportunityCompetitorProduct>
    {
	    string dbms;
        public OpportunityCompetitorProductMap()
        { 
				this.ToTable("OpportunityCompetitorProducts");
		
		    this.HasKey(t => new { t.OpportunityId, t.CompetitorId, t.ProductTypeCode });
	 
            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CompetitorId).HasColumnName("CompetitorId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
        }
    }
}
	 