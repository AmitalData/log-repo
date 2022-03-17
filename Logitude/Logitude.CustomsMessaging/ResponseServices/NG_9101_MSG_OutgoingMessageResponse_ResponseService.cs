
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageRestoreServiceReference;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class NG_9101_MSG_OutgoingMessageResponse_ResponseService :
  ResponseServiceBase<MessageWaitingResponseData, NG_9101_MSG_OutgoingMessageResponse, MessageWaitingRequestParams>
    {


        public override void Update(NG_9101_MSG_OutgoingMessageResponse customResponse, MessageWaitingRequestParams requestParams)
        {
            int messageRestoreCount = 0;
            var succeeded = false;
            //if (customResponse.MessageRestoreResponseOutput != null)
            //{
            //    succeeded = true;
            //    messageRestoreCount = customResponse.MessageRestoreResponseOutput.NumOfResults;
            //}

            //this.MyResponseData = new MessageWaitingResponseData()
            //{
            //    Succeeded = succeeded,
            //    MessageRestoreCount = messageRestoreCount
            //};
        }

        public override MessageWaitingResponseData GetResponse(NG_9101_MSG_OutgoingMessageResponse customResponse, MessageWaitingRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}

