using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ConvertProgramInfoMap : EntityTypeConfiguration<ConvertProgramInfo>
    {
        public ConvertProgramInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MethodName)
                .IsRequired()
                .HasMaxLength(65)
                .IsUnicode(false);

            this.Property(t => t.GlobalDBId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ConvertProgramInfoes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MethodName).HasColumnName("MethodName");
            this.Property(t => t.IsApplied).HasColumnName("IsApplied");
            this.Property(t => t.GlobalDBId).HasColumnName("GlobalDBId");

            // Relationships
            this.HasRequired(t => t.GlobalDB)
                .WithMany()
                .HasForeignKey(d => d.GlobalDBId);

        }
    }
}
