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
//using UnifreightIIG.Common.CargoSplitMessageServiceReference;
using UnifreightIIG.Common.CargoSplitSaveServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class MN_MSG8370_CargoSplitMessagingService
        : MessagingServiceBase<
        CargoSplitRequestParams, INF_MSG_GenericResponseData,
        MN_MSG8370_CargoSplitRequest_Message, MN_MSG8374_CargoSplitRequestFeedBack_Message,
        MN_MSG8370_CargoSplitRequestService, MN_MSG8374_CargoSplitResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "8370"; }
        }


        protected override CargoSplitRequestParams CreateDefaultRequestParamsFromCustomsResponse(MN_MSG8374_CargoSplitRequestFeedBack_Message customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new CargoSplitRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(MN_MSG8374_CargoSplitRequestFeedBack_Message customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("MN_MSG8370_CargoSplitMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {

                            }
                        }
                        break;
                    default:
                        return new INF_MSG_GenericResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }


        protected override MN_MSG8374_CargoSplitRequestFeedBack_Message CallWS(MN_MSG8370_CargoSplitRequest_Message customRequest, CargoSplitRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new MN_MSG8374_CargoSplitRequestFeedBack_Message();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICargoSplitSaveOperation>()
                    .CargoSplitSave(
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
