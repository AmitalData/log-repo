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
    public class DCAInDF_8211_CollateralRequestMsgMessagingServices: MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        COLT_NG_8211_MSG10040_CollateralRequestMsg,
        DCAInCustomRequestService,
        DF_8211_CollateralRequestMsgResponseService, DCAInRequestHeader>
    {

        protected override COLT_NG_8211_MSG10040_CollateralRequestMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "8211"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(COLT_NG_8211_MSG10040_CollateralRequestMsg customsResponse)
        {
            var tableName = "Customs.CustomsCollateral";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(), // to check with itzik about array
            };
            return myGenericRequestParams;
        }
    }
}
