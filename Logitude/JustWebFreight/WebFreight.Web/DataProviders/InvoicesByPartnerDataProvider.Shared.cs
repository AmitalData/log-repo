using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class InvoicesByPartnerDataProvider : BaseDataProvider
    {
        public string Date { get; set; }
        public string InvoiceType { get; set; }
        public string DueDate { get; set; }
        public string OurReference { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Description { get; set; }
        public double? Amount { get; set; }
        public DateTime FromPeriod { get; set; }
        public DateTime ToPeriod { get; set; }
        public string CustomerName { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }

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
        public string CustomerPhone { get; set; }

        public List<InvoicesByPartner> InvoicesByPartnerList { get; set; }

        public class InvoicesByPartner
        {
            public DateTime? InvoiceDate { get; set; }
            public string InvoiceType { get; set; }
            public DateTime? DueDate { get; set; }
            public string OurReference { get; set; }
            public string MasterNumber { get; set; }
            public string HouseNumber { get; set; }
            public string Description { get; set; }
            public double? Amount { get; set; }
            public double? AmountInProfitCurrency { get; set; }
            public string YourRefrence { get; set; }
            public string BillToName { get; set; }
            public string ShipmentNumber { get; set; }
        }
    }
}