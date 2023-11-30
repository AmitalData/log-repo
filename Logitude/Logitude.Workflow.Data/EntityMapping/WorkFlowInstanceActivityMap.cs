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
 
    public class WorkFlowInstanceActivityMap : EntityTypeConfiguration<WorkFlowInstanceActivity>
    {
	    string dbms;
        public WorkFlowInstanceActivityMap()
        { 
				this.ToTable("WorkFlowInstanceActivities");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Sequence).HasColumnName("Sequence").IsRequired();

            this.Property(t => t.ActionName).HasColumnName("ActionName").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.StartTime).HasColumnName("StartTime");

            this.Property(t => t.Duration).HasColumnName("Duration").HasPrecision(18, 3);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EndTime).HasColumnName("EndTime");

            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage").IsMaxLength().IsUnicode(false);

            this.Property(t => t.Result).HasColumnName("Result").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.ActionType).HasColumnName("ActionType").HasMaxLength(50).IsUnicode(false);
        }
    }
}
	 