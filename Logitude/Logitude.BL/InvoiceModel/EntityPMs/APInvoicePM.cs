using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class APInvoicePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }
        public string InternalNumber { get; set; }

        public bool CreatedFromAPI { get; set; }
        public string ShipmentTransportModeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InvoiceNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VendorId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VATNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? InvoiceDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentTermId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DueDate { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InvoiceCurrencyId { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string InternalNotes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? SubTotalInLocalCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? SubTotalInInvoiceCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AmountInInvoiceCurrency { get; set; }
        public double? AmountInInvoiceCurrency_Summary { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AmountInLocalCurrency { get; set; }
        public double? AmountInLocalCurrency_Summary { get; set; }

        public double? AmountInProfitCurrency { get; set; }
        public double? AmountInProfitCurrency_Summary { get; set; }

        public string StatusCode { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public bool IsClosed { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }      
        public double? InvoiceExpectedAmount { get; set; }
        public string MainEntityId { get; set; }
        public string MainEntityReference { get; set; }
        public string SearchFields { get; set; }
        public string VendorName { get; set; }
        public string VendorLocalName { get; set; }
        public string VendorCode { get; set; }
        public string VendorType { get; set; }
        public string PaymentTermName { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string StatusName { get; set; }
        public string CreatedByUserName { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public string UpdatedByUserName { get; set; }
        public string APInvoiceTypeName { get; set; }
        public double? AmountDue { get; set; }
        public double? RefundAmount { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string HouseNumber {get; set;}
        public string MasterNumber {get; set;}
        public string MainEntityMasterShipmentNumbers { get; set; }
        public string Description {get; set;}
        public string VendorPartnerTypeId { get; set; }
        public string CreditAccount { get; set; }
        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferStatusName { get; set; }
        public string AccountingExternalCode { get; set; }
        public string AccountingExternalName { get; set; }
        public string PaymentTermExternalId { get; set; }
        public bool ReadyForTransfer { get; set; }
        public bool IsTransferStatusSetManually { get; set; }
        public bool IsMultipleEntities { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public string ApprovedByUserName { get; set; }
        public string VendorContactId { get; set; }
        public string ExternalAccountingEntityId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? OperationalDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VendorGLAccountId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? AccountingDate { get; set; }
        public bool IsExternalEntity { get; set; }

        public bool IsGeneralInvoice { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstApproveDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field1 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field2 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field3 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field4 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field5 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field6 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field7 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field8 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field9 { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field10 { get; set; }

        public string ShipmentsNumbers { get; set; }
        public string MasterNumbers { get; set; }
        public string MasterShipmentNumbers { get; set; }
        public string HouseNumbers { get; set; }

        private List<APInvoiceLinePM> invoiceLines;
        [Include]
        [Composition]
        [Association("APInvoiceAPInvoiceLines", "Id", "APInvoiceId")]
        public virtual List<APInvoiceLinePM> InvoiceLines
        {
            get
            {
                if (invoiceLines == null)
                {
                    invoiceLines = new List<APInvoiceLinePM>();
                }

                return this.invoiceLines;
            }
            set
            {
                if (value != null)
                {
                    invoiceLines = value;
                }
            }
        }

        List<APInvoiceEntityPM> invoiceEntities;
        [Include]
        [Composition]
        [Association("APInvoiceAPInvoiceEntities", "Id", "APInvoiceId")]
        public virtual List<APInvoiceEntityPM> InvoiceEntities
        {
            get
            {
                if (invoiceEntities == null)
                {
                    invoiceEntities = new List<APInvoiceEntityPM>();
                }

                return this.invoiceEntities;
            }
            set
            {
                if (value != null)
                {
                    invoiceEntities = value;
                }
            }
        }
        
        List<APInvoicePaymentPM> invoicePayments;
        [Include]
        [Composition]
        [Association("APInvoiceInvoicePayments", "Id", "APInvoiceId")]
        public virtual List<APInvoicePaymentPM> InvoicePayments
        {
            get
            {
                if (invoicePayments == null)
                {
                    invoicePayments = new List<APInvoicePaymentPM>();
                }

                return this.invoicePayments;
            }
            set
            {
                if (value != null)
                {
                    invoicePayments = value;
                }
            }
        }

        List<APInvoiceMultipleShipmentPM> invoiceMultipleShipments;
        [Include]
        [Composition]
        [Association("APInvoiceAPInvoiceMultipleShipments", "Id", "APInvoiceId")]
        public virtual List<APInvoiceMultipleShipmentPM> InvoiceMultipleShipments
        {
            get
            {
                if (invoiceMultipleShipments == null)
                {
                    invoiceMultipleShipments = new List<APInvoiceMultipleShipmentPM>();
                }

                return this.invoiceMultipleShipments;
            }

            set
            {
                if (value != null)
                {
                    invoiceMultipleShipments = value;
                }
            }
        }


        List<APTransferLinePM> transferLines;
        [Include]
        [Composition]
        [Association("APInvoiceAPTransferLines", "Id", "APInvoiceId")]
        public virtual List<APTransferLinePM> TransferLines
        {
            get
            {
                if (transferLines == null)
                {
                    transferLines = new List<APTransferLinePM>();
                }

                return this.transferLines;
            }

            set
            {
                if (value != null)
                {
                    transferLines = value;
                }
            }
        }

        private List<APInvoiceTotalVATPM> totalVATs;
        [Include]
        [Composition]
        [Association("APInvoiceTotalVATAPInvoice", "Id", "APInvoiceId")]
        public virtual List<APInvoiceTotalVATPM> TotalVATs
        {
            get
            {
                if (totalVATs == null)
                {
                    totalVATs = new List<APInvoiceTotalVATPM>();
                }

                return this.totalVATs;
            }

            set
            {
                if (value != null)
                {
                    totalVATs = value;
                }
            }
        }

        //Dummy Fields
        public bool SetVoided { get; set; }
        public bool SetApproved { get; set; }
        public bool SetCancelApproval { get; set; }
        public bool SetReTransfer { get; set; }
        public bool SetReSendQBO { get; set; }

        public string JournalNumber { get; set; }
        public string JournalId { get; set; }
        public string ShipmentConcurrencyGUID { get; set; }
        public string ShipmentNewConcurrencyGUID { get; set; }

        public string VendorCity { get; set; }
        public string VendorCountry { get; set; }

        public string CreatedByPartner { get; set; }
        public bool TotalVATOnly { get; set; }
        public string VendorVatNumber { get; set; }

        public DateTime? PaidDate { get; set; }
        public Boolean IsNew { get; set; }
        public Boolean IsCopied { get; set; }
        public string CopiedFrom { get; set; }
        public string GlobalTaxCalculation { get; set; }
        public bool IsUpdateFromPaymentService { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConcurrencyGUID { get; set; }
        public string NewConcurrencyGUID { get; set; }
        public bool IsEquipment { get; set; }
        public string ConnectedPaymentsNumbers { get; set; }
    }
}
