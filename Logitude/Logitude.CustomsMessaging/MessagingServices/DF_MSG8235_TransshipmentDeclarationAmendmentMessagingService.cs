using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using Logitude.Server.Tools.Helpers;
using RequestHeader = UnifreightIIG.Common.TransshipmenDeclarationAmendmentRequestMsgRequestServiceReference.RequestHeader;
using UnifreightIIG.Common.TransshipmenDeclarationAmendmentRequestMsgRequestServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_MSG8235_TransshipmentDeclarationAmendmentMessagingService
        : MessagingServiceBase<
        AmendmentRequestParams,
        ExportDeclarationAmendmentResponseData,
        DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg,
        DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg,
        DF_MSG8235_TransshipmentDeclarationAmendmentRequestService,
        DF_NG_8237_TransshipmentDeclerationAmendmentReplyResponseService, RequestHeader>
    {

        public override string MainInterfaceCode
        {
            get
            {
                return "8235T";
            }
        }
        protected override DcaReceivedController GetDcaReceivedController(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customsResponse, AmendmentRequestParams RequestParams)
        {
            if (customsResponse == null)
                return null;

            return new DcaReceivedController();// { DcaAnalyzeAggregateKey = customsResponse.Response.Declaration.ID.Value };
        }

        protected override AmendmentRequestParams CreateDefaultRequestParamsFromCustomsResponse(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customsResponse)
        {
            return new AmendmentRequestParams() { RequestName = "Should not Use !!" };
        }

        protected override ExportDeclarationAmendmentResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customsResponse)
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
                if (string.IsNullOrWhiteSpace(FormattedMessage))
                    FormattedMessage = defaultMessage;
                
                LogMessagingUtil.Instance.AppendLine(FormattedMessage);

                switch (myUnifreightIIGFault.Detail.PlaceFault)
                {
                    case UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGFatalException:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGTechnicalError:
                        {
                            LogMessagingUtil.Instance.AppendLine("DF_MSG8235_TransshipmentDeclarationAmendmentMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);        
                        }
                        break;
                    default:
                        return new ExportDeclarationAmendmentResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

        protected override void PreCallWS(DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg customRequest, AmendmentRequestParams requestParams)
        {
            base.PreCallWS(customRequest, requestParams);
        }

        protected override DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg CallWSSigned(byte[] customRequestSignedByteArry, AmendmentRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg();
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<UnifreightIIG.Common.TheGateway.ITransshipmenDeclarationAmendmentRequestMsgRequestOperation>()
                    .TransshipmenDeclarationAmendmentRequestMsgRequestSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry, },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg CallWS(DF_NG_8235_MSG14000_ExportDeclarationAmendmentRequestMsg customRequest, AmendmentRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg();

            if (requestParams.TestCase != null)
            {
                BuildRequestContentHeaderB4Sign(customRequest);
                exceptionMessage = null;
                return response;
            }

            BuildRequestContentHeaderB4Sign(customRequest);

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<UnifreightIIG.Common.TheGateway.ITransshipmenDeclarationAmendmentRequestMsgRequestOperation>()
                    .TransshipmenDeclarationAmendmentRequestMsgRequest(
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