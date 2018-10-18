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
 
    public class OpportunityStageMap : EntityTypeConfiguration<OpportunityStage>
    {
	    string dbms;
        public OpportunityStageMap()
        { 
				this.ToTable("OpportunityStages");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromStageId).HasColumnName("FromStageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToStageId).HasColumnName("ToStageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
	 