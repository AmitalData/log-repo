
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
using UnifreightIIG.Common.FaultProceduralDetailsServiceReference;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_NG_Web8332_FaultProceduralParamMessagingService
        : MessagingServiceBase<
        FaultProceduralRequestParams, FaultProceduralResponseData,
        DF_NG_Web8332_FaultProceduralParam, DF_NG_Web8333_FaultProceduralDetailsResponse,
        DF_NG_Web8332_FaultProceduralRequestService, DF_NG_Web8333_FaultProceduralDetailsResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "8332"; }
        }

        protected override FaultProceduralResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_Web8333_FaultProceduralDetailsResponse customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("DF_NG_Web8332_FaultProceduralParamMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                               /// _ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as DF_NG_Web8333_FaultProceduralDetailsResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new FaultProceduralResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }


        protected override DF_NG_Web8333_FaultProceduralDetailsResponse CallWS(DF_NG_Web8332_FaultProceduralParam customRequest, FaultProceduralRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_Web8333_FaultProceduralDetailsResponse();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IFaultProceduralDetailsOperation>()
                    .FaultProceduralDetailsOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }
    }
}

