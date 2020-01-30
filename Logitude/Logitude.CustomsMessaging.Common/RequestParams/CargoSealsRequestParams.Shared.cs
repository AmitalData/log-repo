using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CargoSealsRequestParams : RequestParamsBase
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SubjectText { get; set; }
        public string ContentText { get; set; }
        public string RecepientType { get; set; }
        public string Category { get; set; }
    }
}
