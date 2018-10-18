using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using UnifreightIIG.Common.MessageLib.Claim;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInCLAIM_2300_MissingDocumentRequestMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CLAIM_MSG7_MissingDocumentRequest,
        DCAInCustomRequestService,
        CLAIM_2300_MissingDocumentRequestResponseService, DCAInRequestHeader>
    {
        protected override CLAIM_MSG7_MissingDocumentRequest CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "2300"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CLAIM_MSG7_MissingDocumentRequest customsResponse)
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
