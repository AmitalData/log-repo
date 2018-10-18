using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CustomFileCreditResponseData : ResponseDataBase
    {
        public string ApplicationID { get; set; }
        public string CreditStatus { get; set; }
        public string BankCode { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentTime { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public string BillingTaxAmount { get; set; }
        public bool IsTRansGove { get; set; }
        public bool IsReTRansGove { get; set; }
    }
}
