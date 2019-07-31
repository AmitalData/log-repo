
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

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
   
    public class ARInvoiceLine
    {

	    
    public string Id { get; set; }
    
    public int LineNumber { get; set; }
    
    public ChargesType ChargesType { get; set; }
    
    public Currency ForeignCurrency { get; set; }
    
    public double? ForeignExchangeRate { get; set; }
    
    public double? LocalCurrencyAmount { get; set; }
    
    public double? ForeignCurrencyAmount { get; set; }
    
    public string LocalDescription { get; set; }
    
    public string Notes { get; set; }
    
    public DateTime? ValueDate { get; set; }
    
    public DateTime? DateForInterest { get; set; }
    
    public VatType VatType { get; set; }
    
    public string Description { get; set; }
    
    public double? InvoiceCurrencyAmount { get; set; }
    
    public double? ProfitCurrencyAmount { get; set; }
    
    public double? VatPercentage { get; set; }
    
    public double? UnitPriceInForeignCurrency { get; set; }
    
    public DateTime? ExchangeRateDate { get; set; }
    
    public double? Quantity { get; set; }
    
    public int Tenant { get; set; }
    
    public string GLAccountId { get; set; }
    
    public ARInvoiceLineAction ARInvoiceLineAction { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 