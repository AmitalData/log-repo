using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ContinuousResponseOnClaimFileResponseData : ResponseDataBase
    {
        public string ContinuousMessagesTypeCode { get; set; }
        public string ContinuousMessagesTypeName { get; set; }
        public string ClaimRequestNumber { get; set; }
        public string Note { get; set; }
    }
}
