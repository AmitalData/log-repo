using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ARInvoiceIncludeVATRoutingsDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        //public DateTime? Today_DateTime { get; set; }        
        public double? SubTotal_Sum { get; set; }
        public double? GrandTotal_Sum { get; set; }
        public double? VAT_Sum { get; set; }

        public double? TotalVats_1 { get; set; }
        public double? TotalVats_2 { get; set; }
        public double? TotalVats_3 { get; set; }
        public double? TotalVats_4 { get; set; }

        public string VAT1Header { get; set; }
        public string VAT2Header { get; set; }
        public string VAT3Header { get; set; }
        public string VAT4Header { get; set; }

        public string VAT1Code { get; set; }
        public string VAT2Code { get; set; }
        public string VAT3Code { get; set; }
        public string VAT4Code { get; set; }

        public List<ARInvoiceVATRouting> ARInvoiceVATRoutingList { get; set; }
        public List<InvoiceVATRoutingTotals> InvoiceTotalsList { get; set; }
    }

    public class ARInvoiceVATRouting
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string InvoiceNumber { get; set; }
        public string BillToName { get; set; }
        public string PartnerName { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string OurReference { get; set; }
        public string CustomerReference { get; set; }
        public string Routing { get; set; }
        public string InvoiceStatus { get; set; }
        public double? SubTotal { get; set; }
        public double? VAT { get; set; }
        public double? GrandTotal { get; set; }
        public string Currency { get; set; }
        public double? VAT1Amount { get; set; }
        public double? VAT2Amount { get; set; }
        public double? VAT3Amount { get; set; }
        public double? VAT4Amount { get; set; }
        public double? ProfitInLocalCurrency { get; set; }
        public double? ProfitInProfitCurrency { get; set; }
        public string BillToCode { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
    }

    public class InvoiceVATRoutingTotals
    {
        public double? SubTotals_Local { get; set; }
        public double? GrandTotal_Local { get; set; }
        public double? TotalVats { get; set; }        
        public string Currency { get; set; }

        public double? VAT1Amount { get; set; }
        public double? VAT2Amount { get; set; }
        public double? VAT3Amount { get; set; }
        public double? VAT4Amount { get; set; }
    }
}