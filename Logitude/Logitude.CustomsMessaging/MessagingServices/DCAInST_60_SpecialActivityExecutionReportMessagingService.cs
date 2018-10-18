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
using UnifreightIIG.Common.MessageLib.Storage;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInST_60_SpecialActivityExecutionReportMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        ST_NG_60_MSG9_SpecialActivityExecutionReportMessage,
        DCAInCustomRequestService,
        ST_NG_60_SpecialActivityExecutionReportResponseService, DCAInRequestHeader>
    {
        protected override ST_NG_60_MSG9_SpecialActivityExecutionReportMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "60"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(ST_NG_60_MSG9_SpecialActivityExecutionReportMessage customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
    }
}
