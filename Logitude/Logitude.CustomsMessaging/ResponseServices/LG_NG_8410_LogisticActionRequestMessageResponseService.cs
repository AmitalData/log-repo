using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.CargoTracking;
using UnifreightIIG.Common.LogisticActionRequestMessageDecision;
using UnifreightIIG.Common.MessageLib.LogisticActionRequestMessage;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class LG_NG_8410_LogisticActionRequestMessageResponseService :
      ResponseServiceBase<INF_MSG_GenericResponseData, LG_NG_8410_LogisticActionRequestMessage, GenericRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(LG_NG_8410_LogisticActionRequestMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(LG_NG_8410_LogisticActionRequestMessage customResponse, GenericRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
        } 
    }
}
