using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class GuaranteeCertificateResponseData : ResponseDataBase
    {
        public string ResponseStatusXML { get; set; }
        public string DeclarationID { get; set; }

        public GeneralDetails GeneralDetailsData { get; set; }
        public List<Allocation> AllocationList { get; set; }
        public List<Request> RequestList { get; set; }

        public class GeneralDetails
        {
            public string certificateAvailableAmount { get; set; }
            public int certificateID { get; set; }
            public string guaranteeAmount { get; set; }
            public int GuaranteeCertificateStatus { get; set; }
            public string GuaranteeCertificateStatusName { get; set; }
            public int guaranteedId { get; set; }
            public string guaranteedName { get; set; }
            public string guaranteeExternalCertificateNumebr { get; set; }
            public int guaranteeType { get; set; }
            public string guaranteeTypeName { get; set; }
            public string guaranteeValidityDate { get; set; }
            public string totalCertificateAllocation { get; set; }
        }

        public class Allocation
        {
            public decimal certificateAllocationAmount { get; set; }
            public string displayFileNumber { get; set; }
            public string fileNumber { get; set; }
            public int fileType { get; set; }
            public string fileTypeName { get; set; }
            public int Numeral { get; set; }
            public DateTime updateDate { get; set; }
            public DateTime validity { get; set; }
        }

        public class Request
        {
            public DateTime createTime { get; set; }
            public string displayFileNumber { get; set; }
            public string fileNumber { get; set; }
            public int guaranteeStatus { get; set; }
            public string guaranteeStatusName { get; set; }
            public int guarenteeRequestNumber { get; set; }
            public int? Numeral { get; set; }
            public bool NumeralSpecified { get; set; }
            public string requestDescription { get; set; }
            public decimal? requestedExecutionValue { get; set; }
            public bool requestedExecutionValueSpecified { get; set; }
            public DateTime? requestedValidityDate { get; set; }
            public bool requestedValidityDateSpecified { get; set; }
        }
    }
}
