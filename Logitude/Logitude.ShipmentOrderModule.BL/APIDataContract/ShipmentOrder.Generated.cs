
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
    
    public string OrderNumber { get; set; }
    
    public TransportMode TransportMode { get; set; }
    
    public Card Consignee { get; set; }
    
    public Card Shipper { get; set; }
    
    public Card Agent { get; set; }
    
    public Incoterm Incoterm { get; set; }
    
    public User AccountManager { get; set; }
    
    public string PONumber { get; set; }
    
    public string Master { get; set; }
    
    public string House { get; set; }
    
    public Vessel Vessel { get; set; }
    
    public Card CustomsAgent { get; set; }
    
    public SpecialServicesType SpecialServicesType { get; set; }
    
    public Card Forwarder { get; set; }
    
    public bool IsReadyForPickup { get; set; }
    
    public string DescriptionOfGoods { get; set; }
    
    public string TransportModeName { get; set; }
    
    public string CustomerReferences { get; set; }
    
    public DateTime? PickupEstimatedDateTime { get; set; }
    
    public DateTime? PickupActualDateTime { get; set; }
    
    public DateTime? BookingConfirmationDate { get; set; }
    
    public DateTime? ETD { get; set; }
    
    public DateTime? ETA { get; set; }
    
    public DateTime? ATD { get; set; }
    
    public DateTime? ATA { get; set; }
    
    public Direction Direction { get; set; }
    
    public string ShipmentNumber { get; set; }
    
    public DateTime? SupplyDateTime { get; set; }
    
    public ShipmentOrderPort OriginPort { get; set; }
    
    public ShipmentOrderPort DestinationPort { get; set; }
    
    public ShipmentOrderPort Gateway { get; set; }
    
    public string CasualImporterName { get; set; }
    
    public string CasualSupplierName { get; set; }
    
    public ShipmentLevel ShipmentLevel { get; set; }
    
    public DateTime? PODate { get; set; }
    
    public string BookingConfirmationNumber { get; set; }
    
    public string CarrierNumber { get; set; }
    
    public Card Carrier { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public string SecurityKey { get; set; }
    
    public bool IsCancelled { get; set; }
    
    public int? Quantity { get; set; }
    
    public double? GrossWeight { get; set; }
    
    public double? Volume { get; set; }
    
    public Card Customer { get; set; }
    
    public string LastExceptionDescription { get; set; }
    
    public DateTime? LastExceptionDate { get; set; }
    
    public DateTime? OnHandDate { get; set; }
    
    public string OnHandNumber { get; set; }
    
    public Card PlaceOfDelivery { get; set; }
    
    public bool DangerousGoods { get; set; }
    
    public ShipmentType ShipmentType { get; set; }
    
    public PackageType PackageType { get; set; }
    
    public int? CustomerTenantNumber { get; set; }
    
    public string CustomerShipmentNumber { get; set; }
    
    public bool IsOperationalClosed { get; set; }
    }
} 