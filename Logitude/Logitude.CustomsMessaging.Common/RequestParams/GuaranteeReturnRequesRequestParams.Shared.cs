using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class GuaranteeReturnRequesRequestParams : RequestParamsBase
    {
        public string GuaranteeExternalCertificateNumber { get; set; }
        public string GuaranteeAmountToReturn { get; set; }
        public string GuaranteeCertificateType { get; set; }
        public string Reason { get; set; }
        public string FileNumber { get; set; }
        public string Numeral { get; set; }
        public string GuaranteedID { get; set; }
        public string MsgId { get; set; }
        public string GuarantorID { get; set; }
        public List<string> ConditionCodeList { get; set; }
    }
}
