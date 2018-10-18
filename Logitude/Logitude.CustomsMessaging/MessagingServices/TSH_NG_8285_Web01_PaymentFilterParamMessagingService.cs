
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.PaymentFilterParamServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TSH_NG_8285_Web01_PaymentFilterParamMessagingService
         : MessagingServiceBase<
        TSH_NG_8285_Web01_PaymentRequestParams,
        TSH_NG_8285_Web01_PaymentResponseData,
        TSH_NG_8285_Web01_PaymentFilterParam,
        TSH_NG_8286_Web02_PaymentList,
        TSH_NG_8285_Web01_PaymentFilterParamRequestService,
        TSH_NG_8286_Web02_PaymentListResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "8285"; } }

        public static TSH_NG_8285_Web01_PaymentResponseData SendInteractive(TSH_NG_8285_Web01_PaymentRequestParams searchParams) 
        {
            
            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;

            var myTSH_NG_8285_Web01_PaymentFilterParamMessagingService = new Logitude.CustomsMessaging.MessagingServices.TSH_NG_8285_Web01_PaymentFilterParamMessagingService();
            var resData = myTSH_NG_8285_Web01_PaymentFilterParamMessagingService.Send(searchParams);
            return resData;

        }


        protected override TSH_NG_8286_Web02_PaymentList CallWS(
            TSH_NG_8285_Web01_PaymentFilterParam customRequest, 
            TSH_NG_8285_Web01_PaymentRequestParams requestParams, 
            out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TSH_NG_8286_Web02_PaymentList();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            
            customRequest.RequestContentHeader = new  RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
           
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IPaymentFilterParamOperation>()
                    .PaymentFilterParamOperation(
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
