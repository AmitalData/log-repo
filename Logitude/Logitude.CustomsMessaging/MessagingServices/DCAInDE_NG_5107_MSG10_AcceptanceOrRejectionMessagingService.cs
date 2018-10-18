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
using UnifreightIIG.Common.MessageLib.Deficit;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDE_NG_5107_MSG10_AcceptanceOrRejectionMessagingService
        : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DE_NG_5107_MSG10_AcceptanceOrRejectionMessage,
        DCAInCustomRequestService,
        DE_NG_5107_MSG10_AcceptanceOrRejectionMessageResponseService, DCAInRequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "5107"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DE_NG_5107_MSG10_AcceptanceOrRejectionMessage customsResponse)
        {
            var tableName = "Customs.Declaration";
            //ResolveTenant() ==CustomsAgentToTenant(_CustomResponse.NoticeToClient.customsAgent);

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(),
                RequestName = "Acceptance of IMport dec # " + customsResponse.ResponseContentHeader.ApplicationID.ToString(),

            };
            return myGenericRequestParams;

        }

        protected override DE_NG_5107_MSG10_AcceptanceOrRejectionMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        /*protected override RequestSheetParam GetSheetDetailsFromRequestParam(GenericRequestParams requestParams)
        {
            var myRequestSheetParam = new Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam();
            myRequestSheetParam.RequestDescription = "מסר דחיה/אישור גרעון עצמי " + requestParams.AppicationId;
            return myRequestSheetParam;
        }*/
    }
}
