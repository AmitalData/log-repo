using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuotePriceStepMap : EntityTypeConfiguration<QuotePriceSteps>
    {
        public QuotePriceStepMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuoteId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuoteChargeId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("QuotePriceSteps");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SaleUnitPrice).HasColumnName("SaleUnitPrice");
            this.Property(t => t.Step).HasColumnName("Step");
            this.Property(t => t.CostUnitPrice).HasColumnName("CostUnitPrice");
            this.Property(t => t.MarkupValue).HasColumnName("MarkupValue");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
            this.Property(t => t.QuoteChargeId).HasColumnName("QuoteChargeId");

            this.HasRequired(t => t.Quote).WithMany().HasForeignKey(d => d.QuoteId);
            this.HasOptional(t => t.QuoteCharge).WithMany().HasForeignKey(d => d.QuoteChargeId);
        }
    }
}
