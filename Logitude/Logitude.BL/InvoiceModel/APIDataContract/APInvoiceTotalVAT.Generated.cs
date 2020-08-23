
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
   
    public partial class APInvoiceTotalVAT
    {

	    
    public string Id { get; set; }
    
    public VatType VatType { get; set; }
    
    public double? VatPercent { get; set; }
    
    public double InvoiceCurrencyVATAmount { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 