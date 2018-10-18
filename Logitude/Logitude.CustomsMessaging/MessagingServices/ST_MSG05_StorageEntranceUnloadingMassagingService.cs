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
using UnifreightIIG.Common.MessageLib.Storage;
using UnifreightIIG.Common.StorageEntranceUnloadingServiceReference;
using UnifreightIIG.Common.TheGateway;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class ST_MSG05_StorageEntranceUnloadingMassagingService : MessagingServiceBase<
        StorageEntranceUnloadingRequestParams,
        StorageEntranceUnloadingResponseData,
        ST_NG_20_MSG5_StorageEntranceUnloading,
        INF_MSG_Generic,
        ST_MSG05_StorageEntranceUnloadingRequestService,
        ST_MSG05_StorageEntranceUnloadingResponseService, RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "20"; }
        }

        protected override INF_MSG_Generic CallWS(ST_NG_20_MSG5_StorageEntranceUnloading customRequest, StorageEntranceUnloadingRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new INF_MSG_Generic();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IStorageEntranceUnloadingOperation>()
                    .StorageEntranceUnloading(
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
