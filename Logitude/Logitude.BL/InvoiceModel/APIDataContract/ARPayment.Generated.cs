
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

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
   
    public partial class ARPayment
    {

	    
    public string Id { get; set; }
    
    public int Tenant { get; set; }
    
    public string PaymentNo { get; set; }
    
    public AccountingPaymentMethod AccountingPaymentMethod { get; set; }
    
    public double? AmountInLocalCurrency { get; set; }
    
    public double? AmountInPaymentCurrency { get; set; }
    
    public string PaidBy { get; set; }
    
    public double? PaymentCurrencyExchangeRate { get; set; }
    
    public DateTime? ExchangeRateDate { get; set; }
    
    public User CreatedByUser { get; set; }
    
    public string LocalCurrencyCode { get; set; }
    
    public Branch Branch { get; set; }
    
    public Currency PaymentCurrency { get; set; }
    
    public Card BillTo { get; set; }
    
    public string ChequeOrPaymentRef { get; set; }
    
    public string Bank { get; set; }
    
    public string BankBranch { get; set; }
    
    public string Account { get; set; }
    
    public DateTime? ValueDate { get; set; }
    
    public DateTime? RegisterDate { get; set; }
    
    public CreditCardType CreditCardType { get; set; }
    
    public string PaymentCurrencyCode { get; set; }
    
    public DateTime? CreateDate { get; set; }
    
    public List<ARPaymentInvoice> PaymentInvoices { get; set; }
    
    public List<ARPaymentCheque> ARPaymentCheques { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 