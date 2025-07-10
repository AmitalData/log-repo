using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CloseTables
{

    public struct InvoiceApiStepEnum
    {
        public const string OpenInvoiceApiSession = "1";
        public const string CloseInvoiceApiSession = "2";
        public const string GetInvoiceApiInvoicesList = "3";
        public const string GetInvoiceApiInvoice = "4";
        public const string GenerateInvoice = "5";
        public const string GetConfirmationNumber = "6";
        public const string ApproveInvoice = "7";
        public const string PrintOrSendInvoice = "8";
    }

}
