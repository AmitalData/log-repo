using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AccountingTests.Models
{
    public class APInvoiceLinePM
    {
        public int Tenant { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeId { get; set; }
        public string Description { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? ForiegnCurrencyAmount { get; set; }
        public string VatTypeId { get; set; }
        public string VatTypeName { get; set; }
        public double? VatPercentage { get; set; }
        public string EntityId { get; set; }


    }
}
