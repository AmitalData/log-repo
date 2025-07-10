using Simplog.Server.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using AmitalCloud.Shipment.Def.Validators;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
//using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(ShipmentReceivableValidator), "IsShipmentReceivableValid")]
    public partial class ShipmentReceivablePM : EntityPM      //: ChildEntitiesCustomFieldPM
    {
        [Key]
        public string Id { get; set; }
        //public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ChargesGroupCode { get; set; }
        public bool? IsExpenseCharge { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipmentReceivableLineStatusCode { get; set; }
        public string ShipmentReceivableLineStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
        public string MeasurementShortName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Quantity { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? UnitPrice { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? TotalAmount { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? TotalAmountLocal { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool AWBPrint { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Rate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public decimal PayableLocal { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string UpdateByUserId { get; set; }
        public string UpdateByUserName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string PrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string DueTypeCode { get; set; }
        public string DueTypeName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ARInvoiceLineId { get; set; }

        public int ViewOrder { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsFromQuote { get; set; }
        public bool IsFixedPrice { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsExchangeRateFixed { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? AmountInProfitCurrency { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? ProfitCurrencyExchangeRate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ARInvoiceId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string IATACodeId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? QuoteSaleMinAmount { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? QuoteSaleMaxAmount { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string QuoteChargeId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsChargeBySteps { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string VatTypeId { get; set; }

        // Dummy to show in Grid Columns (inside Payables) + Map it to Invoice line
        public string ShipmentNumber { get; set; }
        public string UOMPercentage { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsBackToBack { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsExpense { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipmentReceivableParentId { get; set; }

        //List<ShipmentReceivablePM> childShipmentReceivables;
        [Include]
        [Association("ShipmentReceivableChildShipmentReceivable", "Id", "ShipmentReceivableParentId")]
        [Composition]
        public List<ShipmentReceivablePM> ChildShipmentReceivables
        {
            get
            {
                if (childShipmentReceivables == null)
                {
                    childShipmentReceivables = new List<ShipmentReceivablePM>();
                }

                return childShipmentReceivables;
            }

            set
            {
                childShipmentReceivables = value;
            }
        }

        //public ChangeSetOperation ChangeSetOp { get; set; }
        public ChangeSetOperation ChildChangeOp { get; set; }
        public List<ShipmentReceivablePM> ChildShipmentReceivablesChangeSet { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? VatAmountLocal { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? VatAmountProfit { get; set; }
        public string ChangeSet { get; set; }
        public string PayableVendorId { get; set; }
    }
}
