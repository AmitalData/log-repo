using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class NewPaymentRequestParams:RequestParamsBase
    {
        public string PaymentNumber { get; set; }
        public string ExternalId { get; set; } // customer/Client ID 
        public string FirstEntityID { get; set; }
        public string SecondEntityID { get; set; }
        public string ThirdEntityID { get; set; }
        public int RequestParamsVersion { get; set; } 
    }
}
