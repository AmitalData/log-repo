using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class QuoteGroupSectionMap : EntityTypeConfiguration<QuoteGroupSection>
    {
        public QuoteGroupSectionMap()
        {
            this.ToTable("QuoteGroupSections");

            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(40).IsUnicode(false);
        }
    }
}
