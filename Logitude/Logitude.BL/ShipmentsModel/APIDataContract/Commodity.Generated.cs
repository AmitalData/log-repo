
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
   
    public class Commodity
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public string DescriptionOfGoods { get; set; }
    
    public double? ChargeableWeight { get; set; }
    
    public double? ChargeRate { get; set; }
    
    public double? ChargeAmount { get; set; }
    
    public string CommodityNumber { get; set; }
    
    public int? NumberOfPackages { get; set; }
    
    public double? GrossWeight { get; set; }
    
    public double? Volume { get; set; }
    
    public double? VolumetricWeight { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 