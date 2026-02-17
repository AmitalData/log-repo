using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class TranslationMap : EntityTypeConfiguration<Translation>
    {
        public TranslationMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TranslationHeaderCode).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TranslatedText).IsRequired().HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.TextCodeId).IsRequired().HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.TranslatedTextPlural).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.TranslatedByUserId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Translations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TranslationHeaderCode).HasColumnName("TranslationHeaderCode");
            this.Property(t => t.TranslatedText).HasColumnName("TranslatedText");
            this.Property(t => t.TextCodeId).HasColumnName("TextCodeId");
            this.Property(t => t.TranslatedTextPlural).HasColumnName("TranslatedTextPlural");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TranslatedByUserId).HasColumnName("TranslatedByUserId");
            this.Property(t => t.TranslateDate).HasColumnName("TranslateDate");
            this.Property(t => t.UpdateDateGMT).HasColumnName("UpdateDateGMT");

            this.HasOptional(t => t.TranslatedByUser).WithMany().HasForeignKey(d => d.TranslatedByUserId);
        }
    }
}
