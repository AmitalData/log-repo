using Simplog.Server.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.DataContracts;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class APInvoiceLinePM
    {
        [Key]
        public string APInvoiceId { get; set; }
        [Key]
        public int LineNumber { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? InvoiceCurrencyAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatTypeId { get; set; }
        public double? VatPercentage { get; set; }
        public double? VatRecognizedPercentage { get; set; }
        public string VatTypeName { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public bool VatIsMultiPercentage { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VendorId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? OpenAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ForiegnCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ForiegnCurrencyAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ForiegnExchangeRate { get; set; }

        public double? LocalCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }     
        public string EntityId { get; set; }
        public string EntityPayableId { get; set; }
        public double? RefundAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargesTypeName { get; set; }

        public string ObjectTableId { get; set; }
        public string EntityReference { get; set; }       
        public string VendorName { get; set; }                    
        public double? OtherInvoicesAmounts { get; set; }
        public double? ExpectedAmount { get; set; }        
        public double? CorrectionAmount { get; set; }
        public string CorrectionNote { get; set; }
        public string CorrectionByUserId { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public string AmountTypeCode { get; set; }
        public string ForiegnCurrencyCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }  

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DebitAccount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalDescription { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargeTypeGLAccountId { get; set; }
        public bool AuthorizedSignatory { get; set; }

        public string PrepaidCollectId { get; set; }

        public string ContainerTypeId { get; set; }
        public string ContainerTypeCode { get; set; }
        public int? Quantity { get; set; }
    }
}
