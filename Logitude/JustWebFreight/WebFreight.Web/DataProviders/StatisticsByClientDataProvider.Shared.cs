using System;
using System.Collections.Generic;
using System.Linq;


namespace WebFreight.Web.DataProviders
{
    public class StatisticsByClientDataProvider : BaseDataProvider
    {
        public string Name { get; set; }
        public DateTime? FromPeriod { get; set; }
        public DateTime? ToPeriod { get; set; }
        public int TotalShipments { get; set; }
        public double? TotalGrossWeight { get; set; }
        public double? TotalVolume { get; set; }
        public double? TotalTEU { get; set; }
        public double? TotalReceivables { get; set; }
        public double? TotalPayables { get; set; }
        public double? TotalProfit { get; set; }
        public string Customer { get; set; }
        public string Currency { get; set; }

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

        public List<StatisticsByClientReport> StatisticsList_NoGroup { get; set; }
        public List<StatisticsByClientGroup> StatisticsGroupList { get; set; }
    }

    public class StatisticsByClientGroup
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<StatisticsByClientReport> StatisticsRecordList { get; set; }
    }

    public class StatisticsByClientReport
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string TransportMode { get; set; }
        public string Direction { get; set; }
        public string TransmodeDirection { get; set; }
        public int NumberOfShipments { get; set; }
        public double? GrossWeight { get; set; }
        public double? Volume { get; set; }
        public double? TEU { get; set; }
        public double? Receivables { get; set; }
        public double? Payables { get; set; }
        public double? Profit { get; set; }
        public string CustomerField1 { get; set; }
        public string CustomerField2 { get; set; }
        public string CustomerField3 { get; set; }
        public string CustomerField4 { get; set; }
        public string CustomerField5 { get; set; }
        public string CustomerField6 { get; set; }
        public string CustomerField7 { get; set; }
        public string CustomerField8 { get; set; }
        public string CustomerField9 { get; set; }
        public string CustomerField10 { get; set; }
        public string CustomerSalesman { get; set; }
        public double? ChargeableWeightInKg { get; set; }
    }
}