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

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class LG_NG_8411_SendLogisticActionRequestDecisionResponseService 
        //: ResponseServiceBase<INF_MSG_GenericResponseData, LG_NG_8411_SendLogisticActionRequestDecision, GenericRequestParams>
    {
        //public override INF_MSG_GenericResponseData GetResponse(LG_NG_8411_SendLogisticActionRequestDecision customResponse, GenericRequestParams requestParams)
        //{
        //    return this.MyResponseData;
        //}

        //public override void Update(LG_NG_8411_SendLogisticActionRequestDecision customResponse, GenericRequestParams requestParams)
        //{
        //    UpdateLogisticActionRequesrTable(customResponse, requestParams);

        //    this.MyResponseData = new INF_MSG_GenericResponseData();
        //    this.MyResponseData.Succeeded = true;
        //    this.MyResponseData.HasException = false;
        //}

        private static void UpdateLogisticActionRequesrTable(LG_NG_8411_SendLogisticActionRequestDecision customResponse, GenericRequestParams requestParams)
        {
            LogisticActionRequestQueryService larQs = new LogisticActionRequestQueryService(requestParams.Tenant);
            var cargoIdentifier = customResponse.LogisticActionRequestDecision.CargoIdentifier;
            LogisticActionRequestPM larPM = larQs.GetByCargoKey(cargoIdentifier.cargoIdentifierKey1, cargoIdentifier.cargoIdentifierKey2, cargoIdentifier.cargoIdentifierKey3, cargoIdentifier.cargoIdentifierType);
            
            if (larPM == null) return;

            var resData = customResponse.LogisticActionRequestDecision;

            larPM.RequestNumber = resData.RequestNumber.ToString();
            larPM.ResponseStatusCode = resData.ResponseStatus.ToString();
            larPM.CustomsUserName = resData.UserName;
            larPM.DecisionRmarks = resData.DecisionRmarks;

            UpdateLARPM(requestParams, larPM);
        }

        private static void UpdateLARPM(GenericRequestParams requestParams, LogisticActionRequestPM larPM)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var logisticActionRequestUpdateService = new LogisticActionRequestUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            larPM.ChangeSetOp = ChangeSetOperation.Update;
            logisticActionRequestUpdateService.Update(larPM, true);
        }
    }
}
