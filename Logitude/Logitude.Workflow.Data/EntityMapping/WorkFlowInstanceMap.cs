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
 
    public class WorkFlowInstanceMap : EntityTypeConfiguration<WorkFlowInstance>
    {
	    string dbms;
        public WorkFlowInstanceMap()
        { 
				this.ToTable("WorkFlowInstances");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.StartTime).HasColumnName("StartTime");

            this.Property(t => t.EndTime).HasColumnName("EndTime");

            this.Property(t => t.BusinessKey).HasColumnName("BusinessKey").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.Duration).HasColumnName("Duration").HasPrecision(18, 3);

            this.Property(t => t.WorkFlowVersionId).HasColumnName("WorkFlowVersionId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RetryAttemptsNumber).HasColumnName("RetryAttemptsNumber").IsRequired();

            this.Property(t => t.WorkflowId).HasColumnName("WorkflowId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NumberOfActivities).HasColumnName("NumberOfActivities");
        }
    }
}
	 