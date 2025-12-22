using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoiceLineMap : EntityTypeConfiguration<ARInvoiceLine>
    {
        public ARInvoiceLineMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ChargesTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ForiegnCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ARInvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);            
            this.Property(t => t.ReceivableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MeasurementId).HasMaxLength(15).IsUnicode(false);            
            this.Property(t => t.EntityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreditAccount).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Description).IsRequired().HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.LocalDescription).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.GLAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LineActionCode).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.VatPercentage).IsOptional();
            this.Property(t => t.PrepaidCollectId).HasMaxLength(1).IsUnicode(false);

            this.ToTable("ARInvoiceLines");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId");
            this.Property(t => t.ForiegnCurrencyId).HasColumnName("ForiegnCurrencyId");
            this.Property(t => t.ForiegnCurrencyAmount).HasColumnName("ForiegnCurrencyAmount").IsRequired();
            this.Property(t => t.LocalCurrencyAmount).HasColumnName("LocalCurrencyAmount").IsRequired();
            this.Property(t => t.InvoiceCurrencyAmount).HasColumnName("InvoiceCurrencyAmount").IsRequired();
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.ARInvoiceId).HasColumnName("ARInvoiceId");
            this.Property(t => t.ForiegnExchangeRate).HasColumnName("ForiegnExchangeRate").IsRequired();
            this.Property(t => t.LineNumber).HasColumnName("LineNumber");
            this.Property(t => t.ReceivableId).HasColumnName("ReceivableId");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.MeasurementId).HasColumnName("MeasurementId");
            this.Property(t => t.IsExchangeRateFixed).HasColumnName("IsExchangeRateFixed");           
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ProfitCurrencyAmount).HasColumnName("ProfitCurrencyAmount").IsRequired();
            this.Property(t => t.VatPercentage).HasColumnName("VatPercentage");
            this.Property(t => t.ExchangeRateDate).HasColumnName("ExchangeRateDate");
            this.Property(t => t.CreditAccount).HasColumnName("CreditAccount");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.LocalDescription).HasColumnName("LocalDescription");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.DateForInterest).HasColumnName("DateForInterest");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId");
            this.Property(t => t.LineActionCode).HasColumnName("LineActionCode");
            this.Property(t => t.IsBackToBack).HasColumnName("IsBackToBack");
            this.Property(t => t.IsExpense).HasColumnName("IsExpense");
            this.Property(t => t.PrepaidCollectId).HasColumnName("PrepaidCollectId");
            this.Property(t => t.IsRegionalTax).HasColumnName("IsRegionalTax");
            this.Property(t => t.ReceivableCreditGLAccountId).HasColumnName("ReceivableCreditGLAccountId");

            // Relationships
            this.HasRequired(t => t.ChargesType).WithMany().HasForeignKey(d => d.ChargesTypeId);
            this.HasRequired(t => t.Currency).WithMany().HasForeignKey(d => d.ForiegnCurrencyId);
            this.HasRequired(t => t.ARInvoice).WithMany().HasForeignKey(d => d.ARInvoiceId);
            this.HasOptional(t => t.Measurement).WithMany().HasForeignKey(d => d.MeasurementId);
            this.HasRequired(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
            this.HasOptional(t => t.ARInvoiceLineAction).WithMany().HasForeignKey(d => d.LineActionCode);
            this.HasOptional(t => t.PrepaidCollect).WithMany().HasForeignKey(d => d.PrepaidCollectId);
        }
    }
}
