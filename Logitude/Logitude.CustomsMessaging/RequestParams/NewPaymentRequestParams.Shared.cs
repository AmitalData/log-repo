using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestParams
{
    public class NewPaymentRequestParams:RequestParamsBase
    {
        public string PaymentNumber { get; set; }
        public string ExternalId { get; set; } // customer/Client ID 
    }
}
