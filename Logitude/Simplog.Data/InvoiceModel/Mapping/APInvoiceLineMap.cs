using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class APInvoiceLineMap : EntityTypeConfiguration<APInvoiceLine>
    {
        public APInvoiceLineMap()
        {            
            this.HasKey(t => new { t.APInvoiceId, t.LineNumber });
            this.Property(t => t.APInvoiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LineNumber).HasDatabaseGeneratedOption(null);
            this.Property(t => t.ChargesTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.EntityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EntityPayableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ForiegnCurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DebitAccount).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Description).IsRequired().HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.LocalDescription).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.ChargeTypeGLAccountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatPercentage).IsOptional();
            this.Property(t => t.PrepaidCollectId).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ContainerTypeId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("APInvoiceLines");
            this.Property(t => t.APInvoiceId).HasColumnName("APInvoiceId");
            this.Property(t => t.LineNumber).HasColumnName("LineNumber");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId");
            this.Property(t => t.InvoiceCurrencyAmount).HasColumnName("InvoiceCurrencyAmount").IsRequired();
            this.Property(t => t.LocalCurrencyAmount).HasColumnName("LocalCurrencyAmount").IsRequired();
            this.Property(t => t.ProfitCurrencyAmount).HasColumnName("ProfitCurrencyAmount").IsRequired();
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityPayableId).HasColumnName("EntityPayableId");
            this.Property(t => t.RefundAmount).HasColumnName("RefundAmount");
            this.Property(t => t.VatPercentage).HasColumnName("VatPercentage");
            this.Property(t => t.ForiegnCurrencyId).HasColumnName("ForiegnCurrencyId");
            this.Property(t => t.ForiegnCurrencyAmount).HasColumnName("ForiegnCurrencyAmount");
            this.Property(t => t.ForiegnExchangeRate).HasColumnName("ForiegnExchangeRate");
            this.Property(t => t.DebitAccount).HasColumnName("DebitAccount");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.LocalDescription).HasColumnName("LocalDescription");
            this.Property(t => t.VatAmount).HasColumnName("VatAmount");
            this.Property(t => t.ChargeTypeGLAccountId).HasColumnName("ChargeTypeGLAccountId");
            this.Property(t => t.AuthorizedSignatory).HasColumnName("AuthorizedSignatory");
            this.Property(t => t.PrepaidCollectId).HasColumnName("PrepaidCollectId");
            this.Property(t => t.ContainerTypeId).HasColumnName("ContainerTypeId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");

            this.HasRequired(t => t.APInvoice).WithMany().HasForeignKey(d => d.APInvoiceId);
            this.HasRequired(t => t.ChargesType).WithMany().HasForeignKey(d => d.ChargesTypeId);
            this.HasRequired(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
            this.HasOptional(t => t.Currency).WithMany().HasForeignKey(d => d.ForiegnCurrencyId);
            this.HasOptional(t => t.PrepaidCollect).WithMany().HasForeignKey(d => d.PrepaidCollectId);
            this.HasOptional(t => t.ContainerType).WithMany().HasForeignKey(d => d.ContainerTypeId);
        }
    }
}
