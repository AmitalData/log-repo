using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Invoice.Domain.EntityPOCOs;

namespace AmitalCloud.Invoice.Domain.EntityMapping
{
    public class APInvoiceEntityMap : EntityTypeConfiguration<APInvoiceEntity>
    {
        public APInvoiceEntityMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EntityId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EntityReference).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.APInvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("APInvoiceEntities");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityReference).HasColumnName("EntityReference");
            this.Property(t => t.APInvoiceId).HasColumnName("APInvoiceId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");

            this.HasRequired(t => t.APInvoice).WithMany().HasForeignKey(d => d.APInvoiceId);
        }
    }
}
