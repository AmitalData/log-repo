using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CommonIIGInterface;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public abstract class ResponseServiceBase<TResponseData,TCustomResponse,TRequestParams>
        where TRequestParams: RequestParamsBase
        where TCustomResponse: IINF_MSG_Generic
    {
        public TResponseData MyResponseData { get; set; } //need result on CH_NG_190_MSG1_NoticeToClientResponseService.Update call from worker role !!
        public RequestSheetParam MyRequestSheetParam { get; set; } 
        
        public abstract void Update(TCustomResponse customResponse,TRequestParams requestParams);
        public abstract TResponseData GetResponse(TCustomResponse customResponse, TRequestParams requestParams);


        public virtual void OnRequestFail(TCustomResponse customResponse, TRequestParams requestParams) {
            CustomsRequestsSheetDomainModelUtil.ReleaseConcurrentKey(requestParams);
            
        }

        public virtual Action<TCustomResponse> GetActionShrinkCustomResponse()
        {
            return null;
        }
    }
}
