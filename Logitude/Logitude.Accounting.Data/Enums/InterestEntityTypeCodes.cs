using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.Enums
{
    public struct InterestEntityTypeCodes
    {
        public const string ARInvoice = "IN";
        public const string ARPayment = "PY";
        public const string Journal = "JR";
        public const string Adjustments = "AJ";
        public const string InterestReport = "IR";
    }

}
