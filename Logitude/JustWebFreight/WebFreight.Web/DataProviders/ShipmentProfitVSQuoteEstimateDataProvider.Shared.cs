using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentProfitVSQuoteEstimateDataProvider: BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Customer { get; set; }
        public string Salesman { get; set; }
        public List<ProfitVSEstimateShipmentItem> Shipments { get; set; }
    }

    public class ProfitVSEstimateShipmentItem
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SalesmanUserId { get; set; }
        public string SalesmanUserName { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentType { get; set; }
        public string Routing { get; set; }

        public string QuoteNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? EstimatedProfit { get; set; }
        public double? ActualProfit { get; set; }
        public double? Margin { get; set; }
    }
}