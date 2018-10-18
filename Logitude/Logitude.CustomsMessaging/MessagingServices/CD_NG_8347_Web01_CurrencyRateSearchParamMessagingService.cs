
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CurrencyRateServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CD_NG_8347_Web01_CurrencyRateSearchParamMessagingService: MessagingServiceBase<
        CD_NG_8347_Web01_CurrencyRateSearchRequestParams,
        CD_NG_8348_Web02_CurrencyRateDetailResponseData,
        CD_NG_8347_Web01_CurrencyRateSearchParam,
        CD_NG_8348_Web02_CurrencyRateDetail,
        CD_NG_8347_Web01_CurrencyRateSearchParamRequestService,
        CD_NG_8348_Web02_CurrencyRateDetailResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "8347"; } }

        protected override DcaReceivedController GetDcaReceivedController(CD_NG_8348_Web02_CurrencyRateDetail customsResponse, CD_NG_8347_Web01_CurrencyRateSearchRequestParams RequestParams)
        {

            if (customsResponse != null && customsResponse.CurrencyRateList != null && customsResponse.CurrencyRateList.Length == 1)
            {
                return new DcaReceivedController()
                {
                    DcaAnalyzeAggregateKey = customsResponse.CurrencyRateList[0].currencyTypeID + ":" + customsResponse.CurrencyRateList[0].startDate.ToString()
                };
            }
            return null;
        }

        protected override bool? IsOurEnvironment(UnifreightIIG.Common.CurrencyRateServiceReference.CD_NG_8348_Web02_CurrencyRateDetail customsResponse, Logitude.CustomsMessaging.Common.RequestParams.CD_NG_8347_Web01_CurrencyRateSearchRequestParams RequestParams)
        {
            if (customsResponse != null && customsResponse.CurrencyRateList != null && customsResponse.CurrencyRateList.Count() == 1)
            {
                return true;
            }
            return false;
            
        }  
        protected override CD_NG_8348_Web02_CurrencyRateDetail CallWSSigned(byte[] customRequestSignedByteArry, CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CD_NG_8348_Web02_CurrencyRateDetail();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICurrencyRateRequestOperation>()
                    .CurrencyRateRequestSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }
        
        protected override CD_NG_8348_Web02_CurrencyRateDetail CallWS(CD_NG_8347_Web01_CurrencyRateSearchParam customRequest, CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CD_NG_8348_Web02_CurrencyRateDetail();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICurrencyRateRequestOperation>()
                    .CurrencyRateRequestOperation(
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
