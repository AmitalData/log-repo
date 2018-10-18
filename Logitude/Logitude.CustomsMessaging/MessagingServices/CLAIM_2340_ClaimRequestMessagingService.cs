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
using UnifreightIIG.Common.ClaimAnswerServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CLAIM_2340_ClaimRequestMessagingService : MessagingServiceBase<
        CLAIM_2340_ClaimRequestRequestParams,
        ClaimAnswerResponseData,
        CLAIM_MSG1_ClaimRequest,
        CLAIM_MSG22_ClaimAnswer,
        CLAIM_2340_ClaimRequestRequestService,
        CLAIM_2345_ClaimAnswerResponseService, RequestHeader>
    {
        protected override CLAIM_MSG22_ClaimAnswer CallWS(CLAIM_MSG1_ClaimRequest customRequest, CLAIM_2340_ClaimRequestRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CLAIM_MSG22_ClaimAnswer();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IClaimAnswerOperation>()
                    .ClaimAnswer(
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
            get { return "2340"; }
        }


        protected override CLAIM_2340_ClaimRequestRequestParams CreateDefaultRequestParamsFromCustomsResponse(CLAIM_MSG22_ClaimAnswer customsResponse)
        {
            var tableName = "Customs.Claim";

            var myGenericRequestParams = new CLAIM_2340_ClaimRequestRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }
        protected override CLAIM_MSG22_ClaimAnswer CallWSSigned(byte[] customRequestSignedByteArry, CLAIM_2340_ClaimRequestRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CLAIM_MSG22_ClaimAnswer();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IClaimAnswerOperation>()
                    .ClaimAnswerSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }
    }
}
