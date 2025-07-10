using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ExternalLinkMap : EntityTypeConfiguration<ExternalLink>
    {
        public ExternalLinkMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Ref).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Link).IsRequired().HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.ExpirationDate).IsRequired();
            this.Property(t => t.ActivityLog).IsRequired();
            this.Property(t => t.Params).IsRequired().HasMaxLength(100).IsUnicode(false);

            this.ToTable("ExternalLinks");
            this.Property(t => t.Id).HasColumnName(columnName: "Id");
            this.Property(t => t.Ref).HasColumnName(columnName: "Ref");
            this.Property(t => t.Link).HasColumnName("Link");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.ActivityLog).HasColumnName("ActivityLog");
            this.Property(t => t.Params).HasColumnName("Params");
        }
    }
}
