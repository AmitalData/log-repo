using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.VendorSearchServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class VE_MSG051_VendorSearchByCustomsAgentMessagingService : MessagingServiceBase<
        VE_MSG051_VendorSearchByCustomsAgentRequestParams, 
        VE_MSG052_VendorSearchResultsForCustomsAgentResponseData,
        VE_MSG051_VendorSearchByCustomsAgentMessage, VE_MSG052_VendorSearchResultsForCustomsAgentMessage, 
        VE_MSG051_VendorSearchByCustomsAgentRequestService, VE_MSG052_VendorSearchResultsForCustomsAgentResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "3650"; } }

        protected override VE_MSG051_VendorSearchByCustomsAgentRequestParams CreateDefaultRequestParamsFromCustomsResponse(VE_MSG052_VendorSearchResultsForCustomsAgentMessage customsResponse)
        {
            return new VE_MSG051_VendorSearchByCustomsAgentRequestParams() { RequestName="SHould not be use due - Response only after Our request !!" };
        }
        protected override VE_MSG052_VendorSearchResultsForCustomsAgentMessage CallWSSigned(byte[] customRequestSignedByteArry, VE_MSG051_VendorSearchByCustomsAgentRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new VE_MSG052_VendorSearchResultsForCustomsAgentMessage();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };


            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IVendorSearch>()
                    .VendorSearchSign(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    new ESBRequestSigned() { SignedByteArry = customRequestSignedByteArry },
                    ref this._IIGGatewayMoreParams,
                    out response);
            }


            return response;
        }
        protected override VE_MSG052_VendorSearchResultsForCustomsAgentMessage CallWS(VE_MSG051_VendorSearchByCustomsAgentMessage customRequest, VE_MSG051_VendorSearchByCustomsAgentRequestParams requestParams, out string exceptionMessage)
        {
            VE_MSG052_VendorSearchResultsForCustomsAgentMessage customResponse;

            BuildRequestContentHeaderB4Sign(customRequest); //??
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            //var ExternalId = Guid.NewGuid().ToString();

            using (var myUnifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = myUnifreightSdkGateway.GetChannel<IVendorSearch>().VendorSearch(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out customResponse);





            }

                exceptionMessage = null;
                var ExeptionDescription = "";
                //if (customResponse.ResponseContentHeader.Exception != null)
                //{
                //    foreach (var rec in customResponse.ResponseContentHeader.Exception)
                //    {

                //        if (!String.IsNullOrWhiteSpace(ExeptionDescription))
                //        {
                //            ExeptionDescription += Environment.NewLine;
                //        }
                //        ExeptionDescription += rec.ExeptionDescription;

                //    }


                //    exceptionMessage = ExeptionDescription;

                //}
            
            return customResponse;
        }

        
    }
}
