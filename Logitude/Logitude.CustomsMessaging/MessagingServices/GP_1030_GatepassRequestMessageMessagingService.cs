using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.GatepassFeedbackMServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class GP_1030_GatepassRequestMessageMessagingService : MessagingServiceBase<
        GatepassRequestMessageRequestParams,
        GatepassFeedbackMessageResponseData,
        GP_NG_1030_MSG1_GatepassRequestMessage,
        GP_NG_1035_MSG2_GatepassFeedbackMessage,
        GP_1030_GatepassRequestMessageRequestService,
        GP_1035_GatepassFeedbackMessageResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "1030"; }
        }

        protected override GP_NG_1035_MSG2_GatepassFeedbackMessage CallWS(GP_NG_1030_MSG1_GatepassRequestMessage customRequest, GatepassRequestMessageRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new GP_NG_1035_MSG2_GatepassFeedbackMessage();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGatepassFeedbackMOperation>()
                    .GatepassFeedbackM(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override bool? IsOurEnvironment(GP_NG_1035_MSG2_GatepassFeedbackMessage customsResponse, GatepassRequestMessageRequestParams RequestParams)
        {
            return false;
        }
    }
}
