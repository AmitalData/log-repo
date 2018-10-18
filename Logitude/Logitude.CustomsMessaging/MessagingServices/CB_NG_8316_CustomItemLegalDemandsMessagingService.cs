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
using UnifreightIIG.Common.CustomItemLegalDemandsServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CB_NG_8316_CustomItemLegalDemandsMessagingService : MessagingServiceBase<
        CustomItemLegalDemandsRequestParams, CustomItemLegalDemandsResponseData,
        CB_NG_8316_CustomItemLegalDemandsIn, CB_NG_8316_CustomItemLegalDemandsOut,
        CB_NG_8316_CustomItemLegalDemandsRequestService, CB_NG_8316_CustomItemLegalDemandsResponseService,
        RequestHeader>
    {
        protected override CB_NG_8316_CustomItemLegalDemandsOut CallWS(CB_NG_8316_CustomItemLegalDemandsIn customRequest, CustomItemLegalDemandsRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CB_NG_8316_CustomItemLegalDemandsOut();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICustomItemLegalDemandsOperation>()
                    .CustomItemLegalDemands(
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
            get { return "8316"; }
        }
    }
}
