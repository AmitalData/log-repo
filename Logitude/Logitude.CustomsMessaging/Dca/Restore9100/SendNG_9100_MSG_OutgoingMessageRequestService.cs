using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.Dca.Restore9100
{
    public class SendNG_9100_MSG_OutgoingMessageRequestService
    {

        public 
            (NG_9101_MSG_OutgoingMessageResponse response,IResponseHeaderOrFault _ResponseHeader, string exceptionMessage) 
            CallWS(
            NG_9100_MSG_OutgoingMessageRequest customRequest, 
            CustomsSettingPM customsSetting,
            MoreParams _IIGGatewayMoreParams,
            string RequestsSheetExternalId
            )
        {
            IResponseHeaderOrFault _ResponseHeader = null;
            string exceptionMessage = null;
            var response = new NG_9101_MSG_OutgoingMessageResponse();

            


            using (var uifreightSdkGateway = new UnifreightSdkGateway(customsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IOutgoingMessageRequestOperation>()
                    .OutgoingMessageRequest(
                    RequestsSheetExternalId,
                    customsSetting.CustomsAgentId,
                    customRequest,
                    ref _IIGGatewayMoreParams,
                    out response);
            }

            return (response, _ResponseHeader, exceptionMessage);
        }
    }
    
}
