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
        public APInvoicePM ShipmentAPInvoice { get; set; }
        public ARInvoicePM ShipmentARInvoice { get; set; }
    }

    public class ShipmentAPInvoice
    {

    }
}
