using System;
using System.Collections.Generic;
using System.Linq;


namespace WebFreight.Web.DataProviders
{
    public class ProfitByShipmentDataProvider:BaseDataProvider
    {
        public string Name { get; set; }
        public DateTime FromPeriod { get; set; }
        public DateTime ToPeriod { get; set; }
        public string Direction { get; set; }
        public string Currency { get; set; }
        public string Customer { get; set; }
        public double? TotalWeight { get; set; }
        public double? TotalChargeableWeightMT { get; set; }
        public double? TotalTEU { get; set; }
        public double? TotalReceivables { get; set; }
        public double? TotalPayables { get; set; }
        public double? TotalProfit { get; set; }
        public double? TotalMargin { get; set; }
        public string Agent { get; set; }
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
        public List<ProfitByShipmentReport> ProfitByShipmentReportList { get; set; }

        public class ProfitByShipmentReport
        {
            public string ShipmentNo { get; set; }
            public string Customer { get; set; }
            public string Agent { get; set; }
            public string Salesman { get; set; }
            public string ShipperConsignee { get; set; }
            public string Carrier { get; set; }
            public DateTime? ArrivalDepartureDate { get; set; }
            public double? Weight { get; set; }
            public double? ChargeableWeightMT { get; set; }
            public double? TEU { get; set; }
            public double? Receivables { get; set; }
            public double? Payables { get; set; }
            public double? PayablesExpectedAmount { get; set; }
            public double? Profit { get; set; }
            public double? Margin { get; set; }
            public string WeightUnit { get; set; }
            public string Routing { get; set; }
            public string ShipmentType { get; set; }
            public string AccountManagerName { get; set; }
            public string OperationalStatus { get; set; }
            public string AccountingStatus { get; set; }
            public string ShipmentMasterNumber { set; get; }
            public string Origin { set; get; }
            public string Destination { set; get; }
            public DateTime ShipmentCreateDate { get; set; }
            public double? ChargeableWeight { get; set; }
            public DateTime? OperationalDate { get; set; }
            public string Arrival { get; set; }
            public string Departure { get; set; }
            public string Department { get; set; }
            public string Incoterm { get; set; }
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
            public string Notes { get; set; }
            public string RealShipmentType { get; set; }
            public string Master { get; set; }
            public string FromPortCode { get; set; }
            public string FromPortName { get; set; }
            public string FinalDestinationPortCode { get; set; }
            public string FinalDestinationPortName { get; set; }
            public string ConsigneeReference1 { get; set; }
            public string ConsigneeReference2 { get; set; }
            //public string PayablesExpectedAmount { get; set; }

        }
    }
}