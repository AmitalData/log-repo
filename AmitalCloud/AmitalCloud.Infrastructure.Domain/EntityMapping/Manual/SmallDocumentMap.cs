using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class SmallDocumentMap : EntityTypeConfiguration<SmallDocument>
    {
        public SmallDocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)//GUID
                .IsUnicode(false);

            this.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(8000)
                .IsUnicode(true);






            // Table & Column Mappings
            this.ToTable("SmallDocuments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Content).HasColumnName("Content");





        }
    }
}
