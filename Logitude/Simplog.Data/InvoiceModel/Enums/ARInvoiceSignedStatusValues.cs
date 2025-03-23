using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Enums
{
    public struct ARInvoiceSignedStatusValues
    {
        public const string NotSigned = "0";
        public const string SignedButNotYetSent = "1";
        public const string SigningFailed = "2";
        public const string SignedAndSentByEmail = "3";
        public const string SignedButSendingByEmailFailed = "4";
    }
}
