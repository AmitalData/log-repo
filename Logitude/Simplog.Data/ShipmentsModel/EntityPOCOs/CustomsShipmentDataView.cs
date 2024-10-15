using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class CustomsShipmentDataView
    {
        [Key]
        public string ShipmentNumber { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CustomerId { get; set; }
        public string ReferenceValue { get; set; }
        public string DeclarationOfficeCode { get; set; }
        public string Mawb { get; set; }
        public string House { get; set; }
        public string DescriptionOfGoods { get; set; }
        public decimal GrossWeight { get; set; }
        public int NumberOfPackages { get; set; }
        public int DepartmentId { get; set; }
        public string CarrierCode { get; set; }
        public DateTime MawbDate { get; set; }
        public string IskaNumber { get; set; }
        public int FreightForwarderId { get; set; }
        public DateTime EstimatedFinalArrivalDate { get; set; }
        public string PackageTypeCode { get; set; }
        public decimal ChargeableWeight { get; set; }
        public int ReferantUserId { get; set; }
        public string ReferenceType { get; set; }
        public string TransportModeId { get; set; }
        public string Vessel { get; set; }
        public DateTime HawbDate { get; set; }
        public string FlightVoyageNumber { get; set; }
        public int IncotermId { get; set; }
        public DateTime FinalArrivalDate { get; set; }
        public decimal VolumetricWeight { get; set; }
        public decimal Volume { get; set; }
        public string Commodity { get; set; }
        public int SalesmanUserId { get; set; }
		public string OriginCountryCode { get; set; }

	}
}
