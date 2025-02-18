using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ExternalLinkMap : EntityTypeConfiguration<ExternalLink>
    {
        public ExternalLinkMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Ref).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Link).HasMaxLength(100).IsUnicode(true);

            this.ToTable("ExternalLinks");
            this.Property(t => t.Id).HasColumnName(columnName: "Id");
            this.Property(t => t.Ref).HasColumnName(columnName: "Ref");
            this.Property(t => t.Link).HasColumnName("Link");
        }
    }
}
