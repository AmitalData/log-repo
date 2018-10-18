
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class VE_MSG052_VendorSearchResultsForCustomsAgentResponseData:ResponseDataBase
    {
        public int? NumberOfResult { get; set; }

        public List<VendorResult> VendorResults { get; set; }
    }
    public class VendorResult
    {

        public string Id { get; set; }

        public string StatusCode { get; set; }

        public string CityName { get; set; }

        public string CountryCode { get; set; }

        public string DunsNumber { get; set; }

        public string MainAddressLine { get; set; }

        public string PostalCode { get; set; }

        public string SubCountryCode { get; set; }

        public int Tenant { get; set; }

        public string VendorName { get; set; }

        public string VendorTypeCode { get; set; }

        public string CountryName {get; set;}

        public string SubCountryName { get; set; }

        public string VendorNumber { get; set; }
        public string VATNumber { get; set; }
        public bool Exists { get; set; }
        public bool InActive { get; set; }
        public bool IsPalestinian { get; set; }
        public List<VendorCommunicationResult> VendorCommunications { get; set; }

        public string VendorTypeName { get; set; }

        public string StatusName { get; set; }

    }
    public class VendorCommunicationResult
    {

        public string CommunicationAddress { get; set; }

        public string CommunicationType { get; set; }

        public string CommunicationTypeName { get; set; }


    }

   
}
