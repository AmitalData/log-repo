using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentOutMap : EntityTypeConfiguration<DocumentOut>
    {
        public DocumentOutMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            //this.Property(t => t.Note)
            //    .HasMaxLength(250)
            //    .IsUnicode(true);

            //this.Property(t => t.EntityId)
            //    .IsRequired()
            //    .HasMaxLength(15)
            //    .IsUnicode(false);

            this.Property(t => t.IssuedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            //this.Property(t => t.DocumentTypeId)
            //    .IsRequired()
            //    .HasMaxLength(15)
            //    .IsUnicode(false);

            //this.Property(t => t.ObjectTableId)
            //    .IsRequired()
            //    .HasMaxLength(15)
            //    .IsUnicode(false);

            //this.Property(t => t.ChildEntityId)
            //    .HasMaxLength(15)
            //    .IsUnicode(false);

            //this.Property(t => t.ChildEntityReference)
            //    .HasMaxLength(20)
            //    .IsUnicode(false);

            this.Property(t => t.EmailTemplateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentTemplateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.XamlDocumentId)
                .HasMaxLength(15)
                .IsUnicode(false);

            //this.Ignore(d => d.IsCopy);
            // Table & Column Mappings
            this.ToTable("DocumentOuts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsBlobExist).HasColumnName("IsBlobExist");
            this.Property(t => t.Issued).HasColumnName("Issued");
            this.Property(t => t.EditableFields).HasColumnName("EditableFields");
            this.Property(t => t.EmailTemplateId).HasColumnName("EmailTemplateId");
            this.Property(t => t.DocumentTemplateId).HasColumnName("DocumentTemplateId");
            this.Property(t => t.XamlDocumentId).HasColumnName("XamlDocumentId");
            this.Property(t => t.NeedsRebuild).HasColumnName("NeedsRebuild");

            this.HasRequired(t => t.DocumentsFiling).WithOptional(t => t.DocumentOut);

            this.HasOptional(t => t.XamlDocument)
                .WithMany()
                .HasForeignKey(d => d.XamlDocumentId);
            this.Property(t => t.IssuedDate).HasColumnName("IssuedDate");
            //this.Property(t => t.Note).HasColumnName("Note");
            //this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.IssuedByUserId).HasColumnName("IssuedByUserId");
            //this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
           
            //this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            //this.Property(t => t.ChildEntityId).HasColumnName("ChildEntityId");
            //this.Property(t => t.ChildEntityReference).HasColumnName("ChildEntityReference");
           
            //this.Property(t => t.IsDuplex).HasColumnName("IsDuplex");

            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.DocOuts)
            //    .HasForeignKey(d => d.ObjectTableId)
            //    .WillCascadeOnDelete(false);

            //this.HasRequired(t => t.DocumentType)
            //    .WithMany()
            //    .HasForeignKey(d => d.DocumentTypeId)
            //    .WillCascadeOnDelete(false);
            this.HasOptional(t => t.IssuedByUser)
                .WithMany()
                .HasForeignKey(d => d.IssuedByUserId)
                .WillCascadeOnDelete(false);

        }
    }
}
