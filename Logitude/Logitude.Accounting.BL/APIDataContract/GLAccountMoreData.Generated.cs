
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
   
    public partial class GLAccountMoreData
    {

	    
    public string AccountId { get; set; }
    
    public decimal BalanceInLocalCurrency { get; set; }
    
    public decimal LocalBalanceInDue { get; set; }
    
    public DateTime? NextDueDate { get; set; }
    
    public decimal? TotalOpenChequesInLocalCur { get; set; }
    
    public decimal? TotFutureOpenChequesInLocalCur { get; set; }
    }
} 