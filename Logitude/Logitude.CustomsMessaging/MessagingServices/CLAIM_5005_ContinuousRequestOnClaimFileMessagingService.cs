using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.ContinuousRequestOnClaimFileServiceReference;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CLAIM_5005_ContinuousRequestOnClaimFileMessagingService : MessagingServiceBase<
        ContinuousRequestOnClaimFileRequestParams,
        ContinuousResponseOnClaimFileResponseData,
        CLAIM_MSG9_ContinuousRequestOnClaimFile,
        CLAIM_MSG13_ContinuousResponseOnClaimFile,
        CLAIM_5005_ContinuousRequestOnClaimFileRequestService,
        CLAIM_5013_ContinuousResponseOnClaimFileResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "5005"; }
        }

        protected override CLAIM_MSG13_ContinuousResponseOnClaimFile CallWS(CLAIM_MSG9_ContinuousRequestOnClaimFile customRequest, ContinuousRequestOnClaimFileRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CLAIM_MSG13_ContinuousResponseOnClaimFile();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IContinuousRequestOnClaimFileOperation>()
                    .ContinuousRequestOnClaimFile(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);

            }
            return response;
        }

        protected override CLAIM_MSG13_ContinuousResponseOnClaimFile CallWSSigned(byte[] customRequestSignedByteArry, ContinuousRequestOnClaimFileRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CLAIM_MSG13_ContinuousResponseOnClaimFile();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IContinuousRequestOnClaimFileOperation>()
                    .ContinuousRequestOnClaimFileSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        protected override ContinuousResponseOnClaimFileResponseData GetIIGBLExceptionFromReponseHeader(CLAIM_MSG13_ContinuousResponseOnClaimFile customsResponse)
        {
            try
            {
                var responseContentHeader = customsResponse.GetResponseContentHeader() as IResponseContentHeader;
                ThrowIIGBLException(_ResponseHeader, responseContentHeader);

            }
            catch (System.ServiceModel.FaultException<UnifreightIIGFault> myUnifreightIIGFault)
            {
                var defaultMessage = "Sending request to IIG Server Failed ";
                var FormattedMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(myUnifreightIIGFault);
                if (String.IsNullOrWhiteSpace(FormattedMessage))
                {
                    FormattedMessage = defaultMessage;
                }
                LogMessagingUtil.Instance.AppendLine(FormattedMessage);

                switch (myUnifreightIIGFault.Detail.PlaceFault)
                {

                    case UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGFatalException:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGTechnicalError:
                        {
                            LogMessagingUtil.Instance.AppendLine("CLAIM_5005_ContinuousRequestOnClaimFileMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //_ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as DF_NG_2754_MSG10004_RetrieveImportDeclarationResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new ContinuousResponseOnClaimFileResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }
    }
}
