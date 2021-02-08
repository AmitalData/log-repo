using System;
using System.Collections.Generic;
using System.Linq;


namespace WebFreight.Web.DataProviders
{
    public class AgedAccountsReceivableDataProvider : BaseDataProvider
    {
        public double? DaysPastDue1_30 { get; set; }
        public double? DaysPastDue31_60 { get; set; }
        public double? DaysPastDue61_90 { get; set; }
        public double? DaysPastDue91_120 { get; set; }
        public double? Over120DaysPastDue { get; set; }
        public double? CurrentDue { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Currency { get; set; }
        public string Rate { get; set; }

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


        public List<AgedAccountsReceivable> AgedAccountsReceivableList { get; set; }
        public class AgedAccountsReceivable
        {
            public string CardCode { get; set; }
            public string CardTypeName { get; set; }
            public string CustomerName { get; set; }
            public string PaymentTerm { get; set; }
            public string PartnerName { get; set; }

            public double? DaysPastDue1_30 { get; set; }
            public double? DaysPastDue31_60 { get; set; }
            public double? DaysPastDue61_90 { get; set; }
            public double? DaysPastDue91_120 { get; set; }
            public double? Over120DaysPastDue { get; set; }
            public double? CustomerTotals { get; set; }
            public double? CurrentDue { get; set; }

            public double? DaysPastDue1_15 { get; set; }
            public double? DaysPastDue16_30 { get; set; }
            public double? DaysPastDue1_24 { get; set; }
            public double? DaysPastDue25_30 { get; set; }

            public double? DaysPastDue31_45 { get; set; }
            public double? DaysPastDue46_60 { get; set; }
            public double? DaysPastDue61_75 { get; set; }
            public double? DaysPastDue76_90 { get; set; }
            public double? DaysPastDue91_105 { get; set; }
            public double? DaysPastDue106_120 { get; set; }
        }
    }
}