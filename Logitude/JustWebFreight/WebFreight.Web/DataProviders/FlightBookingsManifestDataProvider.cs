using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class FlightBookingsManifestDataProvider : BaseDataProvider
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string CustomAgent { get; set; }
        public string UserName { get; set; }
        public List<ReportGroup> CommodityAgentGroupList { get; set; }
        public List<ReportGroup> MasterCommodityAgentGroupList { get; set; }
        public List<ReportGroup> Reference4GroupList { get; set; }

        public FlightBookingsManifestDataProvider()
        {
            this.CommodityAgentGroupList = new List<ReportGroup>();
            this.MasterCommodityAgentGroupList = new List<ReportGroup>();
            this.Reference4GroupList = new List<ReportGroup>();
        }
    }

    public class ReportGroup
    {
        public string CommodityNumber { get; set; }
        public string CommodityName { get; set; }
        public string MasterLong { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ATD { get; set; }
        public string MasterNumber { get; set; }
        public string Reference4 { get; set; }
        public string CustomAgentImportId { get; set; }
        public string CustomAgentImportName { get; set; }
        public string FlightNumber { get; set; }
        public List<ReportGroupData> ReportGroupDataList { get; set; }

        public ReportGroup()
        {
            this.ReportGroupDataList = new List<ReportGroupData>();
        }
    }

    public class ReportGroupData
    {
        public string House { get; set; }
        public string CommodityNumber { get; set; }
        public string CommodityName { get; set; }
        public string Master { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string ShipperId { get; set; }
        public string ConsigneeId { get; set; }
        public string Routing { get; set; }
        public int? Quantity { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public string MoveType { get; set; }
        public string CustomAgentImportName { get; set; }
        public string CustomAgentImportId { get; set; }
        public string PackageReference1 { get; set; }
        public string PackageReference2 { get; set; }
        public string PackageReference3 { get; set; }
        public string PackageReference4 { get; set; }
        public string Dimensions { get; set; }
        public string MasterLong { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ETA { get; set; }
        public string ShipmentField1 { get; set; }
        public string ShipmentField2 { get; set; }
        public string ShipmentField3 { get; set; }
        public string ShipmentField4 { get; set; }
        public string ShipmentField5 { get; set; }
        public string ShipmentField6 { get; set; }
        public string ShipmentField7 { get; set; }
        public string ShipmentField8 { get; set; }
        public string ShipmentField9 { get; set; }
        public string ShipmentField10 { get; set; }
        public string ShipmentField11 { get; set; }
        public string ShipmentField12 { get; set; }
        public string ShipmentField13 { get; set; }
        public string ShipmentField14 { get; set; }
        public string ShipmentField15 { get; set; }
        public string ShipmentField16 { get; set; }
        public string ShipmentField17 { get; set; }
        public string ShipmentField18 { get; set; }
        public string ShipmentField19 { get; set; }
        public string ShipmentField20 { get; set; }
        public string ShipmentField21 { get; set; }
        public string ShipmentField22 { get; set; }
        public string ShipmentField23 { get; set; }
        public string ShipmentField24 { get; set; }
        public string ShipmentField25 { get; set; }
        public string ShipmentField26 { get; set; }
        public string ShipmentField27 { get; set; }
        public string ShipmentField28 { get; set; }
        public string ShipmentField29 { get; set; }
        public string ShipmentField30 { get; set; }
        public string ShipmentField31 { get; set; }
        public string ShipmentField32 { get; set; }
        public string ShipmentField33 { get; set; }
        public string ShipmentField34 { get; set; }
        public string ShipmentField35 { get; set; }
        public string ShipmentField36 { get; set; }
        public string ShipmentField37 { get; set; }
        public string ShipmentField38 { get; set; }
        public string ShipmentField39 { get; set; }
        public string ShipmentField40 { get; set; }
        public string FlightNumber { get; set; }
    }
}