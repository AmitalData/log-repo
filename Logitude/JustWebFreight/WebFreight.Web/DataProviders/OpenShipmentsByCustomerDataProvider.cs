using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class OpenShipmentsByCustomerDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string SelectedCustomerName { get; set; }        
        public List<OpenShipmentsRecord> OpenShipmentsRecordList { get; set; }
    }

    public class OpenShipmentsRecord
    {
        [Key]
        public int Id { get; set; }
        public string TransPortModeCode { get; set; }
        public string TransPortModeName { get; set; }
        public string DirectionCode { get; set; }
        public string DirectionName { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CustomerName { get; set; }
        public string ShipperName { get; set; }
        public string ShipmentType { get; set; }
        public string Status { get; set; }
        public string LastSharedEvent { get; set; }
        public string IncotermCode { get; set; }
        public string Routing { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string AgentName { get; set; }
        public string CustomerReference1 { get; set; }
        public double? VolumInCBM { get; set; }
        public double? TEU { get; set; }
        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Notes { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfLoadingName { get; set; }
        public string PortOfDischarge { get; set; }
        public string PortOfDischargeName { get; set; }
        public int? NumberOfContainers { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATA { get; set; }
        public bool IsCancelled { get; set; }
        public string ContainersNumbersArray { get; set; }
        public string ContainersNumbersAndTypesArray { get; set; }
        public DateTime? LastSharedEventDate { get; set; }
        public string LastSharedEventNote { get; set; }
        public string BookingNumber { get; set; }
        public string Consignee { get; set; }
        public int? PackagesCount { get; set; }
        public int? InsidePackagesCount { get; set; }
        public string FullStatus { get; set; }

        public DateTime? Transshipment1ETA { get; set; }
        public DateTime? Transshipment2ETA { get; set; }
        public DateTime? Transshipment3ETA { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment3ETD { get; set; }
        public string ShippingLine { get; set; }
        public string Voyage { get; set; }
        public string Vessel { get; set; }
    }
}