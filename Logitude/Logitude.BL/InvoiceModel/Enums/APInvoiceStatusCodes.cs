using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Enums
{
    public struct APInvoiceStatusCodes
    {
        public const string WaitingForApproval = "WA";
        public const string Unpaid = "AD";
        public const string Void = "VD";
        public const string PaidPartially = "PP";
        public const string Paid = "PD";

    }
}
