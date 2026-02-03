
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.BankAccountToRefundUpdateReplayServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TPG_NG_2018_BankAccountToRefundUpdateReplayResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, INF_MSG_Generic, BankAccountToRefundRequestParams>
    {
        public override void Update(INF_MSG_Generic customResponse, BankAccountToRefundRequestParams requestParams)
        {
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;

                return;
            }

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            if (!string.IsNullOrWhiteSpace(customResponse?.ResponseContentHeader?.Remark))
            {
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Remark;
                return;
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, BankAccountToRefundRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
