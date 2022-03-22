using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Common.RequestParams;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class LG_NG_8410_LogisticActionRequestMessageResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, LogisticActionRequestRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, LogisticActionRequestRequestParams requestParams) =>
            this.MyResponseData;        

        public override void Update(INF_MSG_Generic customResponse, LogisticActionRequestRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            bool hasException = customResponse.ResponseContentHeader.Exception != null;

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = hasException;
            this.MyResponseData.ApplicationID = requestParams.LogisticActionRequestId; 
            this.MyResponseData.UserMessage = hasException ? customResponse.ResponseContentHeader.Exception[0].ExeptionDescription : "המסר התקבל בהצלחה במכס";


            LogisticActionRequestQueryService logisticActionRequestQueryService = new LogisticActionRequestQueryService(requestParams.Tenant);
            LogisticActionRequestUpdateService logisticActionRequestUpdateService = new LogisticActionRequestUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var logisticActionRequestPM = logisticActionRequestQueryService.GetSingle(requestParams.LogisticActionRequestId, false, false);

            logisticActionRequestPM.OperationalStatus = (customResponse.ResponseContentHeader.Exception != null) ? "נכשלה" : "נשלחה";
            logisticActionRequestPM.ChangeSetOp = ChangeSetOperation.Update;
            logisticActionRequestUpdateService.Update(logisticActionRequestPM, true);
        }
    }
}
