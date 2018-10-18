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
using UnifreightIIG.Common.MessageLib.Collateral;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDF_8213_CollateralAnswerApprovalMsgMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        COLT_NG_8213_MSG10042_CollateralAnswerApprovalMsg,
        DCAInCustomRequestService,
        DF_8213_CollateralAnswerApprovalMsgResponseService, DCAInRequestHeader>
    {
        protected override COLT_NG_8213_MSG10042_CollateralAnswerApprovalMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "8213"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(COLT_NG_8213_MSG10042_CollateralAnswerApprovalMsg customsResponse)
        {
            var tableName = "Customs.CustomsCollateral";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(), // to check with itzik about array
            };
            return myGenericRequestParams;
        }

        protected override RequestSheetParam GetSheetDetailsFromRequestParam(GenericRequestParams requestParams)
        {
            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                return null;
            }
            return new RequestSheetParam() { RequestDescription = "דרישה לבטוחה - " + requestParams.AppicationId }; // to check with itzik about array!!!
        }
    }
}
