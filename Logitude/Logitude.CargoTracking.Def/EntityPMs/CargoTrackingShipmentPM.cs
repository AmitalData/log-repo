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
        public string CustomsBrokerReference { get; set; }
        public ShipmentCustomsData CustomsData { get; set; }
        public List<RoutingStep> RoutingSteps { get; set; }
        public List<CargoShipmentPackage> Packages { get; set; }
        public List<CargoDocumentsFiling> DocumentsFilings { get; set; }
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


}
