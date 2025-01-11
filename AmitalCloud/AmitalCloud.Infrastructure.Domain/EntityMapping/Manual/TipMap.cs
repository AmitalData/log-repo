using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class TipMap : EntityTypeConfiguration<Tip>
    {
        public TipMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.ShortTextCode)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ShortTextCodeCode)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Tips");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.VisibilityDefaultValue).HasColumnName("VisibilityDefaultValue");
            this.Property(t => t.ShortTextCode).HasColumnName("ShortTextCode");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.ShortTextCodeCode).HasColumnName("ShortTextCodeCode");

            // Relationships
            this.HasRequired(t => t.ObjectTable)
                .WithMany()
                .HasForeignKey(d => d.ObjectTableId);
            this.HasRequired(t => t.TextCode)
                .WithMany()
                .HasForeignKey(d => d.ShortTextCode);

        }
    }
}
