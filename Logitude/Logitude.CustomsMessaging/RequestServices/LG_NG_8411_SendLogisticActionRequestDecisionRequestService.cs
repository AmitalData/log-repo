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
using Logitude.CustomsMessaging.RequestServices;
using UnifreightIIG.Common.LogisticActionRequestMessageDecision;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class LG_NG_8411_SendLogisticActionRequestDecisionRequestService : RequestServiceBase<LG_NG_8411_SendLogisticActionRequestDecision, LogisticActionRequestRequestParams>
    {
        public override LG_NG_8411_SendLogisticActionRequestDecision GetRequest(LogisticActionRequestRequestParams requestParams)
        {
            var myMsg = new LG_NG_8411_SendLogisticActionRequestDecision();

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.LogisticActionRequest");
            //this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
            //this.MyRequestSheetParam.CustomFileNo = requestParams.CustomsFile;
            this.MyRequestSheetParam.RequestDescription = "בקשת ביטול יצוא";

            return myMsg;
        }

    }
}
