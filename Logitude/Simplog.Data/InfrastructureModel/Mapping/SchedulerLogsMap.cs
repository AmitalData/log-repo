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
    public class SchedulerLogsMap : EntityTypeConfiguration<SchedulerLogs>
    {
        public SchedulerLogsMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Tenant).IsRequired();
            

            this.Property(t => t.Id)
                 .IsRequired()
                 .HasMaxLength(15)
                 .IsUnicode(false);

            this.Property(t => t.Log)
                .IsUnicode(false);

            this.Property(t => t.HistoryId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

          
            // Table & Column Mappings
            this.ToTable("SchedulerLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.Log).HasColumnName("Log"); 
            this.Property(t => t.HistoryId).HasColumnName("HistoryId"); 

            this.HasRequired(t => t.TaskSchedulerHistory).WithMany()
               .HasForeignKey(d => d.HistoryId)
               .WillCascadeOnDelete(false);  
        }
    }
}
