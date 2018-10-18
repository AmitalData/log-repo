
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUniDebug01_MsgMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        UniDebug01_Msg,
        DCAInCustomRequestService,
        UniDebug01_MsgResponseService, DCAInRequestHeader>
    {
        
        public override string MainInterfaceCode
        {
            get { return "UIT01"; }
        }
        
        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(UniDebug01_Msg customsResponse)
        {
            var tableName = "Customs.Declaration";
            //ResolveTenant() ==CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);
            //var myDeclarationQueryService = new DeclarationQueryService(context);

            

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                ///LoggingEntityId = customsResponse.NoticeToClient.checkId.ToString()
            };
            myGenericRequestParams.RequestName = "UniDebug01 " + customsResponse.Remarks;
            return myGenericRequestParams;

        }
        protected override UniDebug01_Msg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
        protected override DcaReceivedController GetDcaReceivedController(UniDebug01_Msg customsResponse, GenericRequestParams RequestParams)
        {
            return new DcaReceivedController()
            {
                DcaAnalyzeAggregateKey = customsResponse.DeclarationID
            };
        }
       
    }
}
