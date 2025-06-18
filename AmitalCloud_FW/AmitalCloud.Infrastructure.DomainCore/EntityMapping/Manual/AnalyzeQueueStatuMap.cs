using AmitalCloud.Infrastructure.Domain.EntityClasses;
using System.Data.Entity.ModelConfiguration;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class AnalyzeQueueStatuMap : EntityTypeConfiguration<AnalyzeQueueStatus>
    {
        public AnalyzeQueueStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("AnalyzeQueueStatus");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
