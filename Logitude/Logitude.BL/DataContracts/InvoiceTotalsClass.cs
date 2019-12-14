using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class InvoiceTotalsClass
    {
        [Key]
        public string Id { get; set; }
        public string VatTypeId { get; set; }
        public double? VatTypePercentage { get; set; }
        public string RowLabel { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string ExternalVatCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public string VatTypeCell { get; set; }
        public double VatRecognizedPercentage { get; set; }
        public DateTime DateForVatInterest { get; set; }
        public int LineNumber { get; set; }
    }
}