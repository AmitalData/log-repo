using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentChargesAnalysisDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string SelectedCustomerName { get; set; }
        public string SelectedCurrency { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public double? TotalOpenReceivables { get; set; }
        public double? TotalAccountedReceivables { get; set; }
        public double? TotalOpenPayables { get; set; }
        public double? TotalAccountedPayables { get; set; }

        public List<ShipmentAnalysisRecord> ShipmentAnalysisRecordList { get; set; }
    }

    public class ShipmentAnalysisRecord
    {
        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public string SelectedDateLable { get; set; }
        public DateTime? Date { get; set; }
        public string ChargeTypeCode { get; set; }
        public string ChargeTypeName { get; set; }
        public double? OpenReceivables { get; set; }
        public double? AccountedReceivables { get; set; }
        public double? OpenPayables { get; set; }
        public double? AccountedPayables { get; set; }
        public int? TotalQuantity { get; set; }
        public double? TotalGrossWeight { get; set; }
        public double? TotalChargeableWeight { get; set; }
        public double? TotalTEU { get; set; }
        public DateTime? OperationalDate { get; set; }
        public DateTime? DateOfLoading { get; set; }
        public string TransportMode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double? ValueOfGoods { get; set; }
        public string FlightNumber { get; set; }
        public string ChargeGroupName { get; set; }
        public string ChargeGroupCode { get; set; }
    }
}