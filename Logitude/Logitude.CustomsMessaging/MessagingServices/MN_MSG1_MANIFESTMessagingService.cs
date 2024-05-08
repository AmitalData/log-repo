using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.FakeMessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.MANIFESTRequestServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class MN_MSG1_MANIFESTMessagingService
    : MessagingServiceBase<
    MANIFESTRequestRequestParams,
    MANIFESTRequestResponseData,
    MN_MSG1_MANIFEST,
    MN_MSG4_SendManifestFeedBack_Message,
    MN_MSG1_MANIFESTRequestService, MN_MSG4_SendManifestFeedBack_MessageResponseService,
    RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "1170"; }
        }

        protected override MANIFESTRequestResponseData GetIIGBLExceptionFromReponseHeader(MN_MSG4_SendManifestFeedBack_Message customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("MN_MSG1_MANIFESTMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //_ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as MN_MSG4_SendManifestFeedBack_MessageResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new MANIFESTRequestResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

        protected override MN_MSG4_SendManifestFeedBack_Message CallWSSigned(byte[] customRequestSignedByteArry, MANIFESTRequestRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new MN_MSG4_SendManifestFeedBack_Message();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IMANIFESTRequestOperation>()
                    .MANIFESTRequestOperationSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override MN_MSG4_SendManifestFeedBack_Message CallWS(MN_MSG1_MANIFEST customRequest, MANIFESTRequestRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new MN_MSG4_SendManifestFeedBack_Message();

            if (requestParams.TestCase != null)
            {
                var Fake = new Fake_1770_MN_MSG1_MANIFESTResponse(requestParams);
                _ResponseHeader = Fake.CallWS(requestParams, out response);


                exceptionMessage = null;
                return response;


            }

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            var sw = Stopwatch.StartNew();
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IMANIFESTRequestOperation>()
                    .MANIFESTRequestOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            LogMessagingUtil.Instance.AppendLine("UnifreightSdkGateway:Took:" + sw.Elapsed.ToString());

            return response;
        }
    }
}

