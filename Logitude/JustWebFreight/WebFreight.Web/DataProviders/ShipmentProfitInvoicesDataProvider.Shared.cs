using System;
using System.Collections.Generic;
using System.Linq;


namespace WebFreight.Web.DataProviders
{
    public class ShipmentProfitInvoicesDataProvider : BaseDataProvider
    {
        public string ShipmentNumber { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Incoterm { get; set; }
        public string Activity { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public string OriginLocation { get; set; }
        public string OriginCountryCode { get; set; }
        public string DestinationLocation { get; set; }
        public string DestinationCountryCode { get; set; }
        public DateTime? POLATD { get; set; }
        public string POLLocation { get; set; }
        public string POLCountryCode { get; set; }
        public DateTime? PODATA { get; set; }
        public string PODLocation { get; set; }
        public string PODCountryCode { get; set; }
        public string ContainersLabel { get; set; }
        public string ContainersValue { get; set; }

        public double PayableAmountInProfitCurrency { get; set; }
        public double PayableAmountInLocalCurrency { get; set; }
        public double ReceivableAmountInProfitCurrency { get; set; }
        public double ReceivableAmountInLocalCurrency { get; set; }
        public double ProfitAmountInProfitCurrency { get; set; }
        public double ProfitAmountInLocalCurrency { get; set; }
        public List<PayableInvoiceProvider> PayableInvoices { get; set; }
        public List<ReceivableInvoiceProvider> ReceivableInvoices { get; set; }
    }

    public class PayableInvoiceProvider
    {
        public string InvoiceNumber { get; set; }
        public string Vendor { get; set; }
        public double AmountInProfitCurrency { get; set; }
        public double AmountInLocalCurrency { get; set; }
        public string Status { get; set; }
    }

    public class ReceivableInvoiceProvider
    {
        public string InvoiceNumber { get; set; }
        public string BillTo { get; set; }
        public double AmountInProfitCurrency { get; set; }
        public double AmountInLocalCurrency { get; set; }
        public string Status { get; set; }
    }


}