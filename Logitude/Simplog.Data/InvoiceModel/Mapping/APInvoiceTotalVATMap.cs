using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class APInvoiceTotalVATMap : EntityTypeConfiguration<APInvoiceTotalVAT>
    {
        public APInvoiceTotalVATMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.APInvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ExternalVATCard).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ExternalTAXItemId).HasMaxLength(25).IsUnicode(false);

            this.ToTable("APInvoiceTotalVATs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InvoiceCurrencyVATAmount).HasColumnName("InvoiceCurrencyVATAmount");
            this.Property(t => t.LocalVATAmount).HasColumnName("LocalVATAmount");
            this.Property(t => t.InvoiceCurrencyVatableAmount).HasColumnName("InvoiceCurrencyVatableAmount");
            this.Property(t => t.LocalVatableAmount).HasColumnName("LocalVatableAmount");
            this.Property(t => t.VatPercent).HasColumnName("VatPercent");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.APInvoiceId).HasColumnName("APInvoiceId");
            this.Property(t => t.ProfitCurrencyVATAmount).HasColumnName("ProfitCurrencyVATAmount");
            this.Property(t => t.ProfitVatableAmount).HasColumnName("ProfitVatableAmount");
            this.Property(t => t.ExternalVATCard).HasColumnName("ExternalVATCard");
            this.Property(t => t.ExternalTAXItemId).HasColumnName("ExternalTAXItemId");

            this.HasRequired(t => t.APInvoice).WithMany().HasForeignKey(d => d.APInvoiceId);
            this.HasRequired(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);

        }
    }
}
