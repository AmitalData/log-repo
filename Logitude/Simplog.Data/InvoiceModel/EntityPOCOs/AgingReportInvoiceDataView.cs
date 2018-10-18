using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class AgingReportInvoiceDataView
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime DueDate { get; set; }
        public string DateRange { get; set; }
        
        public string StatusCode { get; set; }
        public double AmountDueInLocalCurrency { get; set; }
        public double AmountDueInProfitCurrency { get; set; }
        public string BillToId { get; set; }
        public int IndexOrder { get; set; }
        public string BranchId { get; set; }
    }
}
