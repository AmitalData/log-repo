using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.Validators;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(QuoteChargesValidator), "IsQuoteChargeValid")]
    public class QuoteChargePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteId { get; set; }
        public double? CostMaxAmount { get; set; }
        public double? CostMinAmount { get; set; }
        public double? SaleMinAmount { get; set; }
        public double? SaleMaxAmount { get; set; }
        public bool IsChargeBySteps { get; set; }
        public bool IsAllIN { get; set; }
        public int ViewOrder { get; set; }

        public double? SaleRatio { get; set; }
        public double? CostRatio { get; set; }




        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime ValueDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ChargesTypeDescription { get; set; }
        public string ChargesTypeLocalName { get; set; }       
        public string ChargesGroupCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CostMeasurementId { get; set; }
        public string CostMeasurementCode { get; set; }
        public string CostMeasurementShortName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SaleMeasurementId { get; set; }
        public string SaleMeasurementCode { get; set; }
        public string SaleMeasurementShortName { get; set; }
        public string SaleMeasurementLocalName { get; set; }

        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CostCurrencyId { get; set; }
        public string CostCurrencyCode { get; set; }
        public double? CostExchangeRate { get; set; }
        public bool CostIsFixedRate { get; set; }
        public double? CostQuantity { get; set; }
        public double? CostUnitPrice { get; set; }
        public double? CostTotalAmount { get; set; }
        public double? CostTotalAmountLocal { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SaleCurrencyId { get; set; }
        public string SaleCurrencyCode { get; set; }
        public double? SaleExchangeRate { get; set; }
        public bool SaleIsFixedRate { get; set; }
        public double? SaleQuantity { get; set; }
        public double? SaleUnitPrice { get; set; }
        public double? SaleTotalAmount { get; set; }
        public double? SaleTotalAmountLocal { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MarkUpTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? MarkUpValue { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerType1MarkUpTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerType2MarkUpTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerType3MarkUpTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerType4MarkUpTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerType5MarkUpTypeCode { get; set; }

        public double? ContainerType1MarkUpValue { get; set; }
        public double? ContainerType2MarkUpValue { get; set; }
        public double? ContainerType3MarkUpValue { get; set; }
        public double? ContainerType4MarkUpValue { get; set; }
        public double? ContainerType5MarkUpValue { get; set; }        

        public double? CostContainerType1UnitPrice { get; set; }
        public double? CostContainerType2UnitPrice { get; set; }
        public double? CostContainerType3UnitPrice { get; set; }
        public double? CostContainerType4UnitPrice { get; set; }
        public double? CostContainerType5UnitPrice { get; set; }

        public double? SaleContainerType1UnitPrice { get; set; }
        public double? SaleContainerType2UnitPrice { get; set; }
        public double? SaleContainerType3UnitPrice { get; set; }
        public double? SaleContainerType4UnitPrice { get; set; }
        public double? SaleContainerType5UnitPrice { get; set; }

        // Sale Fields in Sale Currency
        public double? SaleUnitPriceInSaleCurrency { get; set; }
        public double? SaleUnitPrice1InSaleCurrency { get; set; }
        public double? SaleUnitPrice2InSaleCurrency { get; set; }
        public double? SaleUnitPrice3InSaleCurrency { get; set; }
        public double? SaleUnitPrice4InSaleCurrency { get; set; }
        public double? SaleUnitPrice5InSaleCurrency { get; set; }
        public double? SaleAmountInSaleCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }

        public double? VatAmount { get; set; }
        public string VatTypeName { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public bool VatIsMultiPercentage { get; set; }

        // Dummy
        public double? CostUnitPriceInSaleCurrency { get; set; }       
        public double? CostUnitPrice1InSaleCurrency { get; set; }
        public double? CostUnitPrice2InSaleCurrency { get; set; }
        public double? CostUnitPrice3InSaleCurrency { get; set; }
        public double? CostUnitPrice4InSaleCurrency { get; set; }
        public double? CostUnitPrice5InSaleCurrency { get; set; }
        public double? CostAmountInSaleCurrency { get; set; }

        public bool IsBackToBack { get; set; }

        public string MarkUpText { get; set; }
        public string ContainerType1MarkUpText { get; set; }
        public string ContainerType2MarkUpText { get; set; }
        public string ContainerType3MarkUpText { get; set; }
        public string ContainerType4MarkUpText { get; set; }
        public string ContainerType5MarkUpText { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        private List<QuotePriceStepsPM> quoteChargePriceSteps;
        [Include]
        [Association("QuoteChargeQuotePriceSteps", "Id", "QuoteChargeId")]
        [Composition]
        public virtual List<QuotePriceStepsPM> QuoteChargePriceSteps
        {
            get
            {
                if (this.quoteChargePriceSteps == null)
                {
                    quoteChargePriceSteps = new List<QuotePriceStepsPM>();
                }

                return this.quoteChargePriceSteps;
            }

            set
            {
                if (value != null)
                {
                    quoteChargePriceSteps = value;
                }
            }
        }

        private List<QuotePriceStepsPM> quoteChargePriceStepsChangeSet;
        public List<QuotePriceStepsPM> QuoteChargePriceStepsChangeSet
        {
            get
            {
                if (quoteChargePriceStepsChangeSet == null)
                {
                    quoteChargePriceStepsChangeSet = new List<QuotePriceStepsPM>();
                }

                return quoteChargePriceStepsChangeSet;
            }

            set
            {
                quoteChargePriceStepsChangeSet = value;
            }
        }

        public bool IsCostAllIn { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TariffId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TariffNumber { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TariffLineId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int TariffVersion { get; set; }

        public bool HasPickup { get; set; }
        public bool HasDelivery { get; set; }

        public bool IsRegionalTax { get; set; }
    }
}
