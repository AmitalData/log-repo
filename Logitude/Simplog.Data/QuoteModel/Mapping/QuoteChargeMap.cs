using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteChargeMap : EntityTypeConfiguration<QuoteCharge>
    {
        public QuoteChargeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.QuoteId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ChargesTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SaleCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SaleMeasurementId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CostMeasurementId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerType1MarkUpTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ContainerType2MarkUpTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ContainerType3MarkUpTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ContainerType4MarkUpTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ContainerType5MarkUpTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.MarkUpTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.VendorId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CostCurrencyId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TariffId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TariffNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.TariffLineId).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.SaleTariffId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SaleTariffNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.SaleTariffLineId).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.MarkUpCurrencyId).HasMaxLength(15).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("QuoteCharges");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SaleQuantity).HasColumnName("SaleQuantity");
            this.Property(t => t.SaleUnitPrice).HasColumnName("SaleUnitPrice");
            this.Property(t => t.SaleTotalAmount).HasColumnName("SaleTotalAmount");
            this.Property(t => t.SaleTotalAmountLocal).HasColumnName("SaleTotalAmountLocal");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.SaleExchangeRate).HasColumnName("SaleExchangeRate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId");
            this.Property(t => t.SaleCurrencyId).HasColumnName("SaleCurrencyId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.SaleMeasurementId).HasColumnName("SaleMeasurementId");
            this.Property(t => t.SaleContainerType1UnitPrice).HasColumnName("SaleContainerType1UnitPrice");
            this.Property(t => t.SaleContainerType2UnitPrice).HasColumnName("SaleContainerType2UnitPrice");
            this.Property(t => t.SaleContainerType3UnitPrice).HasColumnName("SaleContainerType3UnitPrice");
            this.Property(t => t.SaleContainerType4UnitPrice).HasColumnName("SaleContainerType4UnitPrice");
            this.Property(t => t.SaleContainerType5UnitPrice).HasColumnName("SaleContainerType5UnitPrice");
            this.Property(t => t.CostMeasurementId).HasColumnName("CostMeasurementId");
            this.Property(t => t.CostUnitPrice).HasColumnName("CostUnitPrice");
            this.Property(t => t.CostContainerType1UnitPrice).HasColumnName("CostContainerType1UnitPrice");
            this.Property(t => t.CostContainerType2UnitPrice).HasColumnName("CostContainerType2UnitPrice");
            this.Property(t => t.CostContainerType3UnitPrice).HasColumnName("CostContainerType3UnitPrice");
            this.Property(t => t.CostContainerType4UnitPrice).HasColumnName("CostContainerType4UnitPrice");
            this.Property(t => t.CostContainerType5UnitPrice).HasColumnName("CostContainerType5UnitPrice");
            this.Property(t => t.ContainerType1MarkUpTypeCode).HasColumnName("ContainerType1MarkUpTypeCode");
            this.Property(t => t.ContainerType2MarkUpTypeCode).HasColumnName("ContainerType2MarkUpTypeCode");
            this.Property(t => t.ContainerType3MarkUpTypeCode).HasColumnName("ContainerType3MarkUpTypeCode");
            this.Property(t => t.ContainerType4MarkUpTypeCode).HasColumnName("ContainerType4MarkUpTypeCode");
            this.Property(t => t.ContainerType5MarkUpTypeCode).HasColumnName("ContainerType5MarkUpTypeCode");
            this.Property(t => t.ContainerType1MarkUpValue).HasColumnName("ContainerType1MarkUpValue");
            this.Property(t => t.ContainerType2MarkUpValue).HasColumnName("ContainerType2MarkUpValue");
            this.Property(t => t.ContainerType3MarkUpValue).HasColumnName("ContainerType3MarkUpValue");
            this.Property(t => t.ContainerType4MarkUpValue).HasColumnName("ContainerType4MarkUpValue");
            this.Property(t => t.ContainerType5MarkUpValue).HasColumnName("ContainerType5MarkUpValue");
            this.Property(t => t.MarkUpTypeCode).HasColumnName("MarkUpTypeCode");
            this.Property(t => t.MarkUpValue).HasColumnName("MarkUpValue");
            this.Property(t => t.MarkUpCurrencyId).HasColumnName("MarkUpCurrencyId");

            this.Property(t => t.IsAllIN).HasColumnName("IsAllIN");
            this.Property(t => t.VendorId).HasColumnName("VendorId");
            this.Property(t => t.CostQuantity).HasColumnName("CostQuantity");
            this.Property(t => t.CostTotalAmount).HasColumnName("CostTotalAmount");
            this.Property(t => t.CostTotalAmountLocal).HasColumnName("CostTotalAmountLocal");
            this.Property(t => t.SaleIsFixedRate).HasColumnName("SaleIsFixedRate");
            this.Property(t => t.CostExchangeRate).HasColumnName("CostExchangeRate").IsRequired();
            this.Property(t => t.CostIsFixedRate).HasColumnName("CostIsFixedRate");
            this.Property(t => t.CostCurrencyId).HasColumnName("CostCurrencyId");
            this.Property(t => t.CostMaxAmount).HasColumnName("CostMaxAmount");
            this.Property(t => t.CostMinAmount).HasColumnName("CostMinAmount");
            this.Property(t => t.SaleMinAmount).HasColumnName("SaleMinAmount");
            this.Property(t => t.SaleMaxAmount).HasColumnName("SaleMaxAmount");
            this.Property(t => t.IsChargeBySteps).HasColumnName("IsChargeBySteps");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.VatPercentage).HasColumnName("VatPercentage");
            this.Property(t => t.SaleUnitPriceInSaleCurrency).HasColumnName("SaleUnitPriceInSaleCurrency");
            this.Property(t => t.SaleUnitPrice1InSaleCurrency).HasColumnName("SaleUnitPrice1InSaleCurrency");
            this.Property(t => t.SaleUnitPrice2InSaleCurrency).HasColumnName("SaleUnitPrice2InSaleCurrency");
            this.Property(t => t.SaleUnitPrice3InSaleCurrency).HasColumnName("SaleUnitPrice3InSaleCurrency");
            this.Property(t => t.SaleUnitPrice4InSaleCurrency).HasColumnName("SaleUnitPrice4InSaleCurrency");
            this.Property(t => t.SaleUnitPrice5InSaleCurrency).HasColumnName("SaleUnitPrice5InSaleCurrency");
            this.Property(t => t.SaleAmountInSaleCurrency).HasColumnName("SaleAmountInSaleCurrency");
            this.Property(t => t.IsCostAllIn).HasColumnName("IsCostAllIn");
            this.Property(t => t.TariffNumber).HasColumnName("TariffNumber");
            this.Property(t => t.TariffLineId).HasColumnName("TariffLineId");
            this.Property(t => t.TariffId).HasColumnName("TariffId");
            this.Property(t => t.TariffVersion).HasColumnName("TariffVersion");
            this.Property(t => t.SaleTariffNumber).HasColumnName("SaleTariffNumber");
            this.Property(t => t.SaleTariffLineId).HasColumnName("SaleTariffLineId");
            this.Property(t => t.SaleTariffId).HasColumnName("SaleTariffId");
            this.Property(t => t.SaleTariffVersion).HasColumnName("SaleTariffVersion");
            this.Property(t => t.IsRegionalTax).HasColumnName("IsRegionalTax");
            this.Property(t => t.VATAmountInLocalCurrency).HasColumnName("VATAmountInLocalCurrency");
            this.Property(t => t.VATAmountInQuoteSaleCurrency).HasColumnName("VATAmountInQuoteSaleCurrency");
            this.Property(t => t.VATAmountInLineSaleCurrency).HasColumnName("VATAmountInLineSaleCurrency");
            this.Property(t => t.SaleTotalAmountLocalIncludingVAT).HasColumnName("SaleTotalAmountLocalIncludingVAT");
            this.Property(t => t.SaleAmountInSaleCurrencyIncludingVAT).HasColumnName("SaleAmountInSaleCurrencyIncludingVAT");
            this.Property(t => t.SaleTotalAmountIncludingVAT).HasColumnName("SaleTotalAmountIncludingVAT");

            // Relationships
            this.HasOptional(t => t.VendorCard).WithMany().HasForeignKey(d => d.VendorId);
            this.HasRequired(t => t.ChargesType).WithMany().HasForeignKey(d => d.ChargesTypeId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.CostCurrency).WithMany().HasForeignKey(d => d.CostCurrencyId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Currency).WithMany().HasForeignKey(d => d.SaleCurrencyId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.ContainerType1MarkUpType).WithMany().HasForeignKey(d => d.ContainerType1MarkUpTypeCode);
            this.HasOptional(t => t.ContainerType2MarkUpType).WithMany().HasForeignKey(d => d.ContainerType2MarkUpTypeCode);
            this.HasOptional(t => t.ContainerType3MarkUpType).WithMany().HasForeignKey(d => d.ContainerType3MarkUpTypeCode);
            this.HasOptional(t => t.ContainerType4MarkUpType).WithMany().HasForeignKey(d => d.ContainerType4MarkUpTypeCode);
            this.HasOptional(t => t.ContainerType5MarkUpType).WithMany().HasForeignKey(d => d.ContainerType5MarkUpTypeCode);
            this.HasOptional(t => t.MarkUpType).WithMany().HasForeignKey(d => d.MarkUpTypeCode);
            this.HasRequired(t => t.CostMeasurement).WithMany().HasForeignKey(d => d.CostMeasurementId);
            this.HasRequired(t => t.SaleMeasurement).WithMany().HasForeignKey(d => d.SaleMeasurementId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Quote).WithMany().HasForeignKey(d => d.QuoteId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.VatType).WithMany().HasForeignKey(d => d.VatTypeId);
            this.HasOptional(t => t.MarkUpCurrency).WithMany().HasForeignKey(d => d.MarkUpCurrencyId).WillCascadeOnDelete(false);
        }
    }
}
