using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    class DigitalInvoicesCounterDataViewMap : EntityTypeConfiguration<DigitalInvoicesCounterDataView>
    {
        public DigitalInvoicesCounterDataViewMap()
        {
            // Primary Key
            this.HasKey(t => new {t.Tenant, t.PartnerId, t.BillToId});

            this.Property(t => t.PartnerId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.BillToId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DigitalInvoicesCounterDataView");
            this.Property(t => t.PartnerId).HasColumnName("PartnerId");
            this.Property(t => t.BillToId).HasColumnName("BillToId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.MaxOpenAmount).HasColumnName("MaxOpenAmount");
            this.Property(t => t.MinOpenAmount).HasColumnName("MinOpenAmount");
            this.Property(t => t.MaxTotalAmount).HasColumnName("MaxTotalAmount");
            this.Property(t => t.MinTotalAmount).HasColumnName("MinTotalAmount");
        }
    }
}
