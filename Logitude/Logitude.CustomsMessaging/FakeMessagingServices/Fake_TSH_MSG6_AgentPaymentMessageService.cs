using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using UnifreightIIG.Common.AgentPaymentReplyServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
     class Fake_TSH_MSG6_AgentPaymentMessageService
    {
        private TSH_MSG7_AgentPaymentReplyAgentPaymentReply _agentPaymentReply;
        private TSH_MSG7_AgentPaymentReplyAgentPaymentMethods[] _agentPaymentMethods;
        private ResponseContentHeader _responseContentHeader;
        private dynamic params1;
        private Boolean parsePaymentId;
        private int _paymentID=0;

        internal TSH_MSG7_AgentPaymentReply GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            params1 = JObject.Parse(requestParamsData.TestCase.Param1);
            parsePaymentId = int.TryParse(Convert.ToString(params1.PaymentId), out _paymentID);
            SetAgentPaymentReply(requestParamsData);
            SetAgentPaymentMethods(requestParamsData);
            SetResponseContentHeader();
            TSH_MSG7_AgentPaymentReply fake = new TSH_MSG7_AgentPaymentReply()
            {
                AgentPaymentReply = _agentPaymentReply,
                AgentPaymentMethods = _agentPaymentMethods,
                ResponseContentHeader = _responseContentHeader
            };

            return fake;
        }
        public void SetAgentPaymentReply(GenericRequestParams requestParamsData)
        {
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            _agentPaymentReply = new TSH_MSG7_AgentPaymentReplyAgentPaymentReply
            {
                paymentID= Convert.ToInt32(_dec.DeclarationNumber.Substring(_dec.DeclarationNumber.Length - 4)),
                status = 3
            };
            if(parsePaymentId && _paymentID != 0)
            {
                _agentPaymentReply.paymentID = _paymentID;
            }
        }
        public void SetAgentPaymentMethods(GenericRequestParams requestParamsData)
        {
            _agentPaymentMethods = new TSH_MSG7_AgentPaymentReplyAgentPaymentMethods[2];
            _agentPaymentMethods[0] = new TSH_MSG7_AgentPaymentReplyAgentPaymentMethods
            {
                type = 1,
                amount = 99,
                paymentMethodStatus = 2
            };
            _agentPaymentMethods[1] = new TSH_MSG7_AgentPaymentReplyAgentPaymentMethods
            {
                type = 2,
                amount = 99,
                paymentMethodStatus = 2
            };
        }
        public void SetResponseContentHeader()
        {
            _responseContentHeader = new ResponseContentHeader()
            {
                TransmitionDateTime = DateTime.Now,
                Remark = "",
                Exception = null,
                ApplicationID = 0
            };
        }
    }
}