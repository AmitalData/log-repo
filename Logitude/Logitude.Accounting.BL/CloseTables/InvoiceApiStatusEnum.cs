using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CloseTables
{

    public struct InvoiceApiStatusEnum
    {
        public const string Created = "1";
        public const string Pending = "2";
        public const string InProgress = "3";
        public const string Done = "4";
        public const string Failed = "5";
    }

}
