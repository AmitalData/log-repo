using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageRestoreServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
        public class SYSTBL_NG_9011_MSG_MessageRestore_ResponseService :
      ResponseServiceBase<MessageRestoreResponseData, SYSTBL_NG_9011_MSG_MessageRestoreResponse, MessageRestoreRequestParams>
    {


        public override void Update(SYSTBL_NG_9011_MSG_MessageRestoreResponse customResponse, MessageRestoreRequestParams requestParams)
        {
            int messageRestoreCount = 0;
            var succeeded = false;
            if (customResponse.MessageRestoreResponseOutput != null)
            {
                succeeded = true;
                messageRestoreCount = customResponse.MessageRestoreResponseOutput.NumOfResults;
            }

            this.MyResponseData = new MessageRestoreResponseData()
            {
                Succeeded = succeeded,
                MessageRestoreCount = messageRestoreCount
            };            
        }

        public override MessageRestoreResponseData GetResponse(SYSTBL_NG_9011_MSG_MessageRestoreResponse customResponse, MessageRestoreRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}

