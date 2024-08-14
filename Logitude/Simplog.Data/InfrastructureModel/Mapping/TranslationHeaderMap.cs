using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
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
