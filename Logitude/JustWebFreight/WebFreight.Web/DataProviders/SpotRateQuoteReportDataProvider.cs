using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class SpotRateQuoteReportDataProvider : BaseDataProvider
    {
        public SpotRateQuoteReportDataProvider()
        {
            QuoteCharges = new List<QuoteChargeItem>();
        }
        public List<QuoteChargeItem> QuoteCharges { get; set; }
        public string ProfitCurrency { get; set; }
        public string LocalCurrency { get; set; }
    }

    public class QuoteChargeItem
    {
        public DateTime CreateDate { get; set; }
        public string QuoteNumber { get; set; }
        public string QuoteId { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string ChargeType { get; set; }


        public string CostMeasurement { get; set; }
        public string CostCurrency { get; set; }
        public double? CostQuantity { get; set; }
        public double? CostUnitPrice { get; set; }
        public double? CostTotalAmount { get; set; }


        public string SaleMeasurement { get; set; }
        public string SaleCurrency { get; set; }
        public double? SaleQuantity { get; set; }
        public double? SaleUnitPrice { get; set; }
        public double? SaleTotalAmount { get; set; }
        public double? SaleAmountInProfitCurrency { get; set; }
        public double? SaleTotalAmountLocal { get; set; }


        public double? ExpectedProfit { get; set; }
        public bool ConnectedToShipment { get; set; }
        public string ConnectedShipmentsNumbers { get; set; }
        public string Stage { get; set; }
    }
}