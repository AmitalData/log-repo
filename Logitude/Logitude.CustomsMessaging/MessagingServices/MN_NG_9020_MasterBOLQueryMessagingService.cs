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
using UnifreightIIG.Common.MasterBOLQueryServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class MN_NG_9020_MasterBOLQueryMessagingService : MessagingServiceBase
        <MasterBOLQueryRequestParams,
        MasterBOLFeedBackResponseData,
        MN_NG_9020_MasterBOLQuery_Message,
        MN_NG_9021_MasterBOLFeedBack_Message,
        MN_NG_9020_MasterBOLQueryRequestService,
        MN_NG_9021_MasterBOLFeedBackResponseService, RequestHeader>
    {
        protected override MN_NG_9021_MasterBOLFeedBack_Message CallWS(MN_NG_9020_MasterBOLQuery_Message customRequest, MasterBOLQueryRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new MN_NG_9021_MasterBOLFeedBack_Message();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IMasterBOLQueryOperation>()
                    .MasterBOLQuery(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        public override string MainInterfaceCode { get { return "9020"; } }

    }
}
