using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestParams
{
    public class ClientSearchRequestParams : RequestParamsBase
    {
        public string ExternalId { get; set; }
        public string PassportNumber { get; set; }
        public string PassportTypeCode { get; set; }
        public string PassportCountryCode { get; set; }

    }
}
