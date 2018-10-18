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
using UnifreightIIG.Common.RTGSInfoServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TSH_WEB8289_9060_RTGSInfoQueryMessagingService : MessagingServiceBase<
        CreditQueryRequestParams, RTGSInfoQueryResponseData,
        TSH_NG_8289_Web05_CreditQuery, TSH_NG_9060_Web07_RTGSInfo,
        TSH_WEB8289_9060_RTGSInfoQueryRequestService, TSH_WEB8289_9060_RTGSInfoQueryResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "8289Z"; }
        }

        protected override CreditQueryRequestParams CreateDefaultRequestParamsFromCustomsResponse(TSH_NG_9060_Web07_RTGSInfo customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new CreditQueryRequestParams()
            {
                ///LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }

        protected override TSH_NG_9060_Web07_RTGSInfo CallWS(TSH_NG_8289_Web05_CreditQuery customRequest, CreditQueryRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TSH_NG_9060_Web07_RTGSInfo();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IRTGSInfoCreditQueryOperation>()
                    .RTGSInfoCreditQuery(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        protected override TSH_NG_9060_Web07_RTGSInfo CallWSSigned(byte[] customRequestSignedByteArry, CreditQueryRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TSH_NG_9060_Web07_RTGSInfo();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IRTGSInfoCreditQueryOperation>()
                    .RTGSInfoCreditQuerySign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }
    }


}


