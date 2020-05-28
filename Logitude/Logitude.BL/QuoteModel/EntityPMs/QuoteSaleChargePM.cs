using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteSaleChargePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteId { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ChargesTypeDescription { get; set; }
        public string ChargesGroupCode { get; set; }
        public string ChargesTypeLocalName { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public double? SaleExchangeRate { get; set; }
        public string MarkUpTypeCode { get; set; }
        public double? MarkUpValue { get; set; }
        public string Notes { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ValueDate { get; set; }        
        public string QuoteTypeCode { get; set; }        
        public string IsAllIN { get; set; }
        public string FixedAmountCode { get; set; }
        public string ForeignAmountFixed { get; set; }
        public string LocalAmountFixed { get; set; }
        public string ContainerType1MarkUpTypeCode { get; set; }
        public string ContainerType2MarkUpTypeCode { get; set; }
        public string ContainerType3MarkUpTypeCode { get; set; }
        public string ContainerType4MarkUpTypeCode { get; set; }
        public string ContainerType5MarkUpTypeCode { get; set; }
        public double? ContainerType1MarkUpValue { get; set; }
        public double? ContainerType2MarkUpValue { get; set; }
        public double? ContainerType3MarkUpValue { get; set; }
        public double? ContainerType4MarkUpValue { get; set; }
        public double? ContainerType5MarkUpValue { get; set; }
        public string SaleMeasurementId { get; set; }
        public string SaleMeasurementCode { get; set; }
        public string SaleMeasurementShortName { get; set; }
        public string SaleMeasurementLocalName { get; set; }        
        public double? SaleQuantity { get; set; }
        public double? SaleUnitPrice { get; set; }
        public double? SaleTotalAmount { get; set; }
        public double? SaleTotalAmountLocal { get; set; }
        public double? SaleContainerType1UnitPrice { get; set; }
        public double? SaleContainerType2UnitPrice { get; set; }
        public double? SaleContainerType3UnitPrice { get; set; }
        public double? SaleContainerType4UnitPrice { get; set; }
        public double? SaleContainerType5UnitPrice { get; set; }
        public string PriceBreaks { get; set; }
        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }
        public string VatTypeName { get; set; }
        public double? VatAmount { get; set; }
        public string UOMPercentage { get; set; }
        public double? SaleUnitPriceInSaleCurrency { get; set; }
        public double? SaleUnitPrice1InSaleCurrency { get; set; }
        public double? SaleUnitPrice2InSaleCurrency { get; set; }
        public double? SaleUnitPrice3InSaleCurrency { get; set; }
        public double? SaleUnitPrice4InSaleCurrency { get; set; }
        public double? SaleUnitPrice5InSaleCurrency { get; set; }
        public double? SaleAmountInSaleCurrency { get; set; }
        public double? CostMaxAmount { get; set; }
        public double? CostMinAmount { get; set; }
        public double? SaleMinAmount { get; set; }
        public double? SaleMaxAmount { get; set; }

        public string MarkUpText { get; set; }
        public string ContainerType1MarkUpText { get; set; }
        public string ContainerType2MarkUpText { get; set; }
        public string ContainerType3MarkUpText { get; set; }
        public string ContainerType4MarkUpText { get; set; }
        public string ContainerType5MarkUpText { get; set; }


        public int ChargesGroupViewOrder { get; set; }
        public int ChargesTypeViewOrder { get; set; }
        public string ChargesGroupName { get; set; }
        public bool IsChargeBySteps { get; set; }
        


    }
}
