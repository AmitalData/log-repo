
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
                 GetOptions =  new NG_9100_MSG_OutgoingMessageRequestGetOptions() { 
                      fromDate = requestParams.FromDate.Value,
                     toDate = requestParams.ToDate.Value,
                     ServiceName =requestParams.InterfaceManagementsCode,
                      
                      
                 },
                  PeekWay = new NG_9100_MSG_OutgoingMessageRequestPeekWay()
                  {
                      Peek_Way = 3,
                      Take = 99,
                  },
            };

           
            return customReq;
        }
    }
}

