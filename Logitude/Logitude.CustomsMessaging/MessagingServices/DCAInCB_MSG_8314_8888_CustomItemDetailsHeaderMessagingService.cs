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
        GenericRequestParams,
        INF_MSG_GenericResponseData,
         CB_NG_8314_CustomItemDetailsHeaderIn,
         CB_NG_8888_CustomsItemOut,
         Get_CB_MSG_8314_8888_CustomItemDetailsHeaderRequestService,
        Get_CB_MSG_8314_8888_CustomItemDetailsHeaderResponseService,
        DCAInRequestHeader
        >
    {

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(CB_NG_8888_CustomsItemOut customsResponse)
        {
            

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsItem"),
                //LoggingEntityId = customsResponse.DebtNotificationMessag.debtNotificationID.ToString()
            };
            return myGenericRequestParams;
        }
        protected override CB_NG_8888_CustomsItemOut CallWS(CB_NG_8314_CustomItemDetailsHeaderIn customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;  
            return new CB_NG_8888_CustomsItemOut(); 
        }

        public override string MainInterfaceCode
        {
            get { return "8314"; }
        }
    }
}
