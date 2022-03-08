using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;

using Simplog.Data.InfrastructureModel.Repositories;
using UnifreightIIG.Common.LogisticActionRequestMessageDecision;

namespace Logitude.CustomsMessaging.MessagingServices
{ //9022
    public class DCAInLG_NG_8411_SendLogisticActionRequestDecisionMessagingService : 
    MessagingServiceBase<
        LogisticActionRequestRequestParams,
        LogisticActionRequestResponseData,
        LG_NG_8411_SendLogisticActionRequestDecision,
        LG_NG_8411_SendLogisticActionRequestDecision,
        DCAInCustomRequestService,
        LG_NG_8411_SendLogisticActionRequestDecisionRequestService,
        RequestHeader
    >
    {

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(LG_NG_8411_SendLogisticActionRequestDecision customsResponse)
        {
            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
            };
            return myGenericRequestParams;

        }

        protected override LG_NG_8411_SendLogisticActionRequestDecision CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check
            var response = new LG_NG_8411_SendLogisticActionRequestDecision();
            return response;
        }

        public override string MainInterfaceCode { get { return "8411"; } }

    }
}
