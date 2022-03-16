
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInLG_NG_8410_LogisticActionRequestMessageMessagingService
        : MessagingServiceBase<
        LogisticActionRequestRequestParams,
        INF_MSG_GenericResponseData,
        LG_NG_8410_LogisticActionRequestMessage,
        INF_MSG_Generic,
        LG_NG_8410_LogisticActionRequestMessageRequestService,
        LG_NG_8410_LogisticActionRequestMessageResponseService,
        RequestHeader>
    {
        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(INF_MSG_Generic customsResponse)
        {
            ICustomContext dbContext = CustomContext.GetContext(RequestParams.Tenant);
            var logisticActionRequestPM = new LogisticActionRequestQueryService(RequestParams.Tenant).GetSingle(RequestParams.LogisticActionRequestId, false, false);

            logisticActionRequestPM.OperationalStatus = "נכשלה";
            logisticActionRequestPM.ChangeSetOp = ChangeSetOperation.Update;
            new LogisticActionRequestUpdateService(dbContext, new Dictionary<string, IContext>(), RequestParams.Tenant)
                .Update(logisticActionRequestPM, true);

            return base.GetIIGBLExceptionFromReponseHeader(customsResponse);
        }


        protected override LogisticActionRequestRequestParams CreateDefaultRequestParamsFromCustomsResponse(INF_MSG_Generic customsResponse)
        {
            var myRequestParams = new LogisticActionRequestRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.LogisticActionRequest"),
            };
            return myRequestParams;
        }


        protected override INF_MSG_Generic CallWS(LG_NG_8410_LogisticActionRequestMessage customRequest, LogisticActionRequestRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check            
            var response = new INF_MSG_Generic();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ILogisticActionRequestMessageOperation>()
                    .LogisticActionRequestMessage(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }


        public override string MainInterfaceCode { get { return "8410"; } }
    }
}
