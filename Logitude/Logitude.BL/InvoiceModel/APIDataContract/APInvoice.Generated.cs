
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1; 
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using System.Xml.Serialization;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
   
    public partial class APInvoice
    {

	    
    public int Tenant { get; set; }
    
    public string InternalNumber { get; set; }
    
    public Vendor Vendor { get; set; }
    
    public string VATNumber { get; set; }
    
    public string InvoiceNumber { get; set; }
    
    public Currency InvoiceCurrency { get; set; }
    
    public double? InvoiceCurrencyExchangeRate { get; set; }
    
    public DateTime? InvoiceDate { get; set; }
    
    public DateTime? AccountingDate { get; set; }
    
    public PaymentTerm PaymentTerm { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public DateTime? ExchangeRateDate { get; set; }
    
    public Currency LocalCurrency { get; set; }
    
    public string InternalNotes { get; set; }
    
    public double? SubTotalInLocalCurrency { get; set; }
    
    public double? SubTotalInInvoiceCurrency { get; set; }
    
    public double? AmountInLocalCurrency { get; set; }
    
    public APInvoiceStatus Status { get; set; }
    
    public Currency ProfitCurrency { get; set; }
    
    public double? ProfitCurrencyExchangeRate { get; set; }
    
    public double? AmountInProfitCurrency { get; set; }
    
    public User UpdatedByUser { get; set; }
    
    public DateTime? UpdateDate { get; set; }
    
    public double? AmountDue { get; set; }
    
    public double? AmountDueInLocalCurrency { get; set; }
    
    public double? AmountDueInProfitCurrency { get; set; }
    
    public double? RefundAmount { get; set; }
    
    public Branch Branch { get; set; }
    
    public string HouseNumber { get; set; }
    
    public string MasterNumber { get; set; }
    
    public string Description { get; set; }
    
    public string AccountingExternalCode { get; set; }
    
    public string CreditAccount { get; set; }
    
    public string PaymentTermExternalId { get; set; }
    
    public APInvoiceTransferStatus TransferStatus { get; set; }
    
    public DateTime? ApprovedDate { get; set; }
    
    public User ApprovedByUser { get; set; }
    
    public bool IsExternalEntity { get; set; }
    
    public bool IsGeneralInvoice { get; set; }
    
    public string ExternalAccountingEntityId { get; set; }
    
    public string Id { get; set; }
    
    public List<APInvoiceLine> InvoiceLines { get; set; }
    
    public double? AmountInInvoiceCurrency { get; set; }
    
    public double? InvoiceExpectedAmount { get; set; }
    
    public string EntityReference { get; set; }
    
    public string EntityType { get; set; }
    
    public string VendorGLAccount { get; set; }
    
    public List<APInvoiceTotalVAT> TotalVATs { get; set; }
    
    public bool TotalVATOnly { get; set; }
    
    public User CreatedByUser { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    public string ConfirmationNumber { get; set; }

       public bool IsPrepaidExpenses { get; set; }


    }
} 