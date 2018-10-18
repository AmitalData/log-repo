using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class APInvoicePaymentMap : EntityTypeConfiguration<APInvoicePayment>
    {
        public APInvoicePaymentMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.APPaymentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.APInvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ForeignCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("APInvoicePayments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount");
            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount");
            this.Property(t => t.PaymentAmount).HasColumnName("PaymentAmount");
            this.Property(t => t.APPaymentId).HasColumnName("APPaymentId");
            this.Property(t => t.APInvoiceId).HasColumnName("APInvoiceId");
            this.Property(t => t.ForeignCurrencyId).HasColumnName("ForeignCurrencyId");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");

            this.HasRequired(t => t.APInvoice).WithMany().HasForeignKey(d => d.APInvoiceId);
            this.HasRequired(t => t.APPayment).WithMany().HasForeignKey(d => d.APPaymentId);
            this.HasRequired(t => t.ForeignCurrency).WithMany().HasForeignKey(d => d.ForeignCurrencyId);
        }
    }
}
