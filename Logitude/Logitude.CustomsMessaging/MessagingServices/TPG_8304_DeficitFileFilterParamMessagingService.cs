using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.DeficitFileServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TPG_8304_DeficitFileFilterParamMessagingService : MessagingServiceBase<
        DeficitFileFilterRequestParams,
        DeficitFilesDetailResponseData,
        TPG_NG_8304_Web03_DeficitFileFilterParam,
        TPG_NG_8246_Web04_DeficitFilesDetail,
        TPG_8304_DeficitFileFilterParamRequestService,
        TPG_8246_DeficitFilesDetailResponseService, RequestHeader>
    {

        protected override TPG_NG_8246_Web04_DeficitFilesDetail CallWS(TPG_NG_8304_Web03_DeficitFileFilterParam customRequest, DeficitFileFilterRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TPG_NG_8246_Web04_DeficitFilesDetail();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IDeficitFileOperation>()
                    .DeficitFileOperation(
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
            get { return "8304"; }
        }
    }
}
