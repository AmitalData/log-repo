using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class SearchIndexEditHistoryMap : EntityTypeConfiguration<SearchIndexEditHistory>
    {
        public SearchIndexEditHistoryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.CreateDate).IsRequired();
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Screen).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.ScreenParam).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Entname).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.KeyVal).IsRequired().HasMaxLength(1000).IsUnicode(true);

            this.ToTable("SearchIndexEditHistories");
            this.Property(t => t.Id).HasColumnName(columnName: "Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.Screen).HasColumnName("Screen");
            this.Property(t => t.ScreenParam).HasColumnName("ScreenParam");
            this.Property(t => t.Entname).HasColumnName("Entname");
            this.Property(t => t.KeyVal).HasColumnName("KeyVal");
        }
    }
}
