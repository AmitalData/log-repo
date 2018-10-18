using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{

    public class TSH_NG_8285_Web01_PaymentRequestParams : RequestParamsBase
    {
        public string AgentID { get; set; }
        public string AgentExternalId { get; set; }
        public string PaymentID { get; set; }
        public string PaymentType { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentMethodType { get; set; }

        public string ClientId { get; set; }
        public string ExternalID { get; set; }
        public string CustomFileNo { get; set; }
        public string EntityType { get; set; }
        public string EntityExternalID { get; set; }
        public string PaymentOrderStatus { get; set; }
        public string PaymentProcess { get; set; }
        public DateTime? paymentDateFrom { get; set; }
        public DateTime? paymentDateTo { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }
        public string CustomBankId { get; set; }
        public string BankID { get; set; }
        public string BranchID { get; set; }
        public string BankAccount { get; set; }
    }
}
