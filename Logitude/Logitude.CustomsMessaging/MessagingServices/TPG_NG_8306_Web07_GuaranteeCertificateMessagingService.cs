//Yuval Chalup 07.09.2015 TASK-15037
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
using UnifreightIIG.Common.GuaranteeCertificateFilterParamServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TPG_NG_8306_Web07_GuaranteeCertificateMessagingService : MessagingServiceBase<
        GuaranteeCertificateRequestParams,
        GuaranteeCertificateResponseData,
        TPG_NG_8306_Web07_GuaranteeCertificateFilterParam,
        TPG_NG_8248_Web08_GuaranteeCertificateDetail,
        TPG_NG_8306_Web07_GuaranteeCertificateRequestService,
        TPG_NG_8248_Web08_GuaranteeCertificateResponseService, RequestHeader>
    {
        protected override TPG_NG_8248_Web08_GuaranteeCertificateDetail CallWS(TPG_NG_8306_Web07_GuaranteeCertificateFilterParam customRequest, GuaranteeCertificateRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TPG_NG_8248_Web08_GuaranteeCertificateDetail();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGuaranteeCertificateFilterParamOperation>()
                    .GuaranteeCertificateFilter(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }
            return response;
        }

        protected override GuaranteeCertificateResponseData GetIIGBLExceptionFromReponseHeader(TPG_NG_8248_Web08_GuaranteeCertificateDetail customsResponse)
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
                            LogMessagingUtil.Instance.AppendLine("TPG_NG_8306_Web07_GuaranteeCertificateMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                               // _ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as TPG_NG_8248_Web08_GuaranteeCertificateResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new GuaranteeCertificateResponseData() { UserMessage = FormattedMessage, HasException = true };
                }
            }

            return null;
        }

        protected override GuaranteeCertificateRequestParams CreateDefaultRequestParamsFromCustomsResponse(TPG_NG_8248_Web08_GuaranteeCertificateDetail customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new GuaranteeCertificateRequestParams()
            {
                ///LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }

        public override string MainInterfaceCode
        {
            get { return "8306"; }
        }

    }
}

