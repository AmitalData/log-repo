using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestParams
{
    public class VE_MSG051_VendorSearchByCustomsAgentRequestParams : RequestParamsBase
    {
        public string VendorName { get; set; }

        public int? DunsNumber { get; set; }

        public string CityName { get; set; }

        public string CountryCode { get; set; }

        public string MainAddressLine { get; set; }

        public string PostalCode { get; set; }

        public string SubCountryCode { get; set; }

        public bool? isSearchPreviousName { get; set; }

        public string LicensedDealerNumber { get; set; }

        public int? VendorNumber { get; set; }

        public int? VendorTypeCode { get; set; }
    }
}
