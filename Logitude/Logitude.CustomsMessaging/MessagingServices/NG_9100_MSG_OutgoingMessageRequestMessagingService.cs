
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Dca;
using Logitude.CustomsMessaging.Dca.Restore9100;
using Logitude.CustomsMessaging.FakeMessagingServices;
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
#if true

            if (requestParams.TestCase != null)
            {
                var fake = new Fake_9100(requestParams).CallWS();
                _ResponseHeader = fake.header;
                exceptionMessage = fake.exceptionMessage;
                DcaDirect9200TenantService.testResponse = fake.response;
                DcaDirect9200TenantService.SkipCorrelationClearForTests = true;
                var customsSettingQueryService = new CustomsSettingQueryService(requestParams.Tenant);
                var customsSetting = customsSettingQueryService.GetSingleByTenant(requestParams.Tenant);
                var downloadDcaMessageSheetWR = new DcaDownloadTenantService(customsSetting);
                downloadDcaMessageSheetWR.DownloadAll(null,null);
                return null;
            }
            var sendNG_9100_MSG_OutgoingMessageRequestService = new SendNG_9100_MSG_OutgoingMessageRequestService();
            var result = sendNG_9100_MSG_OutgoingMessageRequestService.CallWS(customRequest, base.CustomsSetting, _IIGGatewayMoreParams, this.RequestsSheetExternalId);
            exceptionMessage = result.exceptionMessage;
            _ResponseHeader = result._ResponseHeader;
            return result.response;
#else

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
#endif
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
