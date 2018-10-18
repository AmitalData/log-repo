using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{    
    public class QuoteTotalVATMap : EntityTypeConfiguration<QuoteTotalVAT>
    {
        public QuoteTotalVATMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuoteId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ExternalVATCard).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ExternalTAXItemId).HasMaxLength(25).IsUnicode(false);

            this.ToTable("QuoteTotalVATs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.VatPercent).HasColumnName("VatPercent");
            this.Property(t => t.ExternalVATCard).HasColumnName("ExternalVATCard");
            this.Property(t => t.ExternalTAXItemId).HasColumnName("ExternalTAXItemId");
            this.Property(t => t.QuoteCurrencyVATAmount).HasColumnName("QuoteCurrencyVATAmount");
            this.Property(t => t.QuoteCurrencyVatableAmount).HasColumnName("QuoteCurrencyVatableAmount");
            this.Property(t => t.LocalCurrencyVATAmount).HasColumnName("LocalCurrencyVATAmount");
            this.Property(t => t.LocalCurrencyVatableAmount).HasColumnName("LocalCurrencyVatableAmount");
            this.Property(t => t.ProfitCurrencyVatableAmount).HasColumnName("ProfitCurrencyVatableAmount");
            this.Property(t => t.ProfitCurrencyVATAmount).HasColumnName("ProfitCurrencyVATAmount");

            this.HasRequired(t => t.Quote).WithMany().HasForeignKey(d => d.QuoteId);
            this.HasRequired(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
        }
    }
}
