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
using UnifreightIIG.Common.SaveCLAIMServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TPG_NG_8244_ClaimFileFilterParamMessagingService : MessagingServiceBase
        <TPG_NG_8244_ClaimFileFilterRequestParams,
        TPG_NG_8245_ClaimFilesDetailResponseData,
        TPG_NG_8244_Web01_ClaimFileFilterParam,
        TPG_NG_8245_Web02_ClaimFilesDetail,
        TPG_NG_8244_ClaimFileFilterParamRequestService,
        TPG_NG_8245_ClaimFilesDetailResponseService, RequestHeader>
    {
        protected override TPG_NG_8245_Web02_ClaimFilesDetail CallWS(TPG_NG_8244_Web01_ClaimFileFilterParam customRequest, TPG_NG_8244_ClaimFileFilterRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TPG_NG_8245_Web02_ClaimFilesDetail();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ISaveCLAIMOperation>()
                    .SaveCLAIM(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        protected override TPG_NG_8245_Web02_ClaimFilesDetail CallWSSigned(byte[] customRequestSignedByteArry, TPG_NG_8244_ClaimFileFilterRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TPG_NG_8245_Web02_ClaimFilesDetail();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ISaveCLAIMOperation>()
                    .SaveCLAIMSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        public override string MainInterfaceCode
        {
            get { return "8244"; }
        }
    }
}
