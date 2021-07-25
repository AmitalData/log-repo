
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
   
    public partial class House
    {

	    
    public string Id { get; set; }
    
    public ShipmentType ShipmentType { get; set; }
    
    public TransportMode TransportMode { get; set; }
    
    public Card Shipper { get; set; }
    
    public string ShipperReference1 { get; set; }
    
    public string ShipperReference2 { get; set; }
    
    public Card Consignee { get; set; }
    
    public string ConsigneeReference1 { get; set; }
    
    public string ConsigneeReference2 { get; set; }
    
    public Card Customer { get; set; }
    
    public Port FromPort { get; set; }
    
    public Port ToPort { get; set; }
    
    public WeightUnit GrossWeightUnit { get; set; }
    
    public WeightUnit ChargeableWeightUnit { get; set; }
    
    public VolumeUnit VolumeUnit { get; set; }
    
    public Incoterm Incoterm { get; set; }
    
    public string HouseNo { get; set; }
    
    public DateTime? HouseDate { get; set; }
    
    public string DescriptionOfGoods { get; set; }
    
    public List<AirPackage> AirPackages { get; set; }
    
    public List<OceanOrInlandPackage> OceanOrInlandPackages { get; set; }
    
    public List<Container> Containers { get; set; }
    
    public string Commodity { get; set; }
    
    public Branch Branch { get; set; }
    
    public Department Department { get; set; }
    
    public double? TEU { get; set; }
    
    public int? NumberOfPackages { get; set; }
    
    public double? GrossWeight { get; set; }
    
    public double? Volume { get; set; }
    
    public double? VolumetricWeight { get; set; }
    
    public double? ChargeableWeight { get; set; }
    
	[XmlAttribute]
    public string ShipmentNumber { get; set; }
    
    public Direction Direction { get; set; }
    
    public bool IsCancelled { get; set; }
    
    public bool IsOperationalClosed { get; set; }
    
    public bool IsAccountingClosed { get; set; }
    
    public string MasterShipmentDataId { get; set; }
    
    public User CreatedByUser { get; set; }
    
    public List<PickUp> PickUps { get; set; }
    
    public List<Delivery> Deliveries { get; set; }
    
    public List<CustomField> CustomFields { get; set; }
    
    public double? ValueOfGoods { get; set; }
    
    public Currency ValueOfGoodsCurrency { get; set; }
    
    public List<Receivable> Receivables { get; set; }
    
    public List<Payable> Payables { get; set; }
    
    public DimensionsUnit DimensionsUnit { get; set; }
    
    public int? OrderNumberOfPackages { get; set; }
    
    public double? OrderGrossWeight { get; set; }
    
    public double? OrderVolume { get; set; }
    
    public double? OrderChargeableWeight { get; set; }
    
    public bool OrderIsDangerouseGoods { get; set; }
    
    public string MainHarmonize { get; set; }
    
    public User Salesman { get; set; }
    
    public User AccountManager { get; set; }
    
    public SpecialServicesType SpecialServicesType { get; set; }
    
    public Card ShipperNotExporter { get; set; }
    
    public Card Agent { get; set; }
    
    public Card CustomAgentImport { get; set; }
    
    public Card ReleasingAgent { get; set; }
    
    public Card FreightForwarder { get; set; }
    
    public double? Ratio { get; set; }
    
    public string ConcurrencyGUID { get; set; }
    
    public string CustomerReference1 { get; set; }
    
    public EntityStatus Status { get; set; }
    
    public string BookingConfirmationNumber { get; set; }
    
    public DateTime? EstimatedFinalArrivalDate { get; set; }
    
    public DateTime? ActualFinalArrivalDate { get; set; }
    
    public bool IsHTSMissing { get; set; }
    
    public string Notify1Reference { get; set; }
    
    public string Notify1Reference2 { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 