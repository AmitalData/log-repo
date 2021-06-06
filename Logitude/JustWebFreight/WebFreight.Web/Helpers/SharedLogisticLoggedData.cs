using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class SharedLogisticLoggedData
    {
        public string CardName { get; set; }
        public string TenantCompany { get; set; }
        public string ContactName { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public string TenantDateTimeFormat { get; set; }
        public bool IsInvoicesMenuEnabled { get; set; }
        public bool IsAgentShared { get; set; }
        public bool IsShipperShared { get; set; }
        public bool IsConsigneeShared { get; set; }
        public bool DisplayDocumentsAndEvents { get; set; }
        public bool IsQuotesRequestsMenuEnabled { get; set; }

    }
}