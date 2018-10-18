
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
   public class TSH_NG_3053_MSG8_AgentPaymentRequestMessageService
        : MessagingServiceBase<
        NewPaymentRequestParams,
        PaymentOrderReplyResponseData, //INF_MSG_GenericResponseData, //NewPaymentResponseData,
        TSH_NG_3053_MSG8_AgentPaymentRequest,
        TSH_MSG2_PaymentOrderReply,
        TSH_NG_3053_MSG8_AgentPaymentRequestService,
        TSH_MSG2_3050_PaymentOrderReplyResponseService, RequestHeader>
    {

       public override string MainInterfaceCode
       {
           get
           {
               return "3053";
           }
       }
       protected override RequestSheetParam GetSheetDetailsFromRequestParam(NewPaymentRequestParams requestParams)
       {
           if (string.IsNullOrWhiteSpace(requestParams.PaymentNumber))
           {
               return null;
           }
           var my = new Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam();
           my.RequestDescription = "הוראת תשלום " + requestParams.PaymentNumber;
           return my;
       }
       
       protected override NewPaymentRequestParams CreateDefaultRequestParamsFromCustomsResponse(TSH_MSG2_PaymentOrderReply customsResponse)
       {
           var tableName = "Customs.PaymentOrder";

           var myGenericRequestParams = new NewPaymentRequestParams()
           {
               LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
               RequestParamsVersion = 1,
           };
           return myGenericRequestParams;
       }

       protected override TSH_MSG2_PaymentOrderReply CallWS(
           TSH_NG_3053_MSG8_AgentPaymentRequest customRequest,
           NewPaymentRequestParams requestParams, 
           out string exceptionMessage)
       {
           exceptionMessage = null;
           var response = new TSH_MSG2_PaymentOrderReply();
           // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

           using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
           {
               _ResponseHeader = uifreightSdkGateway.GetChannel<IAgentPaymentRequestOperation>()
                   .AgentPaymentRequest(
                   this.RequestsSheetExternalId,
                   base.CustomsSetting.CustomsAgentId,
                   customRequest,
                   ref this._IIGGatewayMoreParams,
                   out response);
           }

           return response;
       }
    }
}
