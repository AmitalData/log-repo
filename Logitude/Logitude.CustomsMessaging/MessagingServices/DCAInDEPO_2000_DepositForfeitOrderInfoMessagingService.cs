


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
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDEPO_2000_DepositForfeitOrderInfoMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DEPO_NG_2000_MSG13_DepositForfeitOrderInfo,
        DCAInCustomRequestService,
        DEPO_2000_MSG13_DepositForfeitOrderInfoResponseService, DCAInRequestHeader>
    {

        protected override DEPO_NG_2000_MSG13_DepositForfeitOrderInfo CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "2000"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DEPO_NG_2000_MSG13_DepositForfeitOrderInfo customsResponse)
        {
            var tableName = "Customs.Deposit";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
    }
}
