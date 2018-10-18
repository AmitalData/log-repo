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
using UnifreightIIG.Common.ClientSearchByIDServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CL_NG_8343_ClientSearchByIDParamMessagingService : MessagingServiceBase<
        ClientSearchRequestParams,
        ClientSearchByIDResponseData,
        CL_NG_8343_Web01_ClientSearchByIDParam,
        CL_NG_8344_Web02_ClientSearchByIDDetail,
        CL_NG_8343_ClientSearchByIDRequestService,
        CL_NG_8344_ClientSearchByIDResponseService, RequestHeader>
    {
        public override string MainInterfaceCode { get { return "8343"; } }

        protected override CL_NG_8344_Web02_ClientSearchByIDDetail CallWS(CL_NG_8343_Web01_ClientSearchByIDParam customRequest, ClientSearchRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CL_NG_8344_Web02_ClientSearchByIDDetail();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                try
                {
                    var cs = uifreightSdkGateway.GetChannel<IClientSearchByIDOperation>();
                    _ResponseHeader = cs.ClientSearchByID(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }

            return response;
        }
    }
}
