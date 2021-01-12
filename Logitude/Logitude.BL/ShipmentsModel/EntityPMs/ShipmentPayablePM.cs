using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.Validators;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(ShipmentPayableValidator), "IsShipmentPayableValid")]
    public class ShipmentPayablePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ChargesGroupCode { get; set; }
        public bool? IsExpenseCharge { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentPayableLineStatusCode { get; set; }
        public string ShipmentPayableLineStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
        public string MeasurementShortName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Quantity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? UnitPrice { get; set; }
                
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ExpectedAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ExpectedAmountLocal { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool AWBPrint { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Rate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdateByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime ValueDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DueTypeCode { get; set; }
        public string DueTypeName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? MinAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? MaxAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ExpectedAmountInProfitCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ProfitCurrencyExchangeRate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentPayableParentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsEditedByUser { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AccountedAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AccountedAmountInLocalCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AccountedAmountInProfitCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? OpenAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? OpenAmountInLocalCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? OpenAmountInProfitCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentPayableAmountTypeCode { get; set; }
        public string ShipmentPayableAmountTypeName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? CorrectionAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CorrectionByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CorrectionNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CorrectionDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IATACodeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsFromQuote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? QuoteCostMinAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? QuoteCostMaxAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteChargeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsChargeBySteps { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatTypeId { get; set; }

        // Dummy Fields
        public int ViewOrder { get; set; }
        
        public string UOMPercentage { get; set; }

        List<ShipmentPayablePM> childShipmentPayables;
        [Include]
        [Association("ShipmentPayableChildShipmentPayable", "Id", "ShipmentPayableParentId")]
        [Composition]
        public List<ShipmentPayablePM> ChildShipmentPayables
        {
            get 
            {
                if (childShipmentPayables == null)
                {
                    childShipmentPayables = new List<ShipmentPayablePM>();
                }
                return childShipmentPayables;
            }
            set { childShipmentPayables = value; }
        }

        public ChangeSetOperation ChangeSetOp { get; set; }
        public ChangeSetOperation ChildChangeOp { get; set; }
        public List<ShipmentPayablePM> ChildShipmentPayablesChangeSet { get; set; }


        // Dummy to show in Grid Columns (inside Payables) + Map it to Invoice line
        public string ShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsBackToBack { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivableId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TariffId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TariffNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int TariffVersion { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TariffLineId { get; set; }

        public bool PayablesDisconnectedFromTariff { get; set; }
    }
}
