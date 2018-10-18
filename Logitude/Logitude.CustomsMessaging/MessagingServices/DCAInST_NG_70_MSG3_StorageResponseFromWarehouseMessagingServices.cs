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
using UnifreightIIG.Common.MessageLib.Storage;
//using UnifreightIIG.Common.StorageResponseFromWarehouseServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInST_NG_70_MSG3_StorageResponseFromWarehouseMessagingServices : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        DCAInCustomRequest,
        ST_NG_70_MSG3_StorageResponseFromWarehouse,
        DCAInCustomRequestService,
        ST_NG_70_MSG3_StorageResponseFromWarehouseResponseService, DCAInRequestHeader>
    {

        protected override ST_NG_70_MSG3_StorageResponseFromWarehouse CallWS(DCAInCustomRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "70"; }
        }

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(ST_NG_70_MSG3_StorageResponseFromWarehouse customsResponse)
        {
            var tableName = "Customs.Declaration";

            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName(tableName),
                //LoggingEntityId = customsResponse.ResponseContentHeader.ApplicationID.ToString(), // to check with itzik about array
            };
            return myGenericRequestParams;
        }
    }
}
