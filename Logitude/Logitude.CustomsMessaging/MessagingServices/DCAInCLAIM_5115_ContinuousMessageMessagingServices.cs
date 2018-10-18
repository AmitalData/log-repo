using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using UnifreightIIG.Common.MessageLib.Claim;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInCLAIM_5115_ContinuousMessageMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CLAIM_MSG8_ContinuousMessage,
        DCAInCustomRequestService,
        CLAIM_5115_ContinuousMessageResponseService, DCAInRequestHeader>
    {
        protected override CLAIM_MSG8_ContinuousMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new System.NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "5115"; }
        }
        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CLAIM_MSG8_ContinuousMessage customsResponse)
        {
            var tableName = "Customs.Claim";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }
}
