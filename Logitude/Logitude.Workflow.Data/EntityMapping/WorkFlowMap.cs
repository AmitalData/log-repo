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
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data;
 
namespace Logitude.Workflow.Data.EntityMapping
{
 
    public class WorkFlowMap : EntityTypeConfiguration<WorkFlow>
    {
	    string dbms;
        public WorkFlowMap()
        { 
				this.ToTable("WorkFlows");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.FlowJson).HasColumnName("FlowJson").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Entity).HasColumnName("Entity").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.Trigger).HasColumnName("Trigger").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.RetriesNumber).HasColumnName("RetriesNumber").IsRequired();

            this.Property(t => t.RetriesDelay).HasColumnName("RetriesDelay").IsRequired().HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.WorkFlowTriggerTypeCode).HasColumnName("WorkFlowTriggerTypeCode").IsRequired().HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.WorkFlowNumber).HasColumnName("WorkFlowNumber").IsRequired().HasMaxLength(100).IsUnicode(true);
        }
    }
}
	 