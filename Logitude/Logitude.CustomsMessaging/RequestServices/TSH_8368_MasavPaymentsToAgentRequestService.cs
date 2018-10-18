using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MasavPaymentsToAgentServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TSH_8368_MasavPaymentsToAgentRequestService : RequestServiceBase<TSH_NG_8368_MSG32_MasavPaymentsToAgentRequest, MasavPaymentsToAgentRequestParams>
    {

        public override TSH_NG_8368_MSG32_MasavPaymentsToAgentRequest GetRequest(MasavPaymentsToAgentRequestParams requestParams)
        {
            //Build request 8368 - Message Request for MasavPayments To Agent
            var masavPaymentsToAgentRequest = new TSH_NG_8368_MSG32_MasavPaymentsToAgentRequest();
            DateTime paymentDate = (DateTime)requestParams.PaymentDate;
            masavPaymentsToAgentRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            masavPaymentsToAgentRequest.MasavAgentPaymentRequest = paymentDate;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "בקשת דוח קופה לסוכן לתאריך " + paymentDate.Date.ToString("dd/MM/yyyy");

            return masavPaymentsToAgentRequest;
        }
    }
}
