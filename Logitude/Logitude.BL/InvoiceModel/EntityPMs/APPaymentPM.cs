using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class APPaymentPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }

        public string PaymentNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintedByUserId { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string ExternalAccountingEntityId { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferError { get; set; }
        public string TransferStatusName { get; set; }
        public bool ReadyForTransfer { get; set; }
        public string AccountingPaymentMethodId { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VendorId { get; set; }
        public string StatusCode { get; set; }
        public bool IsClosed { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        //public string PaymentMethodId { get; set; }
        public string PaymentMethodCode { get; set; }
        public string PrintNotes { get; set; }
        public string InternalNotes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AmountInPaymentCurrency { get; set; }
        public double? PaymentCurrencyExchangeRate { get; set; }
        public DateTime? PaymentCurrencyExchangeRateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VendorAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? RegisterDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? OpenAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChequeOrPaymentRef { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ValueDate { get; set; }
        public string Bank { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankBranch { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Account { get; set; }
        public string SearchFields { get; set; }

        public string CreatedByUserName { get; set; }
        public string VendorName { get; set; }
        public string StatusName { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public string PaymentMethodName { get; set; }
        public string BranchId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CreditCardTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstApproveDate { get; set; }

        private List<APPaymentInvoicePM> paymentInvoices;
        [Include]
        [Composition]
        [Association("APPaymentAPPaymentInvoices", "Id", "APPaymentId")]
        public virtual List<APPaymentInvoicePM> PaymentInvoices
        {
            get
            {
                if (paymentInvoices == null)
                {
                    paymentInvoices = new List<APPaymentInvoicePM>();
                }

                return this.paymentInvoices;
            }
            set
            {
                if (value != null)
                {
                    paymentInvoices = value;
                }
            }
        }

        //Dummy Fields
        public bool SetVoided { get; set; }
        public bool SetApproved { get; set; }
        public bool SetCancelApproval { get; set; }
        public bool HasInvoicesErrors { get; set; }
        public string VendorPartnerTypeId { get; set; }
        public bool SetReSendQBO { get; set; }

        public Decimal? TaxDeductionLocalAmount { get; set; }
        public int? TaxDeductionPercentage { get; set; }
        public string ApprovedByUserId { get; set; }
        public DateTime? ApprovedDateTime { get; set; }

        public string BankAccountId { get; set; }
        public string BranchName { get; set; }

        

    }
}