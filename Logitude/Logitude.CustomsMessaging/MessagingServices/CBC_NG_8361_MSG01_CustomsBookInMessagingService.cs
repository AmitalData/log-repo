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
using UnifreightIIG.Common.CustomsBookServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class CBC_NG_8361_MSG01_CustomsBookInMessagingService
        : MessagingServiceBase<
        CustomsBookInRequestParams, CustomsBookInResponseData,
        CBC_NG_8361_MSG01_CustomsBookIn, CBC_NG_8362_MSG01_CustomsBookOut,
        CBC_NG_8361_MSG01_CustomsBookInRequestService, CBC_NG_8362_MSG01_CustomsBookOutResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "8361"; }
        }

        protected override CustomsBookInRequestParams CreateDefaultRequestParamsFromCustomsResponse(CBC_NG_8362_MSG01_CustomsBookOut customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new CustomsBookInRequestParams()
            {
                //LoggingObjectTableId = ObjectTabelRepository.GetObjectTableByName(tableName),
            };
            return myGenericRequestParams;
        }

        protected override CBC_NG_8362_MSG01_CustomsBookOut CallWS(CBC_NG_8361_MSG01_CustomsBookIn customRequest, CustomsBookInRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new CBC_NG_8362_MSG01_CustomsBookOut();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICustomsBookOperation>()
                    .CustomsBook(
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

