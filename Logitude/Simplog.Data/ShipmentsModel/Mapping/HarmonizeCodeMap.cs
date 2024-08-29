using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class HarmonizeCodeMap : EntityTypeConfiguration<HarmonizeCode>
    {
        public HarmonizeCodeMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(6).IsUnicode(false);
            this.Property(t => t.ChapterCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.SubChapterCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Description).IsRequired().HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.ChapterDescription).IsRequired().HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.SubChapterDescription).IsRequired().HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.SearchFields).IsRequired().HasMaxLength(4000).IsUnicode(true);

            this.ToTable("HarmonizeCodes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ChapterCode).HasColumnName("ChapterCode");
            this.Property(t => t.ChapterDescription).HasColumnName("ChapterDescription");
            this.Property(t => t.SubChapterCode).HasColumnName("SubChapterCode");
            this.Property(t => t.SubChapterDescription).HasColumnName("SubChapterDescription");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
