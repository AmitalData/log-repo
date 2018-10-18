
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GuaranteeReturnRequestApprovalInfoServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class GRNT_5004_GuaranteeReturnResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, INF_MSG_Generic, GuaranteeReturnRequesRequestParams>
    {
        public override void Update(INF_MSG_Generic customResponse, GuaranteeReturnRequesRequestParams requestParams)
        {
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                //LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
        }

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, GuaranteeReturnRequesRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
