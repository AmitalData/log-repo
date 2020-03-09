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
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInDEPO_MSG2030_DepositRequestCreatedInfoMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg,
        DCAInCustomRequestService,
        DEPO_MSG2030_DepositRequestCreatedInfoResponseService, DCAInRequestHeader>
    {
        protected override DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "2030"; }
        }
        protected override DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            var MyFakeDCA_MSG2030_DepositRequestCreatedInfoMessagingServices = new FakeDCA_MSG2030_DepositRequestCreatedInfoMessagingServices();
            return MyFakeDCA_MSG2030_DepositRequestCreatedInfoMessagingServices.GetFakeCustomsResponse(requestParamsData);


        }
        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg customsResponse)
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
