
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
   
    public partial class ARInvoice
    {

	    
    public string Id { get; set; }
    
    public ARInvoiceType InvoiceType { get; set; }
    
    public Card BillTo { get; set; }
    
    public string InvoiceNumber { get; set; }
    
    public DateTime? InvoiceDate { get; set; }
    
    public DateTime? PrintDate { get; set; }
    
    public bool IsPrinted { get; set; }
    
    public string MainEntityReference { get; set; }
    
    public bool IsConstituentInvoice { get; set; }
    
    public bool IsConsolidationInvoice { get; set; }
    
    public Currency InvoiceCurrency { get; set; }
    
    public double? AmountInLocalCurrency { get; set; }
    
    public string CancelledByARInvoice { get; set; }
    
    public User CreatedByUser { get; set; }
    
    public string VATNumber { get; set; }
    
    public Address BillToAddress { get; set; }
    
    public string PrintNotes { get; set; }
    
    public User IssuedByUser { get; set; }

        public ConfirmationNumberStatus Confirmation { get; set; }


        public double? InvoiceCurrencyExchangeRate { get; set; }
    
    public List<ARInvoiceLine> ARInvoiceLines { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public double? SubTotalInInvoiceCurrency { get; set; }
    
    public double? SubTotalInLocalCurrency { get; set; }
    
    public double? AmountInInvoiceCurrency { get; set; }
    
    public bool IsDraft { get; set; }
    
    public double? ProfitCurrencyExchangeRate { get; set; }
    
    public double? AmountInProfitCurrency { get; set; }
    
    public ARInvoiceTransferStatus TransferStatus { get; set; }
    
    public Branch Branch { get; set; }
    
    public Currency LocalCurrency { get; set; }
    
    public int Tenant { get; set; }
    
    public bool IsMultiCurrency { get; set; }
    
    public string CreditARInvoice { get; set; }
    
    public string ExternalAccountingEntityId { get; set; }
    
    public string BillToGLAccount { get; set; }
    
    public ARInvoiceStatus Status { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    public string ConfirmationNumber { get; set; }

        public ConfirmationNumberStatus ConfirmationNumberStatus { get; set; }


    }
} 