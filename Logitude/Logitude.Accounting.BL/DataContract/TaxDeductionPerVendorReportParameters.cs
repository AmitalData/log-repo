using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.Accounting.BL.DataContract
{
    public class TaxDeductionPerVendorReportParameters
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string VendorId { get; set; }
        public string CardId { get; set; }
    }
}