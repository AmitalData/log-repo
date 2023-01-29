using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomItemDetailsServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    
    public  class Get_CB_MSG_8314_8888_CustomItemDetailsHeaderResponseService: ResponseServiceBase<INF_MSG_GenericResponseData, CB_NG_8888_CustomsItemOut, CD_NG_8314_Web01_CustomsItemDetailsRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(
            CB_NG_8888_CustomsItemOut customResponse, CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams)
        {

            return this.MyResponseData;
        }

        public override void Update(CB_NG_8888_CustomsItemOut customResponse, CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
        }

    }
}
