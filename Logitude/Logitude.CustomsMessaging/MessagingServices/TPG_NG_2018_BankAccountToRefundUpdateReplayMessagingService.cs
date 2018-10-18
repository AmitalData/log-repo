
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.BankAccountToRefundUpdateReplayServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TPG_NG_2018_BankAccountToRefundUpdateReplayMessagingService : MessagingServiceBase<
        BankAccountToRefundRequestParams,
        INF_MSG_GenericResponseData,
        TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay,
        INF_MSG_Generic,
        TPG_NG_2018_BankAccountToRefundUpdateReplayRequestService,
        TPG_NG_2018_BankAccountToRefundUpdateReplayResponseService, RequestHeader>
    {
        protected override INF_MSG_Generic CallWS(TPG_NG_2018_MSG4_BankAccountToRefundUpdateReplay customRequest, BankAccountToRefundRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IBankAccountToRefundUpdateReplayOperation>()
                    .BankAccountToRefundUpdateReplay(
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
            get { return "2018"; }
        }
    }
}
