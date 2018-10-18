
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

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
   
    public class PickUp
    {

	    
    public string Id { get; set; }
    
    public DateTime? ETD { get; set; }
    
    public DateTime? ETA { get; set; }
    
    public Card FromPartnerCard { get; set; }
    
    public Card ToPartnerCard { get; set; }
    
    public Port FromPort { get; set; }
    
    public Port ToPort { get; set; }
    }
} 