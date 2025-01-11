using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class PerformanceLogMap : EntityTypeConfiguration<PerformanceLog>
    {
        public PerformanceLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(t => t.ModelName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.MethodName)
                .IsRequired()
                .HasMaxLength(65)
                .IsUnicode(false);

            this.Property(t => t.UserIP)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MethodParameters)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            


            // Table & Column Mappings
            this.ToTable("PerformanceLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LogDateTimeGMT).HasColumnName("LogDateTimeGMT");
            this.Property(t => t.LogDateTimeLocal).HasColumnName("LogDateTimeLocal");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.ModelName).HasColumnName("ModelName");
            this.Property(t => t.MethodName).HasColumnName("MethodName");
            this.Property(t => t.MonitoringService).HasColumnName("MonitoringService");
            this.Property(t => t.ExecutionTime).HasColumnName("ExecutionTime");
            this.Property(t => t.UserIP).HasColumnName("UserIP");
            this.Property(t => t.MethodParameters).HasColumnName("MethodParameters");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ServerTime).HasColumnName("ServerTime");
            
        }
    }
}
