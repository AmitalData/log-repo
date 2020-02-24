using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using UnifreightIIG.Common.AgentPaymentReplyServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
     class Fake_TSH_MSG6_AgentPaymentMessageService
    {
        private TSH_MSG7_AgentPaymentReplyAgentPaymentReply _agentPaymentReply;
        private TSH_MSG7_AgentPaymentReplyAgentPaymentMethods[] _agentPaymentMethods;
        internal TSH_MSG7_AgentPaymentReply GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            SetAgentPaymentReply(requestParamsData);
            SetAgentPaymentMethods(requestParamsData);
            TSH_MSG7_AgentPaymentReply fake = new TSH_MSG7_AgentPaymentReply()
            {
                AgentPaymentReply = _agentPaymentReply,
                AgentPaymentMethods= _agentPaymentMethods
            };

            return fake;
        }
        public void SetAgentPaymentReply(GenericRequestParams requestParamsData)
        {
            _agentPaymentReply = new TSH_MSG7_AgentPaymentReplyAgentPaymentReply
            {
                paymentID=9999,
                status = 3
            };
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
    }
}