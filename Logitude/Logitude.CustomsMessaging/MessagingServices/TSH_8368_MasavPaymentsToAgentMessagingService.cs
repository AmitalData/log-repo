using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.MasavPaymentsToAgentServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TSH_8368_MasavPaymentsToAgentMessagingService : MessagingServiceBase<
        MasavPaymentsToAgentRequestParams,
        MasavPaymentsToAgentResponseData,
        TSH_NG_8368_MSG32_MasavPaymentsToAgentRequest,
        TSH_NG_8356_MSG33_MasavPaymentsToAgent,
        TSH_8368_MasavPaymentsToAgentRequestService,
        TSH_8356_MasavPaymentsToAgentResponseService, RequestHeader>
    {
        protected override TSH_NG_8356_MSG33_MasavPaymentsToAgent CallWS(TSH_NG_8368_MSG32_MasavPaymentsToAgentRequest customRequest, MasavPaymentsToAgentRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TSH_NG_8356_MSG33_MasavPaymentsToAgent();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            customRequest.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IMasavPaymentsToAgentOperation>()
                    .MasavPaymentsToAgent(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        public override string MainInterfaceCode { get { return "8368"; } }

        protected override MasavPaymentsToAgentRequestParams CreateDefaultRequestParamsFromCustomsResponse(TSH_NG_8356_MSG33_MasavPaymentsToAgent customsResponse)
        {
            var myGenericRequestParams = new MasavPaymentsToAgentRequestParams()
            {
                PaymentDate = customsResponse.MasavSentDate.masavSentDate,
            };
            return myGenericRequestParams;
        }
    }
}
