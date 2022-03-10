using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using UnifreightIIG.Common.MessageLib.LogisticActionRequestDecision;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInLG_NG_8411_SendLogisticActionRequestDecisionMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        LG_NG_8411_SendLogisticActionRequestDecision,
        DCAInCustomRequestService,
        LG_NG_8411_SendLogisticActionRequestDecisionResponseService,
        DCAInRequestHeader
        >
    {
        public override string MainInterfaceCode { get { return "8411"; } }


        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(LG_NG_8411_SendLogisticActionRequestDecision customsResponse) =>
             new GenericRequestParams { LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.LogisticActionRequest") };                    


        protected override LG_NG_8411_SendLogisticActionRequestDecision CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check
            return new LG_NG_8411_SendLogisticActionRequestDecision();
        }
    }
}
