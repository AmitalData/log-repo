
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
   
    public partial class Direct
    {

	    
    public string Id { get; set; }
    
    public ShipmentType ShipmentType { get; set; }
    
    public Direction Direction { get; set; }
    
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
    
    public string Master { get; set; }
    
	[XmlAttribute]
    public string ShipmentNumber { get; set; }
    
    public User CreatedByUser { get; set; }
    
    public List<Delivery> Deliveries { get; set; }
    
    public List<PickUp> PickUps { get; set; }
    
    public List<CustomField> CustomFields { get; set; }
    
    public bool IsOperationalClosed { get; set; }
    
    public Vessel Vessel { get; set; }
    
    public DateTime? MainCarriageATA { get; set; }
    
    public DateTime? MainCarriageATD { get; set; }
    
    public bool IsAccountingClosed { get; set; }
    
    public double? ValueOfGoods { get; set; }
    
    public Currency ValueOfGoodsCurrency { get; set; }
    
    public string MainCarriageCarrierNumber { get; set; }
    
    public Card MainCarriageCarrier { get; set; }
    
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
    
    public DateTime? MasterDate { get; set; }
    
    public double? Ratio { get; set; }
    
    public List<MainCarriageLeg> MainCarriageLegs { get; set; }
    
    public string ConcurrencyGUID { get; set; }
    
    public string CustomerReference1 { get; set; }
    
    public EntityStatus Status { get; set; }
    
    public string BookingConfirmationNumber { get; set; }
    
    public DateTime? EstimatedFinalArrivalDate { get; set; }
    
    public DateTime? ActualFinalArrivalDate { get; set; }
    
    public string HouseNo { get; set; }
    
    public Card MainCarriageFromPartner { get; set; }
    
    public Card MainCarriageToPartner { get; set; }
    
    public DateTime? MainCarriageETA { get; set; }
    
    public DateTime? MainCarriageETD { get; set; }
    
    public string TruckNumber { get; set; }
    
    public bool IsHTSMissing { get; set; }
    
    public DateTime? PlannedCargoReadyDate { get; set; }
    
    public DateTime? ApprovedCargoReadyDate { get; set; }
    
    public User HandlerUser { get; set; }
    
    public string Notify1Reference { get; set; }
    
    public string Notify1Reference2 { get; set; }
    
    public string ShipperNotExporterReference1 { get; set; }
    
    public string ShipperNotExporterReference2 { get; set; }
    
    public DateTime? CustomsClearanceDate { get; set; }
    
    public PickUpDeliveryFromToType InlandDomesticFromTypeCode { get; set; }
    
    public PickUpDeliveryFromToType InlandDomesticToTypeCode { get; set; }
    
    public Country InlandDomesticFromCountry { get; set; }
    
    public Country InlandDomesticToCountry { get; set; }
    
    public string InlandDomesticFromZipCode { get; set; }
    
    public string InlandDomesticToZipCode { get; set; }
    
    public string InlandDomesticFromCity { get; set; }
    
    public string InlandDomesticToCity { get; set; }
    
    public Port MainCarriageFromPort { get; set; }
    
    public Port MainCarriageToPort { get; set; }
    
    public DateTime? CutoffDate { get; set; }
    
    public bool IsDangerous { get; set; }
    
    public DateTime? HAWBDate { get; set; }
    
    public Card OnCarriageCarrier { get; set; }
    
    public TransportMode OnCarriageTransportMode { get; set; }
    
    public Port OnCarriageFromPort { get; set; }
    
    public Port OnCarriageToPort { get; set; }
    
    public string OnCarriageCarrierNumber { get; set; }
    
    public DateTime? OnCarriageATD { get; set; }
    
    public DateTime? OnCarriageATA { get; set; }
    
    public DateTime? OnCarriageETD { get; set; }
    
    public DateTime? OnCarriageETA { get; set; }
    
    public Card PreCarriageCarrier { get; set; }
    
    public string PreCarriageCarrierNumber { get; set; }
    
    public TransportMode PreCarriageTransportMode { get; set; }
    
    public Port PreCarriageFromPort { get; set; }
    
    public Port PreCarriageToPort { get; set; }
    
    public DateTime? PreCarriageATD { get; set; }
    
    public DateTime? PreCarriageATA { get; set; }
    
    public DateTime? PreCarriageETD { get; set; }
    
    public DateTime? PreCarriageETA { get; set; }
    
    public Card ConsigneeNotImporter { get; set; }
    
    public string DeclarationNumber { get; set; }
    
    public DateTime? DeclarationDate { get; set; }
    
    public string BookingConfirmationNotes { get; set; }
    
    public DateTime? FreightRelease { get; set; }
    
    public DateTime? TerminalAvailable { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 