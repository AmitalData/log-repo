
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
using UnifreightIIG.Common.GuaranteeReturnRequestApprovalInfoServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class GRNT_5004_GuaranteeReturnRequestMessagingService : MessagingServiceBase<
        GuaranteeReturnRequesRequestParams,
        INF_MSG_GenericResponseData,
        GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequest,
        INF_MSG_Generic,
        GRNT_5004_GuaranteeReturnRequestService,
        GRNT_5004_GuaranteeReturnResponseService, RequestHeader>
    {
        protected override INF_MSG_Generic CallWS(GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequest customRequest, GuaranteeReturnRequesRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGuaranteeReturnRequestApprovalInfoOperation>()
                    .GetGuaranteeReturnRequestApprovalInfo(
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
            get { return "5004"; }
        }
    }
}
