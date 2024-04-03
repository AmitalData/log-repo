using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ControlForInvoiceLinesDataProvider : BaseDataProvider
    {
        public ControlForInvoiceLinesDataProvider()
        {
           InvoiceLine = new List<InvoiceLine>();
        }
       
        public DateTime? InvoiceDateFrom { get; set; }
        public DateTime? InvoiceDateTo { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public string TaxReportId { get; set; }
        public string TaxReportName { get; set; }
        public bool NotIncludedInAnyTaxReport { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string LineActionCode { get; set; }
        public string IsExternalEntity { get; set; }
        public Decimal? TotalExamptFortaxReport { get; set; }
        public Decimal? TotalVAT { get; set; }
        public Decimal? TotalAmountForTaxReport { get; set; }
        public string MainEntityReference { get; set; }
        public string Description { get; set; }
        public double? AmountInLocalCurrency2 { get; set; }
        public string AmountInLocalCurrencyOp { get; set; }
        public Decimal? TotalExamptFortaxReport2 { get; set; }
        public Decimal? TotalVAT2 { get; set; }
        public Decimal? TotalAmountForTaxReport2 { get; set; }
        public string TotalExamptFortaxReportOp { get; set; }
        public string TotalVATOp { get; set; }
        public string TotalAmountForTaxReportOp { get; set; }
        public string MainEntityReferenceOp { get; set; }

       public List<InvoiceLine> InvoiceLine { get; set; }
    }


    public class InvoiceLine
	{
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
    }


}