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
    public class DCAInDEPO_NG_5110_DepositRequestFulfillednfoMsgMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg,
        DCAInCustomRequestService,
        DEPO_NG_5110_DepositRequestFulfillednfoMsgResponseService, DCAInRequestHeader>
    {
        protected override DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "5110"; }
        }
        protected override DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            var MyFake_DCA_NG_5110_DepositRequestFulfillednfoMsgMessagingService = new Fake_DCA_NG_5110_DepositRequestFulfillednfoMsgMessagingService();
            return MyFake_DCA_NG_5110_DepositRequestFulfillednfoMsgMessagingService.GetFakeCustomsResponse(requestParamsData);

        }
        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg customsResponse)
        {
            var tableName = "Customs.Deposit";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }
}
