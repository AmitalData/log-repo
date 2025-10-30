using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{
    public class APInvoicePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }
        public string InternalNumber { get; set; }

        public bool CreatedFromAPI { get; set; }
        public string ShipmentTransportModeId { get; set; }

        
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
        public string LocalCurrencyCode { get; set; }
        public string InternalNotes { get; set; }

        
        public double? SubTotalInLocalCurrency { get; set; }

        
        public double? SubTotalInInvoiceCurrency { get; set; }

        
        public double? AmountInInvoiceCurrency { get; set; }
        public double? AmountInInvoiceCurrency_Summary { get; set; }

        
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
        
        public DateTime? OperationalDate { get; set; }

        
        public string VendorGLAccountId { get; set; }

        
        public DateTime? AccountingDate { get; set; }
        public bool IsExternalEntity { get; set; }

        public bool IsGeneralInvoice { get; set; }

        
        public DateTime? FirstApproveDate { get; set; }

        
       
        public string ShipmentsNumbers { get; set; }
        public string MasterNumbers { get; set; }
        public string MasterShipmentNumbers { get; set; }
        public string HouseNumbers { get; set; }

        private List<APInvoiceLinePM> invoiceLines;
       
        public virtual List<APInvoiceLinePM> InvoiceLines { get; set; }


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
        public bool IsPrepaidExpenses { get; set; }
        public bool HasExpenseAllocationSetting { get; set; }
        public DateTime ExpenseAllocationStartDate { get; set; }

    }
}
