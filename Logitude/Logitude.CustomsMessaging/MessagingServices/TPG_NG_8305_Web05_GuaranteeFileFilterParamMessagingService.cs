// moran 5.7.15 - Task 13442
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
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.GuaranteeFileFilterParamServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class TPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService
         : MessagingServiceBase<
        GuaranteeRequestParams,
        GuaranteeResponseData,
        TPG_NG_8305_Web05_GuaranteeFileFilterParam,
        TPG_NG_8247_Web06_GuaranteeFilesDetail,
        TPG_NG_8305_Web05_GuaranteeFileFilterParamRequestService,
        TPG_NG_8247_Web06_GuaranteeFilesDetailResponseService, RequestHeader>
    {

        public override string MainInterfaceCode { get { return "8305"; } }

        public static GuaranteeResponseData SendInteractive(GuaranteeRequestParams searchParams) 
        {
            
            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;

            var myTPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService = new Logitude.CustomsMessaging.MessagingServices.TPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService();
            var resData = myTPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService.Send(searchParams);
            return resData;

        }


        protected override TPG_NG_8247_Web06_GuaranteeFilesDetail CallWS(
            TPG_NG_8305_Web05_GuaranteeFileFilterParam customRequest,
            GuaranteeRequestParams requestParams, 
            out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new TPG_NG_8247_Web06_GuaranteeFilesDetail();

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
            
            customRequest.RequestContentHeader = new  RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
           
            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IGuaranteeFileFilterParamOperation>()
                    .GuaranteeFileFilter(
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
