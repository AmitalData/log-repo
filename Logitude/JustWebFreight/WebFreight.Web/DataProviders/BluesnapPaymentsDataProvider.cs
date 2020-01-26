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
        public string ShopperId { get; set; }
        public bool ShowAllRecurringTenants { get; set; }
        public List<BlusnapTransactionsList> BlusnapTransactionsList { get; set; }
    }

    public class BlusnapTransactionsList
    {


    }
}