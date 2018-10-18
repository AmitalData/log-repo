using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SaveCLAIMServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TPG_NG_8244_ClaimFileFilterParamRequestService : RequestServiceBase
        <TPG_NG_8244_Web01_ClaimFileFilterParam, TPG_NG_8244_ClaimFileFilterRequestParams>
    {
        public override TPG_NG_8244_Web01_ClaimFileFilterParam GetRequest(TPG_NG_8244_ClaimFileFilterRequestParams requestParams)
        {
            TPG_NG_8244_Web01_ClaimFileFilterParam myClaimFileFilterParams = new TPG_NG_8244_Web01_ClaimFileFilterParam();
            myClaimFileFilterParams.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myClaimFileFilterParams.TPGIdentifier = new TPGIdentifier();
            myClaimFileFilterParams.TPGIdentifier.fileNumber = requestParams.FileNumber;
            if (requestParams.Numeral > 0)
            {
                myClaimFileFilterParams.TPGIdentifier.numeral = (int)requestParams.Numeral;
            }
            myClaimFileFilterParams.TPGIdentifier.FillingNumber = requestParams.FillingNumber;

            return myClaimFileFilterParams;
        }
    }
}
