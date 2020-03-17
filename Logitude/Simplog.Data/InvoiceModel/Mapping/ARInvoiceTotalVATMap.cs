using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoiceTotalVATMap : EntityTypeConfiguration<ARInvoiceTotalVAT>
    {
        public ARInvoiceTotalVATMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARInvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ExternalVATCard).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ExternalTAXItemId).HasMaxLength(25).IsUnicode(false);

            this.ToTable("ARInvoiceTotalVATs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.InvoiceCurrencyVATAmount).HasColumnName("InvoiceCurrencyVATAmount").IsRequired();
            this.Property(t => t.LocalVATAmount).HasColumnName("LocalVATAmount").IsRequired();
            this.Property(t => t.InvoiceCurrencyVatableAmount).HasColumnName("InvoiceCurrencyVatableAmount").IsRequired();
            this.Property(t => t.LocalVatableAmount).HasColumnName("LocalVatableAmount").IsRequired();
            this.Property(t => t.VatPercent).HasColumnName("VatPercent").IsRequired();
            this.Property(t => t.ARInvoiceId).HasColumnName("ARInvoiceId");
            this.Property(t => t.ProfitVatableAmount).HasColumnName("ProfitVatableAmount").IsRequired();
            this.Property(t => t.ProfitCurrencyVATAmount).HasColumnName("ProfitCurrencyVATAmount").IsRequired();
            this.Property(t => t.ExternalVATCard).HasColumnName("ExternalVATCard");
            this.Property(t => t.ExternalTAXItemId).HasColumnName("ExternalTAXItemId");
            this.Property(t => t.IsRegionalTax).HasColumnName("IsRegionalTax");

            this.HasRequired(t => t.ARInvoice).WithMany().HasForeignKey(d => d.ARInvoiceId);
            this.HasRequired(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);

        }
    }
}
