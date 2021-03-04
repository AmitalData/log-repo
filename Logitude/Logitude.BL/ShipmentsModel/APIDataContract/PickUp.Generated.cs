
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

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
   
    public partial class PickUp
    {

	    
    public string Id { get; set; }
    
    public DateTime? ATD { get; set; }
    
    public DateTime? ATA { get; set; }
    
    public DateTime? ETD { get; set; }
    
    public DateTime? ETA { get; set; }
    
    public Port FromPort { get; set; }
    
    public Port ToPort { get; set; }
    
    public Card FromPartnerCard { get; set; }
    
    public Card ToPartnerCard { get; set; }
    
    public Card Carrier { get; set; }
    
    public string TruckNumber { get; set; }
    
    public string Driver { get; set; }
    
    public string TrailerNumber { get; set; }
    
    public string TransportModeCode { get; set; }
    
    public string Notes { get; set; }
    
    public string TruckerNumber { get; set; }
    
    public string PickUpReference { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 