using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{    
    public class ARInvoice
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }      
        public string InvoiceNumber { get; set; }
        public string VatNumber { get; set; }     
        public DateTime? InvoiceDate { get; set; }       
        public DateTime? DueDate { get; set; }
        public DateTime? PrintDate { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }        
        public double? SubTotalInInvoiceCurrency { get; set; }       
        public double? AmountInLocalCurrency { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }       
        public bool IsAutoCredit { get; set; }        
        public bool IsCancelled { get; set; }
        public string CancelledByARInvoiceId { get; set; }       
        public string InternalNotes { get; set; }
        public string PrintNotes { get; set; }        
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string DraftNumber { get; set; }
        public bool IsInvoiceNumberManuallySet { get; set; }
        public bool Sent { get; set; }
        public string SearchFields { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string MainEntityReference { get; set; }
        public double? AmountDue { get; set; }
        public string MainEntityId { get; set; }
        public string CustomerRef { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public bool IsClosed { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsPrinted { get; set; }
        public string Description { get; set; }
        public string DebitAccount { get; set; }
        public DateTime? ExpectedPaymentDate { get; set; }
        public string PaymentTermExternalId { get; set; }
        public bool IsConstituentInvoice { get; set; }
        public bool IsConsolidationInvoice { get; set; }
        public string ConsolidationInvoiceId { get; set; }
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
        public int TransferTries { get; set; }	
        public string TransferError { get; set; }        
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string AccountingExternalCode { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string CreditedByARInvoiceId { get; set; }
        public DateTime? OperationalDate { get; set; }
        public DateTime? DateForVATInterest { get; set; }
        public bool SplitJournalByCurrency { get; set; }
        public bool IsExternalEntity { get; set; }
        public bool IsGeneralInvoice { get; set; }
        public string SATPaymentMethodCode { get; set; }

        public string SATXML { get; set;}
        public string SATAdditionalFieldsXML { get; set; }
        public string RelatedInvoice { get; set; }

        public string SalesmanUserId { get; set; }

        public string ExternalAccountingEntityId { get; set; }

        public bool Intercompany { get; set; }

        public bool IsMultiCurrency { get; set; }

        public Decimal? TotalAmountForTaxReport { get; set; }
        public Decimal? TotaVatableAmountForTaxReport { get; set; }
        public Decimal TotalVAT { get; set; }

        public DateTime? SATApprovalDate { get; set; }

        public bool IsFullAccounting { get; set; }

        [ForeignKey("SalesmanUserId")]
        public virtual User SalesmanUser { get; set; }
        
        public string TransmissionError { get; set; }

        public string MetodoPagoCode { get; set; }
        [ForeignKey("MetodoPagoCode")]
        public virtual MetodoPago MetodoPago { get; set; }

        public string UsoCFDICode { get; set; }

        [ForeignKey("UsoCFDICode")]
        public virtual UsoCFDI UsoCFDI { get; set; }

        [ForeignKey("ProfitCurrencyId")]
        public virtual Currency ProfitCurrency { get; set; }
        public string ProfitCurrencyId { get; set; }

        [ForeignKey("PrepaidCollectId")]
        public virtual PrepaidCollect PrepaidCollect { get; set; }
        public string PrepaidCollectId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }
        public string CreatedByUserId { get; set; }

        [ForeignKey("PaymentTermId")]
        public virtual PaymentTerm PaymentTerm { get; set; }
        public string PaymentTermId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        public string BranchId { get; set; }

        [ForeignKey("ARInvoiceTypeCode")]
        public virtual ARInvoiceType ARInvoiceType { get; set; }
        public string ARInvoiceTypeCode { get; set; }

        [ForeignKey("BillToId")]
        public virtual Card BillTo { get; set; }
        public string BillToId { get; set; }        

        [ForeignKey("BillToAddressId")]
        public virtual Address BillToAddress { get; set; }
        public string BillToAddressId { get; set; }

        [ForeignKey("PrintByUserId")]
        public virtual User PrintByUser { get; set; }
        public string PrintByUserId { get; set; }       

        [ForeignKey("IssuedByUserId")]
        public virtual User IssuedByUser { get; set; }
        public string IssuedByUserId { get; set; }      

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }

        [ForeignKey("InvoiceCurrencyId")]
        public virtual Currency InvoiceCurrency { get; set; }
        public string InvoiceCurrencyId { get; set; }      

        [ForeignKey("LocalCurrencyId")]
        public virtual Currency LocalCurrency { get; set; }
        public string LocalCurrencyId { get; set; }      

        [ForeignKey("StatusCode")]
        public virtual ARInvoiceStatus Status { get; set; }

        [ForeignKey("TransferStatusCode")]
        public virtual ARInvoiceTransferStatus TransferStatus { get; set; }
        public string StatusCode { get; set; }       

        [ForeignKey("ApprovedByUserId")]
        public virtual User ApprovedByUser { get; set; }
        public string ApprovedByUserId { get; set; }

        public bool IsCustomsChargesOnly { get; set; }

        public string SATTransferStatusCode { get; set; }
        [ForeignKey("SATTransferStatusCode")]
        public virtual SATTransferStatus SATTransferStatus { get; set; }

        public string SATInvoiceStatusCode { get; set; }
        [ForeignKey("SATInvoiceStatusCode")]
        public virtual SATInvoiceStatus SATInvoiceStatus { get; set; }

        public virtual ARInvoice CancelledByARInvoice { get; set; }
        public virtual List<ARInvoice> CancelledARInvoices { get; set; }

        public virtual ARInvoice CreditedByARInvoice { get; set; }
        public virtual List<ARInvoice> CreditedARInvoices { get; set; }

        public string BankAccountLiteId { get; set; }
        [ForeignKey("BankAccountLiteId")]
        public virtual BankAccountLite BankAccountLite { get; set; }

        public string ARInvoiceStockId { get; set; }
        public bool IsInvoiceNumberFromStock { get; set; }


        public string ConcurrencyGUID { get; set; }
    }
}