
//using UnifreightIIG.Common.MessageLib.Deposit;
#if true

using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.MessagingServices
{


    //public class DCAInDOC_NG_5101_GNMessageToAgentMessageService
    //: MessagingServiceBase<
    //   GenericRequestParams, INF_MSG_GenericResponseData,
    //   DCAInCustomRequest, DOC_NG_5101_GNMessageToAgent,
    //   DCAInCustomRequestService, DOC_NG_5101_GNMessageToAgentResponseService,
    //   DCAInRequestHeader>


        public class DCAInDEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequestMessagingServices : MessagingServiceBase<
        GenericRequestParams,INF_MSG_GenericResponseData,
        DCAInCustomRequest,DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequest,
        DCAInCustomRequestService, Logitude.CustomsMessaging.ResponseServices.DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequestResponseService, 
        DCAInRequestHeader>
    {
        protected override DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequest CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "2753"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateRequest customsResponse)
        {
            var tableName = "Customs.Tapag";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
    }


    //public class DEPO_NG_2753_MSG4_DepositBankAccountToRefundUpdateResponseService { }
}



#endif