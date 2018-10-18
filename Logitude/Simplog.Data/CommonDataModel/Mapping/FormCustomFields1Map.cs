using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FormCustomFields1Map : EntityTypeConfiguration<FormCustomField>
    {
        public FormCustomFields1Map()
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

            this.Property(t => t.EntityId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FieldCode)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("FormCustomFields1");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.FieldCode).HasColumnName("FieldCode");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");

            // Relationships
            this.HasRequired(t => t.DocumentType)
                .WithMany()
                .HasForeignKey(d => d.DocumentTypeId);
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.FormCustomFields)
            //    .HasForeignKey(d => d.ObjectTableId)
            //    .WillCascadeOnDelete(false);

        }
    }
}
