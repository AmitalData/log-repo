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
using UnifreightIIG.Common.MessageLib.ExportStorage.MN2791;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInMN_MSG2791_ExportDeliveryAnswerMessageMessagingServices
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        MN_MSG2791_ExportDeliveryAnswerMessage,
        DCAInCustomRequestService,
        MN_MSG2791_ExportDeliveryAnswerMessageResponseService, DCAInRequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "2791"; }
        }


        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(MN_MSG2791_ExportDeliveryAnswerMessage customsResponse)
        {
            var tableName = "Customs.Declaration";
            //ResolveTenant() ==CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(),
                //RequestName = "Acceptance of IMport dec # " + customsResponse.ResponseContentHeader.ApplicationID.ToString(),

            };
            return myGenericRequestParams;
        }

        
        protected override MN_MSG2791_ExportDeliveryAnswerMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }
}
