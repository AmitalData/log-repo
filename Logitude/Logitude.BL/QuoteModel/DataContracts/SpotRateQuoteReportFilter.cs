using System;
using System.Collections.Generic;

namespace Logitude.BL.QuoteModel.DataContracts
{
    public class SpotRateQuoteReportFilter
    {
        public string CustomerId { get; set; }
        public string SalesmanId { get; set; }
        public DateTime? OpenDateGraterThan { get; set; }
        public DateTime? ExpirationDateLessThan { get; set; }
    }
}
