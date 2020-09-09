using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.DataContract
{
    public class CloseBalanceInterestReportData
    {
        public string Id { get; set; }
        public decimal? CloseBalance { get; set; }
        public DateTime InterestCalculationDate { get; set; }
        public string InterestReportStatusCode { get; set; }

    }
}
