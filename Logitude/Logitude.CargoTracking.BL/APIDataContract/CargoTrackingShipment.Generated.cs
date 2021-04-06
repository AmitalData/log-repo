
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

namespace Logitude.CargoTracking.BL.APIDataContract.ApiV1
{
   
    public partial class CargoTrackingShipment
    {

	    
    public int Id { get; set; }
    
    public string House { get; set; }
    
    public string ShipmentNumber { get; set; }
    
    public DateTime? ArrivalEstimationDate { get; set; }
    
    public DateTime? ArrivalDate { get; set; }
    
    public DateTime? CustomsPaymentDate { get; set; }
    
    public DateTime? ClearanceDate { get; set; }
    }
} 