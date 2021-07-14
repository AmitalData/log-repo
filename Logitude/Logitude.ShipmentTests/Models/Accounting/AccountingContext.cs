using Logitude.ShipmentTests.Models.Accounting.APInvoice;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentTests.Models.Accounting
{
    public class AccountingContext
    {
        public ShipmentAPInvoice ShipmentAPInvoice { get; set; }
        public ShipmentARInvoice ShipmentARInvoice { get; set; }
    }

    public class ShipmentAPInvoice
    {
        public ShipmentReceivablePM receivablePM;
        public APInvoicePM APInvoicePM;
        public APInvoiceLinePM APInvoiceLinePM;
    }

    public class ShipmentARInvoice
    {
        public ShipmentReceivablePM receivablePM;
        public ARInvoicePM ARInvoicePM;
        public ARInvoiceLinePM ARInvoiceLinePM;
    }
}
