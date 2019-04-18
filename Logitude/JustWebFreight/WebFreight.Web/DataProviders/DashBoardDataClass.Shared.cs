using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class DashBoardDataClass: BaseDataProvider
    {
        [Key]
        public string Id { get; set; }

        public List<GeneralDataClass> GeneralList { get; set; }
        public string TimeRange { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<CustomersDataClass> CustomersList { get; set; }
        public List<CountriesDataClass> CountriesList { get; set; }
        public List<DirectionTransportModeDataClass> DirectionTransportModeList { get; set; }
        public List<ShipmentQuantityDataClass> ShipmentQuantityList { get; set; }
    }

    public class GeneralDataClass
    {
        [Key]
        public string Id { get; set; }
        public int IntegerProperty { get; set; }
        public string DataTypeCode { get; set; }
        public string StringProperty { get; set; }
        public int Day { get; set; }
        public string OwnerId { get; set; }
        public string BusinessUnitId { get; set; }
    }

    public class CustomersDataClass
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public double Total { get; set; }
        public double? SumChargeableWeight { get; set; }
        public double? SumGrossWeight { get; set; }
        public double? ProfitInLocalCurrency { get; set; }
        public double? ProfitInProfitCurrency { get; set; }
        public double? ReceivablesInLocalCurrency { get; set; }
        public double? ReceivablesInProfitCurrency { get; set; }
        public double? GeneralTotal { get; set; }
    }

    public class CountriesDataClass
    {
        [Key]
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public double Total { get; set; }
        public double? SumChargeableWeight { get; set; }
        public double? SumGrossWeight { get; set; }
        public double? TotalLastMonth { get; set; }
        public double? SumChargeableWeightLastMonth { get; set; }
        public double? SumGrossWeightLastMonth { get; set; }
        public double? ProfitInLocalCurrency { get; set; }
        public double? ProfitInProfitCurrency { get; set; }
        public double? ReceivablesInLocalCurrency { get; set; }
        public double? ReceivablesInProfitCurrency { get; set; }
        public double? GeneralTotal { get; set; }
    }

    public class DirectionTransportModeDataClass
    {
        [Key]
        public int Id { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }
        public double Total { get; set; }
        public double? SumChargeableWeight { get; set; }
        public double? SumGrossWeight { get; set; }
        public double? TotalProfitInLocalCurrency { get; set; }
        public double? TotalProfitInProfitCurrency { get; set; }
        public double? ReceivablesInLocalCurrency { get; set; }
        public double? ReceivablesInProfitCurrency { get; set; }
        public string DirectionTransportModeNames { get; set; }
        public double? GeneralTotal { get; set; }
    }

    public class ShipmentQuantityDataClass
    {
        [Key]
        public int Id { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }
        public double Total { get; set; }
        public double? SumChargeableWeight { get; set; }
        public double? SumGrossWeight { get; set; }
        public double? TotalProfitInLocalCurrency { get; set; }
        public double? TotalProfitInProfitCurrency { get; set; }
        public double? ReceivablesInLocalCurrency { get; set; }
        public double? ReceivablesInProfitCurrency { get; set; }
        public string XField { get; set; }
        public double? GeneralTotal { get; set; }
    }
}