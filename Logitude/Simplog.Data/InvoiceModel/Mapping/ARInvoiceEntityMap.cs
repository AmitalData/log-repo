using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoiceEntityMap : EntityTypeConfiguration<ARInvoiceEntity>
    {
        public ARInvoiceEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EntityId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ARInvoiceId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EntityReference)
                .HasMaxLength(20)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ARInvoiceEntities");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ARInvoiceId).HasColumnName("ARInvoiceId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.EntityReference).HasColumnName("EntityReference");

            // Relationships
            this.HasRequired(t => t.ARInvoice)
                .WithMany()
                .HasForeignKey(d => d.ARInvoiceId);
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.ARInvoiceEntities)
            //    .HasForeignKey(d => d.ObjectTableId);

        }
    }
}
