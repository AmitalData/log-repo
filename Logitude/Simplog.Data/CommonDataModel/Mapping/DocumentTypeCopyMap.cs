using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentTypeCopyMap : EntityTypeConfiguration<DocumentTypeCopy>
    {
        public DocumentTypeCopyMap()
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
                .HasMaxLength(7)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DocumentTypeCopies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
            this.Property(t => t.IsSelectedByDefault).HasColumnName("IsSelectedByDefault");
            this.Property(t => t.InActive).HasColumnName("InActive");

            // Relationships
            this.HasRequired(t => t.DocumentType)
                .WithMany()
                .HasForeignKey(d => d.DocumentTypeId);

        }
    }
}
