using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class StatisticsByAgentDataProvider : BaseDataProvider
    {
        public string Name { get; set; }
        public DateTime FromPeriod { get; set; }
        public DateTime ToPeriod { get; set; }
        public int TotalShipments { get; set; }
        public double? TotalGrossWeight { get; set; }
        public double? TotalVolume { get; set; }
        public double? TotalTEU { get; set; }
        public double? TotalReceivables { get; set; }
        public double? TotalPayables { get; set; }
        public double? TotalProfit { get; set; }
        public string Agent { get; set; }
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

        public List<StatisticsByAgentGroup> StatisticsGroupList { get; set; }        
    }

    public class StatisticsByAgentGroup
    {
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public List<StatisticsByAgentReport> StatisticsRecordList { get; set; }
    }

    public class StatisticsByAgentReport
    {
        public string AgentId { get; set; }
        public string AgentName { get; set; }
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
    }
}