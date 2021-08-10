
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

namespace Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1
{
   
    public partial class ShipmentOrder
    {

	    
    public string Id { get; set; }
    
    public int Tenant { get; set; }
    
    public string OrderNumber { get; set; }
    
    public TransportMode TransportMode { get; set; }
    
    public Card Consignee { get; set; }
    
    public Card Shipper { get; set; }
    
    public Card Agent { get; set; }
    
    public Incoterm Incoterm { get; set; }
    
    public User AccountManager { get; set; }
    
    public string PONumber { get; set; }
    
    public ShipmentType ShipmentType { get; set; }
    
    public string Master { get; set; }
    
    public string House { get; set; }
    
    public Vessel Vessel { get; set; }
    
    public Card CustomsAgent { get; set; }
    
    public SpecialServicesType SpecialServicesType { get; set; }
    
    public Card Forwarder { get; set; }
    
    public bool IsReadyForPickup { get; set; }
    
    public string DescriptionOfGoods { get; set; }
    
    public string TransportModeName { get; set; }
    
    public string ShipmentTypeName { get; set; }
    
    public string CustomerReferences { get; set; }
    }
} 