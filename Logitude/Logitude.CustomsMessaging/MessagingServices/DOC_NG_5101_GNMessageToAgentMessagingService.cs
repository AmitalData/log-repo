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
using UnifreightIIG.Common.MessageToAgentServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DOC_NG_5101_GNMessageToAgentMessagingService
        : MessagingServiceBase<
        MessageToAgentRequestParams, MessageToAgentResponseData,
        DOC_NG_5101_GNMessageToAgent, INF_MSG_Generic,
        DOC_NG_5101_GNMessageToAgent_RequestService, INF_MSG_Generic_DOC_NG_5101_GNMessageToAgent_ResponseService, 
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "5101O"; }
        }

        protected override INF_MSG_Generic CallWS(DOC_NG_5101_GNMessageToAgent customRequest, MessageToAgentRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();



            customRequest.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
            //myMP.MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.TestMode;



            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<
                    UnifreightIIG.Common.TheGateway.IMessageToAgentRequestOperation>()
                    .MessageToAgentRequestOperation(
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
