
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
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.ClientSearchServiceReference;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using Logitude.Server.Tools.Helpers;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService
        :
        MessagingServiceBase<
        ClientSearchRequestParams,
        ClientSearchResponseData,
        CL_MSG101_GetCustomerByEntityCustomerIdentification,
        CL_MSG103_WholeClientForCustomsAgent,
        CL_3610_GetCustomerByEntityCustomerIdentificationRequestService,
        CL_MSG103_WholeClientForCutomsAgentResponseService, RequestHeader>
    {
        public override string MainInterfaceCode { get { return "3610"; } }

        protected override ClientSearchRequestParams CreateDefaultRequestParamsFromCustomsResponse(CL_MSG103_WholeClientForCustomsAgent customsResponse)
        {
            return new ClientSearchRequestParams() { RequestName="Dummy all is call back " };
        }

        protected override CL_MSG103_WholeClientForCustomsAgent CallWS(CL_MSG101_GetCustomerByEntityCustomerIdentification customRequest, ClientSearchRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CL_MSG103_WholeClientForCustomsAgent();
            //IResponseHeaderOrFault responseHeader;
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                try
                {
                    var cs = uifreightSdkGateway.GetChannel<IClientSearch>();
                    _ResponseHeader = cs.ClientSearch(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
                }
                catch (System.Exception e)
                {
                   
                    throw;
                }                                  
            }
           
            return response;
        }


        protected override ClientSearchResponseData GetIIGBLExceptionFromReponseHeader(CL_MSG103_WholeClientForCustomsAgent customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("MN_NG_8240_CargoQueryMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                ///_ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as CL_MSG103_WholeClientForCutomsAgentResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new ClientSearchResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }
    }
}
