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
using UnifreightIIG.Common.CustomItemRuleServiceReference;
using UnifreightIIG.Common.MessageLib.Claim;
using UnifreightIIG.Common.MessageLib.EntryExit;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInGet_CB_MSG_8319_CustomItemRuleMessagingService : MessagingServiceBase<
        CustomItemRuleRequestParams,
        CustomItemRuleResponseData,
        CB_NG_8319_CustomItemRuleIn,
        CB_NG_8319_CustomItemRuleOut,
         Get_CB_MSG_8319_CustomItemRuleRequestService,
        Get_CB_MSG_8319_CustomItemRuleResponseService, 
        DCAInRequestHeader>
    {
        protected override CB_NG_8319_CustomItemRuleOut CallWS(CB_NG_8319_CustomItemRuleIn customRequest, CustomItemRuleRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check            
            var response = new CB_NG_8319_CustomItemRuleOut();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICustomItemRuleOperation>()
                    .CustomItemRule(
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
            get { return "8319"; }
        }

        protected override CustomItemRuleRequestParams CreateDefaultRequestParamsFromCustomsResponse(CB_NG_8319_CustomItemRuleOut customsResponse)
        {
            return null;
        }
    }
}
