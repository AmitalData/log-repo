using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestServices
{
    public abstract class RequestServiceBase<TCustomRequest, TRequestParams>
    {

        public RequestSheetParam MyRequestSheetParam { get; set; }
        public abstract TCustomRequest GetRequest(TRequestParams requestParams);

        public virtual void ManipulateRequestParams(TRequestParams requestParams)
        {
        }
        public virtual void PostGetRequest(TCustomRequest customRequest, TRequestParams requestParams)
        {

        }
        
        public virtual Action<TCustomRequest> GetActionShrinkCustomRequest()
        {
            return null;
        }
        public bool ToCancelSheetAfterGetRequest { protected set; get; }

        public virtual void OnRequestFail(TRequestParams requestParams)
        {
            CustomsRequestsSheetDomainModelUtil.ReleaseConcurrentKey((requestParams as RequestParamsBase));

        }

    }
}
