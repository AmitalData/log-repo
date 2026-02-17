using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoicePaymentMap : EntityTypeConfiguration<ARInvoicePayment>
    {
        public ARInvoicePaymentMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARPaymentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARInvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ForeignCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("ARInvoicePayments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount");
            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount");
            this.Property(t => t.PaymentAmount).HasColumnName("PaymentAmount");
            this.Property(t => t.ARPaymentId).HasColumnName("ARPaymentId");
            this.Property(t => t.ARInvoiceId).HasColumnName("ARInvoiceId");
            this.Property(t => t.ForeignCurrencyId).HasColumnName("ForeignCurrencyId");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");

            this.HasRequired(t => t.ARPayment).WithMany().HasForeignKey(d => d.ARPaymentId);
            this.HasRequired(t => t.ARInvoice).WithMany().HasForeignKey(d => d.ARInvoiceId);
            this.HasRequired(t => t.ForeignCurrency).WithMany().HasForeignKey(d => d.ForeignCurrencyId);

        }
    }
}
