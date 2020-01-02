using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ShipperReturnsDataProvider : BaseDataProvider
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string MainCarriageFromPortId { get; set; }
        public string MainCarriageFinalDestinationPortId { get; set; }
        public string Subshipper { get; set; }

        public List<ShipperReturnsPackagesList> ShipperReturnsPackagesList { get; set; }
    }

    public class ShipperReturnsPackagesList
    {
        public DateTime? FlightDate { get; set; }
        public string FlightNumber { get; set; }
        public string HAWB { get; set; }
        public string MAWB { get; set; }
        public string Consignee { get; set; }
        public string InvoiceNumber { get; set; }
        public double? GrossWeight { get; set; }
        public double? Volume { get; set; }
        public double? VolumetricWeight { get; set; }
        public int? Quantity { get; set; }
        public string ShipmentField1 { get; set; }
        public string ShipmentField2 { get; set; }
        public string ShipmentField3 { get; set; }
        public string ShipmentField4 { get; set; }
    }
}