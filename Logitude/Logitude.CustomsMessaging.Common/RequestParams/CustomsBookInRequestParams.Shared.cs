using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CustomsBookInRequestParams : RequestParamsBase
    {
        public DateTime? fromDate { get; set; }
        public bool fromDateSpecified { get; set; }
        public bool isGetHistoricalData { get; set; }
        public DateTime? toDate { get; set; }
        public bool toDateSpecified { get; set; }
    }
}