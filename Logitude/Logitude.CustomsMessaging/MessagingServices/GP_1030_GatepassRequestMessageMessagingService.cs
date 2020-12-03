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
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.GatepassFeedbackMServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class GP_1030_GatepassRequestMessageMessagingService : MessagingServiceBase<
        GatepassRequestMessageRequestParams,
        GatepassFeedbackMessageResponseData,
        GP_NG_1030_MSG1_GatepassRequestMessage,
        GP_NG_1035_MSG2_GatepassFeedbackMessage,
        GP_1030_GatepassRequestMessageRequestService,
        GP_1035_GatepassFeedbackMessageResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "1030"; }
        }

        protected override GP_NG_1035_MSG2_GatepassFeedbackMessage CallWS(GP_NG_1030_MSG1_GatepassRequestMessage customRequest, GatepassRequestMessageRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new GP_NG_1035_MSG2_GatepassFeedbackMessage();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGatepassFeedbackMOperation>()
                    .GatepassFeedbackM(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override bool? IsOurEnvironment(GP_NG_1035_MSG2_GatepassFeedbackMessage customsResponse, GatepassRequestMessageRequestParams RequestParams)
        {
            if (customsResponse.GatepassFeedbackMessage != null)
            {
                if(customsResponse.GatepassFeedbackMessage.FirstOrDefault().gatepassNumber < 500000000)
                {
                    return false;
                }
            }
            return true;
        }

        protected override GatepassFeedbackMessageResponseData GetIIGBLExceptionFromReponseHeader(GP_NG_1035_MSG2_GatepassFeedbackMessage customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("GP_1030_GatepassRequestMessageMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //_ResponseHeader.ErrorDescription = FormattedMessage;
                            }
                        }
                        break;
                    default:
                        return new GatepassFeedbackMessageResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }
    }
}
