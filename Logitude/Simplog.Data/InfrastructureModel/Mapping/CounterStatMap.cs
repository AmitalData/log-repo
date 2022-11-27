using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class CounterStatMap : EntityTypeConfiguration<CounterStat>
    {
        public CounterStatMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Prefix)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CounterId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.BranchCounterCode)
                .HasMaxLength(5)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CounterStats");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Prefix).HasColumnName("Prefix");
            this.Property(t => t.LastValue).HasColumnName("LastValue");
            this.Property(t => t.CounterId).HasColumnName("CounterId");
            this.Property(t => t.BranchCounterCode).HasColumnName("BranchCounterCode");

            // Relationships
            //this.HasRequired(t => t.Counter)
            //    .WithMany(t => t.CounterStats)
            //    .HasForeignKey(d => d.CounterId);

        }
    }
}
