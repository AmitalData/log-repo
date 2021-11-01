using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server; 
using Logitude.Server.Tools; 
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure.DataContracts; 
using Logitude.CargoTracking.Def.Validators;
  
namespace Logitude.CargoTracking.Def.EntityPMs
{
   public partial class CargoTrackingShipmentPM
   {
        [DataMember]
        public string CustomsBrokerReference { get; set; }
        [DataMember]
        public ShipmentCustomsData CustomsData { get; set; }
        [DataMember]
        public List<RoutingStep> RoutingSteps { get; set; }
        [DataMember]
        public List<PartnerCard> PartnerCards { get; set; }
        [DataMember]
        public List<CargoShipmentPackage> Packages { get; set; }
        [DataMember]
        public List<CargoDocumentsFiling> DocumentsFilings { get; set; }
        [DataMember]
        public string ShipmentReferences { get; set; }
        [DataMember]
        public string ForwardingHouse { get; set; }
        [DataMember]
        public string ForwardingMaster { get; set; }
        [DataMember]
        public List<Milestone> Milestones { get; set; }
        [DataMember]
        public string IncotermName { get; set; }
        [DataMember]
        public string FutureMilstoneCode { get; set; }
        [DataMember]
        public DateTime? FutureMilstoneDate { get; set; }
        [DataMember]
        public string FutureMilstoneName { get; set; }

        [DataMember]
        public string WarehouseLegEnglishName { get; set; }
        [DataMember]
        public string TotalTax { get; set; }
        [DataMember]
        public string ShipmentTypeName { get; set; }
        [DataMember]
        public int? ShipmentOrderQuantity { get; set; }
        [DataMember]
        public string ShipmentOrderPONumber { get; set; }
        [DataMember]
        public string RouteFromPortCode { get; set; }
        [DataMember]
        public string RouteToPortCode { get; set; }

    }

    public class CargoDocumentsFiling
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
        public string Code { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeCode { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDigitallySigned { get; set; }
        public string SecurityId { get; set; }
        public string CustomReference { get; set; }
        public bool ShowDetailsMenu { get; set; }
    }

    public class ShipmentCustomsData
    {
        public string DeclarationNumber { get; set; }
        public string DeclarationStatus { get; set; }
        public string CurrencySign { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string GoodsDescription { get; set; }
        public decimal ImporterVatAmount { get; set; }
        public decimal TotalValueInNIS { get; set; }
        public decimal TotalValueInForeignCurrency { get; set; }
        public decimal TotalTax { get; set; }
        public string ImporterId { get; set; }
        public string CargoIdentifier1 { get; set; }
        public string CargoIdentifier2 { get; set; }
        public string CargoIdentifier3 { get; set; }


        public List<CargoTrackingShipmentCustomTaxDetails> TaxDetails;
    }
    public class CargoTrackingShipmentCustomTaxDetails
    {
        public string TaxTypeName { get; set; }
        public string TaxBasis { get; set; }
        public decimal TaxAmount { get; set; }
    }
    public class CargoShipmentPackage
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PackageTypeName { get; set; }
        public string ContainerNumber { get; set; }
        public string ShipperSeal { get; set; }
        public string CarrierSeal { get; set; }
        public int? Quantity { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
    }
    public class RoutingStep
    {
        public RoutingStep()
        {
            Directions = new List<RouteDirection>();
        }
        public bool IsActive { get; set; }
        public string FromPortLabel { get; set; }
        public string ToPortLabel { get; set; }
        public string ToolTipFromPortLabel { get; set; }
        public string ToolTipToPortLabel { get; set; }
        public string Description { get; set; }
        public string TransportModeCode { get; set; }
        public List<RouteDirection> Directions { get; set; }

    }
    public class RouteDirection
    {
        public RouteDirection(DateTime date, string label, string direction)
        {
            Date = date;
            Label = label;
            Direction = direction;
        }
        public DateTime Date { get; set; }
        public string Label { get; set; }
        public string Direction { get; set; }
    }
    public class PartnerCard
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public bool ShowDetails { get; set; } = false;
    }
    public class Milestone
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EstimationDate { get; set; }
        public bool? Done { get; set; }
        public bool? IsEstimation { get; set; }
        public bool? IsCurrent { get; set; }

    }


}
