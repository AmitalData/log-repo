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
using UnifreightIIG.Common.LogisticActionRequestMessageDecision;
using UnifreightIIG.Common.MessageLib.LogisticActionRequestMessage;

namespace Logitude.CustomsMessaging.MessagingServices
{// moran 25.1.15 - Task 9967
    public class DCAInLG_NG_8410_LogisticActionRequestMessageMessagingService 
        //: MessagingServiceBase<
        //GenericRequestParams,
        //INF_MSG_GenericResponseData,
        //LG_NG_8410_LogisticActionRequestMessage,
        //LogisticActionRequestRequestParams,
        //LG_NG_8410_LogisticActionRequestMessageRequestService,
        //LG_NG_8411_SendLogisticActionRequestDecisionResponseService, 
        //DCAInRequestHeader>
    {

        //protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(LogisticActionRequestRequestParams customsResponse)
        //{
        //    var myGenericRequestParams = new GenericRequestParams()
        //    {
        //        LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.LogisticActionRequest"),
        //    };
        //    return myGenericRequestParams;

        //}

        //protected override LogisticActionRequestRequestParams CallWS(LG_NG_8410_LogisticActionRequestMessage customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        //{
        //    exceptionMessage = null; // to check
        //    var response = new LogisticActionRequestRequestParams();
        //    return response;
        //}

        //public override string MainInterfaceCode { get { return "8410"; } }

    }
}
