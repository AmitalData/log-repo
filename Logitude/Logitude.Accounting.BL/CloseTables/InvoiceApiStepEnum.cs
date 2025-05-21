using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CloseTables
{

    public struct InvoiceApiStepEnum
    {
        public const string OpenMagayaSession = "1";
        public const string CloseMagayaSession = "2";
        public const string GetMagayaInvoicesList = "3";
        public const string GetMagayaInvoice = "4";
        public const string GenerateInvoice = "5";
        public const string GetConfirmationNumber = "6";
        public const string ApproveInvoice = "7";
        public const string PrintOrSendInvoice = "8";
    }

}
