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
using UnifreightIIG.Common.MorningMessagesListServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class MM_Web01_MorningMessagesListMessagingService : MessagingServiceBase<
        MorningMessageRequestParams,
        MorningMessageResponseData,
        MM_Web01_MorningMessagesListFilter,
        MM_Web02_MorningMessagesList,
        MM_Web01_MorningMessagesListRequestService,
        MM_Web02_MorningMessagesListResponseService, RequestHeader>
    {
        protected override MM_Web02_MorningMessagesList CallWS(MM_Web01_MorningMessagesListFilter customRequest, MorningMessageRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new MM_Web02_MorningMessagesList();
            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IMorningMessagesListOperation>()
                    .MorningMessagesList(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }

        public override string MainInterfaceCode
        {
            get { return "0102"; }
        }

        protected override RequestSheetParam GetSheetDetailsFromRequestParam(MorningMessageRequestParams requestParams)
        {
            var myRequestSheetParam = new RequestSheetParam();
            myRequestSheetParam.RequestDescription = "שאילתת הודעות בוקר";
            //myRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.Deficit"); to check? 

            return myRequestSheetParam;
        }
    }
}
