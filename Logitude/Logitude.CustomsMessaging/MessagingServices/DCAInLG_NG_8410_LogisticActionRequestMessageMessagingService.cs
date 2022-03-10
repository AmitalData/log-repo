
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;

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
            return new INF_MSG_Generic();
        }

        public override string MainInterfaceCode { get { return "8410"; } }
    }
}
