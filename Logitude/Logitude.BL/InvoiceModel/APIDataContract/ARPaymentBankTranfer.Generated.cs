
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
   
    public partial class ARPaymentBankTranfer
    {

	    
    public string Id { get; set; }
    
    public int Tenant { get; set; }
    
    public int LineNumber { get; set; }
    
    public string PaymentRef { get; set; }
    
    public DateTime ValueDate { get; set; }
    
    public decimal LocalAmount { get; set; }
    
    public decimal ForeignAmount { get; set; }
    
    public string BankAccountNumber { get; set; }
    
    public string BankAccountId { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 