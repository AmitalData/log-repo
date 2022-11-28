using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.Claim;
using UnifreightIIG.Common.MessageLib.EntryExit;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInCC_MSG1_EntryExitToFromCustomsStorageSitesMessageMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CC_MSG1_EntryExitToFromCustomsStorageSitesMessage,
        DCAInCustomRequestService,
        CC_MSG1_EntryExitToFromCustomsStorageSitesMessageResponseService, DCAInRequestHeader>
    {
        protected override CC_MSG1_EntryExitToFromCustomsStorageSitesMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "1050"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CC_MSG1_EntryExitToFromCustomsStorageSitesMessage customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }
}
