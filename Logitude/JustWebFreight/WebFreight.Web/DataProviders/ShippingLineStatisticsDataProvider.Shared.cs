using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ShippingLineStatisticsDataProvider: BaseDataProvider
    {
        public string Name { get; set; }
        public DateTime FromPeriod { get; set; }
        public DateTime ToPeriod { get; set; }
        public double? TotalLCLWeight { get; set; }
        public double? TotalTEU { get; set; }
        public int TotalLCLShipments { get; set; }
        public int TotalFCLShipments { get; set; }
        public int TotalShipments { get; set; }
        public string Direction { get; set; }

        public string TenantName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Signature { get; set; }
        public string TenantPhone { get; set; }
        public string TenantFax { get; set; }

        public List<ShippingLineStatisticsReport> ShippingLineStatisticsReportList { get; set; }
        public class ShippingLineStatisticsReport
        {
            public string Carrier { get; set; }
            public double? LCLWeight { get; set; }
            public double? TEU { get; set; }
            public int FCLShipments { get; set; }
            public int LCLShipments { get; set; }
            public int TotalShipments { get; set; }
            public double PercentageFromTotalShipment { get; set; }
        }

        
    }
}