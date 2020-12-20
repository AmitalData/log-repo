using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class BluesnapPaymentsDataProvider
    {
        public DateTime Today_DateTime { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool ShowAllRecurringTenants { get; set; }
        public List<BlusnapTransactionsList> BlusnapTransactionsList { get; set; }
    }

    public class BlusnapTransactionsList
    {
        public int? Tenant { get; set; }
        public string TenantName { get; set; }
        public string CRMcustomer { get; set; }
        public string ShopperId { get; set; }
        public double? AmountToPay { get; set; }
        public int TransactionCount { get; set; }
        public double? PaymentDifference { get; set; }
        public double? TotalPayments { get; set; }
        public int ContractCount { get; set; }
        public string Notes { get; set; }
    }
}