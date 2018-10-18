using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class AirlineStatisticsDataProvider : BaseDataProvider
    {
        public string Name { get; set; }
        public DateTime FromPeriod { get; set; }
        public DateTime ToPeriod { get; set; }
        public double? TotalGrossWeight { get; set; }
        public double? TotalChargeableWeight { get; set; }
        public double? TotalVolume { get; set; }
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

        public List<AirlineStatisticsReport> AirlineStatisticsReportList { get; set; }
        public class AirlineStatisticsReport
        {
            public string AirlineName { get; set; }
            public double? GrossWeight { get; set; }
            public double? PercentageFromTotalGrossWeight { get; set; }
            public double? ChargeableWeight { get; set; }
            public double? Volume { get; set; }
            public int Shipments { get; set; }
            public double PercentageFromTotalShipment { get; set; }
        } 
    }
}