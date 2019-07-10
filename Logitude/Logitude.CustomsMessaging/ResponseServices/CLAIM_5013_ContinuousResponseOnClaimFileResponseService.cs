using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ContinuousRequestOnClaimFileServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CLAIM_5013_ContinuousResponseOnClaimFileResponseService : ResponseServiceBase<ContinuousResponseOnClaimFileResponseData, CLAIM_MSG13_ContinuousResponseOnClaimFile, ContinuousRequestOnClaimFileRequestParams>
    {
        public override ContinuousResponseOnClaimFileResponseData GetResponse(CLAIM_MSG13_ContinuousResponseOnClaimFile customResponse, ContinuousRequestOnClaimFileRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(CLAIM_MSG13_ContinuousResponseOnClaimFile customResponse, ContinuousRequestOnClaimFileRequestParams requestParams)
        {
            throw new NotImplementedException();
        }
    }
}
