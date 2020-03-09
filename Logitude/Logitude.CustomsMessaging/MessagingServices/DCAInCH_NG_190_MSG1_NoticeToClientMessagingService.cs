using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.FakeMessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.PhysicalCheck190;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInCH_NG_190_MSG1_NoticeToClientMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CH_NG_190_MSG1_NoticeToClient,
        DCAInCustomRequestService,
        CH_NG_190_MSG1_NoticeToClientResponseService, DCAInRequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "190"; }
        }
        protected override CH_NG_190_MSG1_NoticeToClient GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            var myFake_DCAInCH_NG_190_MSG1_NoticeToClient_Service = new Fake_DCAInCH_NG_190_MSG1_NoticeToClient_Service();
            return myFake_DCAInCH_NG_190_MSG1_NoticeToClient_Service.GetFakeCustomsResponse(requestParamsData);

            
        }
        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CH_NG_190_MSG1_NoticeToClient customsResponse)
        {
            var tableName="Customs.PhysicalCheck";
            //ResolveTenant() ==CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.NoticeToClient.checkId.ToString()
            };
            myGenericRequestParams.RequestName = @" זימון לבדיקה" + customsResponse.NoticeToClient.checkId.ToString();
            return myGenericRequestParams;

        }
        protected override CH_NG_190_MSG1_NoticeToClient CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
        protected override DcaReceivedController GetDcaReceivedController(CH_NG_190_MSG1_NoticeToClient customsResponse, GenericRequestParams RequestParams)
        {
            if (customsResponse== null)
            {
                return null;
            }
            if (customsResponse.NoticeToClient== null)
            {
                return null;
            }
            if (customsResponse.NoticeToClient.checkId==null)
            {
                return null;
            }
            return new DcaReceivedController() { DcaAnalyzeAggregateKey = customsResponse.NoticeToClient.checkId.ToString() };
        }

        /*protected override RequestSheetParam GetSheetDetailsFromRequestParam(GenericRequestParams requestParams)
        {
            var myRequestSheetParam = new Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam();
            myRequestSheetParam.RequestDescription = "זימון לבדיקה " + requestParams.AppicationId;
            myRequestSheetParam.EntityId1 = requestParams.AppicationId;
            myRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.PhysicalCheck");
            return myRequestSheetParam;
        }*/
    }
}
