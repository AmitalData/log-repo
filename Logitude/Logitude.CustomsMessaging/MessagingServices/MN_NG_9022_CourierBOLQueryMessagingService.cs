
                                                            //Yuval Chalup 29.10.2015 TASK-16002
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
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.CourierBOLQueryServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class MN_NG_9022_CourierBOLQueryMessagingService
    : MessagingServiceBase<
    CourierBOLQueryRequestParams, CourierBOLQueryResponseData,
    MN_NG_9022_CourierBOLQuery_Message, MN_NG_9023_CourierBOLFeedBack_Message,
    MN_NG_9022_CourierBOLQueryRequestService, MN_NG_9023_CourierBOLFeedBackResponseService,
    RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "9022"; }
        }

        /*protected override CreditQueryResponseData GetIIGBLExceptionFromReponseHeader(TSH_NG_8290_Web06_CreditInfo customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("TSH_NG_8289_Web05_CreditQueryMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                _ResponseHeader.ErrorDescription = FormattedMessage;
                                (_ResponseService as TSH_NG_8290_Web06_CreditInfoResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new CreditQueryResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }*/

        protected override MN_NG_9023_CourierBOLFeedBack_Message CallWS(MN_NG_9022_CourierBOLQuery_Message customRequest, CourierBOLQueryRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new MN_NG_9023_CourierBOLFeedBack_Message();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICourierBOLQueryOperation>()
                    .CourierBOLQuery(
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

