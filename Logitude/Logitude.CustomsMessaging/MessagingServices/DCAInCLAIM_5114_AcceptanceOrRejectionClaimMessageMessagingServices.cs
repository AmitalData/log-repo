using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Claim;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInCLAIM_5114_AcceptanceOrRejectionClaimMessageMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CLAIM_MSG10_AcceptanceOrRejectionClaimMessage,
        DCAInCustomRequestService,
        CLAIM_5114_AcceptanceOrRejectionClaimMessageResponseService, DCAInRequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "5114"; }
        }

        protected override CLAIM_MSG10_AcceptanceOrRejectionClaimMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CLAIM_MSG10_AcceptanceOrRejectionClaimMessage customsResponse)
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
