
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
   
    public class InsidePackage
    {

	    
    public string Id { get; set; }
    
    public string Type { get; set; }
    
    public int? Quantity { get; set; }
    
    public double? Width { get; set; }
    
    public double? Length { get; set; }
    
    public double? Height { get; set; }
    
    public double? Volume { get; set; }
    
    public double? GrossWeight { get; set; }
    
    public string Commodity { get; set; }
    
    public string Reference1 { get; set; }
    
    public string Reference2 { get; set; }
    
    public string Reference3 { get; set; }
    
    public string Reference4 { get; set; }
    
    public string Description { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 