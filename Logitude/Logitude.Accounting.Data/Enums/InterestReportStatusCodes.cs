using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.Enums
{
    public class InterestReportStatusCodes
    {
        public static string Draft { get { return "1"; } }
        public static string Invoiced { get { return "2"; } }
        public static string Cancelled { get { return "3"; } }
        public static string ClosedWithoutInvoice { get { return "4"; } }
        public static string InProgress { get { return "5"; } }
        public static string Failed { get { return "6"; } }
        public static string InvoicingInProgress { get { return "8"; } }
        public static string InvoicingFailed { get { return "9"; } }
        public static string InvoicePrintingFailed { get { return "10"; } }
    }
}
