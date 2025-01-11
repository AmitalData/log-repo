using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class DocumentMap : EntityTypeConfiguration<Document>
    {
        public DocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Extension)
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(t => t.Folder)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.FileName)

               .HasMaxLength(120)
               .IsUnicode(true);

            this.Property(t => t.SmallDocumentId)
             .IsOptional()
             .HasMaxLength(15)
             .IsUnicode(false);

            this.Property(t => t.CalculatedFileName)
                .HasMaxLength(120)
                .IsUnicode(true);

            this.Property(t => t.MarkForDelete).IsOptional();

            // Table & Column Mappings
            this.ToTable("Documents");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.Extension).HasColumnName("Extension");
            this.Property(t => t.FileSize).HasColumnName("FileSize");
            this.Property(t => t.HasFile).HasColumnName("HasFile");
            this.Property(t => t.Folder).HasColumnName("Folder");
            this.Property(t => t.SmallDocumentId).HasColumnName("SmallDocumentId");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.CalculatedFileName).HasColumnName("CalculatedFileName");
            this.Property(t => t.IsEncrypted).HasColumnName("IsEncrypted");
            this.Property(t => t.MarkForDelete).HasColumnName("MarkForDelete");
            //relationships

            this.HasOptional(t => t.SmallDocument)
               .WithMany()
               .HasForeignKey(d => d.SmallDocumentId);
        }
    }
}
