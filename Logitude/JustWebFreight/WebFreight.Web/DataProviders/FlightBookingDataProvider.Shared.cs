using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class FlightBookingDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string FlightNumberFilter { get; set; }
        public DateTime? FlightDateFilter { get; set; }
        public string LoggedUserName { get; set; }
        public string FlightTime { get; set; }
        public string Routing { get; set; }
        public int? TotalPieces { get; set; }
        public double? TotalGrossWeight { get; set; }
        public double? TotalVolume { get; set; }
        public int? TotalAWBs{ get; set; }
        public string TotalWeight { get; set; }
        public string TotalQuantity { get; set; }

        public List<FlightBookingRecord> FlightBookingRecordList { get; set; }
    }

    public class FlightBookingRecord
    {
        [Key]
        public int Id { get; set; }
        public string AWBNumber { get; set; }
        public string ShipperName { get; set; }
        public int? Pieces { get; set; }        
        public double? GrossWeight { get; set; }
        public double? Volume { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Dimensions { get; set; }
        public string ChargeableWeight  { get; set; }
        public string DestinationPortCode   { get; set; }
        public string PC { get; set; }
        public string House { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ShipmentNumber  { get; set; }
        public string ShipperAddress { get; set; }
        public double? VolumetricWeight { get; set; }
        public string VolumetricWeightUnit{ get; set; }
        public DateTime? ShipmentCreateDate{ get; set; }
        public string PackagesRef1 { get; set; }
        public string PackagesRef2 { get; set; }
        public string PackagesRef3 { get; set; }
        public string PackagesRef4{ get; set; }
        public string CustomAgentExport { get; set; }
        public string CustomAgentImport { get; set; }
        public int? HAWBsNumbers{ get; set; }
        public string MoveType { get; set; }
        public string SCI{ get; set; }
        public string InvoiceNumber { get; set; }
    }
}