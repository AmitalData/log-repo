using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CustomItemDetailsServiceReference;
using UnifreightIIG.Common.LogisticActionRequestMessageServiceReference;
using UnifreightIIG.Common.MessageLib.EntryExit;
using UnifreightIIG.Common.MessageLib.Fault;
using UnifreightIIG.Common.TheGateway;
using RequestHeader = UnifreightIIG.Common.CustomItemDetailsServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public  class DCAInCB_MSG_8314_8888_CustomItemDetailsHeaderMessagingService: MessagingServiceBase<
        CD_NG_8314_Web01_CustomsItemDetailsRequestParams,
        INF_MSG_GenericResponseData,
         CB_NG_8314_CustomItemDetailsHeaderIn,
         CB_NG_8888_CustomsItemOut,
         Get_CB_MSG_8314_8888_CustomItemDetailsHeaderRequestService,
        Get_CB_MSG_8314_8888_CustomItemDetailsHeaderResponseService,
        DCAInRequestHeader
        >
    {

        protected override CD_NG_8314_Web01_CustomsItemDetailsRequestParams CreateDefaultRequestParamsFromCustomsResponse(CB_NG_8888_CustomsItemOut customsResponse)
        {
            

            var myGenericRequestParams = new CD_NG_8314_Web01_CustomsItemDetailsRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsItem"),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
        protected override CB_NG_8888_CustomsItemOut CallWS(CB_NG_8314_CustomItemDetailsHeaderIn customRequest, CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;  
           var response = new CB_NG_8888_CustomsItemOut();

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<ICustomItemDetailsOperation>()
                    .CustomItemDetails(
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
            get { return "8314"; }
        }
    }
}
