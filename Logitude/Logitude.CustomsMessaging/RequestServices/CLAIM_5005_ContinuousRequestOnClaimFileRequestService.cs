using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ContinuousRequestOnClaimFileServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CLAIM_5005_ContinuousRequestOnClaimFileRequestService : RequestServiceBase<CLAIM_MSG9_ContinuousRequestOnClaimFile, ContinuousRequestOnClaimFileRequestParams>
    {
        public override CLAIM_MSG9_ContinuousRequestOnClaimFile GetRequest(ContinuousRequestOnClaimFileRequestParams requestParams)
        {
            var myCLAIM_MSG9_ContinuousRequestOnClaimFile = new CLAIM_MSG9_ContinuousRequestOnClaimFile();
            myCLAIM_MSG9_ContinuousRequestOnClaimFile.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "בקשה לביטול/ערר תביעה " + requestParams.ClassificationCode;

            return myCLAIM_MSG9_ContinuousRequestOnClaimFile;
        }
    }
}
