
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentReplyServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TSH_MSG6_AgentPaymentMessageService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        TSH_MSG6_AgentPayment,
        TSH_MSG7_AgentPaymentReply,
        TSH_MSG6_AgentPaymentRequestService,
        TSH_MSG7_AgentPaymentReplyResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "3051"; } }


        protected override TSH_MSG7_AgentPaymentReply CallWSSigned(byte[] customRequestSignedByteArry, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TSH_MSG7_AgentPaymentReply();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IAgentPaymentReplyOperation>()
                    .AgentPaymentReplySign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }


        protected override TSH_MSG7_AgentPaymentReply CallWS(TSH_MSG6_AgentPayment customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TSH_MSG7_AgentPaymentReply();
             
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IAgentPaymentReplyOperation >()
                    .AgentPaymentReply(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            
            return response;
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(TSH_MSG7_AgentPaymentReply customsResponse)
        {
            var tableName = "Customs.PaymentOrder";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }

        protected override INF_MSG_GenericResponseData GetIIGBLExceptionFromReponseHeader(TSH_MSG7_AgentPaymentReply customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("TSH_MSG6_AgentPaymentMessageService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                               /// _ResponseHeader.ErrorDescription = FormattedMessage;
                            }
                        }
                        break;
                    default:
                        return new INF_MSG_GenericResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

    }
}
