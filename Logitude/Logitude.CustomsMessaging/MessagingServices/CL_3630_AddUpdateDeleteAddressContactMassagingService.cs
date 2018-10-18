using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientChangeAddressContactPhoneServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CL_3630_AddUpdateDeleteAddressContactMassagingService : MessagingServiceBase<
        AddAddressContactForClient,
        INF_MSG_GenericResponseData,
        CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgent,
        INF_MSG_Generic,
        CL_3630_AddUpdateDeleteAddressContactRequestService,
        CL_3630_AddUpdateDeleteAddressContactResponseService, RequestHeader>
    {
        protected override INF_MSG_Generic CallWS(CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgent customRequest, AddAddressContactForClient requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IClientChangeAddressContactPhoneOperation>()
                    .ClientChangeAddressContactPhone(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        public override string MainInterfaceCode
        {
            get { return "3630"; } 
        }

    }
}
