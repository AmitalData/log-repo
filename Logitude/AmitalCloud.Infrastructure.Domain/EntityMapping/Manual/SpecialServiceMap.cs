using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class SpecialServiceMap : EntityTypeConfiguration<SpecialService>
    {
        public SpecialServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SpecialServiceEnglishName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.SpecialServiceLocalName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.Code)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("SpecialServices");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SpecialServiceEnglishName).HasColumnName("SpecialServiceEnglishName");
            this.Property(t => t.SpecialServiceLocalName).HasColumnName("SpecialServiceLocalName");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.InActive).HasColumnName("InActive");
        }
    }
}
