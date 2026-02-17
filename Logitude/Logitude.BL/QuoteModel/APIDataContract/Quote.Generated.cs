
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

namespace Logitude.BL.QuoteModel.APIDataContract.ApiV1
{
   
    public class Quote
    {

	    
    public string Id { get; set; }
    
    public DateTime? AcceptedDate { get; set; }
    
    public string AgentReference1 { get; set; }
    
    public string AgentReference2 { get; set; }
    
    public double? ChargeableWeight { get; set; }
    
    public string ConsigneeReference1 { get; set; }
    
    public string ConsigneeReference2 { get; set; }
    
    public double? CostTotalAmountInLocalCurrency { get; set; }
    
    public double? CostTotalAmountInSaleCurrency { get; set; }
    
    public string CustomerReference1 { get; set; }
    
    public string CustomerReference2 { get; set; }
    
    public DateTime? DeclinedDate { get; set; }
    
    public string DeliveryLocation { get; set; }
    
    public string DepartureFrequency { get; set; }
    
    public string DescriptionOfGoods { get; set; }
    
    public double? DimFactor { get; set; }
    
    public double? EstimateProfit { get; set; }
    
    public double? EstimateProfitInSaleCurrency { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public double? GrossWeight { get; set; }
    
    public bool IncludeDelivery { get; set; }
    
    public bool IncludePickUp { get; set; }
    
    public int LastVersionNumber { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public string ProductCode { get; set; }
    
    public double? SaleTotalAmountInSaleCurrency { get; set; }
    
    public string Notes { get; set; }
    
    public DateTime? UpdateDate { get; set; }
    
    public string QuoteNumber { get; set; }
    
    public DateTime? SentDate { get; set; }
    
    public string ShipperPickAddressId { get; set; }
    
    public string ShipperReference1 { get; set; }
    
    public string ShipperReference2 { get; set; }
    
    public DateTime? StageDueDate { get; set; }
    
    public string Subject { get; set; }
    
    public double? TEU { get; set; }
    
    public string TotalContainers { get; set; }
    
    public string TransitTime { get; set; }
    
    public double? ValueOfGoods { get; set; }
    
    public double? Volume { get; set; }
    
    public Contact AgentContact { get; set; }
    
    public Card Agent { get; set; }
    
    public Branch Branch { get; set; }
    
    public Department Department { get; set; }
    
    public WeightUnit ChargeableWeightUnit { get; set; }
    
    public Contact ShipperContact { get; set; }
    
    public Card Shipper { get; set; }
    
    public Contact ConsigneeContact { get; set; }
    
    public Card Consignee { get; set; }
    
    public User CreatedByUser { get; set; }
    
    public User UpdatedByUser { get; set; }
    
    public Contact CustomerContact { get; set; }
    
    public Card Customer { get; set; }
    
    public Address DeliveryAddress { get; set; }
    
    public DimensionsUnit DimensionsUnit { get; set; }
    
    public Direction Direction { get; set; }
    
    public TransportMode TransportMode { get; set; }
    
    public Port FromPort { get; set; }
    
    public Port ToPort { get; set; }
    
    public WeightUnit GrossWeightUnit { get; set; }
    
    public Incoterm Incoterm { get; set; }
    
    public Card MainCarriageCarrier { get; set; }
    
    public MoveType MoveType { get; set; }
    
    public Address PickUpAddress { get; set; }
    
    public QuoteType QuoteType { get; set; }
    
    public Currency SaleCurrency { get; set; }
    
    public User SalesmanUser { get; set; }
    
    public ShipmentType ShipmentType { get; set; }
    
    public QuoteStage Stage { get; set; }
    
    public Currency ValueOfGoodsCurrency { get; set; }
    
    public VolumeUnit VolumeUnit { get; set; }
    
    public List<QuoteCharge> QuoteCharges { get; set; }
    
    public PackageType PackageType1 { get; set; }
    
    public PackageType PackageType2 { get; set; }
    
    public PackageType PackageType3 { get; set; }
    
    public PackageType PackageType4 { get; set; }
    
    public PackageType PackageType5 { get; set; }
    
    public int? PackageType1Quantity { get; set; }
    
    public int? PackageType2Quantity { get; set; }
    
    public int? PackageType3Quantity { get; set; }
    
    public int? PackageType4Quantity { get; set; }
    
    public int? PackageType5Quantity { get; set; }
    
    public int? PackagesQuantity { get; set; }
    
    public double? VolumetricWeight { get; set; }
    
    public List<QuotePackage> QuotePackages { get; set; }
    
    public DateTime? StageDate { get; set; }
    
    public string SameOrFixed { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 