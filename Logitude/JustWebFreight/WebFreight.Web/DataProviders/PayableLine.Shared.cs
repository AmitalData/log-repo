using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class PayableLine
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public double? OpenAmount { get; set; }
        public double? OpenAmountInLocal { get; set; }
        public double? OpenAmountInProfit { get; set; }
        public string CurrencyCode { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public double? UnitPrice { get; set; }
        public string UOM { get; set; }
        public double? Quantity { get; set; }
        public string UOMPercentage { get; set; }
        public double? AccountedAmount { get; set; }
        public double? AccountedAmountInLocal { get; set; }
        public double? AccountedAmountInProfit { get; set; }
        public double? ExpectedAmount { get; set; }
        public double? ExpectedAmountInLocal { get; set; }
        public double? ExpectedAmountInProfit { get; set; }
        public string VendorName { get; set; }
    }
}