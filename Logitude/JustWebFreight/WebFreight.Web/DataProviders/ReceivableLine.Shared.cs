using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ReceivableLine
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string Amount { get; set; }
        public string AmountInLocal { get; set; }
        public string AmountInProfit { get; set; }
        public string CurrencyCode { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public double? UnitPrice { get; set; }
        public string UOM { get; set; }
        public double? Quantity { get; set; }
        public string UOMPercentage { get; set; }
        public string PrepaidCollect { get; set; }
        public double? AmountInProfit_Double { get; set; }
    }
}