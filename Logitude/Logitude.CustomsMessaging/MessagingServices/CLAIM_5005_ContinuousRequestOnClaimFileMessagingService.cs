using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ContinuousRequestOnClaimFileServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    class CLAIM_5005_ContinuousRequestOnClaimFileMessagingService : MessagingServiceBase<
        ContinuousRequestOnClaimFileRequestParams,
        ContinuousResponseOnClaimFileResponseData,
        CLAIM_MSG9_ContinuousRequestOnClaimFile,
        CLAIM_MSG13_ContinuousResponseOnClaimFile,
        CLAIM_5005_ContinuousRequestOnClaimFileRequestService,
        CLAIM_5013_ContinuousResponseOnClaimFileResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode => throw new NotImplementedException();

        protected override CLAIM_MSG13_ContinuousResponseOnClaimFile CallWS(CLAIM_MSG9_ContinuousRequestOnClaimFile customRequest, ContinuousRequestOnClaimFileRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }
}
