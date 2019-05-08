
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
   
    public class OceanOrInlandPackage
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public PackageType PackageType { get; set; }
    
    public double? Length { get; set; }
    
    public double? Width { get; set; }
    
    public double? Height { get; set; }
    
    public int? Pieces { get; set; }
    
    public double? Volume { get; set; }
    
    public double? GrossWeight { get; set; }
    
    public string Seal { get; set; }
    
    public string Seal2 { get; set; }
    
    public string Reference1 { get; set; }
    
    public string Reference2 { get; set; }
    
    public string Reference3 { get; set; }
    
    public string Harmonize { get; set; }
    
    public double? Temperature { get; set; }
    
    public double? Ventilation { get; set; }
    
    public bool IsDangerous { get; set; }
    
    public string ClassNumber { get; set; }
    
    public string UnNumber { get; set; }
    
    public string PackagingGroup { get; set; }
    
    public string IMDGCode { get; set; }
    
    public string FlashPoint { get; set; }
    
    public string MaterialDescription { get; set; }
    
    public string CommodityNumber { get; set; }
    
    public string Reference4 { get; set; }
    
    public string Notes { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 