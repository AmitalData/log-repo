
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
   
    public partial class APInvoiceLine
    {

	    
    public string APInvoiceId { get; set; }
    
    public int LineNumber { get; set; }
    
    public int Tenant { get; set; }
    
    public ChargesType ChargesType { get; set; }
    
    public double? InvoiceCurrencyAmount { get; set; }
    
    public double? LocalCurrencyAmount { get; set; }
    
    public double? ProfitCurrencyAmount { get; set; }
    
    public VatType VatType { get; set; }
    
    public string Notes { get; set; }
    
    public double? VatPercentage { get; set; }
    
    public Currency ForiegnCurrency { get; set; }
    
    public double? ForiegnExchangeRate { get; set; }
    
    public double? ForiegnCurrencyAmount { get; set; }
    
    public string DebitAccount { get; set; }
    
    public string Description { get; set; }
    
    public string LocalDescription { get; set; }
    
    public string ChargeTypeGLAccountId { get; set; }
    
    public PrepaidCollect PrepaidCollect { get; set; }
    
    public string ExternalVATCard { get; set; }
    
    public PackageType ContainerType { get; set; }
    
    public int? Quantity { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    public bool? ExcludeFromTaxReport { get; set; }
        public bool? IsPrepaidExpenses { get; set; }

    }
} 