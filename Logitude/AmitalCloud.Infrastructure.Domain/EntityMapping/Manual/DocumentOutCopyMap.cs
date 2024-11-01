using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class DocumentOutCopyMap : EntityTypeConfiguration<DocumentOutCopy>
    {
        public DocumentOutCopyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentOutId)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeCopyId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.LastPrintedByUserId)
              .HasMaxLength(15)
              .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("DocumentOutCopies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentOutId).HasColumnName("DocumentOutId");
            this.Property(t => t.DocumentTypeCopyId).HasColumnName("DocumentTypeCopyId");
            this.Property(t => t.LastPrintedByUserId).HasColumnName("LastPrintedByUserId");
            this.Property(t => t.LastPrintDate).HasColumnName("LastPrintDate");

            // Relationships
            this.HasOptional(t => t.Document)
                .WithMany()
                .HasForeignKey(d => d.DocumentId);
            this.HasRequired(t => t.DocumentOut)
                .WithMany()
                .HasForeignKey(d => d.DocumentOutId);
            this.HasRequired(t => t.DocumentTypeCopy)
                .WithMany()
                .HasForeignKey(d => d.DocumentTypeCopyId);

        }
    }
}
