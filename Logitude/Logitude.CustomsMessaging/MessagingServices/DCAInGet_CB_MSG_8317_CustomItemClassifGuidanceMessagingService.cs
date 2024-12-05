using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference;
using UnifreightIIG.Common.MessageLib.Claim;
using UnifreightIIG.Common.MessageLib.EntryExit;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInGet_CB_MSG_8317_CustomItemClassifGuidanceMessagingService : MessagingServiceBase<
        GetCustomItemClassifGuidanceRequestParams,
        CustomItemClassifGuidanceResponseData,
        CB_NG_8317_CustomItemClassifGuidanceIn,
        CB_NG_8317_CustomItemClassifGuidanceOut,
        Get_CB_MSG_8317_CustomItemClassifGuidanceRequestService,
        Get_CB_MSG_8317_CustomItemClassifGuidanceResponseService, 
        DCAInRequestHeader>
    {
        protected override CB_NG_8317_CustomItemClassifGuidanceOut CallWS(CB_NG_8317_CustomItemClassifGuidanceIn customRequest, GetCustomItemClassifGuidanceRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check            
            var response = new CB_NG_8317_CustomItemClassifGuidanceOut();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICustomItemClassifGuidanceOperation>()
                    .CustomItemClassifGuidance(
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
            get { return "8317"; }
        }

        protected override GetCustomItemClassifGuidanceRequestParams CreateDefaultRequestParamsFromCustomsResponse(CB_NG_8317_CustomItemClassifGuidanceOut customsResponse)
        {
            return null;
        }
    }
}
