using Logitude.Test.Base.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SecurityTests.Models.ARInvoice
{
    public class ARInvoiceAccessStepsContext
    {
        public ARInvoiceAccessStepsContext()
        {
            FirstUserARInvoice = new ARInvoicePM();
            SecondUserARInvoice = new ARInvoicePM();
        }

        public ARInvoicePM FirstUserARInvoice { get; set; }
        public ARInvoicePM SecondUserARInvoice { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}

