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
    public class DCAInST_NG_10_SpecialActivityResponseMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        ST_NG_10_MSG08_SpecialActivityResponseMessage,
        DCAInCustomRequestService,
        ST_NG_10_SpecialActivityResponseMessageResponseService, DCAInRequestHeader>
    {
        protected override ST_NG_10_MSG08_SpecialActivityResponseMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "10"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(ST_NG_10_MSG08_SpecialActivityResponseMessage customsResponse)
        {
            var tableName = "Customs.Storage";

            var myGenericRequestParams = new GenericRequestParams()
            {
                //LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }
}
