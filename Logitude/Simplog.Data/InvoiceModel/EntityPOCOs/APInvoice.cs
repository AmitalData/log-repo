using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class APInvoice
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InternalNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string VendorId { get; set; }
        public string VATNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string PaymentTermId { get; set; }
        public DateTime? DueDate { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string LocalCurrencyId { get; set; }
        public string InternalNotes { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }
        public double? SubTotalInInvoiceCurrency { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string StatusCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public bool IsClosed { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string MainEntityId { get; set; }
        public string MainEntityReference { get; set; }
        public string SearchFields { get; set; }
        public double? AmountDue { get; set; }
        public double? RefundAmount { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }
        public string BranchId { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public string Description { get; set; }
        public string CreditAccount { get; set; }
        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string AccountingExternalCode { get; set; }
        public string PaymentTermExternalId { get; set; }
        public string TransferStatusCode { get; set; }
        public bool IsMultipleEntities { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public DateTime? OperationalDate { get; set; }
        public DateTime? AccountingDate { get; set; }
        public bool IsExternalEntity { get; set; }
        public bool IsGeneralInvoice { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public string ExternalAccountingEntityId { get; set; }
        [ForeignKey("TransferStatusCode")]
        public virtual APInvoiceTransferStatus TransferStatus { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("VendorId")]
        public virtual Card VendorCard { get; set; }

        [ForeignKey("PaymentTermId")]
        public virtual PaymentTerm PaymentTerm { get; set; }

        [ForeignKey("InvoiceCurrencyId")]
        public virtual Currency InvoiceCurrency { get; set; }

        [ForeignKey("LocalCurrencyId")]
        public virtual Currency LocalCurrency { get; set; }

        [ForeignKey("StatusCode")]
        public virtual APInvoiceStatus Status { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ProfitCurrencyId")]
        public virtual Currency ProfitCurrency { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("ApprovedByUserId")]
        public virtual User ApprovedByUser { get; set; }

        public string VendorGLAccountId { get; set; }

        public DateTime? FirstApproveDate { get; set; }

        public string CreatedByPartner { get; set; }

        public bool TotalVATOnly { get; set; }

        public DateTime? PaidDate { get; set; }

        public string ShipmentsNumbers { get; set; }

        public string MasterNumbers { get; set; }
        public string MasterShipmentNumbers { get; set; }
        public string HouseNumbers { get; set; }
        public string GlobalTaxCalculation { get; set; }

        [ForeignKey("GlobalTaxCalculation")]
        public virtual QBOGlobalTaxCalculation QBOGlobalTaxCalculation { get; set; }
        public string ConcurrencyGUID { get; set; }
    }
}