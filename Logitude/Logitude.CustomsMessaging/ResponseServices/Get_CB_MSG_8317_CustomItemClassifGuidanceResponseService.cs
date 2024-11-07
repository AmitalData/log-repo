using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference;
using UnifreightIIG.Common.CustomItemLegalDemandsServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Get_CB_MSG_8317_CustomItemClassifGuidanceResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, CB_NG_8317_CustomItemClassifGuidanceOut, GenericRequestParams>
    {
        public override void Update(CB_NG_8317_CustomItemClassifGuidanceOut customResponse, GenericRequestParams requestParams)
        {
            
        }

        public override INF_MSG_GenericResponseData GetResponse(CB_NG_8317_CustomItemClassifGuidanceOut customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
