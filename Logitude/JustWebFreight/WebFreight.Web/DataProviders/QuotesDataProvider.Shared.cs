using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class QuotesDataProvider: BaseDataProvider
    {
        public string QuoteNumber { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Details { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public string Salesman { get; set; }
        public int? NumberOfQuotesSent { get; set; }
        public int? NumberOfQuotesApproved { get; set; }
        public int? NumberOfQuotesNotApproved { get; set; }
        public int? NumberOfQuotesApprovedWithoutShipment { get; set; }

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

        public List<Quotes> QuotesList { get; set; }

        public class Quotes
        {
            public string QuoteNumber { get; set; }
            public string Type { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Details { get; set; }
            public DateTime? ExpiredDate { get; set; }
            public string Status { get; set; }
            public string DirectionTransportMode { get; set; }
            public DateTime? OpenDate { get; set; }
            public string DeclineReason { get; set; }
            public double? EstimateProfit { get; set; }
        }
    }
}