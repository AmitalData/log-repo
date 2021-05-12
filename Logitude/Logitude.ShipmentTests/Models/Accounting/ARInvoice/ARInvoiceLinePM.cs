using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentTests.Models.Accounting.ARInvoice
{
    public class ARInvoiceLinePM
    {
        public int Tenant { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeName { get; set; }
        public string Description { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? ForiegnCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string ForiegnCurrencyCode { get; set; }
        public string ForiegnCurrencyId { get; set; }
        public string VatTypeId { get; set; }
        public string VatTypeName { get; set; }
        public double? VatPercentage { get; set; }
        public string EntityId { get; set; }
        public double? UnitPrice { get; set; } 
        public double? Quantity { get; set; }
        public double? ForiegnExchangeRate { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }


    }
}
