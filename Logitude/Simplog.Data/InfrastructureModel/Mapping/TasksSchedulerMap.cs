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
    public class TasksSchedulerMap : EntityTypeConfiguration<TasksScheduler>
    {
        public TasksSchedulerMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Tenant).IsRequired();
            

            this.Property(t => t.Id)
                 .IsRequired()
                 .HasMaxLength(15)
                 .IsUnicode(false);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UpdatedBy)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Description)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(t => t.LastRunResult)
                .HasMaxLength(150)
                .IsUnicode(false);

            this.Property(t => t.ServiceClassName)
           .HasMaxLength(100)
           .IsUnicode(false);

            this.Property(t => t.TriggerType)
                .HasMaxLength(1)
                .IsUnicode(false);


            this.Property(t => t.Type)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SchedulerDetailsXML)
                 .IsMaxLength()
                 .IsUnicode(true);
            this.Property(t => t.RepeatInMinutes).IsOptional();
            this.Property(t => t.Status)
              .HasMaxLength(25)
              .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("TasksScheduler");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");

            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Friday).HasColumnName("Friday");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsLastRunError).HasColumnName("IsLastRunError");

            this.Property(t => t.LastRunResult).HasColumnName("LastRunResult");
            this.Property(t => t.LastRunTime).HasColumnName("LastRunTime");
            this.Property(t => t.Monday).HasColumnName("Monday");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.NextRunTime).HasColumnName("NextRunTime");
            this.Property(t => t.RepeatInMinutes).HasColumnName("RepeatInMinutes");
            this.Property(t => t.Satarday).HasColumnName("Satarday");

            this.Property(t => t.ServiceClassName).HasColumnName("ServiceClassName");
            this.Property(t => t.StartDateTime).HasColumnName("StartDateTime");
            this.Property(t => t.Sunday).HasColumnName("Sunday");
            this.Property(t => t.Thursday).HasColumnName("Thursday");
            this.Property(t => t.TriggerType).HasColumnName("TriggerType");
            this.Property(t => t.Tuesday).HasColumnName("Tuesday");
            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");

            this.Property(t => t.Wednesday).HasColumnName("Wednesday");
            this.Property(t => t.MonthlyDay).HasColumnName("MonthlyDay");

            this.Property(t => t.SchedulerDetailsXML).HasColumnName("SchedulerDetailsXML");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.NextRunTimeUTC).HasColumnName("NextRunTimeUTC");
            this.Property(t => t.LastRunTimeUTC).HasColumnName("LastRunTimeUTC");
            this.Property(t => t.StartDateTimeUTC).HasColumnName("StartDateTimeUTC");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Retries).HasColumnName("Retries");


        }
    }
}
