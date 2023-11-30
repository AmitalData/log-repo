using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class StatementByInvoiceDateDataProvider: BaseDataProvider
    {
        public string CustomerName { get; set; }
        public string PartnerName { get; set; }
        public DateTime CurrentDate { get; set; }
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
      
        public List<StatementByInvoiceGroup> GroupList { get; set; }
        public List<StatementByInvoiceRecord> RecordList { get; set; }

        public class StatementByInvoiceGroup
        {
            public string InvoiceCurrency { get; set; }
            public double? TotalCurrentAmount { get; set; }
            public double? TotalPastAmount_30 { get; set; }
            public double? TotalPastAmount_45 { get; set; }
            public double? TotalPastAmount_60 { get; set; }
            public double? TotalPastAmount_90 { get; set; }
            public double? TotalPastAmountOver_90 { get; set; }

            public double? TotalPastAmount1_15 { get; set; }
            public double? TotalPastAmount16_30 { get; set; }
            public double? TotalPastAmount31_60 { get; set; }
            public double? TotalPastAmount61_90 { get; set; }
            public double? TotalPastAmount91_120 { get; set; }
            public double? TotalPastAmountOver_120 { get; set; }

            public double? TotalAmount { get; set; }

            public List<StatementByInvoiceRecord> StatementRecordList { get; set; }
        }

        public class StatementByInvoiceRecord
        {
            public string InvoiceCurrency { get; set; }
            public string ShipmentNumber { get; set; }
            public DateTime ShipmentCreateDate { get; set; }
            public string InvoiceNumber { get; set; }
            public DateTime? SentDate { get; set; } // invoice date
            public int Age { get; set; }
            public string OverDue { get; set; }
            public double? CurrentAmount { get; set; }
            public double? PastAmount_30 { get; set; }
            public double? PastAmount_45 { get; set; }
            public double? PastAmount_60 { get; set; }
            public double? PastAmount_90 { get; set; }
            public double? PastAmountOver_90 { get; set; }

            public double? PastAmount1_15 { get; set; }
            public double? PastAmount16_30 { get; set; }
            public double? PastAmount31_60 { get; set; }
            public double? PastAmount61_90 { get; set; }
            public double? PastAmount91_120 { get; set; }
            public double? PastAmountOver_120 { get; set; }

            public double? PastTotalAmount { get; set; }
            public int DueAge { get; set; }
            public string Customer { get; set; }
            public string Salesman { get; set; }
            public string ShipperRef1 { get; set; }
            public string ShipperRef2 { get; set; }
        }
    }
}