
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

using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;
using UnifreightIIG.Common.TheGateway;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class NG_9100_MSG_OutgoingMessageRequestMessagingService
        : MessagingServiceBase<
        MessageWaitingRequestParams, MessageWaitingResponseData,
        NG_9100_MSG_OutgoingMessageRequest, NG_9101_MSG_OutgoingMessageResponse,
        NG_9100_MSG_OutgoingMessageRequest_RequestService, NG_9101_MSG_OutgoingMessageResponse_ResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode { get { return "9100"; } }


        protected override NG_9101_MSG_OutgoingMessageResponse CallWS(NG_9100_MSG_OutgoingMessageRequest customRequest, MessageWaitingRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new NG_9101_MSG_OutgoingMessageResponse();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IOutgoingMessageRequestOperation>()
                    .OutgoingMessageRequest(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override MessageWaitingRequestParams CreateDefaultRequestParamsFromCustomsResponse(NG_9101_MSG_OutgoingMessageResponse customsResponse)
        {
            //var tableName = "Customs.Declaration";
            var myMessageWaitingRequestParams = new MessageWaitingRequestParams()
            {
                //LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
                RequestName = "Waiting Messages Request by filter",
            };
            return myMessageWaitingRequestParams;
        }
    }
}
