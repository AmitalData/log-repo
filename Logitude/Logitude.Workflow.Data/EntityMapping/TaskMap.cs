using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data;
 
namespace Logitude.Workflow.Data.EntityMapping
{
 
    public class TaskMap : EntityTypeConfiguration<Task>
    {
	    string dbms;
        public TaskMap()
        { 
				this.ToTable("Tasks");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Subject).HasColumnName("Subject").IsRequired().HasMaxLength(50).IsUnicode(true);

            this.Property(t => t.DueDate).HasColumnName("DueDate").IsRequired();

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PriorityId).HasColumnName("PriorityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StatusId).HasColumnName("StatusId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TaskTypeId).HasColumnName("TaskTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityObjectTableId).HasColumnName("EntityObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ClosedByUserId).HasColumnName("ClosedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ClosedDate).HasColumnName("ClosedDate");

            this.Property(t => t.IsClosed).HasColumnName("IsClosed").IsRequired();

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled").IsRequired();

            this.Property(t => t.CheckWithId).HasColumnName("CheckWithId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityNumber).HasColumnName("EntityNumber").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.IsAssigned).HasColumnName("IsAssigned").IsRequired();

            this.Property(t => t.StartDate).HasColumnName("StartDate");
        }
    }
}
	 