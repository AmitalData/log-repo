using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteCostChargePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteId { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ChargesGroupCode { get; set; }
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
        public string CostMeasurementId { get; set; }
        public string CostMeasurementCode { get; set; }
        public string CostMeasurementShortName { get; set; }
        public double? CostQuantity { get; set; }
        public double? CostUnitPrice { get; set; }
        public double? CostTotalAmount { get; set; }
        public double? CostTotalAmountLocal { get; set; }
        public double? CostContainerType1UnitPrice { get; set; }
        public double? CostContainerType2UnitPrice { get; set; }
        public double? CostContainerType3UnitPrice { get; set; }
        public double? CostContainerType4UnitPrice { get; set; }
        public double? CostContainerType5UnitPrice { get; set; }
        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }
        public string VatTypeName { get; set; }
        public string UOMPercentage { get; set; }
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
    }
}
