using Simplog.Server.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.DataContracts;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ARInvoiceLinePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ARInvoiceId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ForiegnCurrencyId { get; set; }
        public string ForiegnCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ForiegnCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? InvoiceCurrencyAmount { get; set; }        

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }

        public double? VatAmount { get; set; }
        public string VatTypeName { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public bool VatIsMultiPercentage { get; set; }

        public DateTime? ExchangeRateDate { get; set; }
        public double? ForiegnExchangeRate { get; set; }
        public int LineNumber { get; set; }
        public string ReceivableId { get; set; }                
        public string PrepaidCollectId { get; set; } //dummy field
        public string MeasurementCode { get; set; } //dummy field

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Quantity { get; set; } //dummy field

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? UnitPrice { get; set; } //dummy field

        public int ViewOrder { get; set; }
        public string MeasurementId { get; set; }
        public bool IsExchangeRateFixed { get; set; }
        public string EntityId { get; set; }
        //public string ObjectTableId { get; set; }//dummy field
        public string EntityReference { get; set; }//dummy field
        public double? ProfitCurrencyAmount { get; set; }
        public string InvoiceLocalCurrencyCode { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreditAccount { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DateForInterest { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ValueDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string GLAccountId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LineActionCode { get; set; }

        public bool IsCustomsCharge { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsBackToBack { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsExpense { get; set; }
    }
}
