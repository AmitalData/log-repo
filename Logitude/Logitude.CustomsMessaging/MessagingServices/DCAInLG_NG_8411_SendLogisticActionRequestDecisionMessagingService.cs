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
//using UnifreightIIG.Common.LogisticActionRequestMessageDecision;
//using UnifreightIIG.Common.MessageLib.LogisticActionRequestMessage;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInLG_NG_8411_SendLogisticActionRequestDecisionMessagingService 
        //: MessagingServiceBase<
        //GenericRequestParams,
        //INF_MSG_GenericResponseData,
        //DCAInCustomRequest,
        //LG_NG_8411_SendLogisticActionRequestDecision,
        //DCAInCustomRequestService,
        //LG_NG_8411_SendLogisticActionRequestDecisionResponseService,
        //DCAInRequestHeader
        //>
    {
        //public override string MainInterfaceCode { get { return "8411"; } }


        //protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(LG_NG_8411_SendLogisticActionRequestDecision customsResponse)
        //{
        //    var myGenericRequestParams = new GenericRequestParams()
        //    {
        //        LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.LogisticActionRequest"),
        //    };
        //    return myGenericRequestParams;

        //}


        //protected override LG_NG_8411_SendLogisticActionRequestDecision CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        //{
        //    exceptionMessage = null; // to check
        //    var response = new LG_NG_8411_SendLogisticActionRequestDecision();
        //    return response;
        //}


    }
}
