using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentTypeCustomFields1Map : EntityTypeConfiguration<DocumentTypeCustomField>
    {
        public DocumentTypeCustomFields1Map()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FieldCode)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.FieldDataTypeCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(t => t.DefaultValue)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DocumentTypeCustomFields1");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.FieldCode).HasColumnName("FieldCode");
            this.Property(t => t.FieldDataTypeCode).HasColumnName("FieldDataTypeCode");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsRequired).HasColumnName("IsRequired");
            this.Property(t => t.DefaultValue).HasColumnName("DefaultValue");
            this.Property(t => t.MultiLine).HasColumnName("MultiLine");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");

            // Relationships
            //this.HasRequired(t => t.FieldDataType)
            //    .WithMany(t => t.DocumentTypeCustomFields)
            //    .HasForeignKey(d => d.FieldDataTypeCode);
            this.HasRequired(t => t.DocumentType)
                .WithMany()
                .HasForeignKey(d => d.DocumentTypeId);

        }
    }
}
