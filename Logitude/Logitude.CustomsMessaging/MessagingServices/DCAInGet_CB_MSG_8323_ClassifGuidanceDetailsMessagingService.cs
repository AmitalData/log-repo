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
using UnifreightIIG.Common.MessageLib.Claim;
using UnifreightIIG.Common.MessageLib.EntryExit;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInGet_CB_MSG_8323_ClassifGuidanceDetailsMessagingService : MessagingServiceBase<
        GetClassifGuidanceDetailsRequestParams,
        GetClassifGuidanceDetailsResponseData,
        CB_NG_8323_ClassifGuidanceDetailsIn,
        CB_NG_8323_ClassifGuidanceDetailsOut, 
         Get_CB_MSG_8323_ClassifGuidanceDetailsRequestService,
        Get_CB_MSG_8323_ClassifGuidanceDetailsResponseService, 
        DCAInRequestHeader>
    {
        protected override CB_NG_8323_ClassifGuidanceDetailsOut CallWS(CB_NG_8323_ClassifGuidanceDetailsIn customRequest, GetClassifGuidanceDetailsRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null; // to check            
            var response = new CB_NG_8323_ClassifGuidanceDetailsOut();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IClassifGuidanceDetailsOperation>()
                    .ClassifGuidanceDetails(
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
            get { return "8323"; }
        }

        protected override GetClassifGuidanceDetailsRequestParams CreateDefaultRequestParamsFromCustomsResponse(CB_NG_8323_ClassifGuidanceDetailsOut customsResponse)
        {
            return null;
        }
    }
}
