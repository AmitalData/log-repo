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
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;
using UnifreightIIG.Common.MessageLib.PhysicalCheck190;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInTSH_MSG2_3050_PaymentOrderReplyMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        CH_NG_190_MSG1_NoticeToClient,
        DCAInCustomRequestService,
        CH_NG_190_MSG1_NoticeToClientResponseService, DCAInRequestHeader>
        
        
        
        
        
        /*: MessagingServiceBase<
        NewPaymentRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        TSH_MSG2_PaymentOrderReply,
        DCAInCustomRequestService,
        TSH_MSG2_3050_PaymentOrderReplyResponseService, DCAInRequestHeader>*/
    {

        /*protected override TSH_MSG2_PaymentOrderReply CallWS(DCAInCustomRequest customRequest, NewPaymentRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "3050"; }
        }

        protected override NewPaymentRequestParams CreateDefaultRequestParamsFromCustomsResponse(TSH_MSG2_PaymentOrderReply customsResponse)
       {
            var tableName = "Customs.PaymentOrder";

            var myGenericRequestParams = new NewPaymentRequestParams()
            {
                LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
                RequestParamsVersion = 1,
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }*/
        protected override CH_NG_190_MSG1_NoticeToClient CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { throw new NotImplementedException(); }
        }
    }
}
