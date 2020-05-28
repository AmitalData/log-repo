using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class TaskSchedulerHistoryMap : EntityTypeConfiguration<TaskSchedulerHistory>
    {
        public TaskSchedulerHistoryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Tenant).IsRequired();
            

            this.Property(t => t.Id)
                 .IsRequired()
                 .HasMaxLength(15)
                 .IsUnicode(false);

            this.Property(t => t.RunResult) 
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(t => t.TaskId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.LogFirstLine)
               .HasMaxLength(1000)
               .IsUnicode(false);

            this.Property(t => t.LogType)
               .HasMaxLength(200)
               .IsUnicode(false);

            this.Property(t => t.LogDocumentId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TaskSchedulerHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EndDateTime).HasColumnName("EndDateTime");
            this.Property(t => t.IsError).HasColumnName("IsError");

            this.Property(t => t.RunResult).HasColumnName("RunResult");
            this.Property(t => t.StartDateTime).HasColumnName("StartDateTime");
            this.Property(t => t.TaskId).HasColumnName("TaskId");
            this.Property(t => t.StartDateTimeUTC).HasColumnName("StartDateTimeUTC");
            this.Property(t => t.EndDateTimeUTC).HasColumnName("EndDateTimeUTC");
            this.Property(t => t.LogType).HasColumnName("LogType");
            this.Property(t => t.LogFirstLine).HasColumnName("LogFirstLine");
            this.Property(t => t.LogDocumentId).HasColumnName("LogDocumentId");

            this.HasRequired(t => t.TaskScheduler).WithMany()
               .HasForeignKey(d => d.TaskId)
               .WillCascadeOnDelete(false);

            this.HasOptional(t => t.LogDocument).WithMany()
               .HasForeignKey(d => d.LogDocumentId)
               .WillCascadeOnDelete(false);
        }
    }
}
