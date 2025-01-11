using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class CustomPickListMap : EntityTypeConfiguration<CustomPickList>
    {
        public CustomPickListMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(100)
                 .IsUnicode(true);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.IsMultipleChoice)
                .IsRequired();
             
            

            // Table & Column Mappings
            this.ToTable("CustomPickLists");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Value).HasColumnName("Value");

            this.Property(t => t.IsMultipleChoice).HasColumnName("IsMultipleChoice");
        }
    }
}
