using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.GetAttachmentServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class GetDOC8318_Web_GetAttachmentMessagingService : MessagingServiceBase<
        GetAttachmentRequestParams,
        AttachmentResponseData,
        DOC_Web_GetAttachment_IN,
        DOC_Web_GetAttachment_OUT,
        GetDOC8318_Web_GetAttachmentRequestService,
        GetDOC8318_Web_GetAttachmentResponseService, 
        DCAInRequestHeader>
    {
        protected override DOC_Web_GetAttachment_OUT CallWS(DOC_Web_GetAttachment_IN customRequest, GetAttachmentRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;   
            var response = new DOC_Web_GetAttachment_OUT();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGetAttachmentOperation>()
                    .GetAttachment(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        public override string MainInterfaceCode
        {
            get { return "DOC8318"; }
        }

        protected override GetAttachmentRequestParams CreateDefaultRequestParamsFromCustomsResponse(DOC_Web_GetAttachment_OUT customsResponse)
        {
            return null;
        }
    }
}
