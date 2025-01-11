using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class TranslationHeaderMap : EntityTypeConfiguration<TranslationHeader>
    {
        public TranslationHeaderMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Description)
                .HasMaxLength(250)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TranslationHeaders");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
