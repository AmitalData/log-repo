using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ARInvoiceList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InvoiceNumber { get; set; }
        public string ARInvoiceTypeCode { get; set; }
        public string ARInvoiceTypeName { get; set; }
        public string BillToPartnerId { get; set; }
        public string BillToPartnerName { get; set; }       
        public string BillToId { get; set; }
        public string BillToName { get; set; }
        public string BillToLocalName { get; set; }
        public string BillToCode { get; set; }
        public string BillToAddressId { get; set; }
        public string VatNumber { get; set; }       
        public DateTime? InvoiceDate { get; set; }       
        public DateTime? DueDate { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintByUserId { get; set; }
        public string PrintByUserName { get; set; }
        public string IssuedByUserId { get; set; }
        public string IssuedByUserName { get; set; }       
        public string InvoiceCurrencyId { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }
        public double? SubTotalInInvoiceCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }       
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public bool IsAutoCredit { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsFromInterestBatchInvoice { get; set; }
        public bool HasDoc { get; set; }

        public string CancelledByARInvoiceId { get; set; }
        public string InternalNotes { get; set; }
        public string PrintNotes { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string PrepaidCollectId { get; set; }
        public string DraftNumber { get; set; }
        public bool IsInvoiceNumberManuallySet { get; set; }
        public bool Sent { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string SearchFields { get; set; }
        public string MainEntityReference { get; set; }
        public double? AmountDue { get; set; }
        public double? AmountPaid { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }
        public string MainEntityId { get; set; }
        public string MasterEntityId { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; }
        public string ProfitCurrencyId { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public DateTime? ExpectedPaymentDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public bool IsPrinted { get; set; }
        public string PaymentTermExternalId { get; set; }
        public bool IsConstituentInvoice { get; set; }
        public bool IsConsolidationInvoice { get; set; }
        public string ConsolidationInvoiceId { get; set; }
        public string CustomerRef { get; set; }
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
        public string DebitAccount { get; set; }
        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferStatusName { get; set; }
        public string AccountingExternalCode { get; set; }
        public bool ReadyForTransfer { get; set; }
        public bool IsDueDateColorRed { get; set; }
        public bool IsDigitalDueDateColorRed { get; set; }
        public bool IsExpectedPaymentDateColorRed { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public string ApprovedByUserName { get; set; }
        public DateTime? OperationalDate { get; set; }
        public DateTime? DateForInterest { get; set; }
        public bool SplitJournalByCurrency { get; set; }
        public bool IsExternalEntity { get; set; }
        public bool IsGeneralInvoice { get; set; }
        public string SATPaymentMethodCode { get; set; }
        public string SalesmanUserId { get; set; }
        public string SalesmanUserName { get; set; }
        public bool IsCustomsChargesOnly { get; set; }
        public string RelatedInvoice { get; set; }
        public bool IsCustomsInvoice { get; set; }
        public string CreditedByARInvoiceId { get; set; }
        public string CreditedByARInvoiceTypeCode { get; set; }
        //public string MetodoPagoCode { get; set; }
        public string MetodoPagoCode { get; set; }
        public string UsoCFDICode { get; set; }
        public string PeriodCode { get; set; }
        public string RegimenFiscalCode { get; set; }
        public string Period { get; set; }
        public string SATTransferStatusCode { get; set; }
        public string SATTransferStatusName { get; set; }
        public string TransmissionError { get; set; }

        public string SATInvoiceStatusCode { get; set; }
        public string SATInvoiceStatusName { get; set; }
        public bool Intercompany { get; set; }
        public string BankAccountLiteId { get; set; }
        public bool HasInterestFeature { get; set; }

        public bool IsMultiCurrency { get; set; }

        public Decimal? TotalAmountForTaxReport { get; set; }
        public Decimal? TotaVatableAmountForTaxReport { get; set; }
        public Decimal? TotalVAT { get; set; }
        public DateTime? SATApprovalDate { get; set; }
        public string DocumentFilingId { get; set; }
        public bool IsFullAccounting { get; set; }
        public string ARInvoiceStockId { get; set; }
        public bool IsInvoiceNumberFromStock { get; set; }
        #region Ayman: it is a very bad code to add properties this way
        //public bool IsDueDateColorRed
        //{
        //    get
        //    {
        //        if (DueDate != null)
        //        {
        //            if (this.StatusCode != "PD" && this.DueDate.Value < DateTime.Now.Date)
        //            {
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //}

        //public bool IsExpectedPaymentDateColorRed
        //{
        //    get
        //    {
        //        if (ExpectedPaymentDate != null)
        //        {
        //            if (this.StatusCode != "PD" && this.ExpectedPaymentDate.Value < DateTime.Now.Date)
        //            {
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //}
        #endregion

        public string BillToCity { get; set; }
        public string BillToCountry { get; set; }
        public string CreatedByPartner { get; set; }

        public string SATXML { get; set; }

        public string BillToGLAccountId { get; set; }

        public string RegionalTaxId { get; set; }
        public double? RegionalTaxPercentage { get; set; }
        public string PaidStatus { get; set; }

        public DateTime? PaidDate { get; set; }

        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string ShipmentsNumbers { get; set; }
        public string MasterNumbers { get; set; }
        public string MasterShipmentNumbers { get; set; }
        public string HouseNumbers { get; set; }
        public string GlobalTaxCalculation { get; set; }
        public string PaymentReferences { get; set; }
        public string SATCancelReasonCode { get; set; }
        public string DigitalPortalSearchFields { get; set; }
        public Decimal? TotalExamptFortaxReport { get; set; }
        public string DocumentTemplateId { get; set; }
        public string ConcurrencyGUID { get; set; }
        public double? TotalAmountNotForTaxReport { get; set; }

        public string ReportUrl { get; set; }
        public bool IsAutoCredited { get; set; }
        public string IsSigned { get; set; }

    }
}