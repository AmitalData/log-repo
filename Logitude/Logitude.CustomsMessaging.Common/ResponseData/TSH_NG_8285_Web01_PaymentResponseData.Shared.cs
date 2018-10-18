using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class TSH_NG_8285_Web01_PaymentResponseData : ResponseDataBase
    {
        public List<PaymentsDetailsResult> PaymentsDetailsList { get; set; }
    }

    public class PaymentsDetailsResult
    {
        public string PaymentID { get; set; }
        public string PaymentType { get; set; }
        public string PaymentAmount { get; set; }
        public string Importer { get; set; }
        public string Agent { get; set; }
        public string PaymentMethodType { get; set; }
        public string PaymentStatus { get; set; }
        public string EntityExternalID { get; set; }
        public string EntityType { get; set; }
    }

    
}
