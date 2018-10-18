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
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.MessageLib.Client;
using UnifreightIIG.Common.MessageLib.Vendor;

namespace Logitude.CustomsMessaging.MessagingServices
{
    // moran 11.1.15 - Task 9921
    public class DCAInLO_NG_3720_MSG313_PoaUpdateForCustomsAgentMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        LO_NG_3720_MSG313_PoaUpdateForCustomsAgent,
        DCAInCustomRequestService,
        LO_NG_3720_MSG313_PoaUpdateForCustomsAgentResponseService, DCAInRequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "3720"; }
        }

        protected override LO_NG_3720_MSG313_PoaUpdateForCustomsAgent CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; 
            var response = new LO_NG_3720_MSG313_PoaUpdateForCustomsAgent();
            
            return response;
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(LO_NG_3720_MSG313_PoaUpdateForCustomsAgent customsResponse)
        {
            var tableName = "Customs.Client";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.POA.authorizedExternalId.ToString(),
            };
            return myGenericRequestParams;
        }

    }
}
