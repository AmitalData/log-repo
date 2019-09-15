
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

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
   
    public class ARPaymentCheque
    {

	    
    public string Id { get; set; }
    
    public int Tenant { get; set; }
    
    public string ChequeNumber { get; set; }
    
    public DateTime ValueDate { get; set; }
    
    public decimal LocalAmount { get; set; }
    
    public decimal ForeignAmount { get; set; }
    
    public string BankName { get; set; }
    
    public string BankBranch { get; set; }
    
    public string BankAccount { get; set; }
    }
} 