using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ControlForInvoiceLinesDataView
    {
        public int Tenant { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public string LineActionCode { get; set; }
        public double? VatPercentage { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public string AccountTypeCode { get; set; }
        public string Displaynumber { get; set; }
        public string LocalName1 { get; set; }
        public string LocalName2 { get; set; }
        public string Code { get; set; }
        public string TypeCode { get; set; }
        public string LocalName3 { get; set; }
        public DateTime? TaxReportMonth { get; set; }

        public DateTime? CreateDate { get; set; }
        //public DateTime? InvoiceDate { get; set; }
        public string TaxReportId { get; set; }
        public double? AmountInLocalCurrency { get; set; }
       // public string LineActionCode { get; set; }
        public bool IsExternalEntity { get; set; }
        public Decimal? TotalExamptFortaxReport { get; set; }
        public Decimal TotalVAT { get; set; }
        public Decimal? TotalAmountForTaxReport { get; set; }
        public string MainEntityReference { get; set; }
        public string StatusCode { get; set; }
        //public string Description { get; set; }
    }
}
