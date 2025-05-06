using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using UnifreightIIG.Common.ClassifGuidanceDetailsServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.CustomItemClassifGuidanceServiceReference;
using UnifreightIIG.Common.CustomItemMekachServiceReference;
using UnifreightIIG.Common.MessageLib.Claim;
using UnifreightIIG.Common.MessageLib.EntryExit;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInGet_CB_MSG_8318_CustomItemMekachMessagingService : MessagingServiceBase<
        CustomItemMekachRequestParams,
        CustomItemMekachResponseData,
        CB_NG_8318_CustomItemMekachIn,
        CB_NG_8318_CustomItemMekachOut,
         Get_CB_MSG_8318_CustomItemMekachRequestService,
        Get_CB_MSG_8318_CustomItemMekachResponseService, 
        DCAInRequestHeader>
    {
        protected override CB_NG_8318_CustomItemMekachOut CallWS(CB_NG_8318_CustomItemMekachIn customRequest, CustomItemMekachRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check            
            var response = new CB_NG_8318_CustomItemMekachOut();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICustomItemMekachOperation>()
                    .CustomItemMekach(
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
            get { return "8318"; }
        }

        protected override CustomItemMekachRequestParams CreateDefaultRequestParamsFromCustomsResponse(CB_NG_8318_CustomItemMekachOut customsResponse)
        {
            return null;
        }
    }
}
