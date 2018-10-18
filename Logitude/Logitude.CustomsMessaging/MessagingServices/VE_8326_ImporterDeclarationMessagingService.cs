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
using UnifreightIIG.Common.ImporterDeclarationsDetailServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class VE_8326_ImporterDeclarationMessagingService : MessagingServiceBase<
        ImporterDeclarationRequestParams,
        ImporterDeclarationResponseData,
        VE_NG_8326_Web01_ImporterDeclarationsParam,
        VE_NG_8327_Web02_ImporterDeclarationsDetail,
        VE_8326_ImporterDeclarationRequestService,
        VE_8327_ImporterDeclarationResponseService, RequestHeader>
    {
        protected override VE_NG_8327_Web02_ImporterDeclarationsDetail CallWS(VE_NG_8326_Web01_ImporterDeclarationsParam customRequest, ImporterDeclarationRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new VE_NG_8327_Web02_ImporterDeclarationsDetail();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IImporterDeclarationsDetailOperation>()
                    .GetImporterDeclarationsDetail(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            var test = false;
            if (test)
            {
                response.Exception = null;
                response.ResponseContentHeader = null;
            }
            return response;
        }

        public override string MainInterfaceCode
        {
            get { return "8326"; }
        }
    }
}
