using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessageAnalyzer;
using Logitude.CustomsMessaging.RequestServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Gurntee;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInGRNT_1812_CreateGurateeRequestInfoMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        GRNT_MSG15_createGurateeRequestInfo,
        DCAInCustomRequestService,
        GRNT_MSG15_createGurateeRequestInfoResponseService, DCAInRequestHeader>
    {
        protected override GRNT_MSG15_createGurateeRequestInfo CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "1812"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(GRNT_MSG15_createGurateeRequestInfo customsResponse)
        {
            var tableName = "Customs.Guarantee";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }
}
