
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class NG_9100_MSG_OutgoingMessageRequest_RequestService
        : RequestServiceBase<NG_9100_MSG_OutgoingMessageRequest, MessageWaitingRequestParams>
    {
        public override NG_9100_MSG_OutgoingMessageRequest GetRequest(MessageWaitingRequestParams requestParams)
        {

            var customReq = new NG_9100_MSG_OutgoingMessageRequest()
            {
                GetOptions = new NG_9100_MSG_OutgoingMessageRequestGetOptions()
                {

                    fromDate = (requestParams.FromDate != null) ? requestParams.FromDate.Value : DateTime.MinValue,
                    toDate = (requestParams.ToDate != null) ? requestParams.ToDate.Value : DateTime.MinValue,
                    CorrelationId = string.IsNullOrWhiteSpace(requestParams.CorrelationID) ? null : requestParams.CorrelationID,
                    ServiceName = string.IsNullOrWhiteSpace(requestParams.ServiceName) ? null : requestParams.ServiceName,


                },
                PeekWay = new NG_9100_MSG_OutgoingMessageRequestPeekWay()
                {
                    Peek_Way = 3,
                    Take = 999,
                },
            };


            //customReq = new NG_9100_MSG_OutgoingMessageRequest()
            //{
            //    GetOptions = new NG_9100_MSG_OutgoingMessageRequestGetOptions()
            //    {
            //        //fromDate = requestParams.FromDate.Value,
            //        //toDate = requestParams.ToDate.Value,
            //        //ServiceName = requestParams.InterfaceManagementsCode,
            //         CorrelationId= "caf3f024-f14a-4738-8c38-9ee57ac0f420"

            //    },
            //    PeekWay = new NG_9100_MSG_OutgoingMessageRequestPeekWay()
            //    {
            //        Peek_Way = 3,
            //        Take = 100,
            //    },
            //};

            return customReq;
        }
    }
}

