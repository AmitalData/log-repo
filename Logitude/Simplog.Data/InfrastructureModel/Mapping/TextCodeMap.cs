using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class TextCodeMap : EntityTypeConfiguration<TextCode>
    {
        public TextCodeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Code).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DefaultText).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TextCodeTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Id).IsRequired().HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.DefaultTextPlural).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.SpellCheckedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LocalDefaultText).HasMaxLength(500).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("TextCodes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.DefaultText).HasColumnName("DefaultText");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.TextCodeTypeCode).HasColumnName("TextCodeTypeCode");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DefaultTextPlural).HasColumnName("DefaultTextPlural");
            this.Property(t => t.IsSpellChecked).HasColumnName("IsSpellChecked");
            this.Property(t => t.SpellCheckDate).HasColumnName("SpellCheckDate");
            this.Property(t => t.SpellCheckedByUserId).HasColumnName("SpellCheckedByUserId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.LocalDefaultText).HasColumnName("LocalDefaultText");

            // Relationships
            this.HasRequired(t => t.ObjectTable).WithMany().HasForeignKey(d => d.ObjectTableId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.SpellCheckedByUser).WithMany().HasForeignKey(d => d.SpellCheckedByUserId);
        }
    }
}
