using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.MessageLib.CargoTracking;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{// moran 25.1.15 - Task 9967
    public class DCAInLP_NG_8400_MSG01_LogisticPermitMessageMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        LP_NG_8400_MSG01_LogisticPermitMessage,
        DCAInCustomRequestService,
        LP_NG_8400_MSG01_LogisticPermitMessageResponseService, DCAInRequestHeader>

    {

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(LP_NG_8400_MSG01_LogisticPermitMessage customsResponse)
        {
            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
            };
            return myGenericRequestParams;

        }

        protected override LP_NG_8400_MSG01_LogisticPermitMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check
            var response = new LP_NG_8400_MSG01_LogisticPermitMessage();
            return response;
        }

        public override string MainInterfaceCode { get { return "8400"; } }

    }
}
