using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class MarkUpTypeMap : EntityTypeConfiguration<MarkUpType>
    {
        public MarkUpTypeMap()
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
                .HasMaxLength(80)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("MarkUpTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
