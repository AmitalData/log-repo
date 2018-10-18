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
using UnifreightIIG.Common.ClientAddMessageServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CL_MSG100_AddClientMassagingService : MessagingServiceBase<
        CreateClientRequestParams,
        INF_MSG_GenericResponseData,
        CL_MSG100_AddClientMessage,
        INF_MSG_Generic,
        CL_MSG100_AddClientRequestService,
        CL_MSG100_AddClientResponseService, RequestHeader>
    {
        protected override INF_MSG_Generic CallWS(CL_MSG100_AddClientMessage customRequest, CreateClientRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IClientAddMessageOperation>()
                    .ClientAddMessage(
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
            get { return "3600"; }
        }
    }
}
