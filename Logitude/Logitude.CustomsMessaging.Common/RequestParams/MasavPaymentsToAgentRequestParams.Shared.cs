using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class MasavPaymentsToAgentRequestParams : RequestParamsBase
    {
        public DateTime? PaymentDate { get; set; }
    }
}
