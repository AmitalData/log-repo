                                                            //Yuval Chalup 23.06.2015 TASK-13278
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
using UnifreightIIG.Common.CreditQueryServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TSH_NG_8289_Web05_CreditQueryMessagingService
        : MessagingServiceBase<
        CreditQueryRequestParams, CreditQueryResponseData,
        TSH_NG_8289_Web05_CreditQuery, TSH_NG_8290_Web06_CreditInfo,
        TSH_NG_8289_Web05_CreditQueryRequestService, TSH_NG_8290_Web06_CreditInfoResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "8289"; }
        }

        protected override CreditQueryRequestParams CreateDefaultRequestParamsFromCustomsResponse(TSH_NG_8290_Web06_CreditInfo customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new CreditQueryRequestParams()
            {
                ///LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
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

        protected override TSH_NG_8290_Web06_CreditInfo CallWS(TSH_NG_8289_Web05_CreditQuery customRequest, CreditQueryRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TSH_NG_8290_Web06_CreditInfo();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICreditQueryOperation>()
                    .CreditQueryOperation(
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

