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
using UnifreightIIG.Common.WarehouseBlockBalanceServiceReference;
//IWarehouseBlockBalanceOperation
//ResponseHeader WarehouseBlockBalance(string ExternalId, string ConsumerID, ST_8328_Web01_WarehouseBlockBalanceFilterParam myRequest, ref MoreParams myMoreParams, out ST_8329_Web02_WarehouseBlockBalanceDetail myResponse);
namespace Logitude.CustomsMessaging.MessagingServices
{
    public class ST_8328_Web01_WarehouseBlockBalanceMessagingService
        : MessagingServiceBase<
        ST_8328_Web01_WarehouseBlockBalanceRequestParams,ST_8328_Web01_WarehouseBlockBalanceResponseData,
        ST_8328_Web01_WarehouseBlockBalanceFilterParam, ST_8329_Web02_WarehouseBlockBalanceDetail,
        ST_8328_Web01_WarehouseBlockBalanceFilterParamRequestService, ST_8329_Web02_WarehouseBlockBalanceDetailResponseService, RequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "8328"; }
        }

        protected override ST_8329_Web02_WarehouseBlockBalanceDetail CallWS(ST_8328_Web01_WarehouseBlockBalanceFilterParam customRequest, ST_8328_Web01_WarehouseBlockBalanceRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new ST_8329_Web02_WarehouseBlockBalanceDetail();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IWarehouseBlockBalanceOperation>()
                    .WarehouseBlockBalance(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }
        // moran 3.11.14 - Task 7933 -->
        protected override ST_8328_Web01_WarehouseBlockBalanceResponseData GetIIGBLExceptionFromReponseHeader(ST_8329_Web02_WarehouseBlockBalanceDetail customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("ST_8328_Web01_WarehouseBlockBalanceMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                               // _ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as ST_8329_Web02_WarehouseBlockBalanceDetailResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new ST_8328_Web01_WarehouseBlockBalanceResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

        protected override RequestSheetParam GetSheetDetailsFromRequestParam(ST_8328_Web01_WarehouseBlockBalanceRequestParams requestParams)
        {
            var myRequestSheetParam = new RequestSheetParam();
            myRequestSheetParam.RequestDescription = "שאילתא ליתרות מלאי בגוש";
            return myRequestSheetParam;
        }

        
    }
}
