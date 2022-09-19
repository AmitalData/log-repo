using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuoteCharge
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }       
        public string QuoteId { get; set; }                      
        public string ChargesTypeId { get; set; }
        public string VendorId { get; set; }
        public string Notes { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ValueDate { get; set; }
        public bool IsAllIN { get; set; }
        public bool IsChargeBySteps { get; set; }
        public double? CostMinAmount { get; set; }
        public double? CostMaxAmount { get; set; }
        public double? SaleMinAmount { get; set; }
        public double? SaleMaxAmount { get; set; }

        // Cost        
        public string CostCurrencyId { get; set; }
        public double? CostExchangeRate { get; set; }
        public bool CostIsFixedRate { get; set; }
        public string CostMeasurementId { get; set; }
        public double? CostQuantity { get; set; }
        public double? CostUnitPrice { get; set; }
        public double? CostContainerType1UnitPrice { get; set; }
        public double? CostContainerType2UnitPrice { get; set; }
        public double? CostContainerType3UnitPrice { get; set; }
        public double? CostContainerType4UnitPrice { get; set; }
        public double? CostContainerType5UnitPrice { get; set; }
        public double? CostTotalAmount { get; set; }
        public double? CostTotalAmountLocal { get; set; }
        public double? CostAmountInSaleCurrency { get; set; }
        public bool IsCostAllIn { get; set; }
        public string TariffId { get; set; }
        public string TariffNumber { get; set; }
        public string TariffLineId { get; set; }
        public int TariffVersion { get; set; }
        public string SaleTariffId { get; set; }
        public string SaleTariffNumber { get; set; }
        public string SaleTariffLineId { get; set; }
        public int SaleTariffVersion { get; set; }

        // Sale                
        public string SaleCurrencyId { get; set; }
        public double? SaleExchangeRate { get; set; }
        public bool SaleIsFixedRate { get; set; }
        public string SaleMeasurementId { get; set; }
        public double? SaleQuantity { get; set; }        
        public double? SaleUnitPrice { get; set; }
        public double? SaleContainerType1UnitPrice { get; set; }
        public double? SaleContainerType2UnitPrice { get; set; }
        public double? SaleContainerType3UnitPrice { get; set; }
        public double? SaleContainerType4UnitPrice { get; set; }
        public double? SaleContainerType5UnitPrice { get; set; }
        public double? SaleTotalAmount { get; set; }
        public double? SaleTotalAmountLocal { get; set; }

        // Sale Fields in Sale Currency
        public double? SaleUnitPriceInSaleCurrency { get; set; }
        public double? SaleUnitPrice1InSaleCurrency { get; set; }
        public double? SaleUnitPrice2InSaleCurrency { get; set; }
        public double? SaleUnitPrice3InSaleCurrency { get; set; }
        public double? SaleUnitPrice4InSaleCurrency { get; set; }
        public double? SaleUnitPrice5InSaleCurrency { get; set; }
        public double? SaleAmountInSaleCurrency { get; set; }

        // Markup
        public double? MarkUpValue { get; set; }
        public string MarkUpTypeCode { get; set; }
        public double? ContainerType1MarkUpValue { get; set; }
        public double? ContainerType2MarkUpValue { get; set; }
        public double? ContainerType3MarkUpValue { get; set; }
        public double? ContainerType4MarkUpValue { get; set; }
        public double? ContainerType5MarkUpValue { get; set; }
        public string ContainerType1MarkUpTypeCode { get; set; }
        public string ContainerType2MarkUpTypeCode { get; set; }
        public string ContainerType3MarkUpTypeCode { get; set; }
        public string ContainerType4MarkUpTypeCode { get; set; }
        public string ContainerType5MarkUpTypeCode { get; set; }

        public string MarkUpCurrencyId { get; set; }

        [ForeignKey("MarkUpCurrencyId")]
        public virtual Currency MarkUpCurrency { get; set; }

        [ForeignKey("QuoteId")]
        public virtual Quote Quote { get; set; }

        [ForeignKey("ChargesTypeId")]
        public virtual ChargesType ChargesType { get; set; }

        [ForeignKey("SaleMeasurementId")]
        public virtual Measurement SaleMeasurement { get; set; }

        [ForeignKey("CostMeasurementId")]
        public virtual Measurement CostMeasurement { get; set; }

        [ForeignKey("SaleCurrencyId")]
        public virtual Currency Currency { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("ContainerType1MarkUpTypeCode")]
        public virtual MarkUpType ContainerType1MarkUpType { get; set; }

        [ForeignKey("ContainerType2MarkUpTypeCode")]
        public virtual MarkUpType ContainerType2MarkUpType { get; set; }

        [ForeignKey("ContainerType3MarkUpTypeCode")]
        public virtual MarkUpType ContainerType3MarkUpType { get; set; }

        [ForeignKey("ContainerType4MarkUpTypeCode")]
        public virtual MarkUpType ContainerType4MarkUpType { get; set; }

        [ForeignKey("ContainerType5MarkUpTypeCode")]
        public virtual MarkUpType ContainerType5MarkUpType { get; set; }

        [ForeignKey("MarkUpTypeCode")]
        public virtual MarkUpType MarkUpType { get; set; }

        [ForeignKey("VendorId")]
        public virtual Card VendorCard { get; set; }

        [ForeignKey("CostCurrencyId")]
        public virtual Currency CostCurrency { get; set; }

        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }
        public bool IsRegionalTax { get; set; }

        public double? VATAmountInLocalCurrency { get; set; }
        public double? VATAmountInQuoteSaleCurrency { get; set; }
        public double? VATAmountInLineSaleCurrency { get; set; }
        public double? SaleTotalAmountLocalIncludingVAT { get; set; }
        public double? SaleAmountInSaleCurrencyIncludingVAT { get; set; }
        public double? SaleTotalAmountIncludingVAT { get; set; }
    }
}