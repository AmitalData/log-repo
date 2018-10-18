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
using UnifreightIIG.Common.MessageLib.Vendor;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInVE_3681_VendorErrorOrCriticalChangeMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        VE_MSG012_VendorErrorOrCriticalChangeMessage,
        DCAInCustomRequestService,
        VE_3681_VendorErrorOrCriticalChangeResponseService, DCAInRequestHeader>
    {

        public override string MainInterfaceCode
        {
            get { return "3681"; }
        }

        protected override VE_MSG012_VendorErrorOrCriticalChangeMessage CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(VE_MSG012_VendorErrorOrCriticalChangeMessage customsResponse)
        {
            var tableName = "Customs.CustomsVendor";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                LoggingEntityId = customsResponse.VendorErrorOrCriticalChangeMessage.vendorID.ToString(),
            };
            return myGenericRequestParams;
        }

    }
}
