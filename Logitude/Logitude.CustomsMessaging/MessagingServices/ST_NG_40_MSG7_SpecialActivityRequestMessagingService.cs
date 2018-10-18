//Yuval Chalup 25.06.2015 TASK-8907
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.SpecialActivityRequestMessageServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class ST_NG_40_MSG7_SpecialActivityRequestMessagingService : MessagingServiceBase<
        SpecialActivityRequestParams,
        INF_MSG_GenericResponseData,
        ST_NG_40_MSG7_SpecialActivityRequestMessage,
        INF_MSG_Generic,
        ST_NG_40_MSG7_SpecialActivityRequestService,
        ST_NG_40_MSG7_SpecialActivityResponseService, RequestHeader>
    {
        protected override INF_MSG_Generic CallWS(ST_NG_40_MSG7_SpecialActivityRequestMessage customRequest, SpecialActivityRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ISpecialActivityRequestMessageOperation>()
                    .SpecialActivityRequestMessageOperation(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(INF_MSG_Generic customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("ST_NG_40_MSG7_SpecialActivityRequestMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                        }
                        break;
                    default:
                        return new INF_MSG_GenericResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

        public override string MainInterfaceCode
        {
            get { return "40"; }
        }

    }
}

