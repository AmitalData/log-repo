using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ValidationTypeMap : EntityTypeConfiguration<ValidationType>
    {
        public ValidationTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(true);

            this.Property(t => t.ClassName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Field)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ValidationTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Field).HasColumnName("Field");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
