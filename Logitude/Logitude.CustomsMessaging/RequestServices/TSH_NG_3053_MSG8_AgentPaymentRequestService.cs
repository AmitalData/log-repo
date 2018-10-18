using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TSH_NG_3053_MSG8_AgentPaymentRequestService : RequestServiceBase<TSH_NG_3053_MSG8_AgentPaymentRequest, NewPaymentRequestParams>
    {
        public override TSH_NG_3053_MSG8_AgentPaymentRequest GetRequest(NewPaymentRequestParams requestParams)
        {
            //Build request 3053- Ask for payment Details
            var myTSH_NG_3053_MSG8_AgentPaymentRequest = new TSH_NG_3053_MSG8_AgentPaymentRequest();
            myTSH_NG_3053_MSG8_AgentPaymentRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myTSH_NG_3053_MSG8_AgentPaymentRequest.AgentPaymentRequest = new TSH_NG_3053_MSG8_AgentPaymentRequestAgentPaymentRequest();
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var ClientSearchQueryService = new ClientQueryService(dbContext);
            int externalID = 0;
            int paymentID = 0;
            var clientPM = new ClientPM();

            int.TryParse(requestParams.PaymentNumber , out paymentID);
            myTSH_NG_3053_MSG8_AgentPaymentRequest.AgentPaymentRequest.paymentID = paymentID;
            clientPM = ClientSearchQueryService.GetSingle(requestParams.ExternalId, false, false);
            int.TryParse(clientPM.Code, out externalID);
            myTSH_NG_3053_MSG8_AgentPaymentRequest.AgentPaymentRequest.externalID = externalID;
            myTSH_NG_3053_MSG8_AgentPaymentRequest.AgentPaymentRequest.externalIDSpecified = externalID > 0 ? true : false;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
            this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
            this.MyRequestSheetParam.RequestDescription = "אחזור הוראת תשלום " + requestParams.PaymentNumber;

            return myTSH_NG_3053_MSG8_AgentPaymentRequest;
        }
    }
}
